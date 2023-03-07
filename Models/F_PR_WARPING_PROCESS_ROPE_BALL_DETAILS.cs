using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_PR_WARPING_PROCESS_ROPE_BALL_DETAILS:BaseEntity
    {
        public F_PR_WARPING_PROCESS_ROPE_BALL_DETAILS()
        {
            F_DYEING_PROCESS_ROPE_DETAILS = new HashSet<F_DYEING_PROCESS_ROPE_DETAILS>();
        }
        public int BALLID { get; set; }
        public int? WARPID { get; set; }
        [Display(Name = "Ball No")]
        public int? BALL_ID_FK { get; set; }
        [Display(Name = "Ball Length")]
        public double? BALL_LENGTH { get; set; }
        [Display(Name = "Link Ball No")]
        public int? LINK_BALL_NO { get; set; }
        [Display(Name = "Link Ball Length")]
        public double? LINK_BALL_LENGTH { get; set; }
        [Display(Name = "Shift Name")]
        public string SHIFT_NAME { get; set; }
        [Display(Name = "Count Name")]
        public int? COUNT_ID { get; set; }
        [Display(Name = "Machine No")]
        public int? MACHINE_NO { get; set; }
        [Display(Name = "Operator")]
        public int? OPERATOR { get; set; }
        [Display(Name = "Break/Ends")]
        public string BREAKS_ENDS { get; set; }
        [Display(Name = "Ends/Rope")]
        public string ENDS_ROPE { get; set; }
        [Display(Name = "Lead Line")]
        public int? LEADLINE { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }
        public string OPT4 { get; set; }
        public string OPT5 { get; set; }

        public BAS_YARN_COUNTINFO COUNT_ { get; set; }
        //public F_PR_WARPING_PROCESS_ROPE_DETAILS WARP_PROG_ { get; set; }
        public F_PR_WARPING_PROCESS_ROPE_MASTER WARP_PROG { get; set; }
        public F_BAS_BALL_INFO BALL_ID_FKNavigation { get; set; }
        public F_BAS_BALL_INFO BALL_ID_FK_Link { get; set; }
        public F_PR_WARPING_MACHINE MACHINE_NONavigation { get; set; }
        public F_HRD_EMPLOYEE EMP { get; set; }
        public ICollection<F_DYEING_PROCESS_ROPE_DETAILS> F_DYEING_PROCESS_ROPE_DETAILS { get; set; }
    }
}
