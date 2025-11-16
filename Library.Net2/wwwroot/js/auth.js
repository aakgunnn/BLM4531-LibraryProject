// Authentication Utilities

function isLoggedIn() {
    return !!localStorage.getItem('token');
}

function getCurrentUser() {
    const userStr = localStorage.getItem('user');
    return userStr ? JSON.parse(userStr) : null;
}

function isAdmin() {
    const user = getCurrentUser();
    return user && user.role === 'Admin';
}

function logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    window.location.href = '/pages/login.html';
}

function requireAuth() {
    if (!isLoggedIn()) {
        window.location.href = '/pages/login.html';
        return false;
    }
    return true;
}

function requireAdmin() {
    if (!isLoggedIn()) {
        window.location.href = '/pages/login.html';
        return false;
    }
    if (!isAdmin()) {
        alert('Bu sayfaya erişim yetkiniz yok.');
        window.location.href = '/pages/catalog.html';
        return false;
    }
    return true;
}

// Update navigation based on auth state
function updateNavigation() {
    const navMenu = document.getElementById('navMenu');
    if (!navMenu) return;

    if (isLoggedIn()) {
        const user = getCurrentUser();
        const menuItems = isAdmin() ? `
            <li class="nav-item">
                <a class="nav-link" href="/pages/admin-dashboard.html">
                    <i class="bi bi-speedometer2"></i> Yönetim Paneli
                </a>
            </li>
            <li class="nav-item">
                <a class="nav-link" href="/pages/catalog.html">
                    <i class="bi bi-book"></i> Katalog
                </a>
            </li>
        ` : `
            <li class="nav-item">
                <a class="nav-link" href="/pages/catalog.html">
                    <i class="bi bi-book"></i> Katalog
                </a>
            </li>
            <li class="nav-item">
                <a class="nav-link" href="/pages/my-loans.html">
                    <i class="bi bi-bookmark"></i> Ödünçlerim
                </a>
            </li>
        `;

        navMenu.innerHTML = `
            ${menuItems}
            <li class="nav-item dropdown">
                <a class="nav-link dropdown-toggle" href="#" id="userDropdown" role="button" 
                   data-bs-toggle="dropdown" aria-expanded="false">
                    <i class="bi bi-person-circle"></i> ${user.fullName}
                </a>
                <ul class="dropdown-menu dropdown-menu-end" aria-labelledby="userDropdown">
                    <li><a class="dropdown-item" href="#" onclick="logout()">
                        <i class="bi bi-box-arrow-right"></i> Çıkış Yap
                    </a></li>
                </ul>
            </li>
        `;
    }
}

// Run on page load
document.addEventListener('DOMContentLoaded', updateNavigation);

