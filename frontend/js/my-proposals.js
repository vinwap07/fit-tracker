const STATUS_MAP = {
    0: { label: 'На рассмотрении', cls: 'badge-pending' },
    1: { label: 'Одобрено', cls: 'badge-approved' },
    2: { label: 'Отклонено', cls: 'badge-rejected' },
    3: { label: 'На доработке', cls: 'badge-revision' },
    Pending:       { label: 'На рассмотрении', cls: 'badge-pending' },
    Approved:      { label: 'Одобрено', cls: 'badge-approved' },
    Rejected:      { label: 'Отклонено', cls: 'badge-rejected' },
    NeedsRevision: { label: 'На доработке', cls: 'badge-revision' },
};

function statusBadge(status) {
    const s = STATUS_MAP[status] || { label: status, cls: '' };
    return `<span class="proposal-badge ${s.cls}">${s.label}</span>`;
}

document.addEventListener('DOMContentLoaded', async () => {
    document.getElementById('logout-btn').addEventListener('click', async (e) => {
        e.preventDefault();
        try { await API.post('/api/auth/logout', { token: '' }); } catch (_) {}
        window.location.href = 'auth.html';
    });

    const grid = document.getElementById('proposals-grid');
    const loading = document.getElementById('loading');
    const emptyMsg = document.getElementById('empty-msg');
    const alertEl = document.getElementById('alert');

    try {
        const proposals = await API.get('/api/exerciseproposal/my');
        loading.style.display = 'none';

        if (!proposals || proposals.length === 0) {
            emptyMsg.style.display = 'block';
            return;
        }

        grid.innerHTML = proposals.map(p => `
            <div class="card proposal-card">
                <div style="display:flex; justify-content:space-between; align-items:center; margin-bottom:12px;">
                    <h3 class="card-title" style="margin:0;">${esc(p.name)}</h3>
                    ${statusBadge(p.status)}
                </div>
                <a href="proposal-detail.html?id=${p.id}" class="btn btn-secondary" style="font-size:0.85rem;">Подробнее</a>
            </div>
        `).join('');
    } catch (err) {
        loading.style.display = 'none';
        alertEl.textContent = err.message || 'Ошибка загрузки';
        alertEl.classList.add('show');
    }
});

function esc(s) {
    const d = document.createElement('div');
    d.textContent = s;
    return d.innerHTML;
}
