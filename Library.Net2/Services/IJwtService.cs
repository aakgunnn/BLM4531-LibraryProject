using Library.Net2.Models.Domain;

namespace Library.Net2.Services;

public interface IJwtService
{
    string GenerateToken(User user);
    int? GetUserIdFromToken(string token);
}

