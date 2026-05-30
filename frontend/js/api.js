/** Все запросы к API идут сюда. Переопределение: window.__FITTRACKER_API__ = 'http://...' до подключения скрипта. */
const API_BASE_URL =
    typeof window.__FITTRACKER_API__ === 'string' && window.__FITTRACKER_API__.trim()
        ? window.__FITTRACKER_API__.replace(/\/+$/, '')
        : 'http://localhost:5223';
const API = {
    async request(method, url, body = null, isFormData = false) {
        const options = {
            method,
            credentials: 'include',
            headers: {}
        };

        if (body && !isFormData) {
            options.headers['Content-Type'] = 'application/json';
            options.body = JSON.stringify(body);
        } else if (body && isFormData) {
            options.body = body;
        }

        let response;
        try {
            response = await fetch(`${API_BASE_URL}${url}`, options);
        } catch (err) {
            console.error('[API] fetch failed:', err);
            throw new Error(
                `Нет связи с сервером (${err.message || 'network error'}). Запущен ли API на порту 5223?`,
                { cause: err }
            );
        }

        if (response.status === 401) {
            if (!window.location.pathname.includes('auth.html')) {
                window.location.href = getPagePath('auth.html');
            }
            throw new Error('Unauthorized');
        }

        if (!response.ok) {
            const errorData = await response.json().catch(() => null);
            let apiMsg =
                errorData?.message ||
                errorData?.Message ||
                errorData?.errorMessage;
            if (!apiMsg && errorData?.errors) {
                const msgs = Object.values(errorData.errors).flat();
                apiMsg = msgs.join('; ');
            }
            if (!apiMsg) apiMsg = errorData?.title;
            throw new Error(apiMsg || `HTTP ${response.status}`);
        }

        const text = await response.text();
        return text ? JSON.parse(text) : null;
    },

    get(url) {
        return this.request('GET', url);
    },

    post(url, body, isFormData = false) {
        return this.request('POST', url, body, isFormData);
    },

    put(url, body) {
        return this.request('PUT', url, body);
    },

    delete(url) {
        return this.request('DELETE', url);
    }
};

function getPagePath(page) {
    const currentPath = window.location.pathname;
    if (currentPath.includes('/pages/')) {
        return page;
    }
    return 'pages/' + page;
}

function getBasePath() {
    const currentPath = window.location.pathname;
    if (currentPath.includes('/pages/')) {
        return '../';
    }
    return '';
}

async function checkModeratorLink() {
    try {
        const me = await API.get('/api/auth/me');
        if (me && me.role && me.role.toLowerCase() === 'moderator') {
            const link = document.getElementById('mod-link');
            if (link) link.style.display = '';
        }
    } catch (_) {}
}

document.addEventListener('DOMContentLoaded', () => {
    if (!window.location.pathname.includes('auth.html')) {
        checkModeratorLink();
    }
});
