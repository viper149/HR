using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.ViewModels.Account
{
    public class ExtendRegisterViewModel : RegisterViewModel
    {
        [Required]
        [Display(Name = "Bulk Email Addresses", Prompt = "Add Emails Separated By Comma/Space [ , \" \"]")]
        [Remote(action: "IsAnyBulkEmailsInUse", controller: "Account")]
        public new string Email { get; set; }
    }
}
