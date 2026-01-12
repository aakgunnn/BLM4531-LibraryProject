namespace Library.Net2.Models.DTOs.Statistics;

/// <summary>
/// Genel kütüphane istatistikleri
/// </summary>
public class LibraryStatisticsDto
{
    public int TotalBooks { get; set; }
    public int AvailableBooks { get; set; }
    public int TotalUsers { get; set; }
    public int TotalLoans { get; set; }
    public int ActiveLoans { get; set; }
    public int PendingLoans { get; set; }
    public int LateLoans { get; set; }
    public int ReturnedLoans { get; set; }
    public int TotalCategories { get; set; }
}

/// <summary>
/// Kategori bazlı kitap dağılımı
/// </summary>
public class CategoryStatisticsDto
{
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public int BookCount { get; set; }
    public int LoanCount { get; set; }
}

/// <summary>
/// Aylık ödünç istatistikleri
/// </summary>
public class MonthlyLoanStatisticsDto
{
    public int Year { get; set; }
    public int Month { get; set; }
    public string MonthName { get; set; } = string.Empty;
    public int LoanCount { get; set; }
    public int ReturnCount { get; set; }
}

/// <summary>
/// En çok ödünç alınan kitaplar
/// </summary>
public class TopBookDto
{
    public int BookId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public int LoanCount { get; set; }
}
