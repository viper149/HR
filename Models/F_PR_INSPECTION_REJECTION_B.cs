using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_PR_INSPECTION_REJECTION_B : BaseEntity
    {
        public int IBR_ID { get; set; }
        [Display(Name = "Trans. Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? TRANS_DATE { get; set; }
        [Display(Name = "Style Name")]
        [Required(ErrorMessage = "{0} must be selected")]
        public int? DOFF_ID { get; set; }
        [Display(Name = "Rejection Yds")]
        public double? REDECTION_YDS { get; set; }
        [Display(Name = "Section")]
        public int? SECTION_ID { get; set; }
        [Display(Name = "Defect")]
        public int? DEFECT_ID { get; set; }
        [Display(Name = "Doffing Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? DOFFING_DATE { get; set; }
        [Display(Name = "Doffing Length")]
        public double? DOFFING_LENGTH { get; set; }
        [Display(Name = "Shift")]
        public int? SHIFT { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }
        public string OPT4 { get; set; }
        public string OPT5 { get; set; }
        public string OPT6 { get; set; }
        [NotMapped]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Production Date")]
        public DateTime? SearchDate { get; set; }
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

        public F_PR_INSPECTION_DEFECTINFO DEFECT_ { get; set; }
        public F_PR_WEAVING_PROCESS_DETAILS_B DOFF_ { get; set; }
        public F_BAS_SECTION SECTION_ { get; set; }
        public F_HR_SHIFT_INFO SHIFTNavigation { get; set; }
    }
}
