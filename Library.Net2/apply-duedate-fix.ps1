# PowerShell script to fix DueDate column in PostgreSQL
# This makes the DueDate column nullable to support Pending loan status

$connectionString = "Host=localhost;Port=5432;Database=LibraryDb;Username=postgres;Password=1806"

Add-Type -Path "C:\Users\90506\.nuget\packages\npgsql\8.0.5\lib\net8.0\Npgsql.dll"

try {
    $connection = New-Object Npgsql.NpgsqlConnection($connectionString)
    $connection.Open()
    
    $command = $connection.CreateCommand()
    $command.CommandText = 'ALTER TABLE "Loans" ALTER COLUMN "DueDate" DROP NOT NULL;'
    
    $result = $command.ExecuteNonQuery()
    
    Write-Host "✅ DueDate kolonu başarıyla nullable yapıldı!" -ForegroundColor Green
    Write-Host "Artık Pending durumundaki ödünç talepleri oluşturulabilir." -ForegroundColor Green
    
    $connection.Close()
}
catch {
    Write-Host "❌ Hata: $_" -ForegroundColor Red
    Write-Host ""
    Write-Host "Alternatif olarak, PostgreSQL client'ınızda şu komutu çalıştırın:" -ForegroundColor Yellow
    Write-Host 'ALTER TABLE "Loans" ALTER COLUMN "DueDate" DROP NOT NULL;' -ForegroundColor Cyan
}
