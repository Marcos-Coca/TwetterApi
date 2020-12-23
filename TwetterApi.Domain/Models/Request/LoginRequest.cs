using System.ComponentModel.DataAnnotations;
using TwetterApi.Domain.Interfaces.Repositories;

namespace TwetterApi.Domain.Models.Request
{
    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        [EmailExist]
        public string Email { get; set; }
        [Required]
        [MinLength(6)]
        public string Password { get; set; }
    }
    public class EmailExistAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value,ValidationContext validationContext)
        {

            IUserRepository userRepository = validationContext.GetService(typeof(IUserRepository)) as IUserRepository;
            
            string email = value.ToString();

            if (userRepository.GetUserByEmail(email) == null)
                return new ValidationResult("Email doesn't exist");

            return ValidationResult.Success;
        }
    }
}
