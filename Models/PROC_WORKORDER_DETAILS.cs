using System;
using System.ComponentModel.DataAnnotations;

namespace DenimERP.Models
{
    public partial class PROC_WORKORDER_DETAILS
    {
        public int WODID { get; set; }
        public int? WOID { get; set; }
        [Display(Name = "Product Code")]
        public int? PRODID { get; set; }
        [Display(Name = "Product Name")]
        public string PRODNAME { get; set; }
        [Display(Name = "Unit")]
        public string UNIT { get; set; }
        [Display(Name = "Qty")]
        public double? QTY { get; set; }
        [Display(Name = "Rate")]
        public double? RATE { get; set; }
        [Display(Name = "Amount")]
        public double? AMOUNT { get; set; }
        [Display(Name = "Indent No.")]
        public int? INDENTNO { get; set; }
        [Display(Name = "Bs Unit")]
        public string BSUNIT { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }
        public string OPT4 { get; set; } 
        public string OPT5 { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? CREATED_AT { get; set; }
        public string UPDATED_BY { get; set; }
        public DateTime? UPDATED_AT { get; set; }

        public F_GS_PRODUCT_INFORMATION GsProductInfo { get; set; }
        public PROC_WORKORDER_MASTER WorkOrderMaster { get; set; }
        public F_YS_INDENT_MASTER YsIndentMaster { get; set; }

    }
}
