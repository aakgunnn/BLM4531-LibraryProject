# Visual Studio'da Projeyi Çalıştırma

## 🚀 Hızlı Başlatma

### Yöntem 1: F5 ile Debug Modu (ÖNERİLEN)

1. **Visual Studio'yu açın**
2. **Solution'ı açın:**
   - `Library.Net2.sln` dosyasını açın
   - VEYA: File > Open > Project/Solution > `Library.Net2.sln` seçin

3. **Çalıştırın:**
   - Klavyede `F5` tuşuna basın
   - VEYA üst menüden yeşil "play" butonuna tıklayın (▶ Library.Net2)
   - VEYA: Debug > Start Debugging

4. **Sonuç:**
   - Tarayıcı otomatik açılacak
   - Swagger UI görünecek: https://localhost:7252/swagger
   - Output penceresinde logları görebilirsiniz

### Yöntem 2: Ctrl+F5 ile (Debug Olmadan)

1. `Ctrl + F5` tuşlarına basın
2. VEYA: Debug > Start Without Debugging
3. Daha hızlı başlar (debugger olmadan)

---

## 📊 Çıktıları Görüntüleme

### Output Penceresi (Loglar)
1. **View > Output** menüsüne gidin (veya `Ctrl + Alt + O`)
2. Üstteki dropdown'dan "Show output from:" kısmından **Library.Net2** veya **Debug** seçin
3. Burada görecekleriniz:
   - Migration logları
   - Seed data logları
   - Hata mesajları
   - Başlatma bilgileri

### Error List (Hatalar)
1. **View > Error List** menüsüne gidin (veya `Ctrl + \, E`)
2. Build hataları burada görünür

---

## 🔍 Ne Görmeli

### Başarılı Başlatma
Output penceresinde şunları görmelisiniz:

```
info: Microsoft.EntityFrameworkCore.Migrations[20402]
      Applying migration '20251108071404_InitialCreatePostgreSQL'.
...
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:7252
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5101
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
```

### Sorun Olursa
Kırmızı hata mesajları görürseniz, tüm çıktıyı kopyalayın ve bana gönderin.

---

## 🎯 Test Sayfaları

Proje başladıktan sonra tarayıcıda açın:

### 1. Swagger UI (API Dokümantasyonu)
```
https://localhost:7252/swagger
```
- Tüm API endpoint'lerini görürsünüz
- API'yi test edebilirsiniz

### 2. Ana Sayfa
```
https://localhost:7252/
```
- Landing page
- Giriş/Kayıt linkleri

### 3. Frontend Sayfaları
```
https://localhost:7252/pages/login.html
https://localhost:7252/pages/register.html
https://localhost:7252/pages/catalog.html
https://localhost:7252/pages/admin-dashboard.html
```

---

## 🔧 Sorun Giderme

### Port Çakışması
Eğer "Address already in use" hatası alırsanız:
1. `launchSettings.json` dosyasını açın
2. Port numaralarını değiştirin
3. Veya çakışan uygulamayı kapatın

### PostgreSQL Bağlantı Hatası
Output'ta PostgreSQL hatası görürseniz:
1. PostgreSQL servisinin çalıştığından emin olun
2. Connection string'i kontrol edin (`appsettings.json`)

### Migration Hatası
"Migration failed" görürseniz:
1. Package Manager Console'u açın: **Tools > NuGet Package Manager > Package Manager Console**
2. Şu komutu çalıştırın:
   ```powershell
   Update-Database
   ```

---

## 💡 İpuçları

### Debug Modunda Breakpoint Koyma
1. Bir kod satırının soluna tıklayın (kırmızı nokta oluşur)
2. F5 ile başlatın
3. O satıra geldiğinde duracak

### Hot Reload (Canlı Yenileme)
- .NET 8'de hot reload aktif
- Kod değişiklikleriniz otomatik yansıyacak (çoğu durumda)

### Restart
- `Shift + F5` ile durdur
- `F5` ile tekrar başlat

---

## ✅ Test Hesapları

Migration sonrası kullanabileceğiniz hesaplar:

**Admin:**
- Email: `admin@library.com`
- Şifre: `Admin123!`

**Üye:**
- Email: `ahmet@example.com`
- Şifre: `Member123!`

---

## 📝 Notlar

- İlk çalıştırmada migration ve seed data yüklendiği için 10-15 saniye sürebilir
- Output penceresini mutlaka açık tutun
- HTTPS sertifika uyarısı alırsanız "Continue" / "Advanced" > "Proceed" seçin

