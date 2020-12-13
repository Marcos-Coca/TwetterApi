using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TwetterApi.Models.Repositories;
using TwetterApi.Models.Response;

namespace TwetterApi.Models.Request
{
    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        [EmailExist]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
    public class EmailExistAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value,ValidationContext validationContext)
        {

            IUserRepository userRepository = validationContext.GetService(typeof(IUserRepository)) as IUserRepository;
            
            string email = value.ToString();

            if (userRepository.GetUserByEmail(email) == null)
                return new ValidationResult("");

            return ValidationResult.Success;
        }
    }
}
