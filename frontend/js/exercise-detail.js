document.addEventListener('DOMContentLoaded', async () => {
    const params = new URLSearchParams(window.location.search);
    const exerciseId = params.get('id');

    if (!exerciseId) {
        window.location.href = 'exercises.html';
        return;
    }

    await loadExerciseDetails(exerciseId);
    setupVideoCompare();

    document.getElementById('logout-btn').addEventListener('click', async (e) => {
        e.preventDefault();
        try { await API.post('/api/auth/logout', { token: '' }); } catch (_) {}
        window.location.href = 'auth.html';
    });
});

async function loadExerciseDetails(id) {
    try {
        const exercise = await API.get(`/api/exercisecatalog/${id}`);

        document.getElementById('exercise-name').textContent = exercise.name;
        document.getElementById('exercise-description').textContent = exercise.description || 'Описание отсутствует';
        document.getElementById('exercise-met').textContent = exercise.met;
        document.getElementById('exercise-photo').src = exercise.photoUrl || '';
        document.getElementById('exercise-photo').alt = exercise.name;

        if (exercise.performanceVideoUrl) {
            const refVideo = document.getElementById('reference-video');
            refVideo.src = exercise.performanceVideoUrl;
            document.getElementById('performance-video-section').style.display = 'block';

            const compareRefVideo = document.getElementById('compare-ref-video');
            compareRefVideo.src = exercise.performanceVideoUrl;
        }
    } catch (err) {
        document.getElementById('exercise-name').textContent = 'Ошибка загрузки';
        document.getElementById('exercise-description').textContent = err.message;
    }
}

function setupVideoCompare() {
    const uploadArea = document.getElementById('user-video-upload');
    const fileInput = document.getElementById('user-video-input');
    const compareGrid = document.getElementById('compare-grid');
    const syncControls = document.getElementById('sync-controls');
    const userVideo = document.getElementById('compare-user-video');
    const refVideo = document.getElementById('compare-ref-video');
    const trimStart = document.getElementById('trim-start');
    const trimEnd = document.getElementById('trim-end');
    const trimStartVal = document.getElementById('trim-start-val');
    const trimEndVal = document.getElementById('trim-end-val');

    let userVideoUrl = null;
    let trimStartTime = 0;
    let trimEndTime = 0;

    uploadArea.addEventListener('click', () => fileInput.click());

    uploadArea.addEventListener('dragover', (e) => {
        e.preventDefault();
        uploadArea.style.borderColor = 'var(--accent)';
    });

    uploadArea.addEventListener('dragleave', () => {
        uploadArea.style.borderColor = '';
    });

    uploadArea.addEventListener('drop', (e) => {
        e.preventDefault();
        uploadArea.style.borderColor = '';
        const file = e.dataTransfer.files[0];
        if (file && file.type.startsWith('video/')) {
            handleVideoFile(file);
        }
    });

    fileInput.addEventListener('change', (e) => {
        const file = e.target.files[0];
        if (file) handleVideoFile(file);
    });

    function handleVideoFile(file) {
        if (userVideoUrl) URL.revokeObjectURL(userVideoUrl);
        userVideoUrl = URL.createObjectURL(file);
        userVideo.src = userVideoUrl;

        document.getElementById('upload-preview').textContent = `✓ ${file.name}`;
        compareGrid.style.display = 'grid';
        syncControls.style.display = 'flex';

        userVideo.addEventListener('loadedmetadata', () => {
            const duration = userVideo.duration;
            trimStart.max = duration;
            trimEnd.max = duration;
            trimEnd.value = duration;
            trimStartTime = 0;
            trimEndTime = duration;
            trimStartVal.textContent = '0s';
            trimEndVal.textContent = `${duration.toFixed(1)}s`;
        }, { once: true });
    }

    trimStart.addEventListener('input', (e) => {
        trimStartTime = parseFloat(e.target.value);
        trimStartVal.textContent = `${trimStartTime.toFixed(1)}s`;
        if (trimStartTime >= trimEndTime) {
            trimStart.value = trimEndTime - 0.1;
            trimStartTime = trimEndTime - 0.1;
        }
        userVideo.currentTime = trimStartTime;
    });

    trimEnd.addEventListener('input', (e) => {
        trimEndTime = parseFloat(e.target.value);
        trimEndVal.textContent = `${trimEndTime.toFixed(1)}s`;
        if (trimEndTime <= trimStartTime) {
            trimEnd.value = trimStartTime + 0.1;
            trimEndTime = trimStartTime + 0.1;
        }
    });

    let animationFrame = null;

    function monitorTrim() {
        if (userVideo.currentTime >= trimEndTime) {
            userVideo.pause();
            refVideo.pause();
            cancelAnimationFrame(animationFrame);
            return;
        }
        animationFrame = requestAnimationFrame(monitorTrim);
    }

    document.getElementById('sync-start-btn').addEventListener('click', () => {
        refVideo.currentTime = 0;
        userVideo.currentTime = trimStartTime;

        const playBoth = () => {
            refVideo.play();
            userVideo.play();
            monitorTrim();
        };

        if (refVideo.readyState >= 2 && userVideo.readyState >= 2) {
            playBoth();
        } else {
            let ready = 0;
            const checkReady = () => { if (++ready === 2) playBoth(); };
            refVideo.addEventListener('canplay', checkReady, { once: true });
            userVideo.addEventListener('canplay', checkReady, { once: true });
        }
    });

    document.getElementById('sync-pause-btn').addEventListener('click', () => {
        refVideo.pause();
        userVideo.pause();
        cancelAnimationFrame(animationFrame);
    });

    document.getElementById('sync-reset-btn').addEventListener('click', () => {
        refVideo.pause();
        userVideo.pause();
        refVideo.currentTime = 0;
        userVideo.currentTime = trimStartTime;
        cancelAnimationFrame(animationFrame);
    });
}
