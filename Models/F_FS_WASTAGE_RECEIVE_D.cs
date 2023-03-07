using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_FS_WASTAGE_RECEIVE_D : BaseEntity
    {
        public int TRNSID { get; set; }
        [Display(Name = "Transection Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? TRNSDATE { get; set; }
        [Display(Name = "Wastage product")]
        public int? WPID { get; set; }
        [Display(Name = "Receive QTY")]
        public int? RCV_QTY { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        [Display(Name = "")]
        public int? WRID { get; set; }
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

        public F_WASTE_PRODUCTINFO WP { get; set; }
        public F_FS_WASTAGE_RECEIVE_M WR { get; set; }
    }
}
