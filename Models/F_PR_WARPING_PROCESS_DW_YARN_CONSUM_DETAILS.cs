using System.ComponentModel.DataAnnotations;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_PR_WARPING_PROCESS_DW_YARN_CONSUM_DETAILS : BaseEntity
    {
        public int CONSM_ID { get; set; }
        public int? WARP_ID { get; set; }
        [Display(Name = "Count")]
        public int? COUNT_ID { get; set; }
        [Display(Name = "Budget(KGs)")]
        public double? BGT_KG { get; set; }
        [Display(Name = "Consumption")]
        public double? CONSM { get; set; }
        [Display(Name = "Waste")]
        public double? WASTE { get; set; }
        [Display(Name = "Waste(%)")]
        public double? WASTE_PERCENTAGE { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }

        public RND_FABRIC_COUNTINFO COUNT_ { get; set; }
        public F_PR_WARPING_PROCESS_DW_MASTER WARP_ { get; set; }
    }
}
