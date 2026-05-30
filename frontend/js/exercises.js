document.addEventListener('DOMContentLoaded', async () => {
    await loadExercises();

    document.getElementById('search-btn').addEventListener('click', () => {
        const query = document.getElementById('search-input').value.trim();
        loadExercises(query);
    });

    document.getElementById('search-input').addEventListener('keydown', (e) => {
        if (e.key === 'Enter') {
            const query = e.target.value.trim();
            loadExercises(query);
        }
    });

    document.getElementById('logout-btn').addEventListener('click', async (e) => {
        e.preventDefault();
        try { await API.post('/api/auth/logout', { token: '' }); } catch (_) {}
        window.location.href = 'auth.html';
    });
});

async function loadExercises(searchName = '') {
    const grid = document.getElementById('exercises-grid');
    grid.innerHTML = '<div class="loader-screen" style="grid-column:1/-1;height:200px;"><div class="loader-spinner"></div></div>';

    try {
        let exercises;
        if (searchName) {
            exercises = await API.get(`/api/exercisecatalog/search?name=${encodeURIComponent(searchName)}`);
        } else {
            exercises = await API.get('/api/exercisecatalog');
        }

        if (!exercises || exercises.length === 0) {
            grid.innerHTML = '<p style="grid-column:1/-1;color:var(--text-secondary);text-align:center;">Упражнения не найдены</p>';
            return;
        }

        grid.innerHTML = exercises.map(ex => `
            <div class="exercise-card" onclick="openExercise('${ex.id || ''}')">
                <img class="exercise-card-img" src="${ex.photoUrl || ''}" alt="${ex.name}" onerror="this.src='data:image/svg+xml,<svg xmlns=%22http://www.w3.org/2000/svg%22 viewBox=%220 0 300 180%22><rect fill=%22%232a2a2a%22 width=%22300%22 height=%22180%22/><text fill=%22%23707070%22 x=%2250%25%22 y=%2250%25%22 dominant-baseline=%22middle%22 text-anchor=%22middle%22 font-size=%2214%22>Нет фото</text></svg>'">
                <div class="exercise-card-body">
                    <div class="exercise-card-title">${ex.name}</div>
                </div>
            </div>
        `).join('');
    } catch (err) {
        grid.innerHTML = `<p style="grid-column:1/-1;color:var(--danger);">Ошибка загрузки: ${err.message}</p>`;
    }
}

function openExercise(id) {
    if (id) {
        window.location.href = `exercise-detail.html?id=${id}`;
    }
}
