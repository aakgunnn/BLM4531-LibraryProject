# Kütüphane Yönetim Sistemi 📚

Modern ve kullanıcı dostu bir kütüphane yönetim sistemi. ASP.NET Core 8, PostgreSQL ve vanilla JavaScript ile geliştirilmiştir.

## 📋 Proje Hakkında

Bu proje, kütüphane operasyonlarının dijital ortamda yönetilmesini sağlar. Üyeler kitap katalogunu görüntüleyip arama yapar, uygun kitapları ödünç alır ve iade eder. Görevliler (admin) kitap ve kategori yönetimi, stok güncelleme, gecikme ve raporlama işlemlerini yürütür.

## ✅ Tamamlanan Özellikler (Bu Hafta)

### 🗄️ Database Katmanı
- ✅ PostgreSQL veritabanı entegrasyonu
- ✅ Entity Framework Core 8 ORM
- ✅ Domain Entity'leri (User, Book, Category, Loan)
- ✅ Entity Configurations ve ilişkiler
- ✅ Migration sistemi
- ✅ Seed Data (test kategorileri, admin kullanıcı, örnek kitaplar)

### 🎨 Frontend Katmanı
- ✅ Modern ve responsive tasarım (Bootstrap 5)
- ✅ Kullanıcı sayfaları:
  - Ana sayfa (landing page)
  - Giriş & Kayıt sayfaları
  - Kitap kataloğu (arama ve filtreleme)
  - Ödünçlerim sayfası
- ✅ Admin sayfaları:
  - Dashboard (istatistikler)
  - Ödünç yönetimi (onaylama/reddetme)
  - Kitap yönetimi (ekleme/güncelleme)
  - Kategori yönetimi
- ✅ JavaScript API Client servisi
- ✅ Auth utility fonksiyonları

## 🔜 Gelecek Haftalarda Yapılacaklar

### Backend Geliştirme
- [ ] Repository Pattern implementasyonu
- [ ] Application Services katmanı
- [ ] JWT Authentication & Authorization
- [ ] REST API Controllers
- [ ] DTOs ve validation
- [ ] Global exception handling
- [ ] İş kuralları (ödünç uygunluğu, gecikme hesabı)

### Test & Deployment
- [ ] API testleri
- [ ] Docker containerization
- [ ] Production deployment

## 🛠️ Teknoloji Stack'i

- **Backend:** ASP.NET Core 8 Web API
- **Database:** PostgreSQL 16
- **ORM:** Entity Framework Core 8
- **Frontend:** HTML5, CSS3, JavaScript (ES6+)
- **UI Framework:** Bootstrap 5
- **Authentication:** JWT Bearer Token (gelecek sprint)

## 📦 Kurulum

### Gereksinimler
- .NET 8 SDK
- PostgreSQL 16+
- Visual Studio 2022 veya VS Code

### Adımlar

1. **Repository'yi klonlayın:**
```bash
git clone <repository-url>
cd Library.Net2
```

2. **PostgreSQL veritabanını hazırlayın:**
```sql
CREATE DATABASE LibraryDb;
```

3. **Connection string'i güncelleyin:**
`appsettings.json` dosyasında PostgreSQL bilgilerinizi düzenleyin:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=LibraryDb;Username=postgres;Password=your_password"
  }
}
```

4. **Projeyi çalıştırın:**
```bash
cd Library.Net2
dotnet run
```

5. **Tarayıcıda açın:**
```
https://localhost:5001
```

## 👥 Demo Hesaplar

### Admin Hesabı
- **Email:** admin@library.com
- **Şifre:** Admin123!

### Üye Hesabı
- **Email:** ahmet@example.com
- **Şifre:** Member123!

## 📊 Database Şeması

### Users
- id, full_name, email (unique), password_hash, role, created_at, updated_at

### Categories
- id, name, is_active, created_at, updated_at

### Books
- id, title, author, category_id, isbn, publish_year, is_available, created_at, updated_at

### Loans
- id, user_id, book_id, loan_date, due_date, return_date, status, admin_note, created_at, updated_at

## 🎯 Roller ve Yetkiler

### Member (Üye)
- Kitapları listeleme ve arama
- Ödünç talebi oluşturma
- Aktif ödünçleri görüntüleme
- İade talebi oluşturma

### Admin (Görevli)
- Tüm üye yetkileri
- Kitap ve kategori yönetimi
- Ödünç taleplerini onaylama/reddetme
- Gecikmeleri izleme
- Raporlama

## 📱 Sayfa Yapısı

```
/                       → Ana sayfa (landing)
/pages/login.html       → Giriş sayfası
/pages/register.html    → Kayıt sayfası
/pages/catalog.html     → Kitap kataloğu
/pages/my-loans.html    → Ödünçlerim
/pages/admin-dashboard.html → Admin paneli
```

## 🚀 API Endpoints (Planlanmış)

### Authentication
- `POST /api/auth/register` - Yeni üye kaydı
- `POST /api/auth/login` - Giriş yapma
- `GET /api/auth/me` - Kullanıcı bilgileri

### Books & Categories
- `GET /api/books` - Kitap listesi
- `POST /api/books` - Yeni kitap ekleme (Admin)
- `PUT /api/books/{id}` - Kitap güncelleme (Admin)
- `DELETE /api/books/{id}` - Kitap silme (Admin)
- `GET /api/categories` - Kategori listesi
- `POST /api/categories` - Kategori ekleme (Admin)

### Loans
- `POST /api/loans` - Ödünç talebi
- `GET /api/loans-user/{userId}` - Kullanıcı ödünçleri
- `PUT /api/loans/{id}/return` - İade talebi
- `GET /api/admin/loans` - Tüm ödünçler (Admin)
- `PUT /api/admin/loans/{id}/approve` - Ödünç onaylama (Admin)
- `PUT /api/admin/loans/{id}/reject` - Ödünç reddetme (Admin)

## 🎨 Tasarım Özellikleri

- Modern gradient renk paleti
- Smooth animasyonlar ve transitions
- Responsive design (mobil uyumlu)
- Kullanıcı dostu form validasyonları
- Loading state'leri
- Toast/Alert bildirimleri
- Icon kütüphanesi (Bootstrap Icons)

## 📝 Notlar

- Migration'lar otomatik olarak uygulanır
- Seed data ilk çalıştırmada yüklenir
- CORS tüm origin'lere açık (development için)
- HTTPS redirect aktif

## 🤝 Katkıda Bulunma

Bu proje eğitim amaçlıdır. Önerileriniz için issue açabilirsiniz.

## 📄 Lisans

MIT License

---

**Geliştirici:** Library.Net2 Team  
**Tarih:** Kasım 2025  
**Versiyon:** 0.1.0 (Development)

