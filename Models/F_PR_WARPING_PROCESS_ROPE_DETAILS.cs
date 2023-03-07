using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_PR_WARPING_PROCESS_ROPE_DETAILS : BaseEntity
    {
        public F_PR_WARPING_PROCESS_ROPE_DETAILS()
        {
            //F_PR_WARPING_PROCESS_ROPE_BALL_DETAILS = new HashSet<F_PR_WARPING_PROCESS_ROPE_BALL_DETAILS>();
            //FPrWarpingProcessRopeBallDetailsList = new List<F_PR_WARPING_PROCESS_ROPE_BALL_DETAILS>();
        }

        public int WARP_PROG_ID { get; set; }
        [Display(Name = "Program No")]
        public int? SETID { get; set; }
        [Display(Name = "No of Ball")]
        public string BALL_NO { get; set; }
        [Display(Name = "Warp Length/Set")]
        public double? WARP_LENGTH_PER_SET { get; set; }
        public int? WARPID { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }
        public string OPT4 { get; set; }
        public string OPT5 { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }

        [NotMapped]
        public string WarpingPendingSets { get; set; }

        public PL_PRODUCTION_SETDISTRIBUTION PL_PRODUCTION_SETDISTRIBUTION { get; set; }
        public F_PR_WARPING_PROCESS_ROPE_MASTER WARP { get; set; }
        //public ICollection<F_PR_WARPING_PROCESS_ROPE_BALL_DETAILS> F_PR_WARPING_PROCESS_ROPE_BALL_DETAILS { get; set; }
        //[NotMapped]
        //public List<F_PR_WARPING_PROCESS_ROPE_BALL_DETAILS> FPrWarpingProcessRopeBallDetailsList { get; set; }
    }
}
