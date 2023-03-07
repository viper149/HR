using System;
using System.Collections.Generic;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_GEN_S_QC_APPROVE : BaseEntity
    {
        public F_GEN_S_QC_APPROVE()
        {
            F_GEN_S_RECEIVE_MASTER = new HashSet<F_GEN_S_RECEIVE_MASTER>();
        }
        public int GSQCAID { get; set; }
        public int? GSQCANO { get; set; }
        public string APPROVED_BY { get; set; }
        public DateTime? GQCADATE { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }

        public ICollection<F_GEN_S_RECEIVE_MASTER> F_GEN_S_RECEIVE_MASTER { get; set; }
    }
}
