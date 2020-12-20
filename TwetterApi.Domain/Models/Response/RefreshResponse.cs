using Newtonsoft.Json;

namespace TwetterApi.Domain.Models.Response
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
