using System;
using System.Collections.Generic;

namespace DenimERP.Models
{
    public partial class F_PR_SLASHER_MACHINE_INFO
    {
        public F_PR_SLASHER_MACHINE_INFO()
        {
            F_PR_SLASHER_DYEING_DETAILS = new HashSet<F_PR_SLASHER_DYEING_DETAILS>();
        }

        public int SL_MID { get; set; }
        public string SL_MNO { get; set; }
        public string DETAILS { get; set; }
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? CREATED_AT { get; set; }
        public string UPDATED_BY { get; set; }
        public DateTime? UPDATED_AT { get; set; }

        public ICollection<F_PR_SLASHER_DYEING_DETAILS> F_PR_SLASHER_DYEING_DETAILS { get; set; }
    }
}
