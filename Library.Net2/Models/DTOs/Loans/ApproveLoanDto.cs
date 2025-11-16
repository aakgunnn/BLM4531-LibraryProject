namespace Library.Net2.Models.DTOs.Loans;

public class ApproveLoanDto
{
    public int DurationDays { get; set; } = 15; // Default 15 gün
    public string? AdminNote { get; set; }
}
