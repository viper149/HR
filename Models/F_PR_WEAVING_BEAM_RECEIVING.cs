using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DenimERP.Models
{
    public partial class F_PR_WEAVING_BEAM_RECEIVING
    {
        public F_PR_WEAVING_BEAM_RECEIVING()
        {
            //F_PR_WEAVING_WEFT_YARN_CONSUM_DETAILS = new HashSet<F_PR_WEAVING_WEFT_YARN_CONSUM_DETAILS>();
        }

        public int RCVID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        public DateTime? RCVDATE { get; set; }
        public int? RCVDBY { get; set; }
        public int? SETID { get; set; }
        public int? LOOMSPEED { get; set; }
        public int? EFFICIENCY { get; set; }
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? CREATED_AT { get; set; }
        public string UPDATED_BY { get; set; }
        public DateTime? UPDATED_AT { get; set; }

        public F_HRD_EMPLOYEE RCVDBYNavigation { get; set; }
        public PL_PRODUCTION_SETDISTRIBUTION SET { get; set; }
        //public ICollection<F_PR_WEAVING_WEFT_YARN_CONSUM_DETAILS> F_PR_WEAVING_WEFT_YARN_CONSUM_DETAILS { get; set; }
    }
}
