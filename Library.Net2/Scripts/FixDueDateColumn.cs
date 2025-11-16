// Bu dosyayı Program.cs'in başında bir kere çalıştırın
// using Npgsql;

/*
var connectionString = "Host=localhost;Port=5432;Database=LibraryDb;Username=postgres;Password=1806";
using var connection = new NpgsqlConnection(connectionString);
await connection.OpenAsync();

var command = connection.CreateCommand();
command.CommandText = @"ALTER TABLE ""Loans"" ALTER COLUMN ""DueDate"" DROP NOT NULL;";
await command.ExecuteNonQueryAsync();

Console.WriteLine("✅ DueDate kolonu nullable yapıldı!");
*/


