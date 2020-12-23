using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using TwetterApi.Domain.Models.Request;
using TwetterApi.Domain.Models.Response;
using TwetterApi.Application.AuthService;

namespace TwetterApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest model)
        {
            AuthResponse authResponse = _authService.Login(model,IpAddress());

            if (authResponse == null)
            {
                return BadRequest("Invalid Email or Password");
            }

            SetTokenCookie(authResponse.RefreshToken);
            authResponse.RefreshToken = null;

            return Ok(authResponse);
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequest model)
        {
            AuthResponse authResponse = _authService.Register(model, IpAddress());

            SetTokenCookie(authResponse.RefreshToken);
            authResponse.RefreshToken = null;

            return Ok(authResponse);
        }

        [HttpPost("refresh-token")]
        public IActionResult RefreshToken()
        {
            string refreshToken = Request.Cookies["refreshToken"];

            RefreshResponse refreshResponse = _authService.RefreshToken(refreshToken, IpAddress());

            if (refreshResponse == null)
            {
                return Unauthorized("Invalid token");
            }

            SetTokenCookie(refreshResponse.RefreshToken);

            return Ok(refreshResponse);
        }

        [Authorize]
        [HttpPost("revoke-token")]
        public IActionResult RevokeToken()
        {
            string token = Request.Cookies["refreshToken"];


            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("Token is required");
            }

            var revokedResponse = _authService.RevokeToken(token, IpAddress());

            if (!revokedResponse)
            {
                return NotFound("Token not found");
            }

            Response.Cookies.Delete("refreshToken");

            return Ok("Token revoked");
        }

        #region Private methods

        private void SetTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }
        private string IpAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }

        #endregion
    }
}
