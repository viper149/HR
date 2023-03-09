using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HRMS.Models;
using HRMS.ViewModels.Employee;
using Microsoft.AspNetCore.Http;

namespace HRMS.ViewModels
{
    public class EditUserViewModel
    {
        public EditUserViewModel()
        {
            Claims = new List<string>();
            Roles = new List<string>();
            AspNetUserTypeses = new List<AspNetUserTypes>();
            ExtendFHrEmployees = new List<ExtendFHrdEmployee>();
            Companies = new List<COMPANY_INFO>();
        }

        public string Id { get; set; }
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public IFormFile NewPhotoPath { get; set; }
        public string OldPhotoPath { get; set; }
        public int? TYPEID { get; set; }
        [Display(Name = "Employee")]
        public int? EMPID { get; set; }
        [Display(Name ="Company")]
        public int? BID { get; set; }

        public AspNetUserTypes AspNetUserTypes { get; set; }
        public List<ExtendFHrdEmployee> ExtendFHrEmployees { get; set; }
        public List<COMPANY_INFO> Companies { get; set; }
        public List<AspNetUserTypes> AspNetUserTypeses { get; set; }
        public List<string> Claims { get; set; }
        public IList<string> Roles { get; set; }
    }
}
