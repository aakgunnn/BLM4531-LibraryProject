# Connection String Güncelleme Rehberi

## Adım 1: PostgreSQL Şifrenizi Öğrenin

PostgreSQL kurulumu sırasında belirlediğiniz şifreyi hatırlayın. Eğer hatırlamıyorsanız:

**Varsayılan:** Kurulum sırasında belirlediğiniz şifre (genellikle `postgres`)

## Adım 2: Connection String'i Güncelleyin

### Dosya 1: `Library.Net2/appsettings.json`

Bu dosyayı açın ve şu satırı bulun:
```json
"DefaultConnection": "Host=localhost;Port=5432;Database=LibraryDb;Username=postgres;Password=postgres"
```

`Password=postgres` kısmındaki `postgres` kelimesini kendi PostgreSQL şifrenizle değiştirin:
```json
"DefaultConnection": "Host=localhost;Port=5432;Database=LibraryDb;Username=postgres;Password=KENDI_SIFRENIZ"
```

### Dosya 2: `Library.Net2/appsettings.Development.json`

Aynı değişikliği bu dosyada da yapın.

## Adım 3: PostgreSQL Servisini Başlatın

**Windows Services ile:**
1. `Win + R` tuşlarına basın
2. `services.msc` yazın ve Enter'a basın
3. "PostgreSQL" servisini bulun
4. Durumu "Stopped" ise, sağ tıklayıp "Start" seçin

**PowerShell ile:**
```powershell
# Servis adını bulun
Get-Service | Where-Object {$_.DisplayName -like "*PostgreSQL*"}

# Servisi başlatın (yukarıdaki komuttan öğrendiğiniz adı kullanın)
Start-Service -Name "postgresql-x64-16"
```

## Adım 4: Projeyi Çalıştırın

```bash
cd Library.Net2
dotnet run
```

## Hata Durumunda

### "Connection refused" veya "Unable to connect" hatası:
- PostgreSQL servisinin çalıştığından emin olun
- Port 5432'nin açık olduğunu kontrol edin

### "Authentication failed" hatası:
- Şifrenin doğru olduğundan emin olun
- Kullanıcı adının `postgres` olduğundan emin olun

### "Database does not exist" hatası:
- Migration otomatik oluşturur, ancak manuel oluşturmak isterseniz:
  ```sql
  CREATE DATABASE "LibraryDb";
  ```

