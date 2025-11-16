using Library.Net2.Models.DTOs.Common;
using Library.Net2.Models.DTOs.Loans;
using Library.Net2.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Library.Net2.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LoansController : ControllerBase
{
    private readonly ILoanService _loanService;

    public LoansController(ILoanService loanService)
    {
        _loanService = loanService;
    }

    /// <summary>
    /// Ödünç alma talebi oluştur (Kullanıcı - Admin hariç)
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Member")] // Sadece Member'lar ödünç talebi oluşturabilir
    public async Task<ActionResult<ApiResponse<LoanResponseDto>>> CreateLoanRequest([FromBody] CreateLoanDto dto)
    {
        try
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var userRole = User.FindFirstValue(ClaimTypes.Role);
            
            // Çift kontrol: Admin ödünç talebi oluşturamaz
            if (userRole == "Admin")
            {
                return Forbid("Admin kullanıcıları ödünç talebi oluşturamaz.");
            }
            
            var loan = await _loanService.CreateLoanRequestAsync(userId, dto);
            
            return Ok(new ApiResponse<LoanResponseDto>
            {
                Success = true,
                Message = "Ödünç talebi oluşturuldu. Admin onayı bekleniyor.",
                Data = loan
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<LoanResponseDto>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    /// <summary>
    /// Kullanıcının kendi ödünçlerini listele
    /// </summary>
    [HttpGet("my")]
    public async Task<ActionResult<ApiResponse<IEnumerable<LoanResponseDto>>>> GetMyLoans()
    {
        try
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var loans = await _loanService.GetUserLoansAsync(userId);
            
            return Ok(new ApiResponse<IEnumerable<LoanResponseDto>>
            {
                Success = true,
                Data = loans
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<IEnumerable<LoanResponseDto>>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    /// <summary>
    /// Kitap iade talebi (Kullanıcı)
    /// </summary>
    [HttpPut("{id}/return")]
    public async Task<ActionResult<ApiResponse<LoanResponseDto>>> ReturnLoan(int id)
    {
        try
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var loan = await _loanService.ReturnLoanAsync(id, userId);
            
            return Ok(new ApiResponse<LoanResponseDto>
            {
                Success = true,
                Message = "Kitap başarıyla iade edildi.",
                Data = loan
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<LoanResponseDto>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    /// <summary>
    /// Ödünç detayını getir
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<LoanResponseDto>>> GetLoanById(int id)
    {
        try
        {
            var loan = await _loanService.GetLoanByIdAsync(id);
            
            if (loan == null)
            {
                return NotFound(new ApiResponse<LoanResponseDto>
                {
                    Success = false,
                    Message = "Ödünç kaydı bulunamadı."
                });
            }

            // Kullanıcı sadece kendi ödüncünü görebilir (Admin hariç)
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var userRole = User.FindFirstValue(ClaimTypes.Role);
            
            if (userRole != "Admin" && loan.UserId != userId)
            {
                return Forbid();
            }

            return Ok(new ApiResponse<LoanResponseDto>
            {
                Success = true,
                Data = loan
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<LoanResponseDto>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }
}

