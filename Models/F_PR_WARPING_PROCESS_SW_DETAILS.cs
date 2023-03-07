using System;

namespace DenimERP.Models
{
    public partial class F_PR_WARPING_PROCESS_SW_DETAILS
    {
        public int SW_D_ID { get; set; }
        public int? SW_ID { get; set; }
        public int? BALL_NO { get; set; }
        public string BALL_LENGTH { get; set; }
        public int? LINK_BALL_NO { get; set; }
        public string LINK_BALL_LENGTH { get; set; }
        public string SHIFTNAME { get; set; }
        public int? COUNTID { get; set; }
        public int? MACHINE_ID { get; set; }
        public string BREAKS_ENDS { get; set; }
        public string ENDS_ROPE { get; set; }
        public string LEADLINE { get; set; }
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? CREATED_AT { get; set; }
        public string UPDATED_BY { get; set; }
        public DateTime? UPDATED_AT { get; set; }

        public F_BAS_BALL_INFO BALL_NONavigation { get; set; }
        public RND_FABRIC_COUNTINFO COUNT { get; set; }
        public F_BAS_BALL_INFO LINK_BALL_NONavigation { get; set; }
        public F_PR_WARPING_MACHINE MACHINE_ { get; set; }
        public F_PR_WARPING_PROCESS_SW_MASTER SW_ { get; set; }
    }
}
