using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [Remote(action: "IsUserNameInUse", controller: "Account")]
        [Display(Name = "User Name", Prompt = "User Name")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Remote(action: "IsEmailInUse", controller: "Account")]
        //[ValidEmailDomainAttribute("https://localhost:44306/", ErrorMessage = "Only from [https://localhost:44306/] is allowed.")]
        [Display(Name = "Email Address", Prompt = "Email Address")]
        public virtual string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password", Prompt = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password", Prompt = "Confirm password")]
        [Compare("Password",
            ErrorMessage = "Password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Phone Number", Prompt = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Photo")]
        public IFormFile PhotoPath { get; set; }
    }
}
