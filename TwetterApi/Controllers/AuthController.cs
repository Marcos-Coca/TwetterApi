using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using TwetterApi.Models.Request;
using TwetterApi.Services;

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
            var response = _authService.Login(model,ipAddress());

            if (response == null)
                return BadRequest();

            setTokenCookie(response.RefreshToken);

            return Ok(response);
           
        }
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequest model)
        {
            var response = _authService.Register(model, ipAddress());

            setTokenCookie(response.RefreshToken);

            return Ok(response);
        }

        [HttpPost("refresh-token")]
        public IActionResult RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var response = _authService.RefreshToken(refreshToken, ipAddress());

            if (response == null)
                return Unauthorized(new { message = "Invalid token" });

            setTokenCookie(response.RefreshToken);

            return Ok(response);
        }

        [Authorize]
        [HttpPost("revoke-token")]

        public IActionResult RevokeToken([FromBody] RevokeTokenRequest model)
        {
            //Accept token from request body or cookie

            var token = Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(token))
                return BadRequest(new { message = "Token is required" });

            var response = _authService.RevokeToken(token, ipAddress());

            if (!response)
                return NotFound(new { message = "Token not found" });

            return Ok(new { message = "Token revoked" });
        }
        [Authorize]
        [HttpGet("refresh-tokens")]
        public IActionResult GetRefreshTokens()
        {
            int userId = int.Parse(getUserId());
            var refreshTokens = _authService.GetUserRefreshTokens(userId);

            if (refreshTokens == null) return NotFound();

            return Ok(refreshTokens);
        }

        //helper methods

        private void setTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }

        public string getUserId()
        {
           return User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }

        private string ipAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
    }
}
