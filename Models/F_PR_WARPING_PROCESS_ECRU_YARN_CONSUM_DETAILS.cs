using System;

namespace DenimERP.Models
{
    public partial class F_PR_WARPING_PROCESS_ECRU_YARN_CONSUM_DETAILS
    {
        public int CONSM_ID { get; set; }
        public int? ECRU_ID { get; set; }
        public int? COUNT_ID { get; set; }
        public double? BGT_KG { get; set; }
        public double? CONSM { get; set; }
        public double? WASTE { get; set; }
        public double? WASTE_PERCENTAGE { get; set; }
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? CREATED_AT { get; set; }
        public string UPDATED_BY { get; set; }
        public DateTime? UPDATED_AT { get; set; }

        public RND_FABRIC_COUNTINFO COUNT_ { get; set; }
        public F_PR_WARPING_PROCESS_ECRU_MASTER ECRU_ { get; set; }
    }
}
