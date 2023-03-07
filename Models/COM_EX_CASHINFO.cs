using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace DenimERP.Models
{
    public partial class COM_EX_CASHINFO
    {
        public int CASHID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [Display(Name = "Cash No")]
        [Required(ErrorMessage = "The field {0} can not be empty.")]
        public string CASHNO { get; set; }
        [Display(Name = "Back to Back L/C")]
        public string BACKTOBACK_LCTYPE { get; set; }
        [Display(Name = "L/C No")]
        [Required(ErrorMessage = "The field {0} can not be empty.")]
        public int LCID { get; set; }
        [Display(Name = "Master L/C Value")]
        public double? MLCVALUE { get; set; }
        [Display(Name = "Mushak No")]
        public string VCNO { get; set; }
        [Display(Name = "Mushak Date")]
        public string VCDATE { get; set; }
        [Display(Name = "Qty(Yds)")]
        public string ITEMQTY_YDS { get; set; }
        [Display(Name = "Qty(Mtr)")]
        public string ITEMQTY_MTS { get; set; }
        [Display(Name = "Rate")]
        public string RATE { get; set; }
        [Display(Name = "Fabric Description")]
        public string FABDESCRIPTION { get; set; }
        [Display(Name = "Fabric Description(Top)")]
        public string FABDESCRIPTION_DETAILS { get; set; }
        [Display(Name = "Yarn Cash BTMA No")]
        public string YARN_CASH_BTMA_NO { get; set; }
        [Display(Name = "Yarn Cash BTMA Date")]
        public string YARN_CASH_BTMA_DATE { get; set; }
        [Display(Name = "Delivery Date")]
        public string DELIVERY_DATE { get; set; }
        [Display(Name = "Master L/C(Left)")]
        public string OTHERS { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        [Display(Name = "Submission Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? SUBDATE { get; set; }
        [Display(Name = "Receive Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? RCVDDATE { get; set; }
        [Display(Name = "Issue Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? ISSUEDATE { get; set; }
        [Display(Name = "Cash Notes")]
        public string NOTES { get; set; }
        [Display(Name = "Issue To Bank")]
        public bool ISISSUE2BANK { get; set; }
        [Display(Name = "Cash N/A")]
        public bool ISCASHNA { get; set; }
        [Display(Name = "Master L/C File")]
        public string MLCNO { get; set; }
        [Display(Name = "L/C Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? LCDATE { get; set; }
        [Display(Name = "Garments Qty")]
        public string GARMENT_QTY { get; set; }
        [Display(Name = "L/C Value")]
        public string LCVALUE { get; set; }
        [Display(Name = "L/C Expiry")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? LCEXPIRY { get; set; }
        [Display(Name = "Currency")]
        public int? CURRENCY_ID { get; set; }
        [Display(Name = "Order Value")]
        [Range(1, double.MaxValue, ErrorMessage = "The filed {0} can not be less than {1}")]
        public double? ORDER_VALUE { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }
        public string OPT4 { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? CREATED_AT { get; set; }
        public DateTime? UPDATED_AT { get; set; }
        public string UPDATED_BY { get; set; }

        [NotMapped]
        public string DateFormat
        {
            get
            {
                var dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper();
                return !string.IsNullOrEmpty(dateTimeFormat) ? dateTimeFormat : "DD-MMM-YY";
            }
        }

        public COM_EX_LCINFO LC { get; set; }
        public CURRENCY CURRENCYNavigation { get; set; }
    }
}
