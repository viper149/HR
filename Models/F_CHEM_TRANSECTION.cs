using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace DenimERP.Models
{
    public partial class F_CHEM_TRANSECTION
    {
        public int CTRID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [Display(Name = "Trans. Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? CTRDATE { get; set; }
        [Display(Name = "Chemical Name")]
        public int? PRODUCTID { get; set; }
        [Display(Name = "Receive No.")]
        public int? CRCVID { get; set; }
        [Display(Name = "Receive Type")]
        public int? RCVTID { get; set; }
        [Display(Name = "Issue No.")]
        public int? CISSUEID { get; set; }
        [Display(Name = "Issue Type")]
        public int? ISSUEID { get; set; }
        [Display(Name = "ISSUE ID")]
        public int? GSISSUETRNSID { get; set; }
        [Display(Name = "Opening Balance")]
        public double? OP_BALANCE { get; set; }
        [Display(Name = "Receive Quantity")]
        public double? RCV_QTY { get; set; }
        [Display(Name = "Issue Quantity")]
        public double? ISSUE_QTY { get; set; }
        [Display(Name = "Balance")]
        public double? BALANCE { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        public string OP1 { get; set; }
        public string OP2 { get; set; }
        public string OP3 { get; set; }

        [NotMapped]
        public string DateFormat
        {
            get
            {
                var dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper();
                return !string.IsNullOrEmpty(dateTimeFormat) ? dateTimeFormat : "DD-MMM-YY";
            }
        }

        public F_CHEM_STORE_RECEIVE_DETAILS CRCV { get; set; }
        public F_BAS_ISSUE_TYPE ISSUE { get; set; }
        public F_CHEM_STORE_PRODUCTINFO PRODUCT { get; set; }
        public F_BAS_RECEIVE_TYPE RCVT { get; set; }
        public F_CHEM_ISSUE_DETAILS CISSUE { get; set; }
    }
}
