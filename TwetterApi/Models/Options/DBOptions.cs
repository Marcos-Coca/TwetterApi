using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TwetterApi.Models.Options
{
    public class DBOptions
    {
        public const string DB = "DB";
        public string Server { get; set; }
        public string User { get; set; }
        public string Database { get; set; }
        public string Password { get; set; }
    }
}
