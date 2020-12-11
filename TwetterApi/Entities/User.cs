using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;


namespace TwetterApi.Entities
{
    public class User
    {
        public int Id;
        public string Name;
        public string UserName;
        public string Email;
        public DateTime BirthDate;
        [JsonIgnore]
        public string Password;
        [JsonIgnore]
        public string PhotoUrl;
        public string Biography;
        public string HeaderUrl;

    }
}
