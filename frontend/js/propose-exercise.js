document.addEventListener('DOMContentLoaded', () => {
    setupFileUpload('photo-upload', 'photo-file', 'photo-preview');
    setupFileUpload('perf-video-upload', 'perf-video-file', 'perf-video-preview');
    setupFileUpload('emg-video-upload', 'emg-video-file', 'emg-video-preview');

    document.getElementById('propose-form').addEventListener('submit', handleSubmit);

    document.getElementById('logout-btn').addEventListener('click', async (e) => {
        e.preventDefault();
        try { await API.post('/api/auth/logout', { token: '' }); } catch (_) {}
        window.location.href = 'auth.html';
    });
});

function setupFileUpload(uploadId, inputId, previewId) {
    const uploadArea = document.getElementById(uploadId);
    const input = document.getElementById(inputId);
    const preview = document.getElementById(previewId);

    uploadArea.addEventListener('click', () => input.click());

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
        if (file) {
            const dt = new DataTransfer();
            dt.items.add(file);
            input.files = dt.files;
            preview.textContent = `✓ ${file.name}`;
        }
    });

    input.addEventListener('change', () => {
        if (input.files[0]) {
            preview.textContent = `✓ ${input.files[0].name}`;
        }
    });
}

let _submitting = false;

async function handleSubmit(e) {
    e.preventDefault();
    if (_submitting) return;
    hideAlerts();

    const name = document.getElementById('ex-name').value.trim();
    const description = document.getElementById('ex-description').value.trim();
    const met = parseFloat(document.getElementById('ex-met').value);

    const photoFile = document.getElementById('photo-file').files[0];
    const perfVideoFile = document.getElementById('perf-video-file').files[0];
    const emgVideoFile = document.getElementById('emg-video-file').files[0];

    if (!photoFile || !perfVideoFile || !emgVideoFile) {
        showError('Загрузите все три медиафайла');
        return;
    }

    const formData = new FormData();
    formData.append('name', name);
    formData.append('description', description);
    formData.append('met', met);
    formData.append('photo', photoFile);
    formData.append('performanceVideo', perfVideoFile);
    formData.append('emgVideo', emgVideoFile);

    const btn = document.querySelector('#propose-form button[type="submit"]');
    _submitting = true;
    if (btn) { btn.disabled = true; btn.textContent = 'Отправка…'; }

    try {
        await API.post('/api/exerciseproposal', formData, true);
        showSuccess('Упражнение отправлено на модерацию!');
        document.getElementById('propose-form').reset();
        document.getElementById('photo-preview').textContent = '';
        document.getElementById('perf-video-preview').textContent = '';
        document.getElementById('emg-video-preview').textContent = '';
    } catch (err) {
        showError(err.message || 'Ошибка отправки');
    } finally {
        _submitting = false;
        if (btn) { btn.disabled = false; btn.textContent = 'Предложить упражнение'; }
    }
}

function showError(msg) {
    const el = document.getElementById('alert');
    el.textContent = msg;
    el.classList.add('show');
}

function showSuccess(msg) {
    const el = document.getElementById('success-alert');
    el.textContent = msg;
    el.classList.add('show');
}

function hideAlerts() {
    document.getElementById('alert').classList.remove('show');
    document.getElementById('success-alert').classList.remove('show');
}
