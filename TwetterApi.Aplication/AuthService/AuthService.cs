using System;
using BC = BCrypt.Net.BCrypt;
using System.Text;
using System.Security.Claims;
using System.Security.Cryptography;
using TwetterApi.Domain.Options;
using TwetterApi.Domain.Entities;
using TwetterApi.Domain.Repositories;
using TwetterApi.Domain.Models.Request;
using TwetterApi.Domain.Models.Response;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;

namespace TwetterApi.Application.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly TokenOptions _tokenOptions;
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public AuthService(IUserRepository userRepository,
            IOptions<TokenOptions> tokenOptions,
            IRefreshTokenRepository refreshTokenRepository)
        {
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _tokenOptions = tokenOptions.Value;
        }
        public AuthResponse Login(LoginRequest model, string ipAddress)
        {
            User user = _userRepository.GetUserByEmail(model.Email);

            if (user == null || !BC.Verify(model.Password, user.Password))
                return null;

            var jwtToken = GenerateJwtToken(user.Id);
            var refreshToken = GenerateRefreshToken(ipAddress, user.Id);

            _refreshTokenRepository.SaveRefreshToken(refreshToken);

            return new AuthResponse(user, jwtToken, refreshToken.Token);
        }

        public RefreshResponse RefreshToken(string token, string ipAddress)
        {
            RefreshToken refreshToken = _refreshTokenRepository.GetRefreshToken(token);

            if (refreshToken == null) return null;

            if (!refreshToken.IsActive) return null;

            var newRefreshToken = GenerateRefreshToken(ipAddress, refreshToken.UserId);
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            refreshToken.ReplacedByToken = newRefreshToken.Token;

            _refreshTokenRepository.UpdateRefreshToken(refreshToken);
            _refreshTokenRepository.SaveRefreshToken(newRefreshToken);


            var jwtToken = GenerateJwtToken(refreshToken.UserId);

            return new RefreshResponse(refreshToken.Token, jwtToken);
        }

        public AuthResponse Register(RegisterRequest model, string ipAddress)
        {
            User user = new User()
            {
                Name = model.Name,
                UserName = model.UserName,
                Email = model.Email,
                BirthDate = model.BirthDate,
                Password = BC.HashPassword(model.Password),
            };

            _userRepository.CreateUser(user);
            var createdUser = _userRepository.GetUserByEmail(user.Email);

            var jwtToken = GenerateJwtToken(createdUser.Id);
            var refreshToken = GenerateRefreshToken(ipAddress, createdUser.Id);

            _refreshTokenRepository.SaveRefreshToken(refreshToken);

            return new AuthResponse(createdUser, jwtToken, refreshToken.Token);
        }

        public bool RevokeToken(string token, string ipAddress)
        {
            RefreshToken refreshToken = _refreshTokenRepository.GetRefreshToken(token);

            if (refreshToken == null) return false;

            if (!refreshToken.IsActive) return false;

            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;

            _refreshTokenRepository.UpdateRefreshToken(refreshToken);

            return true;
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
