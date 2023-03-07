using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DenimERP.Models
{
    public partial class F_PR_FINISHING_BEAM_RECEIVE
    {
        public int FDRID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        public int? BEAMID { get; set; }
        public int? SETID { get; set; }
        public int? FABCODE { get; set; }
        public int? RCVBY { get; set; }
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? CREATED_AT { get; set; }
        public string UPDATED_BY { get; set; }
        public DateTime? UPDATED_AT { get; set; }

        public F_PR_WEAVING_PROCESS_BEAM_DETAILS_B BEAM { get; set; }
        public RND_FABRICINFO FABCODENavigation { get; set; }
        public F_HRD_EMPLOYEE RCVBYNavigation { get; set; }
        public PL_PRODUCTION_SETDISTRIBUTION SET { get; set; }
    }
}
