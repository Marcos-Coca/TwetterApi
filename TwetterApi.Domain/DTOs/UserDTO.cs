using System;
using System.Collections.Generic;
using System.Text;

namespace TwetterApi.Domain.DTOs
{
    public class UserDTO
    {
        public int Id;
        public string Name;
        public string UserName;
        public string Email;
        public DateTime BirthDate;
        public string Password;
        public string PhotoUrl;
        public string Biography;
        public string HeaderUrl;
    }
}
