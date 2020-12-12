using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwetterApi.Models.Response;
using TwetterApi.Models.Request;
using TwetterApi.Entities;

namespace TwetterApi.Services
{
    public interface IAuthService
    {
        AuthResponse Login(LoginRequest model,string ipAddress);
        AuthResponse Register(RegisterRequest model,string ipAddress);
        RefreshResponse RefreshToken(string token, string ipAddress);
        bool RevokeToken(string token, string ipAddress);
        IEnumerable<RefreshToken> GetUserRefreshTokens(int userId);
    }
}
