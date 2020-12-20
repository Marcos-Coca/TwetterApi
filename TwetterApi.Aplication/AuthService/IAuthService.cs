using TwetterApi.Domain.Models.Request;
using TwetterApi.Domain.Models.Response;

namespace TwetterApi.Application.AuthService
{
    public interface IAuthService
    {
        AuthResponse Login(LoginRequest model,string ipAddress);
        AuthResponse Register(RegisterRequest model,string ipAddress);
        RefreshResponse RefreshToken(string token, string ipAddress);
        bool RevokeToken(string token, string ipAddress);
    }
}
