using System.ComponentModel.DataAnnotations;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_PR_WARPING_PROCESS_ROPE_YARN_CONSUM_DETAILS: BaseEntity
    {
        public int CONSM_ID { get; set; }
        public int? WARPID { get; set; }
        [Display(Name = "Count Name")]
        public int? COUNT_ID { get; set; }
        [Display(Name = "Budget KG(s)")]
        public double? BGT_KG_TOTAL { get; set; }
        [Display(Name = "Budget KGs(Per Set)")]
        public double? BGT_KG_PER_SET { get; set; }
        [Display(Name = "Total Consumption")]
        public double? CONSUM_TOTAL { get; set; }
        [Display(Name = "Total Waste")]
        public double? WASTE_TOTAL { get; set; }
        [Display(Name = "Waste(%)")]
        public double? WASTE_PERCENTAGE_TOTAL { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }
        public string OPT4 { get; set; }
        public string OPT5 { get; set; }

        public BAS_YARN_COUNTINFO COUNT_ { get; set; }
        public F_PR_WARPING_PROCESS_ROPE_MASTER WARP { get; set; }
    }
}
