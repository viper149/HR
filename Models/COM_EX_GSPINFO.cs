using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace DenimERP.Models
{
    public class COM_EX_GSPINFO
    {
        public int GSPID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [Display(Name = "GSP No")]
        public string GSPNO { get; set; }
        [Display(Name = "Invoice No")]
        public int INVID { get; set; }
        [Display(Name = "Exp L/C No")]
        public string EXPLCNO { get; set; }
        [Display(Name = "Exp L/C Date")]
        public string EXPLCDATE { get; set; }
        [Display(Name = "L/C AMD Date")]
        public string LCAMDDATE { get; set; }
        [Display(Name = "Exp Item")]
        public string EXPITEMS { get; set; }
        [Display(Name = "VAT Chalan No.")]
        public string VCNO { get; set; }
        [Display(Name = "VAT Chalan Date")]
        public string VCDATE { get; set; }
        [Display(Name = "Fabric Description")]
        public string FABDESCRIPTION { get; set; }
        [Display(Name = "Item Qty YDS")]
        public string ITEMQTY_YDS { get; set; }
        [Display(Name = "Item Qty MTS")]
        public string ITEMQTY_MTS { get; set; }
        [Display(Name = "Others")]
        public string OTHERS { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Sub. Date")]
        public DateTime? SUBDATE { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Receive Date")]
        public DateTime? RCVDDATE { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Issue Date")]
        public DateTime? ISSUEDATE { get; set; }
        [Display(Name = "GSP Remarks")]
        public string NOTES { get; set; }
        [Display(Name = "Issued to Bank")]
        public bool ISISSUE2BANK { get; set; }
        [Display(Name = "GSP N/A")]
        public bool ISGSPNA { get; set; }
        [Display(Name = "Delivery Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? DELIVERY_DATE { get; set; }
        [Display(Name = "Extra Information")]
        public string LEFT_EXT { get; set; }
        [Display(Name = "Right")]
        public string RIGHT_EXT { get; set; }

        [Display(Name = "Date Of Delivery ")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? DATE_OF_DELIVERY { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }
        [Display(Name = "Show Weight ")]
        public bool IS_WEIGHT { get; set; }
        [Display(Name = "Show Invoice Amount ")]
        public bool IS_INV_AMOUNT { get; set; }

        [Display(Name = "Sub.Bank/Party Date ")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? SUB_BNK_PRTY_DATE { get; set; }
        [Display(Name = "Submitted To")]
        public string SUBMITTED_TO { get; set; }
        [NotMapped]
        public string DateFormat
        {
            get
            {
                var dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper();
                return !string.IsNullOrEmpty(dateTimeFormat) ? dateTimeFormat : "DD-MMM-YY";
            }
        }

        public COM_EX_INVOICEMASTER INV { get; set; }
    }
}
