using System;
using System.ComponentModel.DataAnnotations;

namespace DenimERP.Models
{
    public partial class ACC_LOAN_MANAGEMENT_D
    {
        [Display(Name = "Trans ID")]
        public int TRANSID { get; set; }
        [Display(Name = "Trans Date")]
        public DateTime? TRANSDATE { get; set; }
        [Display(Name = "Loan ID")]
        public int? LOANID { get; set; }
        [Display(Name = "Invoice No")]
        public int? INVID { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }
        public string OPT4 { get; set; }
        public string OPT5 { get; set; }
        public string OPT6 { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? CREATED_AT { get; set; }
        public string UPDATED_BY { get; set; }
        public DateTime? UPDATED_AT { get; set; }
        
        public COM_IMP_INVOICEINFO INV { get; set; }
        public ACC_LOAN_MANAGEMENT_M LOAN { get; set; }
    }
}
