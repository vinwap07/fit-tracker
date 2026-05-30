const STATUS_MAP = {
    0: 'На рассмотрении', 1: 'Одобрено', 2: 'Отклонено', 3: 'На доработке',
    Pending: 'На рассмотрении', Approved: 'Одобрено', Rejected: 'Отклонено', NeedsRevision: 'На доработке',
};

let proposals = [];

document.addEventListener('DOMContentLoaded', async () => {
    document.getElementById('logout-btn').addEventListener('click', async (e) => {
        e.preventDefault();
        try { await API.post('/api/auth/logout', { token: '' }); } catch (_) {}
        window.location.href = 'auth.html';
    });

    await loadProposals();
});

async function loadProposals() {
    const list = document.getElementById('proposals-list');
    const loading = document.getElementById('loading');
    const emptyMsg = document.getElementById('empty-msg');

    try {
        proposals = await API.get('/api/exerciseproposal');
        loading.style.display = 'none';

        if (!proposals || proposals.length === 0) {
            emptyMsg.style.display = 'block';
            list.innerHTML = '';
            return;
        }
        emptyMsg.style.display = 'none';
        renderList(proposals);
    } catch (err) {
        loading.style.display = 'none';
        if (err.message && err.message.includes('403')) {
            showError('Доступ запрещён. Эта страница только для модераторов.');
        } else {
            showError(err.message || 'Ошибка загрузки');
        }
    }
}

function renderList(items) {
    const list = document.getElementById('proposals-list');
    list.innerHTML = items.map(p => `
        <div class="card" style="margin-bottom:20px;" id="proposal-${p.id}">
            <div style="display:flex; justify-content:space-between; align-items:center; flex-wrap:wrap; gap:12px;">
                <div>
                    <h3 class="card-title" style="margin:0 0 4px 0;">${esc(p.name)}</h3>
                    <span class="page-subtitle" style="font-size:0.85rem;">Статус: ${STATUS_MAP[p.status] || p.status}</span>
                </div>
                <div style="display:flex; gap:8px; flex-wrap:wrap;">
                    <button class="btn btn-primary" style="font-size:0.85rem; padding:8px 16px;"
                            onclick="viewDetails('${p.id}')">Подробнее</button>
                    <button class="btn btn-approve" style="font-size:0.85rem; padding:8px 16px;"
                            onclick="approveProposal('${p.id}')">Одобрить</button>
                    <button class="btn btn-danger" style="font-size:0.85rem; padding:8px 16px;"
                            onclick="openReject('${p.id}')">Отклонить</button>
                    <button class="btn btn-secondary" style="font-size:0.85rem; padding:8px 16px;"
                            onclick="openRevision('${p.id}')">На доработку</button>
                </div>
            </div>
            <div id="details-${p.id}" style="display:none; margin-top:16px;"></div>
        </div>
    `).join('');
}

async function viewDetails(id) {
    const container = document.getElementById(`details-${id}`);
    if (container.style.display !== 'none') {
        container.style.display = 'none';
        return;
    }
    container.innerHTML = '<p class="page-subtitle">Загрузка...</p>';
    container.style.display = 'block';

    try {
        const d = await API.get(`/api/exerciseproposal/${id}`);
        container.innerHTML = `
            <div class="grid-3" style="gap:16px;">
                ${d.photoUrl ? `<div><p class="form-label">Фото</p><img src="${esc(d.photoUrl)}" style="max-width:100%; border-radius:8px;" alt="photo"></div>` : ''}
                ${d.performanceVideoUrl ? `<div><p class="form-label">Видео выполнения</p><video src="${esc(d.performanceVideoUrl)}" controls style="max-width:100%; border-radius:8px;"></video></div>` : ''}
                ${d.emgVideoUrl ? `<div><p class="form-label">Видео ЭМГ</p><video src="${esc(d.emgVideoUrl)}" controls style="max-width:100%; border-radius:8px;"></video></div>` : ''}
            </div>
        `;
    } catch (err) {
        container.innerHTML = `<p style="color:var(--error);">${esc(err.message)}</p>`;
    }
}

async function approveProposal(id) {
    if (!confirm('Одобрить эту заявку?')) return;
    try {
        await API.post(`/api/exerciseproposal/${id}/approve`, {});
        showSuccess('Заявка одобрена');
        await loadProposals();
    } catch (err) {
        showError(err.message || 'Ошибка');
    }
}

let _modalResolve = null;
let _modalAction = '';

function openCommentModal(title) {
    return new Promise(resolve => {
        _modalResolve = resolve;
        document.getElementById('modal-title').textContent = title;
        document.getElementById('modal-comment').value = '';
        document.getElementById('comment-modal').style.display = 'flex';

        document.getElementById('modal-confirm').onclick = () => {
            const comment = document.getElementById('modal-comment').value.trim();
            closeModal();
            resolve(comment || null);
        };
        document.getElementById('modal-cancel').onclick = () => {
            closeModal();
            resolve(null);
        };
    });
}

function closeModal() {
    document.getElementById('comment-modal').style.display = 'none';
    _modalResolve = null;
}

async function openReject(id) {
    const comment = await openCommentModal('Причина отклонения');
    if (comment === null) return;
    try {
        await API.post(`/api/exerciseproposal/${id}/reject`, { proposalId: id, comment });
        showSuccess('Заявка отклонена');
        await loadProposals();
    } catch (err) {
        showError(err.message || 'Ошибка');
    }
}

async function openRevision(id) {
    const comment = await openCommentModal('Комментарий для доработки');
    if (comment === null) return;
    try {
        await API.post(`/api/exerciseproposal/${id}/send-to-revision`, { proposalId: id, comment });
        showSuccess('Заявка отправлена на доработку');
        await loadProposals();
    } catch (err) {
        showError(err.message || 'Ошибка');
    }
}

function showError(msg) {
    const el = document.getElementById('alert');
    el.textContent = msg;
    el.classList.add('show');
    setTimeout(() => el.classList.remove('show'), 5000);
}

function showSuccess(msg) {
    const el = document.getElementById('success-alert');
    el.textContent = msg;
    el.classList.add('show');
    setTimeout(() => el.classList.remove('show'), 4000);
}

function esc(s) {
    const d = document.createElement('div');
    d.textContent = s;
    return d.innerHTML;
}
