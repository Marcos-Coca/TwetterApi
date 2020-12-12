using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TwetterApi.Models.Response
{
    public class RefreshResponse
    {
        [JsonIgnore]
        public string RefreshToken { get; set; }
        public string JwtToken { get; set; }

        public RefreshResponse(string refreshToken,string jwtToken)
        {
            RefreshToken = refreshToken;
            JwtToken = jwtToken;
        }
    }
}
