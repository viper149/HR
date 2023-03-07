using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_QA_YARN_TEST_INFORMATION_POLYESTER : BaseEntity
    {
        public int TESTID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Yarn Test Date")]
        public DateTime TESTDATE { get; set; }
        [Display(Name = "Challan No - Indent No")]
        [Required(ErrorMessage = "{0} must be selected.")]
        public int? YRCVID { get; set; }
        [Display(Name = "Count Name - Lot - Supplier - Rcv Qty")]
        [Required(ErrorMessage = "{0} must be selected.")]
        public int? COUNTID { get; set; }
        [Display(Name = "Shade Color")]
        public int? COLORCODE { get; set; }
        [Display(Name = "Actual Count(Denier)")]
        public double? DENIER_ACT { get; set; }
        [Display(Name = "Tenacity(Gpd)")]
        public double? TENACITY_GPD { get; set; }
        [Display(Name = "Extension (%)")]
        public double? EXTENSION { get; set; }
        [Display(Name = "Actual NIP/Meter ")]
        public double? NIP_MTR_ACT { get; set; }
        [Display(Name = "Spandex(%)")]
        public double? SPANDEX_AGE { get; set; }
        [Display(Name = "Nominal Draft")]
        public double? NOMINAL_DRAFT { get; set; }
        [Display(Name = "Act. No of Filaments")]
        public int? ACTUAL_NO_FILAMENTS { get; set; }
        [Display(Name = "Package Weight (Kg)")]
        public double? PACK_WGT { get; set; }
        [Display(Name = "NIP Retention Strength")]
        public string NIP_RETENTION_STRENGTH { get; set; }
        [Display(Name = "LIM/HIM")]
        public string LIMHIM { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }
        [NotMapped]
        public string DateFormat
        {
            get
            {
                var dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper();
                return !string.IsNullOrEmpty(dateTimeFormat) ? dateTimeFormat : "DD-MMM-YY";
            }
        }
        public BAS_COLOR COLOR { get; set; }
        public F_YS_YARN_RECEIVE_DETAILS YRDT { get; set; }
        public F_YS_YARN_RECEIVE_MASTER YRCV { get; set; }
    }
}
