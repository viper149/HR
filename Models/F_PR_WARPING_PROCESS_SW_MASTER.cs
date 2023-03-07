using System;
using System.Collections.Generic;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_PR_WARPING_PROCESS_SW_MASTER : BaseEntity
    {
        public F_PR_WARPING_PROCESS_SW_MASTER()
        {
            F_PR_WARPING_PROCESS_SW_DETAILS = new HashSet<F_PR_WARPING_PROCESS_SW_DETAILS>();
            F_PR_WARPING_PROCESS_SW_YARN_CONSUM_DETAILS = new HashSet<F_PR_WARPING_PROCESS_SW_YARN_CONSUM_DETAILS>();
        }

        public int SWID { get; set; }
        public DateTime? PRODDATE { get; set; }
        public DateTime? TIME_START { get; set; }
        public DateTime? TIME_END { get; set; }
        public DateTime? DEL_DATE { get; set; }
        public int? SETID { get; set; }
        public string BALL_NO { get; set; }
        public string WARPRATIO { get; set; }
        public string WARPLENGTH { get; set; }
        public bool IS_DECLARE { get; set; }
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }
        
        public PL_PRODUCTION_SETDISTRIBUTION SET { get; set; }
        public ICollection<F_PR_WARPING_PROCESS_SW_DETAILS> F_PR_WARPING_PROCESS_SW_DETAILS { get; set; }
        public ICollection<F_PR_WARPING_PROCESS_SW_YARN_CONSUM_DETAILS> F_PR_WARPING_PROCESS_SW_YARN_CONSUM_DETAILS { get; set; }
    }
}
