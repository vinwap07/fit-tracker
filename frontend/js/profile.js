document.addEventListener('DOMContentLoaded', async () => {
    setupHealthForm();
    try {
        await Promise.all([
            loadUserInfo(),
            loadHealthData(),
            loadWeather(),
            loadWeightChart(),
            loadWorkoutChart(),
            loadCaloriesChart()
        ]);
    } catch (err) {
        console.error('Dashboard load error:', err);
    }

    document.getElementById('logout-btn').addEventListener('click', async (e) => {
        e.preventDefault();
        try {
            await API.post('/api/auth/logout', { token: '' });
        } catch (_) {}
        window.location.href = 'auth.html';
    });
});

/** @type {boolean} */
let healthProfileExists = false;

function hideHealthAlerts() {
    document.getElementById('health-alert').classList.remove('show');
    document.getElementById('health-success').classList.remove('show');
}

function showHealthError(msg) {
    const el = document.getElementById('health-alert');
    el.textContent = msg;
    el.classList.add('show');
}

function showHealthSuccess(msg) {
    const el = document.getElementById('health-success');
    el.textContent = msg;
    el.classList.add('show');
}

function fillHealthForm(health) {
    document.getElementById('hp-height').value = health.height;
    document.getElementById('hp-weight').value = health.weight;
    document.getElementById('hp-sex').value = String(health.sex ?? 0);

    if (health.dateOfBirth) {
        const iso =
            typeof health.dateOfBirth === 'string'
                ? health.dateOfBirth.slice(0, 10)
                : health.dateOfBirth;
        document.getElementById('hp-dob').value = iso;
    }
}

function updateStatCards(health) {
    if (!health) return;
    document.getElementById('stat-weight').textContent = health.weight;
    document.getElementById('stat-height').textContent = health.height;

    if (health.dateOfBirth) {
        const birthDate = new Date(health.dateOfBirth);
        const age = Math.floor((Date.now() - birthDate.getTime()) / (365.25 * 24 * 60 * 60 * 1000));
        document.getElementById('stat-age').textContent = age;
    }

    const sexMap = { 0: 'Мужской', 1: 'Женский' };
    document.getElementById('stat-sex').textContent = sexMap[health.sex] ?? '—';
}

function setupHealthForm() {
    document.getElementById('health-form').addEventListener('submit', async e => {
        e.preventDefault();
        hideHealthAlerts();

        const height = parseInt(document.getElementById('hp-height').value, 10);
        const weight = parseFloat(document.getElementById('hp-weight').value);
        const dateOfBirth = document.getElementById('hp-dob').value;
        const sex = parseInt(document.getElementById('hp-sex').value, 10);

        try {
            const wasExisting = healthProfileExists;
            if (healthProfileExists) {
                await API.put('/api/PersonalHealth/my', {
                    height,
                    weight,
                    dateOfBirth,
                    sex
                });
            } else {
                await API.post('/api/PersonalHealth', {
                    height,
                    weight,
                    dateOfBirth,
                    sex
                });
            }
            showHealthSuccess(wasExisting ? 'Данные обновлены.' : 'Профиль здоровья создан.');
            window.location.reload();
        } catch (err) {
            showHealthError(err.message || 'Не удалось сохранить профиль.');
        }
    });
}

async function loadUserInfo() {
    try {
        const user = await API.get('/api/auth/me');
        document.getElementById('user-name').textContent = user.login || user.email;
    } catch (_) {}
}

async function loadHealthData() {
    try {
        const health = await API.get('/api/PersonalHealth/my');
        if (health) {
            healthProfileExists = true;
            fillHealthForm(health);
            updateStatCards(health);
            document.getElementById('health-hint').textContent =
                'Измените данные и нажмите «Сохранить», чтобы обновить профиль.';
        } else {
            healthProfileExists = false;
            document.getElementById('health-hint').textContent =
                'Заполните профиль здоровья (он нужен для расчёта калорий при тренировках).';
        }
    } catch (_) {
        document.getElementById('health-hint').textContent =
            'Не удалось загрузить профиль. Попробуйте обновить страницу.';
    }
}

async function loadWeather() {
    const tempEl = document.getElementById('weather-temp');
    const descEl = document.getElementById('weather-desc');
    const recEl = document.getElementById('weather-rec');

    try {
        const weather = await API.get('/api/weather');
        tempEl.textContent = `${weather.temperature}\u00B0C`;
        descEl.textContent = weather.description;

        if (weather.isRunningRecommended) {
            recEl.textContent = `\u2713 ${weather.motivationMessage}`;
            recEl.className = 'weather-recommendation good';
        } else {
            recEl.textContent = `\u26A0 ${weather.motivationMessage}`;
            recEl.className = 'weather-recommendation bad';
        }
    } catch (err) {
        tempEl.textContent = 'Не удалось загрузить';
        descEl.textContent = err.message || '';
        recEl.textContent = '';
        recEl.className = 'weather-recommendation';
    }
}

let weightChartInstance;
let workoutChartInstance;
let caloriesChartInstance;

function destroyChart(ref) {
    if (ref) ref.destroy();
}

/** @param {string|number} d */
function parseDurationMinutes(d) {
    if (d == null) return 0;
    if (typeof d === 'number') return d;
    if (typeof d === 'string') {
        const segments = d.split(':').map(s => parseFloat(s.trim()));
        if (segments.every(n => !Number.isNaN(n))) {
            if (segments.length >= 3) {
                const h = segments[segments.length - 3];
                const m = segments[segments.length - 2];
                const s = segments[segments.length - 1];
                return h * 60 + m + s / 60;
            }
            if (segments.length === 2) return segments[0] + segments[1] / 60;
            return segments[0] || 0;
        }
    }
    return 0;
}

async function loadWeightChart() {
    try {
        const data = await API.get('/api/PersonalHealth/my/weight-history');
        if (!data || data.length === 0) return;

        const sorted = data.sort((a, b) => new Date(a.date) - new Date(b.date));
        const labels = sorted.map(p => p.date);
        const values = sorted.map(p => p.weight);

        destroyChart(weightChartInstance);
        weightChartInstance = new Chart(document.getElementById('weight-chart'), {
            type: 'line',
            data: {
                labels,
                datasets: [
                    {
                        label: 'Вес (кг)',
                        data: values,
                        borderColor: '#CCFF00',
                        backgroundColor: 'rgba(204, 255, 0, 0.1)',
                        tension: 0.3,
                        fill: true,
                        pointBackgroundColor: '#CCFF00'
                    }
                ]
            },
            options: chartOptions('кг')
        });
    } catch (_) {}
}

async function loadWorkoutChart() {
    try {
        const data = await API.get('/api/workout/my/date-chart');
        if (!data || data.length === 0) return;

        const sorted = data.sort((a, b) => new Date(a.date) - new Date(b.date));
        const labels = sorted.map(p => p.date);
        const values = sorted.map(p => parseDurationMinutes(p.duration));

        destroyChart(workoutChartInstance);
        workoutChartInstance = new Chart(document.getElementById('workout-chart'), {
            type: 'bar',
            data: {
                labels,
                datasets: [
                    {
                        label: 'Длительность (мин)',
                        data: values,
                        backgroundColor: 'rgba(204, 255, 0, 0.6)',
                        borderColor: '#CCFF00',
                        borderWidth: 1,
                        borderRadius: 4
                    }
                ]
            },
            options: chartOptions('мин')
        });
    } catch (_) {}
}

async function loadCaloriesChart() {
    try {
        const data = await API.get('/api/workout/my/calories-chart');
        if (!data || data.length === 0) return;

        const sorted = data.sort((a, b) => new Date(a.date) - new Date(b.date));
        const labels = sorted.map(p => p.date);
        const values = sorted.map(p => p.calories);

        destroyChart(caloriesChartInstance);
        caloriesChartInstance = new Chart(document.getElementById('calories-chart'), {
            type: 'line',
            data: {
                labels,
                datasets: [
                    {
                        label: 'Калории (ккал)',
                        data: values,
                        borderColor: '#ff6b6b',
                        backgroundColor: 'rgba(255, 107, 107, 0.1)',
                        tension: 0.3,
                        fill: true,
                        pointBackgroundColor: '#ff6b6b'
                    }
                ]
            },
            options: chartOptions('ккал')
        });
    } catch (_) {}
}

function chartOptions(unit) {
    return {
        responsive: true,
        maintainAspectRatio: true,
        plugins: {
            legend: {
                labels: { color: '#b0b0b0' }
            }
        },
        scales: {
            x: {
                ticks: { color: '#707070' },
                grid: { color: 'rgba(255,255,255,0.05)' }
            },
            y: {
                ticks: {
                    color: '#707070',
                    callback: val => `${val} ${unit}`
                },
                grid: { color: 'rgba(255,255,255,0.05)' }
            }
        }
    };
}
