using System.ComponentModel.DataAnnotations;

namespace Library.Net2.Models.DTOs.Loans;

public class CreateLoanRequestDto
{
    [Required(ErrorMessage = "Kitap seçimi zorunludur")]
    public int BookId { get; set; }
}

