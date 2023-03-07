using System.ComponentModel.DataAnnotations;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class COM_IMP_WORK_ORDER_DETAILS : BaseEntity
    {
        public int TRANSID { get; set; }
        [Display(Name = "Work Order No.")]
        public int? WOID { get; set; }
        [Display(Name = "Count Name")]
        public int? COUNTID { get; set; }
        [Display(Name = "Lot")]
        public int? LOTID { get; set; }
        [Display(Name = "Quantity")]
        public double? QTY { get; set; }
        [Display(Name = "Unit Price")]
        public double? UPRICE { get; set; }
        [Display(Name = "Total Price")]
        public double? TOTAL { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        [Display(Name = "Notes")]
        public string NOTES { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }

        public F_YS_INDENT_DETAILS INDD { get; set; }
        public COM_IMP_WORK_ORDER_MASTER WO { get; set; }
        public BAS_YARN_LOTINFO LOT { get; set; }
    }
}
