using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DenimERP.Models
{
    public partial class MERCHANDISER
    {
        public MERCHANDISER()
        {
            H_SAMPLE_FABRIC_DISPATCH_MASTER = new HashSet<H_SAMPLE_FABRIC_DISPATCH_MASTER>();
        }

        public int MERCID { get; set; }
        [Display(Name = "Merchandiser")]
        public string MERCHANDISER_NAME { get; set; }
        [Display(Name = "Designation")]
        public string DESIGNATION { get; set; }
        [Display(Name = "Address")]
        public string ADDRESS { get; set; }
        [Display(Name = "Phone Number")]
        public string PHONE_NUMBER { get; set; }
        [Display(Name = "Email Address")]
        public string EMAIL { get; set; }

        public ICollection<H_SAMPLE_FABRIC_DISPATCH_MASTER> H_SAMPLE_FABRIC_DISPATCH_MASTER { get; set; }
    }
}
