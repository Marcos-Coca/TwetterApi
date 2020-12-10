using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TwetterApi.Models.Common
{
    public class TokenOptions
    {
        public const string Token = "Token";
        public string JwtTokenSecret { get; set; }
    }
}
