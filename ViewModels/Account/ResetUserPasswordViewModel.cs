using System.ComponentModel.DataAnnotations;

namespace DenimERP.ViewModels.Account
{
    public class ResetUserPasswordViewModel
    {
        [Required]
        [Display(Name = "User Name", Prompt = "Type user name here.")]
        public string UserName { get; set; }
        [EmailAddress]
        [Display(Name = "Email Address", Prompt = "Type email address here.")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password", Prompt = "Password")]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password", Prompt = "Confirm password")]
        [Compare("Password",
            ErrorMessage = "Password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}