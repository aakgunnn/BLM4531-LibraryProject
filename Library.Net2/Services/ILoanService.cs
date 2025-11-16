using Library.Net2.Models.DTOs.Loans;
using Library.Net2.Models.Enums;

namespace Library.Net2.Services;

public interface ILoanService
{
    // User Operations
    Task<LoanResponseDto> CreateLoanRequestAsync(int userId, CreateLoanDto dto);
    Task<IEnumerable<LoanResponseDto>> GetUserLoansAsync(int userId);
    Task<LoanResponseDto> ReturnLoanAsync(int loanId, int userId);
    
    // Admin Operations
    Task<IEnumerable<LoanResponseDto>> GetAllLoansAsync(LoanStatus? status = null, int? userId = null, int? categoryId = null);
    Task<LoanResponseDto> ApproveLoanAsync(int loanId, ApproveLoanDto dto);
    Task<LoanResponseDto> RejectLoanAsync(int loanId, RejectLoanDto dto);
    Task<IEnumerable<LoanResponseDto>> GetLateLoansAsync();
    Task<LoanResponseDto?> GetLoanByIdAsync(int loanId);
}


