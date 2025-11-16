# PostgreSQL Bağlantı Testi

## Durum Kontrolü

Proje PostgreSQL'e geçirildi. Test için aşağıdaki adımları izleyin:

### 1. PostgreSQL'in Çalıştığından Emin Olun

Windows'ta PostgreSQL servisinin çalışıp çalışmadığını kontrol edin:
```powershell
Get-Service -Name postgresql*
```

Eğer çalışmıyorsa başlatın:
```powershell
Start-Service -Name postgresql*
```

### 2. Connection String'i Kontrol Edin

`appsettings.json` ve `appsettings.Development.json` dosyalarında connection string'i kontrol edin:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=LibraryDb;Username=postgres;Password=postgres"
  }
}
```

**ÖNEMLİ:** Kendi PostgreSQL kullanıcı adı ve şifrenizi güncelleyin!

### 3. Veritabanını Oluşturun (Opsiyonel)

Migration otomatik olarak veritabanını oluşturur, ancak manuel olarak da oluşturabilirsiniz:

```sql
CREATE DATABASE "LibraryDb";
```

### 4. Projeyi Çalıştırın

```bash
cd Library.Net2
dotnet run
```

### 5. Hata Durumunda

Eğer PostgreSQL bağlantı hatası alırsanız:

1. **PostgreSQL'in çalıştığından emin olun**
2. **Connection string'deki bilgileri kontrol edin:**
   - Host: localhost
   - Port: 5432 (varsayılan)
   - Database: LibraryDb
   - Username: postgres (veya kendi kullanıcı adınız)
   - Password: postgres (veya kendi şifreniz)

3. **PostgreSQL'in dinlediği portu kontrol edin:**
   ```sql
   SHOW port;
   ```

4. **pg_hba.conf dosyasını kontrol edin** (bağlantı reddediliyorsa)

### 6. Başarılı Bağlantı Sonrası

Proje başladığında:
- Swagger UI: http://localhost:5101/swagger
- Ana Sayfa: http://localhost:5101/
- API: http://localhost:5101/api/...

Migration otomatik olarak uygulanacak ve seed data yüklenecek.

### Test Hesapları

Migration sonrası aşağıdaki hesaplar oluşturulacak:

**Admin:**
- Email: admin@library.com
- Şifre: Admin123!

**Üye:**
- Email: ahmet@example.com
- Şifre: Member123!

