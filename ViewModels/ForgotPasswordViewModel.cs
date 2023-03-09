using System.ComponentModel.DataAnnotations;

namespace HRMS.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
