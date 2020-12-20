using Newtonsoft.Json;
using TwetterApi.Domain.Entities;

namespace TwetterApi.Domain.Models.Response
{
    public class AuthResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string PhotoUrl { get; set; }
        public string JwtToken { get; set; }
        [JsonIgnore] // refresh token is returned in http only cookie
        public string RefreshToken { get; set; }

        public AuthResponse(User user, string jwtToken, string refreshToken)
        {
            Id = user.Id;
            Name = user.Name;
            UserName = user.UserName;
            PhotoUrl = user.PhotoUrl;
            JwtToken = jwtToken;
            RefreshToken = refreshToken;
        }
    }
}
