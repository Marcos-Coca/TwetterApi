using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using TwetterApi.Entities;

namespace TwetterApi.Models.Response
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
            JwtToken = jwtToken;
            RefreshToken = refreshToken;
        }
    }
}
