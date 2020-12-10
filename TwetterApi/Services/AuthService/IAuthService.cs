using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwetterApi.Models.Response;
using TwetterApi.Models.Request;

namespace TwetterApi.Services
{
    public interface IAuthService
    {
        AuthResponse Login(LoginRequest model,string ipAddress);
        AuthResponse Register(RegisterRequest model,string ipAddress);
        AuthResponse RefreshToken(string token, string ipAddress);
        bool RevokeToken(string token, string ipAddress);
        
        
    }
}
