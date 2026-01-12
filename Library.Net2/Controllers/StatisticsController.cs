using Library.Net2.Models.DTOs.Common;
using Library.Net2.Models.DTOs.Statistics;
using Library.Net2.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.Net2.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class StatisticsController : ControllerBase
{
    private readonly IStatisticsService _statisticsService;

    public StatisticsController(IStatisticsService statisticsService)
    {
        _statisticsService = statisticsService;
    }

    /// <summary>
    /// Genel kütüphane istatistiklerini getir
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<LibraryStatisticsDto>>> GetLibraryStatistics()
    {
        try
        {
            var stats = await _statisticsService.GetLibraryStatisticsAsync();
            return Ok(ApiResponse<LibraryStatisticsDto>.SuccessResponse(stats));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<LibraryStatisticsDto>.ErrorResponse("Bir hata oluştu: " + ex.Message));
        }
    }

    /// <summary>
    /// Kategori bazlı istatistikleri getir
    /// </summary>
    [HttpGet("categories")]
    public async Task<ActionResult<ApiResponse<IEnumerable<CategoryStatisticsDto>>>> GetCategoryStatistics()
    {
        try
        {
            var stats = await _statisticsService.GetCategoryStatisticsAsync();
            return Ok(ApiResponse<IEnumerable<CategoryStatisticsDto>>.SuccessResponse(stats));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<IEnumerable<CategoryStatisticsDto>>.ErrorResponse("Bir hata oluştu: " + ex.Message));
        }
    }

    /// <summary>
    /// Aylık ödünç istatistiklerini getir
    /// </summary>
    [HttpGet("monthly-loans")]
    public async Task<ActionResult<ApiResponse<IEnumerable<MonthlyLoanStatisticsDto>>>> GetMonthlyLoanStatistics([FromQuery] int months = 6)
    {
        try
        {
            if (months < 1 || months > 12)
            {
                return BadRequest(ApiResponse<IEnumerable<MonthlyLoanStatisticsDto>>.ErrorResponse("Ay sayısı 1-12 arasında olmalıdır."));
            }

            var stats = await _statisticsService.GetMonthlyLoanStatisticsAsync(months);
            return Ok(ApiResponse<IEnumerable<MonthlyLoanStatisticsDto>>.SuccessResponse(stats));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<IEnumerable<MonthlyLoanStatisticsDto>>.ErrorResponse("Bir hata oluştu: " + ex.Message));
        }
    }

    /// <summary>
    /// En çok ödünç alınan kitapları getir
    /// </summary>
    [HttpGet("top-books")]
    public async Task<ActionResult<ApiResponse<IEnumerable<TopBookDto>>>> GetTopBooks([FromQuery] int count = 5)
    {
        try
        {
            if (count < 1 || count > 20)
            {
                return BadRequest(ApiResponse<IEnumerable<TopBookDto>>.ErrorResponse("Kitap sayısı 1-20 arasında olmalıdır."));
            }

            var stats = await _statisticsService.GetTopBooksAsync(count);
            return Ok(ApiResponse<IEnumerable<TopBookDto>>.SuccessResponse(stats));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<IEnumerable<TopBookDto>>.ErrorResponse("Bir hata oluştu: " + ex.Message));
        }
    }
}
