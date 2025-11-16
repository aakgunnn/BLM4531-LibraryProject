using Library.Net2.Models.Enums;

namespace Library.Net2.Models.DTOs.Loans;

public class UpdateLoanStatusDto
{
    public LoanStatus Status { get; set; }
    public string? AdminNote { get; set; }
}




