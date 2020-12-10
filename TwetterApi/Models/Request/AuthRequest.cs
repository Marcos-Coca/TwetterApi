using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace TwetterApi.Models.Request
{
    public class RegisterRequest
    {
        [Required]
        [MaxLength(50)]
        [MinLength(2)]
        public string Name { get; set; }
        [Required]
        [MaxLength(25)]
        [MinLength(2)]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
    }

    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
