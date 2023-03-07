using System.Collections.Generic;

namespace DenimERP.Models
{
    public partial class F_BAS_BALL_INFO
    {
        public F_BAS_BALL_INFO()
        {
            F_PR_WARPING_PROCESS_ROPE_BALL_DETAILS = new HashSet<F_PR_WARPING_PROCESS_ROPE_BALL_DETAILS>();
            F_PR_WARPING_PROCESS_ROPE_BALL_DETAILS_LINK = new HashSet<F_PR_WARPING_PROCESS_ROPE_BALL_DETAILS>();
            F_PR_WARPING_PROCESS_DW_DETAILSBALL_NONavigation = new HashSet<F_PR_WARPING_PROCESS_DW_DETAILS>();
            F_PR_WARPING_PROCESS_DW_DETAILSLINK_BALL_NONavigation = new HashSet<F_PR_WARPING_PROCESS_DW_DETAILS>();
            F_PR_WARPING_PROCESS_ECRU_DETAILSBALL_NONavigation = new HashSet<F_PR_WARPING_PROCESS_ECRU_DETAILS>();
            F_PR_WARPING_PROCESS_ECRU_DETAILSLINK_BALL_NONavigation = new HashSet<F_PR_WARPING_PROCESS_ECRU_DETAILS>();
            F_PR_WARPING_PROCESS_SW_DETAILSBALL_NONavigation = new HashSet<F_PR_WARPING_PROCESS_SW_DETAILS>();
            F_PR_WARPING_PROCESS_SW_DETAILSLINK_BALL_NONavigation = new HashSet<F_PR_WARPING_PROCESS_SW_DETAILS>();

        }

        public int BALLID { get; set; }
        public string BALL_NO { get; set; }
        public string FOR_ { get; set; }
        public string REMARKS { get; set; }

        public ICollection<F_PR_WARPING_PROCESS_ROPE_BALL_DETAILS> F_PR_WARPING_PROCESS_ROPE_BALL_DETAILS { get; set; }
        public ICollection<F_PR_WARPING_PROCESS_ROPE_BALL_DETAILS> F_PR_WARPING_PROCESS_ROPE_BALL_DETAILS_LINK { get; set; }
        public ICollection<F_PR_WARPING_PROCESS_DW_DETAILS> F_PR_WARPING_PROCESS_DW_DETAILSBALL_NONavigation { get; set; }
        public ICollection<F_PR_WARPING_PROCESS_DW_DETAILS> F_PR_WARPING_PROCESS_DW_DETAILSLINK_BALL_NONavigation { get; set; }
        public ICollection<F_PR_WARPING_PROCESS_ECRU_DETAILS> F_PR_WARPING_PROCESS_ECRU_DETAILSBALL_NONavigation { get; set; }
        public ICollection<F_PR_WARPING_PROCESS_ECRU_DETAILS> F_PR_WARPING_PROCESS_ECRU_DETAILSLINK_BALL_NONavigation { get; set; }
        public ICollection<F_PR_WARPING_PROCESS_SW_DETAILS> F_PR_WARPING_PROCESS_SW_DETAILSBALL_NONavigation { get; set; }
        public ICollection<F_PR_WARPING_PROCESS_SW_DETAILS> F_PR_WARPING_PROCESS_SW_DETAILSLINK_BALL_NONavigation { get; set; }
    }
}
