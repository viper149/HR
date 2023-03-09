using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.ViewModels
{
    public class CreateRoleViewModel
    {
        [Required]
        [Display(Name ="Role")]
        [Remote(action: "IsRoleInUse", controller: "Administrator")]
        public string Name { get; set; }
    }
}
