using System.ComponentModel.DataAnnotations;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_PR_WARPING_PROCESS_DW_DETAILS : BaseEntity
    {
        public int WARP_D_ID { get; set; }
        public int? WARP_ID { get; set; }
        [Display(Name = "Beam No")]
        public int? BALL_NO { get; set; }
        [Display(Name = "Ball Length")]
        public string BALL_LENGTH { get; set; }
        [Display(Name = "Link Ball")]
        public int? LINK_BALL_NO { get; set; }
        [Display(Name = "Link Ball Length")]
        public string LINK_BALL_LENGTH { get; set; }
        [Display(Name = "Shift")]
        public string SHIFTNAME { get; set; }
        [Display(Name = "Count")]
        public int? COUNTID { get; set; }
        [Display(Name = "Machine")]
        public int? MACHINE_ID { get; set; }
        [Display(Name = "Break/End")]
        public string BREAKS_ENDS { get; set; }
        [Display(Name = "Ends")]
        public string ENDS_ROPE { get; set; }
        [Display(Name = "Lead line")]
        public string LEADLINE { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }

        public F_BAS_BALL_INFO BALL_NONavigation { get; set; }
        public RND_FABRIC_COUNTINFO COUNT { get; set; }
        public F_BAS_BALL_INFO LINK_BALL_NONavigation { get; set; }
        public F_PR_WARPING_MACHINE MACHINE_ { get; set; }
        public F_PR_WARPING_PROCESS_DW_MASTER WARP_ { get; set; }
    }
}
