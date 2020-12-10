using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwetterApi.Entities;
using TwetterApi.Models.Request;
using TwetterApi.Models.Common;
using TwetterApi.Models.Response;
using TwetterApi.Models.Repositories;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;

namespace TwetterApi.Services
{
    public class AuthService : IAuthService
    {
        private  IUserRepository _userRepository;
        private readonly TokenOptions _tokenOptions;
        public AuthService(IUserRepository userRepository,IOptions<TokenOptions> tokenOptions)
        {
            _userRepository = userRepository;
            _tokenOptions = tokenOptions.Value;
        }
        public AuthResponse Login(LoginRequest model, string ipAddress)
        {
            User user = _userRepository.GetUserByEmail(model.Email);

            if (user == null || user.Password != model.Password)
                return null;

            var jwtToken = generateJwtToken(user);
            var refreshToken = generateRefreshToken(ipAddress,user);

            _userRepository.SaveRefreshToken(refreshToken);

            return new AuthResponse(user, jwtToken, refreshToken.Token); 
        }

        public AuthResponse RefreshToken(string token, string ipAddress)
        {
            throw new NotImplementedException();
        }

        public AuthResponse Register(RegisterRequest userRequest, string ipAddress)
        {
            throw new NotImplementedException();
        }

        public bool RevokeToken(string token, string ipAddress)
        {
            throw new NotImplementedException();
        }

        //helper methods

        private string generateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_tokenOptions.JwtTokenSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name,user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        private RefreshToken generateRefreshToken(string ipAddress,User user)
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[64];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomBytes),
                Expires = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow,
                CreatedByIp = ipAddress,
                User = user
            };
        }
    }
}
