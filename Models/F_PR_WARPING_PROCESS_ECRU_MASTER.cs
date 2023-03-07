using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_PR_WARPING_PROCESS_ECRU_MASTER : BaseEntity
    {
        public F_PR_WARPING_PROCESS_ECRU_MASTER()
        {
            F_PR_WARPING_PROCESS_ECRU_DETAILS = new HashSet<F_PR_WARPING_PROCESS_ECRU_DETAILS>();
            F_PR_WARPING_PROCESS_ECRU_YARN_CONSUM_DETAILS = new HashSet<F_PR_WARPING_PROCESS_ECRU_YARN_CONSUM_DETAILS>();
        }

        public int ECRUID { get; set; }
        public DateTime? PRODDATE { get; set; }
        public DateTime? TIME_START { get; set; }
        public DateTime? TIME_END { get; set; }
        public DateTime? DEL_DATE { get; set; }
        public int? SETID { get; set; }
        public string BALL_NO { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        public string WARPRATIO { get; set; }
        public double? WARPLENGTH { get; set; }
        public bool IS_DECLARE { get; set; }
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }

        [NotMapped]
        public double ? TotalEcruWarping { get; set; }

        public PL_PRODUCTION_SETDISTRIBUTION SET { get; set; }
        public ICollection<F_PR_WARPING_PROCESS_ECRU_DETAILS> F_PR_WARPING_PROCESS_ECRU_DETAILS { get; set; }
        public ICollection<F_PR_WARPING_PROCESS_ECRU_YARN_CONSUM_DETAILS> F_PR_WARPING_PROCESS_ECRU_YARN_CONSUM_DETAILS { get; set; }
    }
}
