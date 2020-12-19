namespace TwetterApi.Domain.Options
{
    public class TokenOptions
    {
        public const string Token = "Token";
        public string JwtTokenSecret { get; set; }
    }
}
