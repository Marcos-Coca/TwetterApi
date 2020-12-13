using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TwetterApi.Models.Response
{
    public class Response
    {
        public string Message { get; set; }
        public object Data { get; set; }
        public bool Success { get; set; }
    }
}
