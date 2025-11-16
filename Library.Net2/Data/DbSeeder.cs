using Library.Net2.Models.Domain;
using Library.Net2.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace Library.Net2.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(LibraryDbContext context)
    {
        // Check if database is already seeded
        if (await context.Users.AnyAsync() || await context.Categories.AnyAsync())
        {
            return;
        }

        // Seed Categories
        var categories = new List<Category>
        {
            new() { Name = "Roman", IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Name = "Bilim Kurgu", IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Name = "Tarih", IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Name = "Biyografi", IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Name = "Teknoloji", IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Name = "Felsefe", IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Name = "Psikoloji", IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
        };
        await context.Categories.AddRangeAsync(categories);
        await context.SaveChangesAsync();

        // Seed Admin User
        // Password: Admin123!
        var adminUser = new User
        {
            FullName = "Admin Kullanıcı",
            Email = "admin@library.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
            Role = UserRole.Admin,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        await context.Users.AddAsync(adminUser);

        // Seed Sample Member User
        // Password: Member123!
        var memberUser = new User
        {
            FullName = "Ahmet Yılmaz",
            Email = "ahmet@example.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Member123!"),
            Role = UserRole.Member,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        await context.Users.AddAsync(memberUser);
        await context.SaveChangesAsync();

        // Seed Books
        var books = new List<Book>
        {
            // Roman
            new() { Title = "Suç ve Ceza", Author = "Fyodor Dostoyevski", CategoryId = 1, ISBN = "9780140449136", PublishYear = 1866, IsAvailable = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Title = "Savaş ve Barış", Author = "Lev Tolstoy", CategoryId = 1, ISBN = "9780140447934", PublishYear = 1869, IsAvailable = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Title = "Kürk Mantolu Madonna", Author = "Sabahattin Ali", CategoryId = 1, ISBN = "9789750718533", PublishYear = 1943, IsAvailable = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            
            // Bilim Kurgu
            new() { Title = "Dune", Author = "Frank Herbert", CategoryId = 2, ISBN = "9780441013593", PublishYear = 1965, IsAvailable = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Title = "Foundation", Author = "Isaac Asimov", CategoryId = 2, ISBN = "9780553293357", PublishYear = 1951, IsAvailable = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Title = "1984", Author = "George Orwell", CategoryId = 2, ISBN = "9780451524935", PublishYear = 1949, IsAvailable = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            
            // Tarih
            new() { Title = "Sapiens", Author = "Yuval Noah Harari", CategoryId = 3, ISBN = "9780062316097", PublishYear = 2011, IsAvailable = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Title = "Nutuk", Author = "Mustafa Kemal Atatürk", CategoryId = 3, ISBN = "9786051060279", PublishYear = 1927, IsAvailable = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            
            // Biyografi
            new() { Title = "Steve Jobs", Author = "Walter Isaacson", CategoryId = 4, ISBN = "9781451648539", PublishYear = 2011, IsAvailable = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Title = "Elon Musk", Author = "Ashlee Vance", CategoryId = 4, ISBN = "9780062301239", PublishYear = 2015, IsAvailable = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            
            // Teknoloji
            new() { Title = "Clean Code", Author = "Robert C. Martin", CategoryId = 5, ISBN = "9780132350884", PublishYear = 2008, IsAvailable = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Title = "The Pragmatic Programmer", Author = "Andrew Hunt", CategoryId = 5, ISBN = "9780135957059", PublishYear = 2019, IsAvailable = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            
            // Felsefe
            new() { Title = "Sofinin Dünyası", Author = "Jostein Gaarder", CategoryId = 6, ISBN = "9789750718540", PublishYear = 1991, IsAvailable = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            
            // Psikoloji
            new() { Title = "İnsan Arayışı", Author = "Viktor Frankl", CategoryId = 7, ISBN = "9789750719355", PublishYear = 1946, IsAvailable = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Title = "Blink", Author = "Malcolm Gladwell", CategoryId = 7, ISBN = "9780316010665", PublishYear = 2005, IsAvailable = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
        };
        await context.Books.AddRangeAsync(books);
        await context.SaveChangesAsync();
    }
}

