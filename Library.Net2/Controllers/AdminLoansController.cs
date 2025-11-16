using Library.Net2.Models.DTOs.Common;
using Library.Net2.Models.DTOs.Loans;
using Library.Net2.Models.Enums;
using Library.Net2.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.Net2.Controllers;

[ApiController]
[Route("api/Admin/Loans")]
[Authorize(Roles = "Admin")]
public class AdminLoansController : ControllerBase
{
    private readonly ILoanService _loanService;

    public AdminLoansController(ILoanService loanService)
    {
        _loanService = loanService;
    }

    /// <summary>
    /// Tüm ödünçleri listele (filtreleme ile)
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<LoanResponseDto>>>> GetAllLoans(
        [FromQuery] LoanStatus? status = null,
        [FromQuery] int? userId = null,
        [FromQuery] int? categoryId = null)
    {
        try
        {
            var loans = await _loanService.GetAllLoansAsync(status, userId, categoryId);
            
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
    /// Ödünç talebini onayla
    /// </summary>
    [HttpPut("{id}/approve")]
    public async Task<ActionResult<ApiResponse<LoanResponseDto>>> ApproveLoan(int id, [FromBody] ApproveLoanDto dto)
    {
        try
        {
            var loan = await _loanService.ApproveLoanAsync(id, dto);
            
            return Ok(new ApiResponse<LoanResponseDto>
            {
                Success = true,
                Message = "Ödünç talebi onaylandı.",
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
    /// Ödünç talebini reddet
    /// </summary>
    [HttpPut("{id}/reject")]
    public async Task<ActionResult<ApiResponse<LoanResponseDto>>> RejectLoan(int id, [FromBody] RejectLoanDto dto)
    {
        try
        {
            var loan = await _loanService.RejectLoanAsync(id, dto);
            
            return Ok(new ApiResponse<LoanResponseDto>
            {
                Success = true,
                Message = "Ödünç talebi reddedildi.",
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
    /// Gecikmiş ödünçleri listele
    /// </summary>
    [HttpGet("late")]
    public async Task<ActionResult<ApiResponse<IEnumerable<LoanResponseDto>>>> GetLateLoans()
    {
        try
        {
            var loans = await _loanService.GetLateLoansAsync();
            
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
    /// Bekleyen talepleri listele (kısayol)
    /// </summary>
    [HttpGet("pending")]
    public async Task<ActionResult<ApiResponse<IEnumerable<LoanResponseDto>>>> GetPendingLoans()
    {
        try
        {
            var loans = await _loanService.GetAllLoansAsync(LoanStatus.Pending);
            
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
}


