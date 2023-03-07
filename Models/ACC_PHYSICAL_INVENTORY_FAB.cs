using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class ACC_PHYSICAL_INVENTORY_FAB : BaseEntity
    {
        public int FPI_ID { get; set; }
        public int? ROLL_ID { get; set; }
        [Display(Name = "Trans. Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? FPI_DATE { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        [Display(Name = "Manual Entry?")]
        public bool IS_MANUAL { get; set; }

        [NotMapped]
        public string EncryptedId { get; set; }
        [NotMapped]
        [Display(Name = "Roll No")]
        [Required(ErrorMessage ="{0} can't be empty.")]
        public string ROLL_NO { get; set; }
        [NotMapped]
        [Display(Name = "Sl. No")]
        public int? INDEX { get; set; }
        [NotMapped]
        public string DateFormat
        {
            get
            {
                var dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper();
                return !string.IsNullOrEmpty(dateTimeFormat) ? dateTimeFormat : "DD-MMM-YY";
            }
        }

        public F_FS_FABRIC_RCV_DETAILS ROLL_ { get; set; }
    }
}
