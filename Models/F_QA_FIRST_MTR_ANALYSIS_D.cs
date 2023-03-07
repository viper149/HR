using System.ComponentModel.DataAnnotations;

namespace DenimERP.Models
{
    public partial class F_QA_FIRST_MTR_ANALYSIS_D
    {
        public int FM_D_ID { get; set; }
        [Display(Name = "Analysis No.")]
        public int? FMID { get; set; }
        [Display(Name = "Lot No.")]
        public int? LOTID { get; set; }
        [Display(Name = "Supplier Name")]
        public int? SUPPLIERID { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }

        public F_QA_FIRST_MTR_ANALYSIS_M FM { get; set; }
        public BAS_YARN_LOTINFO LOT { get; set; }
        public BAS_SUPPLIERINFO SUPPLIER { get; set; }
    }
}
