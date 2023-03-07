using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DenimERP.Models
{
    public partial class PL_BULK_PROG_SETUP_M
    {
        public PL_BULK_PROG_SETUP_M()
        {
            PL_BULK_PROG_SETUP_D = new HashSet<PL_BULK_PROG_SETUP_D>();
        }

        public int BLK_PROGID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [Display(Name = "Order No.")]
        public int ORDERNO { get; set; }
        [Display(Name = "Warp Qty(M)")]
        public double? WARP_QTY { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        [Display(Name = "Style(Required if Bulk to Sample Only)")]
        public int? FABCODE { get; set; }
        [Display(Name = "IS_CLOSED_THE_SO")]
        public bool IS_CLOSED { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }
        public string OPT4 { get; set; }
        public string CREATED_BY { get; set; }
        public string UPDATED_BY { get; set; }
        public DateTime? CREATED_AT { get; set; }
        public DateTime? UPDATED_AT { get; set; }

        public ICollection<PL_BULK_PROG_SETUP_D> PL_BULK_PROG_SETUP_D { get; set; }

        public RND_PRODUCTION_ORDER RndProductionOrder { get; set; }
        public RND_FABRICINFO FABCODENavigation { get; set; }
    }
}
