using System;
using System.Collections.Generic;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_GEN_S_MRR : BaseEntity
    {
        public F_GEN_S_MRR()
        {
            F_GEN_S_RECEIVE_MASTER = new HashSet<F_GEN_S_RECEIVE_MASTER>();
        }

        public int GSMRRID { get; set; }
        public int? GSMRRNO { get; set; }
        public DateTime? GSMRRDATE { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }

        public ICollection<F_GEN_S_RECEIVE_MASTER> F_GEN_S_RECEIVE_MASTER { get; set; }
    }
}
