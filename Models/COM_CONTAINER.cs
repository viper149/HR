using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DenimERP.Models
{
    public partial class COM_CONTAINER
    {
        public COM_CONTAINER()
        {
            COM_IMP_INVOICEINFOS = new HashSet<COM_IMP_INVOICEINFO>();
        }

        [Display(Name = "Container Number")]
        public int CONTAINERID { get; set; }
        [Display(Name = "Container Size")]
        public string CONTAINERSIZE { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }

        public ICollection<COM_IMP_INVOICEINFO> COM_IMP_INVOICEINFOS { get; set; }
    }
}
