using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class H_SAMPLE_RECEIVING_M : BaseEntity
    {
        public H_SAMPLE_RECEIVING_M()
        {
            H_SAMPLE_RECEIVING_D = new HashSet<H_SAMPLE_RECEIVING_D>();
        }

        public int RCVID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Required(ErrorMessage = "Receive Date Can Not Be Empty.")]
        [Display(Name = "Receive Date")]
        public DateTime? RCVDATE { get; set; }
        [Required(ErrorMessage = "You Must Have To Select This One.")]
        public int? DPID { get; set; }
        [Display(Name = "Gate Pass Number")]
        public string GPNO { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Required(ErrorMessage = "Gate Pass Date Can Not Be Empty.")]
        [Display(Name = "Gate Pass Date")]
        public DateTime? GPDATE { get; set; }
        public int? DRID { get; set; }
        public int? VID { get; set; }
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

        public F_SAMPLE_DESPATCH_MASTER DP { get; set; }
        public F_BAS_DRIVERINFO DR { get; set; }
        public F_BAS_VEHICLE_INFO V { get; set; }
        public ICollection<H_SAMPLE_RECEIVING_D> H_SAMPLE_RECEIVING_D { get; set; }
    }
}
