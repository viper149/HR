using System;
using System.ComponentModel.DataAnnotations;

namespace DenimERP.Models
{
    public partial class F_YS_GP_DETAILS
    {
        public int TRNSID { get; set; }
        public DateTime? TRNSDATE { get; set; }
        public int? GPID { get; set; }
        public int? COUNTID { get; set; }
        public int? LOTID { get; set; }
        [Display(Name = "Qty Bags")]
        public decimal? QTY_BAGS { get; set; }
        [Display(Name = "Qty Kgs")]
        public decimal? QTY_KGS { get; set; }
        public int? LOCATION_ID { get; set; }
        public int? LEDGER_ID { get; set; }
        [Display(Name = "Page No")]
        public string PAGENO { get; set; }
        [Display(Name = "Indent No")]
        public int? INDSLID { get; set; }
        [Display(Name = "Stock Type")]
        public int? STOCKID { get; set; }
        public int? OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? CREATED_AT { get; set; }
        public string UPDATED_BY { get; set; }
        public DateTime? UPDATED_AT { get; set; }

        public BAS_YARN_COUNTINFO COUNT { get; set; }
        public F_YS_GP_MASTER GP { get; set; }
        public F_YS_LEDGER LEDGER_ { get; set; }
        public F_YS_LOCATION LOCATION_ { get; set; }
        public BAS_YARN_LOTINFO LOT { get; set; }
        public F_YS_YARN_RECEIVE_DETAILS RCV { get; set; }
        public F_YARN_TRANSACTION_TYPE STOCK { get; set; }

    }
}
