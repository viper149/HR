using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace HRMS.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            Messages = new HashSet<MESSAGE>();
        }

        [DisplayName(displayName: "User Name")]
        public override string UserName { get; set; }

        [DisplayName(displayName: "Phone Number")]
        public override string PhoneNumber { get; set; }

        public string PhotoPath { get; set; }

        public bool ManuallyCreated { get; set; }

        public string Address { get; set; }
        public int? EMPID { get; set; }
        public int? BID { get; set; }
        public int? TYPEID { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Created Date")]
        public virtual DateTime? CREATED_AT { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Updated Date")]
        public virtual DateTime? UPDATED_AT { get; set; }

        [Display(Name = "Created By")]
        public virtual string CREATED_BY { get; set; }

        [Display(Name = "Updated By")]
        public virtual string UPDATED_BY { get; set; }

        public AspNetUserTypes AspNetUserTypes { get; set; }
        public ICollection<MESSAGE> Messages { get; set; }
    }
}
