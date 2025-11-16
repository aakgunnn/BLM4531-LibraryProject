using System.ComponentModel.DataAnnotations;

namespace Library.Net2.Models.DTOs.Auth;

public class RegisterRequestDto
{
    [Required(ErrorMessage = "İsim alanı zorunludur")]
    [StringLength(100, ErrorMessage = "İsim en fazla 100 karakter olabilir")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email alanı zorunludur")]
    [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Şifre alanı zorunludur")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Şifre en az 6 karakter olmalıdır")]
    public string Password { get; set; } = string.Empty;
}

