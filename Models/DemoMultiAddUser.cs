using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DenimERP.Models
{
    public class DemoMultiAddUser
    {
        [Required]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        public List<DemoUser> Users { get; set; }

        public DemoMultiAddUser()
        {
            Users = new List<DemoUser>();
        }
    }
}