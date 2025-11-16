using Library.Net2.Models.Enums;

namespace Library.Net2.Models.DTOs.Loans;

public class LoanDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public int BookId { get; set; }
    public string BookTitle { get; set; } = string.Empty;
    public string BookAuthor { get; set; } = string.Empty;
    public DateTime LoanDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public LoanStatus Status { get; set; }
    public string? AdminNote { get; set; }
    public bool IsLate => !ReturnDate.HasValue && DateTime.UtcNow > DueDate;
}

