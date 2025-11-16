using Library.Net2.Models.Enums;

namespace Library.Net2.Models.DTOs.Auth;

public class UserDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty; // String olarak değiştirildi (frontend için)
}

