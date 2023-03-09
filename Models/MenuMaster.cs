using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Models
{
    public partial class MenuMaster
    {
        public int MenuIdentity { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [Display(Name = "Menu ID", Prompt = "Type The Menu Id")]
        [Required(ErrorMessage = "Menu Id Can Not Be Empty.")]
        [Remote("IsMenuIdInUse", "Administrator")]
        public string MenuID { get; set; }
        [Display(Name = "Menu Name")]
        [Required(ErrorMessage = "Menu Name Can Not Be Empty.")]
        [Remote("IsMenuNameInUse", "Administrator")]
        [MaxLength(250, ErrorMessage = "The field {0} can not exceed max length {1}")]
        public string MenuName { get; set; }
        [Display(Name = "Parent Menu Id")]
        public string Parent_MenuID { get; set; }
        [Display(Name = "Menu File Name", Prompt = "Type Name Of The Action")]
        [Required(ErrorMessage = "Menu File Name/Action Name Can Not Be Empty.")]
        public string MenuFileName { get; set; }
        [Display(Name = "Menu URL", Prompt = "Type Name Of The Controller")]
        [Required(ErrorMessage = "Menu URL/Controller Name Can Not Be Empty.")]
        public string MenuURL { get; set; }
        [Display(Name = "Use In")]
        public bool USE_YN { get; set; }
        [Display(Name = "Parent Menu Icon", Prompt = "<i class=\"fa fa-flask\"></i>")]
        public string ParentMenuIcon { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? Priority { get; set; }
    }
}
