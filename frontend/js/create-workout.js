let availableExercises = [];
let exerciseCount = 0;

document.addEventListener('DOMContentLoaded', async () => {
    try {
        availableExercises = await API.get('/api/exercisecatalog');
    } catch (_) {}

    const now = new Date();
    now.setMinutes(now.getMinutes() - now.getTimezoneOffset());
    document.getElementById('workout-date').value = now.toISOString().slice(0, 16);

    addExerciseRow();

    document.getElementById('add-exercise-btn').addEventListener('click', addExerciseRow);
    document.getElementById('workout-form').addEventListener('submit', handleSubmit);

    document.getElementById('logout-btn').addEventListener('click', async (e) => {
        e.preventDefault();
        try { await API.post('/api/auth/logout', { token: '' }); } catch (_) {}
        window.location.href = 'auth.html';
    });
});

function addExerciseRow() {
    exerciseCount++;
    const container = document.getElementById('exercises-list');

    const row = document.createElement('div');
    row.className = 'grid-3';
    row.style.marginBottom = '12px';
    row.dataset.idx = exerciseCount;

    const options = availableExercises.map(ex =>
        `<option value="${ex.id || ex.name}">${ex.name}</option>`
    ).join('');

    row.innerHTML = `
        <div class="form-group">
            <label class="form-label">Упражнение</label>
            <select class="form-input exercise-select" required>
                <option value="">Выберите...</option>
                ${options}
            </select>
        </div>
        <div class="form-group">
            <label class="form-label">Повторения</label>
            <input type="number" class="form-input exercise-reps" min="1" placeholder="12" required>
        </div>
        <div class="form-group">
            <label class="form-label">Вес (кг)</label>
            <div style="display:flex;gap:8px;">
                <input type="number" class="form-input exercise-weight" min="0" step="0.5" placeholder="50" required>
                <button type="button" class="btn btn-danger btn-sm remove-exercise-btn" title="Удалить">✕</button>
            </div>
        </div>
    `;

    container.appendChild(row);

    row.querySelector('.remove-exercise-btn').addEventListener('click', () => {
        if (document.querySelectorAll('#exercises-list > div').length > 1) {
            row.remove();
        }
    });
}

async function handleSubmit(e) {
    e.preventDefault();
    hideAlerts();

    const date = document.getElementById('workout-date').value;
    const durationRaw = document.getElementById('workout-duration').value.trim();

    if (!/^\d{2}:\d{2}:\d{2}$/.test(durationRaw)) {
        showError('Формат длительности: ЧЧ:ММ:СС (например, 01:30:00)');
        return;
    }

    const rows = document.querySelectorAll('#exercises-list > div');
    const exercises = [];

    for (const row of rows) {
        const id = row.querySelector('.exercise-select').value;
        const reps = parseInt(row.querySelector('.exercise-reps').value);
        const weight = parseFloat(row.querySelector('.exercise-weight').value);

        if (!id) {
            showError('Выберите упражнение для каждой строки');
            return;
        }

        exercises.push({ id, repetitions: reps, weight });
    }

    const body = {
        exercises,
        date: new Date(date).toISOString(),
        duration: durationRaw
    };

    try {
        await API.post('/api/workout', body);
        showSuccess('Тренировка успешно создана!');
        document.getElementById('workout-form').reset();
        document.getElementById('exercises-list').innerHTML = '';
        exerciseCount = 0;
        addExerciseRow();
    } catch (err) {
        showError(err.message || 'Ошибка создания тренировки');
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
