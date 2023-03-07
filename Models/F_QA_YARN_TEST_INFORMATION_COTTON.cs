using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_QA_YARN_TEST_INFORMATION_COTTON : BaseEntity
    {
        public int TESTID { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Yarn Test Date")]
        public DateTime? TESTDATE { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }

        [Display(Name = "Challan No - Indent No")]
        [Required(ErrorMessage = "{0} must be selected.")]
        public int? YRCVID { get; set; }
        [Display(Name = "Count Name - Lot - Supplier - Rcv Qty")]
        [Required(ErrorMessage = "{0} must be selected.")]
        public int? COUNTID { get; set; }
        [Display(Name = "Actual Count(Ne)")]
        public double? COUNT_ACT { get; set; }
        [Display(Name = "Count CV (%)")]
        public double? COUNT_CV { get; set; }
        [Display(Name = "Lea Strength (LB)")]
        public double? LEA_STR_LB { get; set; }
        [Display(Name = "Lea Strength (CV)")]
        public double? LEA_STR_CV { get; set; }
        public double? CSP { get; set; }
        [Display(Name = "Single Strength (CN)")]
        public double? SINGLE_STR_CN { get; set; }
        [Display(Name = "Strength CV (%)")]
        public double? STR_CV { get; set; }
        [Display(Name = "Tenacity (CN/Tex)")]
        public double? TENACITY { get; set; }
        [Display(Name = "Elongation (%)")]
        public double? ELONGATION { get; set; }
        [Display(Name = "Elongation CV (%)")]
        public double? ELONGATION_CV { get; set; }
        [Display(Name = "Cone Length(meter)")]
        public double? CONE_LEN { get; set; }
        public double? TPI { get; set; }
        public double? TM { get; set; }
        [Display(Name = "Moisture (%)")]
        public double? MOISTURE { get; set; }
        [Display(Name = "Spandex (%)")]
        public double? SPAN_AGE { get; set; }
        [Display(Name = "Nominal Draft")]
        public double? NOMI_DRAFT { get; set; }
        [Display(Name = "U (%)")]
        public double? U { get; set; }
        [Display(Name = "Thin (-50%)")]
        public double? THIN_MINUS_50 { get; set; }
        [Display(Name = "Thick (+50%)")]
        public double? THIK_PLUS_50 { get; set; }
        [Display(Name = "Neps  (+200%)")]
        public double? NEPS_200 { get; set; }
        [Display(Name = "Neps (+280%)")]
        public double? NEPS_280 { get; set; }
        [Display(Name = "Hairiness")]
        public double? HAIRINESS { get; set; }
        public double? IPI { get; set; }
        [Display(Name = "TM in UR")]
        public double? TM_TPI { get; set; }
        public double? IQC_STATUS { get; set; }
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
        public F_YS_YARN_RECEIVE_MASTER YRCV { get; set; }
        public F_YS_YARN_RECEIVE_DETAILS YRDT { get; set; }
    }
}
