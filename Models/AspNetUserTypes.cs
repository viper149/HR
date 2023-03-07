using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DenimERP.Models
{
    public class AspNetUserTypes
    {
        public AspNetUserTypes()
        {
            AspNetUsers = new HashSet<ApplicationUser>();
        }

        public int TYPEID { get; set; }
        [Display(Name = "Type Name")]
        public string TYPENAME { get; set; }

        public ICollection<ApplicationUser> AspNetUsers { get; set; }
    }
}
