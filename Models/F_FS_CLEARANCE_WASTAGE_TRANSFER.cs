using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_FS_CLEARANCE_WASTAGE_TRANSFER : BaseEntity
    {
        public int TRANSID { get; set; }
        [Display(Name = "Trans. Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? TRANSDATE { get; set; }
        [Display(Name = "Item Name")]
        public int? ITEM { get; set; }
        [Display(Name = "Quantity (Yds)")]
        public double? QTY_YDS { get; set; }
        [Display(Name = "Quantity (Kg)")]
        public double? QTY_KG { get; set; }
        [Display(Name = "Clearance By")]
        public int? CLRBY { get; set; }
        [Display(Name = "WTR No.")]
        public int? WTRNO { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }

        [NotMapped]
        public string EncryptedId { get; set; }
        [NotMapped]
        public string DateFormat
        {
            get
            {
                var dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper();
                return !string.IsNullOrEmpty(dateTimeFormat) ? dateTimeFormat : "DD-MMM-YY";
            }
        }

        public F_HRD_EMPLOYEE CLRBYNavigation { get; set; }
        public F_FS_CLEARANCE_WASTAGE_ITEM ITEMNavigation { get; set; }
    }
}
