using Npgsql;

var connectionString = "Host=localhost;Port=5432;Database=LibraryDb;Username=postgres;Password=1806";

try
{
    await using var connection = new NpgsqlConnection(connectionString);
    await connection.OpenAsync();
    
    await using var command = connection.CreateCommand();
    command.CommandText = @"ALTER TABLE ""Loans"" ALTER COLUMN ""DueDate"" DROP NOT NULL;";
    
    await command.ExecuteNonQueryAsync();
    
    Console.WriteLine("✅ DueDate kolonu başarıyla nullable yapıldı!");
    Console.WriteLine("Artık Pending durumundaki ödünç talepleri oluşturulabilir.");
}
catch (Exception ex)
{
    Console.WriteLine($"❌ Hata: {ex.Message}");
    Console.WriteLine();
    Console.WriteLine("Alternatif olarak, PostgreSQL client'ınızda şu komutu çalıştırın:");
    Console.WriteLine(@"ALTER TABLE ""Loans"" ALTER COLUMN ""DueDate"" DROP NOT NULL;");
}
