using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwetterApi.Entities;
using TwetterApi.Models.Request;
using TwetterApi.Models.Options;
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
        private readonly IUserRepository _userRepository;
        private readonly ITokenRepository _tokenRepository;
        private readonly TokenOptions _tokenOptions;
        public AuthService(IUserRepository userRepository, IOptions<TokenOptions> tokenOptions, ITokenRepository tokenRepository)
        {
            _userRepository = userRepository;
            _tokenRepository = tokenRepository;
            _tokenOptions = tokenOptions.Value;
        }
        public AuthResponse Login(LoginRequest model, string ipAddress)
        {
            User user = _userRepository.GetUserByEmail(model.Email);

            if (user == null || user.Password != model.Password)
                return null;

            var jwtToken = GenerateJwtToken(user.Id);
            var refreshToken = GenerateRefreshToken(ipAddress, user.Id);

            _tokenRepository.SaveRefreshToken(refreshToken);

            return new AuthResponse(user, jwtToken, refreshToken.Token);
        }

        public RefreshResponse RefreshToken(string token, string ipAddress)
        {
            RefreshToken refreshToken = _tokenRepository.GetRefreshToken(token);

            if (refreshToken == null) return null;

            if (!refreshToken.IsActive) return null;

            var newRefreshToken = GenerateRefreshToken(ipAddress,refreshToken.UserId);
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            refreshToken.ReplacedByToken = newRefreshToken.Token;

            _tokenRepository.UpdateRefreshToken(refreshToken);
            _tokenRepository.SaveRefreshToken(newRefreshToken);
            

            var jwtToken = GenerateJwtToken(refreshToken.UserId);

            return new RefreshResponse(refreshToken.Token, jwtToken);
        }

        public AuthResponse Register(RegisterRequest userRequest, string ipAddress)
        {
            User user = new User()
            {
                Name = userRequest.Name,
                UserName = userRequest.UserName,
                Email = userRequest.Email,
                Password = userRequest.Password,
                BirthDate = userRequest.BirthDate,
            };

            _userRepository.CreateUser(user);
            var createdUser = _userRepository.GetUserByEmail(user.Email);

            var jwtToken = GenerateJwtToken(createdUser.Id);
            var refreshToken = GenerateRefreshToken(ipAddress, createdUser.Id);

            _tokenRepository.SaveRefreshToken(refreshToken);

            return new AuthResponse(createdUser, jwtToken, refreshToken.Token);
        }

        public bool RevokeToken(string token, string ipAddress)
        {
            RefreshToken refreshToken = _tokenRepository.GetRefreshToken(token);

            if (refreshToken == null) return false;

            if (!refreshToken.IsActive) return false;

            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;

            _tokenRepository.UpdateRefreshToken(refreshToken);

            return true;
        }

        public IEnumerable<RefreshToken> GetUserRefreshTokens(int userId)
        {
             return _tokenRepository.GetUserRefreshTokens(userId);
        }



        #region Private methods

        private string GenerateJwtToken(int userId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_tokenOptions.JwtTokenSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier,userId.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        private static RefreshToken GenerateRefreshToken(string ipAddress, int userId)
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
                UserId = userId
            };
        }
        #endregion
    }
}
