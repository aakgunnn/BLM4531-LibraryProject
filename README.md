# 📚 Library.Net2 - Kütüphane Yönetim Sistemi

ASP.NET Core 8 Web API ve PostgreSQL ile geliştirilmiş modern kütüphane yönetim sistemi.

---

## 🚀 Özellikler

### ✅ Kimlik Doğrulama ve Yetkilendirme
- ✅ Kullanıcı kayıt ve giriş sistemi
- ✅ JWT Bearer Token tabanlı kimlik doğrulama
- ✅ Role-Based Authorization (Admin / Member)
- ✅ Güvenli password hashing (BCrypt)

### ✅ Kitap Yönetimi
- ✅ Kitap listeleme (tüm kullanıcılar)
- ✅ Kitap arama ve filtreleme (başlık, yazar, kategori)
- ✅ Kitap ekleme/düzenleme/silme (Admin)
- ✅ Kitap müsaitlik durumu yönetimi (Admin)
- ✅ Kitap kapak resmi yükleme (Admin) - **Backend hazır, frontend test edilmedi**
- ✅ ISBN, yayın yılı, kategori bilgileri

### ✅ Kategori Yönetimi
- ✅ Kategori listeleme
- ✅ Kategori ekleme/düzenleme (Admin)
- ✅ Kategori aktif/pasif durumu (Admin)

### ✅ Ödünç Alma ve İade Sistemi
- ✅ Ödünç alma talebi oluşturma (Member)
- ✅ Ödünç talep onaylama/reddetme (Admin)
- ✅ Kitap iade etme (Member)
- ✅ Otomatik iade tarihi hesaplama (14 gün)
- ✅ Geciken ödünç alma kayıtları takibi
- ✅ Ödünç durumu: Bekliyor, Onaylandı, Reddedildi, İade Edildi

### ✅ Admin Dashboard
- ✅ Kitap, kategori ve ödünç istatistikleri
- ✅ Bekleyen ödünç talepleri yönetimi
- ✅ Aktif ödünç kayıtları görüntüleme
- ✅ Tüm ödünç kayıtlarını listeleme
- ✅ Geciken ödünç kayıtları raporu

### ✅ Kullanıcı Arayüzü
- ✅ Modern ve responsive Bootstrap 5 tasarım
- ✅ Dinamik navbar (login durumuna göre)
- ✅ Admin ve Member için farklı UI deneyimi
- ✅ Kitap katalog sayfası (arama ve filtreleme)
- ✅ Kullanıcı ödünç geçmişi sayfası
- ✅ Admin dashboard ve yönetim panelleri

---

## 🛠️ Teknolojiler

### Backend
- **Framework:** ASP.NET Core 8 Web API
- **Database:** PostgreSQL
- **ORM:** Entity Framework Core 8
- **Authentication:** JWT Bearer Token
- **Password Hashing:** BCrypt.Net
- **Architecture:** Repository Pattern + Unit of Work

### Frontend
- **HTML5, CSS3, JavaScript (ES6+)**
- **Bootstrap 5.3**
- **Bootstrap Icons**
- **Fetch API** (RESTful API iletişimi)

---

## 📁 Proje Yapısı

```
Library.Net2/
│
├── Controllers/               # API Endpoint'leri
│   ├── AuthController.cs      # Login/Register
│   ├── BooksController.cs     # Kitap CRUD + Resim yükleme
│   ├── CategoriesController.cs
│   ├── LoansController.cs     # Kullanıcı ödünç işlemleri
│   └── AdminLoansController.cs # Admin ödünç yönetimi
│
├── Data/
│   ├── AppDbContext.cs        # Database Context
│   ├── DbSeeder.cs            # Seed data
│   └── Configurations/        # Entity configurations
│
├── Models/
│   ├── Domain/                # Entity models
│   │   ├── User.cs
│   │   ├── Book.cs
│   │   ├── Category.cs
│   │   └── Loan.cs
│   │
│   └── DTOs/                  # Data Transfer Objects
│       ├── Auth/
│       ├── Books/
│       ├── Categories/
│       └── Loans/
│
├── Repositories/              # Repository Pattern
│   ├── IRepository.cs
│   ├── Repository.cs
│   ├── IUnitOfWork.cs
│   └── UnitOfWork.cs
│
├── Services/                  # Business Logic
│   ├── IJwtService.cs & JwtService.cs
│   ├── IAuthService.cs & AuthService.cs
│   ├── IBookService.cs & BookService.cs
│   ├── ICategoryService.cs & CategoryService.cs
│   └── ILoanService.cs & LoanService.cs
│
├── Migrations/                # EF Core Migrations
│
└── wwwroot/                   # Frontend (Static Files)
    ├── index.html             # Ana sayfa
    ├── pages/
    │   ├── login.html
    │   ├── register.html
    │   ├── catalog.html       # Kitap kataloğu
    │   ├── my-loans.html      # Kullanıcı ödünç geçmişi
    │   └── admin-dashboard.html
    ├── css/
    │   └── style.css
    ├── js/
    │   ├── api.js             # API client
    │   ├── auth.js            # Auth utilities
    │   └── admin.js           # Admin dashboard logic
    └── images/
        └── books/             # Kitap kapak görselleri
```

---

## 🗄️ Database Schema

### Users
- `Id` (PK)
- `Email` (Unique)
- `FullName`
- `PasswordHash`
- `Role` (Admin / Member)
- `CreatedAt`, `UpdatedAt`

### Books
- `Id` (PK)
- `Title`
- `Author`
- `CategoryId` (FK)
- `ISBN`
- `PublishYear`
- `ImageUrl` *(yeni eklendi)*
- `IsAvailable`
- `CreatedAt`, `UpdatedAt`

### Categories
- `Id` (PK)
- `Name`
- `IsActive`
- `CreatedAt`, `UpdatedAt`

### Loans
- `Id` (PK)
- `BookId` (FK)
- `UserId` (FK)
- `LoanDate`
- `DueDate` (Nullable)
- `ReturnDate` (Nullable)
- `Status` (Enum: Pending, Approved, Rejected, Returned)
- `AdminNote`
- `CreatedAt`, `UpdatedAt`

---

## 🔧 Kurulum ve Çalıştırma

### Gereksinimler
- .NET 8 SDK
- PostgreSQL
- Visual Studio 2022 veya VS Code

### Adımlar

1. **Projeyi klonlayın**
```bash
git clone <repo-url>
cd Library.Net2
```

2. **Veritabanı bağlantı ayarları** (`appsettings.json`)
```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Database=LibraryDb;Username=postgres;Password=your_password"
}
```

3. **Migration'ları uygulayın**
```bash
cd Library.Net2
dotnet ef database update
```

4. **Uygulamayı çalıştırın**
```bash
dotnet run
```

5. **Tarayıcıda açın**
- Frontend: `http://localhost:5000`
- Swagger: `http://localhost:5000/swagger`

---

## 👤 Seed Data (Varsayılan Kullanıcılar)

Uygulama ilk çalıştırıldığında otomatik olarak oluşturulur:

### Admin
- **Email:** `admin@library.com`
- **Şifre:** `Admin123!`
- **Yetki:** Tüm yönetim işlemleri

### Member (Test Kullanıcısı)
- **Email:** `ahmet@test.com`
- **Şifre:** `Test123!`
- **Yetki:** Kitap görüntüleme ve ödünç alma

### Örnek Veriler
- 5 Kategori (Roman, Bilim Kurgu, Tarih, Psikoloji, Felsefe)
- 10 Kitap (her kategoriden örnek kitaplar)

---

## 📡 API Endpoints

### Authentication (`/api/Auth`)
- `POST /register` - Yeni kullanıcı kaydı
- `POST /login` - Giriş yapma (JWT token döner)

### Books (`/api/Books`)
- `GET /` - Tüm kitapları listele
- `GET /{id}` - ID'ye göre kitap getir
- `GET /search?q=...&categoryId=...` - Kitap ara
- `POST /` - Yeni kitap ekle *(Admin)*
- `PUT /{id}` - Kitap güncelle *(Admin)*
- `DELETE /{id}` - Kitap sil *(Admin)*
- `POST /upload-image` - Kitap kapağı yükle *(Admin)* 🆕

### Categories (`/api/Categories`)
- `GET /` - Tüm kategorileri listele
- `POST /` - Yeni kategori ekle *(Admin)*
- `PUT /{id}` - Kategori güncelle *(Admin)*

### Loans - User (`/api/Loans`)
- `POST /` - Ödünç alma talebi oluştur *(Member)*
- `GET /my-loans` - Kendi ödünç kayıtlarım
- `PUT /{id}/return` - Kitap iade et *(Member)*

### Loans - Admin (`/api/Admin/Loans`)
- `GET /` - Tüm ödünç kayıtları *(Admin)*
- `GET /late` - Geciken ödünç kayıtları *(Admin)*
- `PUT /{id}/approve` - Ödünç talebini onayla *(Admin)*
- `PUT /{id}/reject` - Ödünç talebini reddet *(Admin)*

---

## 🔐 Authentication Flow

1. Kullanıcı `/login` veya `/register` ile giriş yapar
2. Backend JWT token oluşturur ve döner
3. Frontend token'ı `localStorage`'da saklar
4. Sonraki isteklerde `Authorization: Bearer <token>` header'ı ile gönderilir
5. Backend token'ı doğrular ve kullanıcı bilgilerini çıkarır

---

## 🎨 Frontend Özellikleri

### Dinamik UI
- Login durumuna göre navbar değişir
- Admin ve Member için farklı kitap kartları
- Admin: Düzenle, Sil, Durum Değiştir butonları
- Member: Sadece Ödünç Al butonu

### Sayfa Koruması
- Giriş yapmadan korumalı sayfalara erişilemez
- Admin sayfalarına sadece Admin erişebilir
- Otomatik yönlendirme

### Responsive Tasarım
- Mobil, tablet ve masaüstü uyumlu
- Bootstrap 5 grid sistemi
- Modern ve kullanıcı dostu arayüz

---

## 🐛 Bilinen Sorunlar ve Geçmiş Hatalar

### Çözülen Hatalar:
1. ✅ Static files (wwwroot) 404 hatası → `WebRootPath` ve `FileProvider` yapılandırması
2. ✅ PostgreSQL DateTime timezone hatası → `Npgsql.EnableLegacyTimestampBehavior`
3. ✅ Frontend `isAdmin()` false döndürme → UserDto.Role string'e çevrildi
4. ✅ Admin UI özelleşmesi → Admin ve Member için ayrı render logic
5. ✅ Migration duplicate table hatası → Migration dosyası temizlendi
6. ✅ `DueDate` null hatası → Loan.DueDate nullable yapıldı
7. ✅ JSON parse error (204 No Content) → api.js'de response kontrolü

### Test Edilmemiş:
- ⚠️ Kitap kapak resmi yükleme (backend hazır, frontend test edilmedi)

---

## 📊 Proje Durumu

### ✅ Tamamlanan Özellikler (Yaklaşık %80-85)
- Authentication & Authorization
- Books & Categories Management
- Loan & Return System
- Admin Dashboard
- Frontend UI
- Database Design

### 🚧 Yarım Kalan / Test Edilmemiş
- Kitap kapak resmi yükleme (kod yazıldı, test edilmedi)
- Email bildirimleri (planlanmadı)
- Detaylı raporlama (planlanmadı)

### ❌ Yapılmadı
- Unit/Integration Tests
- API Documentation (Swagger'da mevcut)
- Docker deployment
- CI/CD Pipeline

---

## 🤝 Katkıda Bulunma

Proje eğitim amaçlı geliştirilmiştir. Katkıda bulunmak için:
1. Fork yapın
2. Feature branch oluşturun (`git checkout -b feature/amazing-feature`)
3. Commit yapın (`git commit -m 'Add amazing feature'`)
4. Push yapın (`git push origin feature/amazing-feature`)
5. Pull Request açın

---

## 📝 Notlar

- Proje, ASP.NET Core Web API ve modern frontend teknolojilerini öğrenmek için geliştirilmiştir
- Production ortamı için ek güvenlik önlemleri (rate limiting, CORS, HTTPS, etc.) alınmalıdır
- Şifreler BCrypt ile hashlenmiş olarak saklanır
- JWT token süresi 7 gün olarak ayarlanmıştır
- Ödünç alma süresi 14 gün olarak belirlenmiştir

---

## 📧 İletişim

Sorularınız için: `your-email@example.com`

---

## 📄 Lisans

Bu proje eğitim amaçlıdır ve açık kaynak olarak paylaşılmıştır.

---

**Son Güncelleme:** 15 Kasım 2024  
**Versiyon:** 1.0  
**Geliştirici:** Library.Net2 Team
