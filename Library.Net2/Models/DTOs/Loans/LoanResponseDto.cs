using Library.Net2.Models.Enums;

namespace Library.Net2.Models.DTOs.Loans;

public class LoanResponseDto
{
    public int Id { get; set; }
    
    // Book Info
    public int BookId { get; set; }
    public string BookTitle { get; set; } = string.Empty;
    public string BookAuthor { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    
    // User Info
    public int UserId { get; set; }
    public string UserFullName { get; set; } = string.Empty;
    public string UserEmail { get; set; } = string.Empty;
    
    // Loan Info
    public DateTime LoanDate { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public LoanStatus Status { get; set; }
    public string? AdminNote { get; set; }
    
    // Computed
    public bool IsLate => DueDate.HasValue && DueDate.Value < DateTime.UtcNow && Status == LoanStatus.Borrowed;
    public int? DaysRemaining => DueDate.HasValue ? (int)(DueDate.Value - DateTime.UtcNow).TotalDays : null;
}


