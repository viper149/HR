using System.ComponentModel.DataAnnotations;

namespace DenimERP.Models
{
    public partial class F_SAMPLE_FABRIC_ISSUE_DETAILS
    {
        public int SFIDID { get; set; }
        public int? SFIID { get; set; }
        public int? FABCODE { get; set; }
        [Display(Name = "Sample Required Qty.")]
        public double? SR_QTY { get; set; }
        [Display(Name = "Issued Qty.")]
        public double? SR_ISSUE_QTY { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        public bool HasRemoved { get; set; }

        public F_SAMPLE_FABRIC_ISSUE F_SAMPLE_FABRIC_ISSUE { get; set; }
        public RND_FABRICINFO Fabricinfo { get; set; }
    }
}
