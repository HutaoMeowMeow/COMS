const API_BASE = '';
const SIGNALR_HUB = '/hubs/monitoring';

let currentUser = null;
let token = localStorage.getItem('coms_token');
let signalRConnection = null;

function getAuthHeaders() {
    const headers = { 'Content-Type': 'application/json' };
    if (token) headers['Authorization'] = `Bearer ${token}`;
    return headers;
}

<<<<<<< HEAD
=======
function showLoading() {
    const existing = document.getElementById('globalLoading');
    if (existing) return;
    const div = document.createElement('div');
    div.id = 'globalLoading';
    div.className = 'position-fixed top-0 start-0 w-100 h-100 d-flex align-items-center justify-content-center';
    div.style.cssText = 'background: rgba(255,255,255,0.7); z-index: 9999;';
    div.innerHTML = '<div class="spinner-border text-primary" role="status"><span class="visually-hidden">Loading...</span></div>';
    document.body.appendChild(div);
}

function hideLoading() {
    const el = document.getElementById('globalLoading');
    if (el) el.remove();
}

>>>>>>> db46004de7d488abfb831db9c6cd307518689719
async function apiCall(url, options = {}) {
    const config = {
        ...options,
        headers: { ...getAuthHeaders(), ...options.headers }
    };
    const response = await fetch(API_BASE + url, config);
    if (response.status === 401) {
        logout();
        throw new Error('Unauthorized');
    }
<<<<<<< HEAD
=======
    if (!response.ok) {
        const text = await response.text();
        let message = 'Request failed';
        try {
            const err = JSON.parse(text);
            message = err.message || message;
        } catch {
            message = text || message;
        }
        throw new Error(message);
    }
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
    return response;
}

function showAlert(message, type = 'info') {
    const alertDiv = document.createElement('div');
    alertDiv.className = `alert alert-${type} alert-dismissible fade show`;
    alertDiv.innerHTML = `${message} <button type="button" class="btn-close" data-bs-dismiss="alert"></button>`;
<<<<<<< HEAD
    const main = document.querySelector('.container-fluid') || document.querySelector('main');
    if (main) main.prepend(alertDiv);
    setTimeout(() => alertDiv.remove(), 5000);
=======
    const main = document.querySelector('main');
    if (main) {
        main.prepend(alertDiv);
        setTimeout(() => alertDiv.remove(), 5000);
    }
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
}

async function loadUser() {
    if (!token) return;
    try {
        const response = await apiCall('/api/Auth/me');
        if (response.ok) {
            currentUser = await response.json();
<<<<<<< HEAD
            const userDisplay = document.getElementById('userDisplay');
            if (userDisplay) {
                userDisplay.textContent = `${currentUser.firstName} ${currentUser.lastName} (${currentUser.role})`;
=======
            const display = document.getElementById('userDisplay');
            if (display) {
                display.textContent = `${currentUser.firstName} ${currentUser.lastName} (${currentUser.role})`;
            }
            const logoutItem = document.getElementById('logoutItem');
            if (logoutItem) {
                logoutItem.style.display = 'block';
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
            }
        }
    } catch (e) {
        console.error('Failed to load user', e);
    }
}

function logout() {
    token = null;
    currentUser = null;
    localStorage.removeItem('coms_token');
    window.location.href = '/Auth/Login';
}

function initSignalR(onMessage) {
    if (signalRConnection) return;
<<<<<<< HEAD
    signalRConnection = new signalR.HubConnectionBuilder()
        .withUrl(SIGNALR_HUB, { accessTokenFactory: () => token })
        .withAutomaticReconnect()
        .build();

    signalRConnection.on('NewReading', (reading) => {
        if (onMessage) onMessage('reading', reading);
    });

    signalRConnection.on('NewAlert', (alert) => {
        if (onMessage) onMessage('alert', alert);
        showAlert(`New Alert: ${alert.title} - ${alert.description}`, 'danger');
    });

    signalRConnection.start().catch(err => console.error('SignalR connection error:', err));
=======
    try {
        signalRConnection = new signalR.HubConnectionBuilder()
            .withUrl(SIGNALR_HUB, { accessTokenFactory: () => token })
            .withAutomaticReconnect()
            .configureLogging(signalR.LogLevel.Information)
            .build();

        signalRConnection.on('NewReading', (reading) => {
            if (onMessage) onMessage('reading', reading);
        });

        signalRConnection.on('NewAlert', (alert) => {
            if (onMessage) onMessage('alert', alert);
            showAlert(`New Alert: ${alert.title} - ${alert.description}`, 'danger');
        });

        signalRConnection.start().catch(err => console.error('SignalR connection error:', err));
    } catch (e) {
        console.error('SignalR init error:', e);
    }
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
}

document.addEventListener('DOMContentLoaded', () => {
    const publicPages = ['/Auth/Login', '/Auth/Register'];
    const path = window.location.pathname;
<<<<<<< HEAD
    if (!publicPages.some(p => path.startsWith(p)) && !token) {
        window.location.href = '/Auth/Login';
        return;
    }
    if (publicPages.some(p => path.startsWith(p)) && token) {
=======
    const isPublic = publicPages.some(p => path === p || path.startsWith(p + '/'));

    if (!isPublic && !token) {
        window.location.href = '/Auth/Login';
        return;
    }
    if (isPublic && token) {
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
        window.location.href = '/Dashboard';
        return;
    }
    loadUser();
<<<<<<< HEAD
});
=======
    loadNotifications();
});

async function loadNotifications() {
    if (!token) return;
    try {
        const response = await apiCall('/api/Notifications');
        if (response.ok) {
            const notifications = await response.json();
            const badge = document.getElementById('notificationBadge');
            const list = document.getElementById('notificationList');
            const dropdown = document.getElementById('notificationDropdown');
            if (!badge || !list || !dropdown) return;

            const unread = notifications.filter(n => n.status === 'Unread').length;
            dropdown.style.display = 'block';

            if (unread > 0) {
                badge.textContent = unread > 99 ? '99+' : unread;
                badge.style.display = 'block';
            } else {
                badge.style.display = 'none';
            }

            if (notifications.length === 0) {
                list.innerHTML = '<span class="dropdown-item-text text-muted">No notifications</span>';
            } else {
                list.innerHTML = notifications.slice(0, 5).map(n => `
                    <a class="dropdown-item ${n.status === 'Unread' ? 'fw-bold' : ''}" href="javascript:void(0)" onclick="markNotificationRead('${n.id}')">
                        <div class="d-flex w-100 justify-content-between">
                            <small>${n.title}</small>
                            <small class="text-muted">${new Date(n.createdAt).toLocaleDateString()}</small>
                        </div>
                        <small class="text-muted text-truncate d-block">${n.message}</small>
                    </a>
                `).join('');
            }
        }
    } catch (e) {
        console.error('Failed to load notifications', e);
    }
}

async function markNotificationRead(id) {
    try {
        await apiCall(`/api/Notifications/${id}/read`, { method: 'PUT' });
        loadNotifications();
    } catch (e) {
        console.error('Failed to mark notification as read', e);
    }
}
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
