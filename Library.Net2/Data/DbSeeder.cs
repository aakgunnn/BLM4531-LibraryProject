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
            new() { Title = "Suç ve Ceza", Author = "Fyodor Dostoyevski", CategoryId = 1, ISBN = "9780140449136", PublishYear = 1866, ImageUrl = "/images/books/crime-and-punishment.png", IsAvailable = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Title = "Savaş ve Barış", Author = "Lev Tolstoy", CategoryId = 1, ISBN = "9780140447934", PublishYear = 1869, ImageUrl = "/images/books/war-and-peace.png", IsAvailable = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Title = "Kürk Mantolu Madonna", Author = "Sabahattin Ali", CategoryId = 1, ISBN = "9789750718533", PublishYear = 1943, ImageUrl = "/images/books/kurk-mantolu-madonna.png", IsAvailable = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            
            // Bilim Kurgu
            new() { Title = "Dune", Author = "Frank Herbert", CategoryId = 2, ISBN = "9780441013593", PublishYear = 1965, ImageUrl = "/images/books/dune.jpg", IsAvailable = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Title = "Foundation", Author = "Isaac Asimov", CategoryId = 2, ISBN = "9780553293357", PublishYear = 1951, ImageUrl = "/images/books/foundation.png", IsAvailable = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Title = "1984", Author = "George Orwell", CategoryId = 2, ISBN = "9780451524935", PublishYear = 1949, ImageUrl = "/images/books/1984.jpg", IsAvailable = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            
            // Tarih
            new() { Title = "Sapiens", Author = "Yuval Noah Harari", CategoryId = 3, ISBN = "9780062316097", PublishYear = 2011, ImageUrl = "/images/books/sapiens.jpg", IsAvailable = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Title = "Nutuk", Author = "Mustafa Kemal Atatürk", CategoryId = 3, ISBN = "9786051060279", PublishYear = 1927, ImageUrl = "/images/books/nutuk.jpg", IsAvailable = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            
            // Biyografi
            new() { Title = "Steve Jobs", Author = "Walter Isaacson", CategoryId = 4, ISBN = "9781451648539", PublishYear = 2011, ImageUrl = "/images/books/steve-jobs.jpg", IsAvailable = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Title = "Elon Musk", Author = "Ashlee Vance", CategoryId = 4, ISBN = "9780062301239", PublishYear = 2015, ImageUrl = "/images/books/elon-musk.jpg", IsAvailable = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            
            // Teknoloji
            new() { Title = "Clean Code", Author = "Robert C. Martin", CategoryId = 5, ISBN = "9780132350884", PublishYear = 2008, ImageUrl = "/images/books/clean-code.jpg", IsAvailable = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Title = "The Pragmatic Programmer", Author = "Andrew Hunt", CategoryId = 5, ISBN = "9780135957059", PublishYear = 2019, ImageUrl = "/images/books/pragmatic-programmer.jpg", IsAvailable = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            
            // Felsefe
            new() { Title = "Sofinin Dünyası", Author = "Jostein Gaarder", CategoryId = 6, ISBN = "9789750718540", PublishYear = 1991, ImageUrl = "/images/books/sophies-world.jpg", IsAvailable = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            
            // Psikoloji
            new() { Title = "İnsan Arayışı", Author = "Viktor Frankl", CategoryId = 7, ISBN = "9789750719355", PublishYear = 1946, ImageUrl = "/images/books/mans-search-for-meaning.jpg", IsAvailable = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Title = "Blink", Author = "Malcolm Gladwell", CategoryId = 7, ISBN = "9780316010665", PublishYear = 2005, ImageUrl = "/images/books/blink.jpg", IsAvailable = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
        };
        await context.Books.AddRangeAsync(books);
        await context.SaveChangesAsync();
    }
    public static async Task ForceUpdateImagesAsync(LibraryDbContext context)
    {
        var books = await context.Books.ToListAsync();
        foreach (var book in books)
        {
            switch (book.Title)
            {
                case "Suç ve Ceza": book.ImageUrl = "/images/books/crime-and-punishment.png"; break;
                case "Savaş ve Barış": book.ImageUrl = "/images/books/war-and-peace.png"; break;
                case "Kürk Mantolu Madonna": book.ImageUrl = "/images/books/kurk-mantolu-madonna.png"; break;
                case "Foundation": book.ImageUrl = "/images/books/foundation.png"; break;
                case "Dune": book.ImageUrl = "/images/books/dune.jpg"; break;
                case "1984": book.ImageUrl = "/images/books/1984.jpg"; break;
                case "Sapiens": book.ImageUrl = "/images/books/sapiens.jpg"; break;
                case "Nutuk": book.ImageUrl = "/images/books/nutuk.jpg"; break;
                case "Steve Jobs": book.ImageUrl = "/images/books/steve-jobs.jpg"; break;
                case "Elon Musk": book.ImageUrl = "/images/books/elon-musk.jpg"; break;
                case "Clean Code": book.ImageUrl = "/images/books/clean-code.jpg"; break;
                case "The Pragmatic Programmer": book.ImageUrl = "/images/books/pragmatic-programmer.jpg"; break;
                case "Sofinin Dünyası": book.ImageUrl = "/images/books/sophies-world.jpg"; break;
                case "İnsan Arayışı": book.ImageUrl = "/images/books/mans-search-for-meaning.jpg"; break;
                case "Blink": book.ImageUrl = "/images/books/blink.jpg"; break;
            }
        }
        await context.SaveChangesAsync();
    }
}

