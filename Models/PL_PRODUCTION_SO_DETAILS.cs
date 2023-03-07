using System;
using System.ComponentModel.DataAnnotations;

namespace DenimERP.Models
{
    public partial class PL_PRODUCTION_SO_DETAILS
    {
        public int PP_SO_ID { get; set; }
        [Display(Name = "PO No.")]
        public int? POID { get; set; }
        public int? GROUPID { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        public string CREATED_BY { get; set; }
        public string UPDATED_BY { get; set; }
        public DateTime? CREATED_AT { get; set; }
        public DateTime UPDATED_AT { get; set; }

        public PL_PRODUCTION_PLAN_MASTER GROUP { get; set; }
        public RND_PRODUCTION_ORDER PO { get; set; }
    }
}
