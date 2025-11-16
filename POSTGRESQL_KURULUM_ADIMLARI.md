# PostgreSQL Kurulum Sonrası Adımlar

## 1. PostgreSQL Servisini Başlatın

Windows'ta PostgreSQL servisini başlatmak için:

**Yöntem 1: PowerShell ile**
```powershell
# Servis adını bulun
Get-Service | Where-Object {$_.DisplayName -like "*PostgreSQL*"}

# Servisi başlatın (servis adını yukarıdaki komuttan öğrenin)
Start-Service -Name "postgresql-x64-16"  # veya kurduğunuz versiyonun adı
```

**Yöntem 2: Services.msc ile**
1. `Win + R` tuşlarına basın
2. `services.msc` yazın ve Enter'a basın
3. "PostgreSQL" servisini bulun
4. Sağ tıklayıp "Start" seçin

## 2. PostgreSQL Kullanıcı Bilgilerinizi Öğrenin

PostgreSQL kurulumu sırasında bir şifre belirlemiş olmalısınız. Eğer hatırlamıyorsanız:

**Varsayılan kullanıcı:** `postgres`
**Şifre:** Kurulum sırasında belirlediğiniz şifre

## 3. Connection String'i Güncelleyin

`Library.Net2/appsettings.json` ve `Library.Net2/appsettings.Development.json` dosyalarını açın ve connection string'i güncelleyin:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=LibraryDb;Username=postgres;Password=KENDI_SIFRENIZ"
  }
}
```

**ÖNEMLİ:** `KENDI_SIFRENIZ` kısmını PostgreSQL kurulumunda belirlediğiniz şifre ile değiştirin!

## 4. Veritabanını Oluşturun (Opsiyonel)

Migration otomatik olarak veritabanını oluşturabilir, ancak manuel olarak da oluşturabilirsiniz:

**pgAdmin ile:**
1. pgAdmin'i açın
2. Servers > PostgreSQL > Databases'e sağ tıklayın
3. "Create" > "Database" seçin
4. Database name: `LibraryDb` yazın
5. "Save" butonuna tıklayın

**psql ile:**
```bash
# PostgreSQL bin klasörüne gidin (genellikle C:\Program Files\PostgreSQL\16\bin)
psql -U postgres

# PostgreSQL'e bağlandıktan sonra:
CREATE DATABASE "LibraryDb";
\q
```

**PowerShell ile:**
```powershell
# PostgreSQL bin klasörüne gidin
cd "C:\Program Files\PostgreSQL\16\bin"

# Veritabanını oluşturun
.\psql.exe -U postgres -c "CREATE DATABASE LibraryDb;"
```

## 5. Projeyi Çalıştırın

```bash
cd Library.Net2
dotnet run
```

Proje başladığında:
- Migration otomatik olarak uygulanacak
- Seed data (test verileri) yüklenecek
- Swagger UI: http://localhost:5101/swagger
- Ana Sayfa: http://localhost:5101/

## 6. Test Hesapları

Migration sonrası aşağıdaki hesaplar oluşturulacak:

**Admin:**
- Email: admin@library.com
- Şifre: Admin123!

**Üye:**
- Email: ahmet@example.com
- Şifre: Member123!

## Sorun Giderme

### "Connection refused" hatası alıyorsanız:
1. PostgreSQL servisinin çalıştığından emin olun
2. Port 5432'nin açık olduğunu kontrol edin
3. Connection string'deki bilgileri kontrol edin

### "Authentication failed" hatası alıyorsanız:
1. Kullanıcı adı ve şifrenin doğru olduğundan emin olun
2. pg_hba.conf dosyasını kontrol edin (genellikle `C:\Program Files\PostgreSQL\16\data\pg_hba.conf`)

### "Database does not exist" hatası alıyorsanız:
- Migration otomatik oluşturur, ancak manuel olarak da oluşturabilirsiniz (yukarıdaki adım 4)

