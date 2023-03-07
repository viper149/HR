using System.ComponentModel.DataAnnotations;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_SAMPLE_DESPATCH_DETAILS : BaseEntity
    {
        public int DPDID { get; set; }
        public int? DPID { get; set; }
        public int? TRNSID { get; set; }
        public int? BYERID { get; set; }
        [Display(Name = "Issue Person")]
        public string ISSUE_PERSON { get; set; }
        [Display(Name = "Unit")]
        public int? UID { get; set; }
        [Display(Name = "Request Quantity")]
        public double? REQ_QTY { get; set; }
        [Display(Name = "Delivery Quantity ")]
        public double? DEL_QTY { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }

        public BAS_BUYERINFO BYER { get; set; }
        public F_SAMPLE_DESPATCH_MASTER DP { get; set; }
        public F_SAMPLE_GARMENT_RCV_D TRNS { get; set; }
        public F_BAS_UNITS U { get; set; }
    }
}
