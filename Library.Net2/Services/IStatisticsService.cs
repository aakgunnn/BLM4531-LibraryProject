using Library.Net2.Models.DTOs.Statistics;

namespace Library.Net2.Services;

public interface IStatisticsService
{
    /// <summary>
    /// Genel kütüphane istatistiklerini getirir
    /// </summary>
    Task<LibraryStatisticsDto> GetLibraryStatisticsAsync();

    /// <summary>
    /// Kategori bazlı istatistikleri getirir
    /// </summary>
    Task<IEnumerable<CategoryStatisticsDto>> GetCategoryStatisticsAsync();

    /// <summary>
    /// Son N ay için aylık ödünç istatistiklerini getirir
    /// </summary>
    Task<IEnumerable<MonthlyLoanStatisticsDto>> GetMonthlyLoanStatisticsAsync(int months = 6);

    /// <summary>
    /// En çok ödünç alınan kitapları getirir
    /// </summary>
    Task<IEnumerable<TopBookDto>> GetTopBooksAsync(int count = 5);
}
