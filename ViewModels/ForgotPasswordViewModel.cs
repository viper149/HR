using System.ComponentModel.DataAnnotations;

namespace DenimERP.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
