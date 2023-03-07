using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_GS_WASTAGE_PARTY : BaseEntity
    {
        public F_GS_WASTAGE_PARTY()
        {
            F_GS_WASTAGE_ISSUE_M = new HashSet<F_GS_WASTAGE_ISSUE_M>();
        }


        public int PID { get; set; }
        [Display(Name = "Party Name")]
        public string PNAME { get; set; }
        [Display(Name = "Party Address")]
        public string ADDRESS { get; set; }
        [Display(Name = "Phone Number ")]
        public string PHONE { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }

        [NotMapped] 
        public string EncryptedId { get; set; }


        public ICollection<F_GS_WASTAGE_ISSUE_M> F_GS_WASTAGE_ISSUE_M { get; set; }
    }
}
