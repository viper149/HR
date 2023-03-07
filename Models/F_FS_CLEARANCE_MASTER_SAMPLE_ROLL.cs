using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_FS_CLEARANCE_MASTER_SAMPLE_ROLL : BaseEntity
    {
        public int MSRID { get; set; }
        [Display(Name = "Trans. Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? MSRDATE { get; set; }
        [Display(Name = "Mail Rcv Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? MAILDATE { get; set; }
        [Display(Name = "Roll No.")]
        public int? ROLLID { get; set; }
        [Display(Name = "Program/Set No.")]
        public int? SETID { get; set; }
        public double? WEBW_B { get; set; }
        public double? WEBW_S { get; set; }
        public double? WEAW_B { get; set; }
        public double? WEAW_S { get; set; }
        public double? SWARP_B { get; set; }
        public double? SWARP_S { get; set; }
        public double? SWEFT_B { get; set; }
        public double? SWEFT_S { get; set; }
        public double? WIBW_B { get; set; }
        public double? WIBW_S { get; set; }
        public double? WIAW_B { get; set; }
        public double? WIAW_S { get; set; }
        public double? EPIBW_B { get; set; }
        public double? EPIBW_S { get; set; }
        public double? EPIAW_B { get; set; }
        public double? EPIAW_S { get; set; }
        public double? PPIBW_B { get; set; }
        public double? PPIBW_S { get; set; }
        public double? PPIAW_B { get; set; }
        public double? PPIAW_S { get; set; }
        public double? SPA_B { get; set; }
        public double? SPA_S { get; set; }
        public double? SPB_B { get; set; }
        public double? SPB_S { get; set; }
        public double? STWARP_B { get; set; }
        public double? STWARP_S { get; set; }
        public double? STWEFT_B { get; set; }
        public double? STWEFT_S { get; set; }
        public string CSV_B { get; set; }
        public string CSV_S { get; set; }
        public string APPEAR_B { get; set; }
        public string APPEAR_S { get; set; }
        public int? SHADE_B { get; set; }
        public int? SHADE_S { get; set; }
        [Display(Name = "Wash Type")]
        public int? WTID { get; set; }
        [Display(Name = "Selection For")]
        public int? RTID { get; set; }
        public string C_QUALITY { get; set; }
        public string C_RND { get; set; }
        public string C_DYING { get; set; }
        public string C_FINISHING { get; set; }
        public string C_HEAD { get; set; }
        public string C_GM { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }
        public string OPT4 { get; set; }
        public int? OPT5 { get; set; }

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

        public F_PR_INSPECTION_PROCESS_DETAILS ROLL { get; set; }
        public F_FS_CLEARANCE_ROLL_TYPE RT { get; set; }
        public PL_PRODUCTION_SETDISTRIBUTION SET { get; set; }
        public F_FS_CLEARANCE_WASH_TYPE WT { get; set; }
    }
}
