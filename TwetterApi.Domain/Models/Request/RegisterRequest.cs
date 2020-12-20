using System;
using System.ComponentModel.DataAnnotations;
using TwetterApi.Domain.Repositories;

namespace TwetterApi.Domain.Models.Request
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
        [UserNameExist]
        public string UserName { get; set; }
        [Required]
        [MinLength(6)]
        public string Password { get; set; }
        [Required]
        [EmailAddress]
        [EmailNotExist]
        public string Email { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
    }
    public class EmailNotExistAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            IUserRepository userRepository = validationContext.GetService(typeof(IUserRepository)) as IUserRepository;

            string email = value.ToString();

            if (userRepository.GetUserByEmail(email) == null)
                return ValidationResult.Success;

            return new ValidationResult("Email already exists");
        }
    }
    public class UserNameExistAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            IUserRepository userRepository = validationContext.GetService(typeof(IUserRepository)) as IUserRepository;

            string userName = value.ToString();

            if (userRepository.GetUserByUserName(userName) == null)
                return ValidationResult.Success;

            return new ValidationResult("User Name already in use");
        }
    }

}
