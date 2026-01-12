using Library.Net2.Data;
using Library.Net2.Models.DTOs.Statistics;
using Library.Net2.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Library.Net2.Services;

public class StatisticsService : IStatisticsService
{
    private readonly LibraryDbContext _context;

    public StatisticsService(LibraryDbContext context)
    {
        _context = context;
    }

    public async Task<LibraryStatisticsDto> GetLibraryStatisticsAsync()
    {
        var totalBooks = await _context.Books.CountAsync();
        var availableBooks = await _context.Books.CountAsync(b => b.IsAvailable);
        var totalUsers = await _context.Users.CountAsync(u => u.Role == UserRole.Member);
        var totalLoans = await _context.Loans.CountAsync();
        var activeLoans = await _context.Loans.CountAsync(l => l.Status == LoanStatus.Borrowed);
        var pendingLoans = await _context.Loans.CountAsync(l => l.Status == LoanStatus.Pending);
        var lateLoans = await _context.Loans.CountAsync(l => l.Status == LoanStatus.Late || 
            (l.Status == LoanStatus.Borrowed && l.DueDate < DateTime.UtcNow));
        var returnedLoans = await _context.Loans.CountAsync(l => l.Status == LoanStatus.Returned);
        var totalCategories = await _context.Categories.CountAsync(c => c.IsActive);

        return new LibraryStatisticsDto
        {
            TotalBooks = totalBooks,
            AvailableBooks = availableBooks,
            TotalUsers = totalUsers,
            TotalLoans = totalLoans,
            ActiveLoans = activeLoans,
            PendingLoans = pendingLoans,
            LateLoans = lateLoans,
            ReturnedLoans = returnedLoans,
            TotalCategories = totalCategories
        };
    }

    public async Task<IEnumerable<CategoryStatisticsDto>> GetCategoryStatisticsAsync()
    {
        var categories = await _context.Categories.Where(c => c.IsActive).ToListAsync();
        var books = await _context.Books.ToListAsync();
        var loans = await _context.Loans.Include(l => l.Book).ToListAsync();

        var result = categories.Select(c => new CategoryStatisticsDto
        {
            CategoryId = c.Id,
            CategoryName = c.Name,
            BookCount = books.Count(b => b.CategoryId == c.Id),
            LoanCount = loans.Count(l => l.Book.CategoryId == c.Id)
        }).ToList();

        return result;
    }

    public async Task<IEnumerable<MonthlyLoanStatisticsDto>> GetMonthlyLoanStatisticsAsync(int months = 6)
    {
        var startDate = DateTime.UtcNow.AddMonths(-months + 1);
        startDate = new DateTime(startDate.Year, startDate.Month, 1);

        var loans = await _context.Loans
            .Where(l => l.LoanDate >= startDate)
            .ToListAsync();

        var result = new List<MonthlyLoanStatisticsDto>();
        var culture = new CultureInfo("tr-TR");

        for (int i = 0; i < months; i++)
        {
            var date = DateTime.UtcNow.AddMonths(-months + 1 + i);
            var year = date.Year;
            var month = date.Month;

            var loanCount = loans.Count(l => l.LoanDate.Year == year && l.LoanDate.Month == month);
            var returnCount = loans.Count(l => l.ReturnDate?.Year == year && l.ReturnDate?.Month == month);

            result.Add(new MonthlyLoanStatisticsDto
            {
                Year = year,
                Month = month,
                MonthName = culture.DateTimeFormat.GetMonthName(month),
                LoanCount = loanCount,
                ReturnCount = returnCount
            });
        }

        return result;
    }

    public async Task<IEnumerable<TopBookDto>> GetTopBooksAsync(int count = 5)
    {
        var books = await _context.Books.Include(b => b.Category).ToListAsync();
        var loans = await _context.Loans.ToListAsync();

        var topBooks = books
            .Select(b => new TopBookDto
            {
                BookId = b.Id,
                Title = b.Title,
                Author = b.Author,
                CategoryName = b.Category?.Name ?? "Uncategorized",
                ImageUrl = b.ImageUrl,
                LoanCount = loans.Count(l => l.BookId == b.Id)
            })
            .OrderByDescending(x => x.LoanCount)
            .Take(count)
            .ToList();

        return topBooks;
    }
}
