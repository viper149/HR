using System;
using System.Collections.Generic;

namespace DenimERP.Models
{
    public partial class F_PR_WARPING_MACHINE
    {
        public F_PR_WARPING_MACHINE()
        {
            F_PR_WARPING_PROCESS_ROPE_BALL_DETAILS = new HashSet<F_PR_WARPING_PROCESS_ROPE_BALL_DETAILS>();
            F_PR_WARPING_PROCESS_DW_DETAILS = new HashSet<F_PR_WARPING_PROCESS_DW_DETAILS>();
            F_PR_WARPING_PROCESS_ECRU_DETAILS = new HashSet<F_PR_WARPING_PROCESS_ECRU_DETAILS>();
            F_PR_WARPING_PROCESS_SW_DETAILS = new HashSet<F_PR_WARPING_PROCESS_SW_DETAILS>();
        }

        public int ID { get; set; }
        public string MACHINE_NAME { get; set; }
        public string TYPE { get; set; }
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }
        public DateTime? CREATED_AT { get; set; }
        public DateTime? UPDATED_AT { get; set; }
        public string CREATED_BY { get; set; }
        public string UPDATED_BY { get; set; }

        public ICollection<F_PR_WARPING_PROCESS_ROPE_BALL_DETAILS> F_PR_WARPING_PROCESS_ROPE_BALL_DETAILS { get; set; }
        public ICollection<F_PR_WARPING_PROCESS_DW_DETAILS> F_PR_WARPING_PROCESS_DW_DETAILS { get; set; }
        public ICollection<F_PR_WARPING_PROCESS_ECRU_DETAILS> F_PR_WARPING_PROCESS_ECRU_DETAILS { get; set; }
        public ICollection<F_PR_WARPING_PROCESS_SW_DETAILS> F_PR_WARPING_PROCESS_SW_DETAILS { get; set; }
    }
}
