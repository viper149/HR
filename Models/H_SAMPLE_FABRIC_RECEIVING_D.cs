using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DenimERP.Models
{
    public partial class H_SAMPLE_FABRIC_RECEIVING_D
    {
        public H_SAMPLE_FABRIC_RECEIVING_D()
        {
            H_SAMPLE_FABRIC_DISPATCH_DETAILS = new HashSet<H_SAMPLE_FABRIC_DISPATCH_DETAILS>();
        }

        public int RCVDID { get; set; }
        public int? RCVID { get; set; }
        public int? DPDID { get; set; }
        [Display(Name = "Qty")]
        public double? QTY { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }

        public F_SAMPLE_FABRIC_DISPATCH_DETAILS DPD { get; set; }
        public H_SAMPLE_FABRIC_RECEIVING_M RCV { get; set; }

        public ICollection<H_SAMPLE_FABRIC_DISPATCH_DETAILS> H_SAMPLE_FABRIC_DISPATCH_DETAILS { get; set; }
    }
}
