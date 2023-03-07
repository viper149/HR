using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class ACC_LOAN_MANAGEMENT_M : BaseEntity
    {
        public ACC_LOAN_MANAGEMENT_M()
        {
            ACC_LOAN_MANAGEMENT_D = new HashSet<ACC_LOAN_MANAGEMENT_D>();
        }

        [Display(Name = "Loan ID")]
        public int LOANID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Loan Date")]
        public DateTime? LOANDATE { get; set; }
        [Display(Name = "Bank Name")]
        public int? BANKID { get; set; }
        [Display(Name = "LC No")]
        public int? LCID { get; set; }
        [Display(Name = "Loan Amount")]
        public double? LOAN_AMT { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Expiry Date")]
        public DateTime? EXP_DATE { get; set; }
        [Display(Name = "Interest Rate")]
        public double? INTEREST_RATE { get; set; }
        public double? BANK_INT { get; set; }
        [Display(Name = "Paid Amount")]
        public double? PAID_AMT { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Paid Date")]
        public DateTime? PAID_DATE { get; set; }
        [Display(Name = "Paid Interest")]
        public double? PAID_INT { get; set; }
        public bool? IS_CLOSE { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }
        public string OPT4 { get; set; }
        public string OPT5 { get; set; }
        public string OPT6 { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }

        [NotMapped]
        public string DateFormat
        {
            get
            {
                var dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper();
                return !string.IsNullOrEmpty(dateTimeFormat) ? dateTimeFormat : "DD-MMM-YY";
            }
        }

        public BAS_BEN_BANK_MASTER BANK { get; set; }
        public COM_IMP_LCINFORMATION LC { get; set; }

        public ICollection<ACC_LOAN_MANAGEMENT_D> ACC_LOAN_MANAGEMENT_D { get; set; }
    }
}
