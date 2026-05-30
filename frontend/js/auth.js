document.addEventListener('DOMContentLoaded', () => {
    const tabs = document.querySelectorAll('.auth-tab');
    const forms = document.querySelectorAll('.auth-form');
    const alertEl = document.getElementById('alert');

    tabs.forEach(tab => {
        tab.addEventListener('click', () => {
            tabs.forEach(t => t.classList.remove('active'));
            forms.forEach(f => f.classList.remove('active'));
            tab.classList.add('active');
            document.getElementById(`${tab.dataset.tab}-form`).classList.add('active');
            hideAlert();
        });
    });

    function showAlert(message) {
        alertEl.textContent = message;
        alertEl.classList.add('show');
    }

    function hideAlert() {
        alertEl.classList.remove('show');
    }

    async function guardSubmit(formEl, submitter, work) {
        if (formEl.dataset.busy === '1') return;
        const btn = submitter ?? formEl.querySelector('button[type="submit"]');
        formEl.dataset.busy = '1';
        if (btn) btn.disabled = true;
        try {
            await work();
        } finally {
            delete formEl.dataset.busy;
            if (btn) btn.disabled = false;
        }
    }

    document.getElementById('login-form').addEventListener('submit', async e => {
        e.preventDefault();
        await guardSubmit(e.target, e.submitter, async () => {
            hideAlert();
            const email = document.getElementById('login-email').value.trim();
            const password = document.getElementById('login-password').value;

            try {
                const result = await API.post('/api/auth/login', { email, password });
                if (result.errorMessage) {
                    showAlert(result.errorMessage);
                    return;
                }
                window.location.href = 'profile.html';
            } catch (err) {
                showAlert(err.message || 'Ошибка входа');
            }
        });
    });

    document.getElementById('register-form').addEventListener('submit', async e => {
        e.preventDefault();
        await guardSubmit(e.target, e.submitter, async () => {
            hideAlert();
            const email = document.getElementById('reg-email').value.trim();
            const login = document.getElementById('reg-login').value.trim();
            const password = document.getElementById('reg-password').value;

            try {
                const result = await API.post('/api/auth/register', { email, login, password });
                if (result.errorMessage) {
                    showAlert(result.errorMessage);
                    return;
                }
                window.location.href = 'profile.html';
            } catch (err) {
                showAlert(err.message || 'Ошибка регистрации');
            }
        });
    });
});
