using Library.Net2.Models.Domain;
using Library.Net2.Models.DTOs.Books;
using Library.Net2.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Library.Net2.Services;

public class BookService : IBookService
{
    private readonly IUnitOfWork _unitOfWork;

    public BookService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<BookDto>> GetAllBooksAsync()
    {
        var books = await _unitOfWork.Books.GetAllAsync();
        
        // Include category names
        var bookDtos = new List<BookDto>();
        foreach (var book in books)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(book.CategoryId);
            bookDtos.Add(MapToDto(book, category?.Name ?? "Uncategorized"));
        }
        
        return bookDtos;
    }

    public async Task<IEnumerable<BookDto>> SearchBooksAsync(string? query, int? categoryId, string? author)
    {
        var books = await _unitOfWork.Books.GetAllAsync();
        
        var filteredBooks = books.AsQueryable();

        if (!string.IsNullOrWhiteSpace(query))
        {
            filteredBooks = filteredBooks.Where(b => 
                b.Title.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                b.Author.Contains(query, StringComparison.OrdinalIgnoreCase));
        }

        if (categoryId.HasValue)
        {
            filteredBooks = filteredBooks.Where(b => b.CategoryId == categoryId.Value);
        }

        if (!string.IsNullOrWhiteSpace(author))
        {
            filteredBooks = filteredBooks.Where(b => 
                b.Author.Contains(author, StringComparison.OrdinalIgnoreCase));
        }

        var bookDtos = new List<BookDto>();
        foreach (var book in filteredBooks)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(book.CategoryId);
            bookDtos.Add(MapToDto(book, category?.Name ?? "Uncategorized"));
        }

        return bookDtos;
    }

    public async Task<BookDto?> GetBookByIdAsync(int id)
    {
        var book = await _unitOfWork.Books.GetByIdAsync(id);
        if (book == null) return null;

        var category = await _unitOfWork.Categories.GetByIdAsync(book.CategoryId);
        return MapToDto(book, category?.Name ?? "Uncategorized");
    }

    public async Task<BookDto> CreateBookAsync(CreateBookDto dto)
    {
        // Validate category exists
        var category = await _unitOfWork.Categories.GetByIdAsync(dto.CategoryId);
        if (category == null)
        {
            throw new InvalidOperationException("Kategori bulunamadı");
        }

        var book = new Book
        {
            Title = dto.Title,
            Author = dto.Author,
            CategoryId = dto.CategoryId,
            ISBN = dto.ISBN,
            PublishYear = dto.PublishYear,
            IsAvailable = dto.IsAvailable,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Books.AddAsync(book);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(book, category.Name);
    }

    public async Task<BookDto> UpdateBookAsync(int id, UpdateBookDto dto)
    {
        var book = await _unitOfWork.Books.GetByIdAsync(id);
        if (book == null)
        {
            throw new InvalidOperationException("Kitap bulunamadı");
        }

        // Validate category exists
        var category = await _unitOfWork.Categories.GetByIdAsync(dto.CategoryId);
        if (category == null)
        {
            throw new InvalidOperationException("Kategori bulunamadı");
        }

        book.Title = dto.Title;
        book.Author = dto.Author;
        book.CategoryId = dto.CategoryId;
        book.ISBN = dto.ISBN;
        book.PublishYear = dto.PublishYear;
        book.IsAvailable = dto.IsAvailable;
        book.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Books.Update(book);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(book, category.Name);
    }

    public async Task<bool> DeleteBookAsync(int id)
    {
        var book = await _unitOfWork.Books.GetByIdAsync(id);
        if (book == null) return false;

        _unitOfWork.Books.Remove(book);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    private static BookDto MapToDto(Book book, string categoryName)
    {
        return new BookDto
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            CategoryId = book.CategoryId,
            CategoryName = categoryName,
            ISBN = book.ISBN,
            PublishYear = book.PublishYear,
            IsAvailable = book.IsAvailable
        };
    }
}

