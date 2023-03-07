using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_FS_FABRIC_LOADING_BILL : BaseEntity
    {
        public int TRANSID { get; set; }
        [Display (Name = "Trans Date")]
        public DateTime? TRANSDATE { get; set; }
        [Display(Name = "Vehicle No.")]
        public int? VEHICLE_ID { get; set; }
        [Display(Name = "Start Time")]
        public DateTime? START_TIME { get; set; }
        [Display(Name = "End Time")]
        public DateTime? END_TIME { get; set; }
        [Display(Name = "Total Time")]
        public string TOTAL_TIME { get; set; }
        [Display(Name = "Rate")]
        public double? RATE { get; set; }
        [Display(Name = "Roll Qty")]
        [Range(1, int.MaxValue, ErrorMessage = "The field {0} must between {1} ~ {2}")]
        public int? ROLL_QTY { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        [Display(Name = "Others Vehicle")]
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }
        public string OPT4 { get; set; }
        public string OPT5 { get; set; }
        [Display(Name = "Bill No.")]
        public string BILL_NO { get; set; }

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

        public F_BAS_VEHICLE_INFO VEHICLE_ { get; set; }
    }
}
