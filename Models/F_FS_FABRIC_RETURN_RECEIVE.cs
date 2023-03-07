using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DenimERP.Models
{
    public partial class F_FS_FABRIC_RETURN_RECEIVE
    {
        public int RCVID { get; set; }
        [Display(Name = "RCV Date")]
        public DateTime? RCVDATE { get; set; }
        public int? BUYER_ID { get; set; }
        [Display(Name = "Return Ch. No")]
        public string DC_NO { get; set; }
        public int? DO_NO { get; set; }
        public int? FABCODE { get; set; }
        public int? PI_NO { get; set; }
        [Display(Name = "Roll Qty")]
        public int? ROLL_QTY { get; set; }
        [Display(Name = "YDS Qty")]
        public int? QTY_YDS { get; set; }
        public string REMARKS { get; set; }
        public int? OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }
        public string OPT4 { get; set; }
        public string CREATED_BY { get; set; }
        public string UPDATED_BY { get; set; }
        public DateTime? CREATED_AT { get; set; }
        public DateTime? UPDATED_AT { get; set; }

        [NotMapped]
        public string EncryptedId { get; set; }

        public BAS_BUYERINFO BUYER_ { get; set; }
        public ACC_EXPORT_DOMASTER DO_NONavigation { get; set; }
        public RND_FABRICINFO FABCODENavigation { get; set; }

        public COM_EX_PIMASTER PI { get; set; }
    }
}
