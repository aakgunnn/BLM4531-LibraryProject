// Admin Dashboard JavaScript
requireAdmin();

let allLoans = [];
let allBooks = [];
let allCategories = [];

// Initialize
async function init() {
    await loadDashboard();
}

// Show/Hide sections
function showSection(section) {
    document.querySelectorAll('.content-section').forEach(el => el.style.display = 'none');
    document.querySelectorAll('.nav-link').forEach(el => el.classList.remove('active'));

    document.getElementById(`${section}Section`).style.display = 'block';
    event.target.classList.add('active');

    if (section === 'loans') loadLoans();
    if (section === 'books') loadBooks();
    if (section === 'categories') loadCategories();
}

// Dashboard Stats
async function loadDashboard() {
    try {
        const booksResponse = await api.get('/api/Books');
        const loansResponse = await api.get('/api/Admin/Loans');

        const books = (booksResponse && booksResponse.data) ? booksResponse.data : [];
        const loans = (loansResponse && loansResponse.data) ? loansResponse.data : [];

        document.getElementById('totalBooks').textContent = books.length;
        document.getElementById('availableBooks').textContent = books.filter(b => b.isAvailable).length;
        // Use normalizeStatus for consistent status comparison
        document.getElementById('pendingLoans').textContent = loans.filter(l => normalizeStatus(l.status) === 'pending').length;
        document.getElementById('activeLoans').textContent = loans.filter(l => normalizeStatus(l.status) === 'borrowed').length;
    } catch (error) {
        console.error('Dashboard yüklenirken hata:', error);
        showAlert('danger', 'Dashboard yüklenirken hata: ' + error.message);
    }
}

// Loans Management
async function loadLoans() {
    try {
        const response = await api.get('/api/Admin/Loans');
        allLoans = (response && response.data) ? response.data : [];
        displayLoans();
    } catch (error) {
        console.error('Ödünçler yüklenirken hata:', error);
        showAlert('danger', 'Ödünçler yüklenemedi: ' + error.message);
    }
}

function displayLoans() {
    const tbody = document.getElementById('loansTableBody');

    if (allLoans.length === 0) {
        tbody.innerHTML = '<tr><td colspan="7" class="text-center">Ödünç kaydı bulunamadı</td></tr>';
        return;
    }

    // Debug: Log first loan to see status format
    console.log('First loan status:', allLoans[0]?.status, 'Type:', typeof allLoans[0]?.status);

    tbody.innerHTML = allLoans.map(loan => {
        const status = normalizeStatus(loan.status);
        return `
        <tr class="${loan.isLate ? 'table-danger' : ''}">
            <td>${loan.id}</td>
            <td>${loan.userFullName || 'N/A'}</td>
            <td>${loan.bookTitle || 'N/A'}</td>
            <td>${formatDate(loan.loanDate)}</td>
            <td>
                ${loan.dueDate ? formatDate(loan.dueDate) : '-'}
                ${loan.isLate ? '<br><span class="badge bg-danger">GECİKMİŞ</span>' : ''}
            </td>
            <td>${getStatusBadge(loan.status)}</td>
            <td>
                ${status === 'pending' ? `
                    <button class="btn btn-sm btn-success" onclick="approveLoan(${loan.id})">
                        <i class="bi bi-check-circle"></i> Onayla
                    </button>
                    <button class="btn btn-sm btn-danger" onclick="rejectLoan(${loan.id})">
                        <i class="bi bi-x-circle"></i> Reddet
                    </button>
                ` : status === 'borrowed' ? `
                    <span class="text-muted">Kullanıcı iade edecek</span>
                ` : status === 'returned' ? `
                    <span class="badge bg-success">Tamamlandı</span>
                ` : '-'}
            </td>
        </tr>
    `}).join('');
}

// Normalize status to lowercase string for comparison
function normalizeStatus(status) {
    if (typeof status === 'string') return status.toLowerCase();
    if (typeof status === 'number') {
        const map = { 0: 'pending', 1: 'borrowed', 2: 'returned', 3: 'late', 4: 'cancelled' };
        return map[status] || 'unknown';
    }
    return 'unknown';
}

async function approveLoan(loanId) {
    if (!confirm('Bu ödünç talebini onaylamak istiyor musunuz?')) return;

    try {
        await api.put(`/api/Admin/Loans/${loanId}/approve`, { durationDays: 15 });
        showAlert('success', 'Ödünç talebi onaylandı! Kitap 15 günlüğüne ödünç verildi.');
        await loadLoans();
        await loadDashboard();
    } catch (error) {
        console.error('Onaylama hatası:', error);
        showAlert('danger', 'Hata: ' + error.message);
    }
}

async function rejectLoan(loanId) {
    const note = prompt('Reddetme sebebi (opsiyonel):');

    try {
        await api.put(`/api/Admin/Loans/${loanId}/reject`, { adminNote: note });
        showAlert('success', 'Ödünç talebi reddedildi!');
        await loadLoans();
        await loadDashboard();
    } catch (error) {
        console.error('Reddetme hatası:', error);
        showAlert('danger', 'Hata: ' + error.message);
    }
}

// Books Management
async function loadBooks() {
    try {
        const booksResponse = await api.get('/api/Books');
        const categoriesResponse = await api.get('/api/Categories');

        allBooks = (booksResponse && booksResponse.data) ? booksResponse.data : [];
        allCategories = (categoriesResponse && categoriesResponse.data) ? categoriesResponse.data : [];

        // Populate category select
        const select = document.getElementById('bookCategorySelect');
        select.innerHTML = allCategories.map(c =>
            `<option value="${c.id}">${c.name}</option>`
        ).join('');

        displayBooks();
    } catch (error) {
        console.error('Kitaplar yüklenirken hata:', error);
        showAlert('danger', 'Kitaplar yüklenemedi: ' + error.message);
    }
}

function displayBooks() {
    const tbody = document.getElementById('booksTableBody');

    if (allBooks.length === 0) {
        tbody.innerHTML = '<tr><td colspan="6" class="text-center">Kitap bulunamadı</td></tr>';
        return;
    }

    tbody.innerHTML = allBooks.map(book => `
        <tr>
            <td>${book.id}</td>
            <td>${book.title}</td>
            <td>${book.author}</td>
            <td>${getCategoryName(book.categoryId)}</td>
            <td>${getAvailabilityBadge(book.isAvailable)}</td>
            <td>
                <button class="btn btn-sm btn-warning" onclick="toggleAvailability(${book.id}, ${!book.isAvailable})">
                    <i class="bi bi-arrow-repeat"></i> ${book.isAvailable ? 'Devre Dışı' : 'Aktif Et'}
                </button>
                <button class="btn btn-sm btn-danger" onclick="deleteBook(${book.id})">
                    <i class="bi bi-trash"></i>
                </button>
            </td>
        </tr>
    `).join('');
}

function getCategoryName(categoryId) {
    const cat = allCategories.find(c => c.id === categoryId);
    return cat ? cat.name : 'N/A';
}

async function toggleAvailability(bookId, newStatus) {
    try {
        await api.put(`/api/books/${bookId}`, { isAvailable: newStatus });
        showAlert('success', 'Kitap durumu güncellendi!');
        await loadBooks();
        await loadDashboard();
    } catch (error) {
        showAlert('danger', 'Hata: ' + error.message);
    }
}

async function deleteBook(bookId) {
    if (!confirm('Bu kitabı silmek istediğinizden emin misiniz?')) return;

    try {
        await api.delete(`/api/Books/${bookId}`);
        showAlert('success', 'Kitap başarıyla silindi!');
        await loadBooks();
    } catch (error) {
        console.error('Kitap silinirken hata:', error);
        showAlert('danger', 'Hata: ' + error.message);
    }
}

// Image Upload Functions
async function uploadBookImage(file) {
    const formData = new FormData();
    formData.append('file', file);

    try {
        const response = await api.post('/api/Books/upload-image', formData);
        return response.data; // Returns the image URL
    } catch (error) {
        throw new Error('Resim yüklenirken hata: ' + error.message);
    }
}

function clearImagePreview() {
    document.getElementById('imagePreview').style.display = 'none';
    document.getElementById('previewImg').src = '';
    document.getElementById('bookImageInput').value = '';
    document.getElementById('bookImageUrl').value = '';
}

// Image preview on file selection
document.getElementById('bookImageInput').addEventListener('change', function (e) {
    const file = e.target.files[0];
    if (file) {
        const reader = new FileReader();
        reader.onload = function (event) {
            document.getElementById('previewImg').src = event.target.result;
            document.getElementById('imagePreview').style.display = 'block';
        };
        reader.readAsDataURL(file);
    }
});

// Add Book Form
document.getElementById('addBookForm').addEventListener('submit', async (e) => {
    e.preventDefault();

    const formData = new FormData(e.target);
    const data = {
        title: formData.get('title'),
        author: formData.get('author'),
        categoryId: parseInt(formData.get('categoryId')),
        isbn: formData.get('isbn') || null,
        publishYear: formData.get('publishYear') ? parseInt(formData.get('publishYear')) : null,
        isAvailable: true
    };

    try {
        // Upload image if selected
        const imageInput = document.getElementById('bookImageInput');
        if (imageInput.files.length > 0) {
            const imageUrl = await uploadBookImage(imageInput.files[0]);
            data.imageUrl = imageUrl;
        }

        await api.post('/api/Books', data);
        showAlert('success', 'Kitap başarıyla eklendi!');
        const modal = document.getElementById('addBookModal');
        const bsModal = bootstrap.Modal.getInstance(modal);
        if (bsModal) bsModal.hide();
        e.target.reset();
        clearImagePreview();
        await loadBooks();
    } catch (error) {
        console.error('Kitap eklenirken hata:', error);
        showAlert('danger', 'Hata: ' + error.message);
    }
});

// Categories Management
async function loadCategories() {
    try {
        const response = await api.get('/api/Categories');
        allCategories = (response && response.data) ? response.data : [];
        displayCategories();
    } catch (error) {
        console.error('Kategoriler yüklenirken hata:', error);
        showAlert('danger', 'Kategoriler yüklenemedi: ' + error.message);
    }
}

function displayCategories() {
    const tbody = document.getElementById('categoriesTableBody');

    if (allCategories.length === 0) {
        tbody.innerHTML = '<tr><td colspan="4" class="text-center">Kategori bulunamadı</td></tr>';
        return;
    }

    tbody.innerHTML = allCategories.map(cat => `
        <tr>
            <td>${cat.id}</td>
            <td>${cat.name}</td>
            <td>
                <span class="badge ${cat.isActive ? 'bg-success' : 'bg-secondary'}">
                    ${cat.isActive ? 'Aktif' : 'Pasif'}
                </span>
            </td>
            <td>
                <button class="btn btn-sm btn-warning" onclick="toggleCategory(${cat.id}, ${!cat.isActive})">
                    <i class="bi bi-arrow-repeat"></i> ${cat.isActive ? 'Pasif Et' : 'Aktif Et'}
                </button>
            </td>
        </tr>
    `).join('');
}

async function toggleCategory(categoryId, newStatus) {
    try {
        await api.put(`/api/categories/${categoryId}`, { isActive: newStatus });
        showAlert('success', 'Kategori durumu güncellendi!');
        await loadCategories();
    } catch (error) {
        showAlert('danger', 'Hata: ' + error.message);
    }
}

// Add Category Form
document.getElementById('addCategoryForm').addEventListener('submit', async (e) => {
    e.preventDefault();

    const formData = new FormData(e.target);
    const data = {
        name: formData.get('name'),
        isActive: true
    };

    try {
        await api.post('/api/Categories', data);
        showAlert('success', 'Kategori başarıyla eklendi!');
        const modal = document.getElementById('addCategoryModal');
        const bsModal = bootstrap.Modal.getInstance(modal);
        if (bsModal) bsModal.hide();
        e.target.reset();
        await loadCategories();
    } catch (error) {
        console.error('Kategori eklenirken hata:', error);
        showAlert('danger', 'Hata: ' + error.message);
    }
});

// Helper Functions
function getStatusBadge(status) {
    const normalized = normalizeStatus(status);
    const badges = {
        'pending': '<span class="badge bg-warning"><i class="bi bi-clock"></i> Beklemede</span>',
        'borrowed': '<span class="badge bg-primary"><i class="bi bi-book"></i> Ödünçte</span>',
        'returned': '<span class="badge bg-success"><i class="bi bi-check-circle"></i> İade Edildi</span>',
        'late': '<span class="badge bg-danger"><i class="bi bi-exclamation-triangle"></i> Gecikmiş</span>',
        'cancelled': '<span class="badge bg-secondary"><i class="bi bi-x-circle"></i> İptal</span>'
    };
    return badges[normalized] || '<span class="badge bg-secondary">Bilinmiyor</span>';
}

function getAvailabilityBadge(isAvailable) {
    return isAvailable
        ? '<span class="badge bg-success"><i class="bi bi-check-circle"></i> Müsait</span>'
        : '<span class="badge bg-danger"><i class="bi bi-x-circle"></i> Ödünçte</span>';
}

function formatDate(dateString) {
    const date = new Date(dateString);
    return date.toLocaleDateString('tr-TR');
}

// Alert Helper
function showAlert(type, message) {
    const alertHtml = `
        <div class="alert alert-${type} alert-dismissible fade show" role="alert">
            ${message}
            <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
        </div>
    `;
    document.getElementById('alertContainer').innerHTML = alertHtml;
    setTimeout(() => {
        document.getElementById('alertContainer').innerHTML = '';
    }, 5000);
}

// Initialize on load
init();

