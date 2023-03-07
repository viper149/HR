using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_PR_WEAVING_PRODUCTION : BaseEntity
    {
       
        public int WV_PRODID { get; set; }
        [Display(Name = "Loom Type")]
        public int? LOOMID { get; set; }
        [Display(Name = "Style Name")]
        public int? FABCODE { get; set; }
        [Display(Name = "SO No")]
        public int? POID { get; set; }
        [Display(Name = "Shift Incharge Name")]
        public int? EMPID { get; set; }
        [Display(Name = "Total Running Loom")]
        public double? TOTAL_RUN_LOOM { get; set; }
        [Display(Name = "Total Production(m)")]
        public double? TOTAL_PROD { get; set; }
        [Display(Name = "Total Efficiency")]
        public double? TOTAL_EFFECIENCE { get; set; }
        [Display(Name = "Total warp Stop")]
        public double? TOTAL_WRPSTOP { get; set; }
        [Display(Name = "Total Weft Stop")]
        public double? TOTAL_WFTSTOP { get; set; }
        [Display(Name = "Total RPM")]
        public double? TOTAL_RPM { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        [Display(Name = "Shift")]
        public string OPT1 { get; set; }
        [Display(Name = "Production Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? OPT2 { get; set; }
        [Display(Name = "Order Type")]
        public string OPT3 { get; set; }
        public string OPT4 { get; set; }
        public string OPT5 { get; set; }


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

        public RND_FABRICINFO FABCODENavigation { get; set; }
        public LOOM_TYPE LOOM { get; set; }
        public RND_PRODUCTION_ORDER PO { get; set; }
        public F_HRD_EMPLOYEE EMP { get; set; }
    }
}
