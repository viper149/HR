using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class H_SAMPLE_DESPATCH_M : BaseEntity
    {
        public H_SAMPLE_DESPATCH_M()
        {
            H_SAMPLE_DESPATCH_D = new HashSet<H_SAMPLE_DESPATCH_D>();
        }

        public int SDID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Required(ErrorMessage = "Sample Dispatch Date Can Not Be Empty")]
        [Display(Name = "Sample Dispatch Date.")]
        public DateTime? SDDATE { get; set; }
        [Display(Name = "Gate Pass Number")]
        public int GPNO { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Required(ErrorMessage = "Gate Pass Date Can Not Be Empty")]
        [Display(Name = "Gate Pass Date")]
        public DateTime GPDATE { get; set; }
        [Required(ErrorMessage = "This Field Can Not Be Empty")]
        public int? HSPID { get; set; }
        public int? BRANDID { get; set; }
        [Display(Name = "Purpose")]
        public int? PURPOSE { get; set; }
        [Display(Name = "Status")]
        public string STATUS { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Return Date.")]
        public DateTime? RTNDATE { get; set; }
        [Required(ErrorMessage = "This Field Can Not Be Empty")]
        public int? HSTID { get; set; }
        [Display(Name = "Through")]
        public string THROUGH { get; set; }
        [Display(Name = "Cost Status")]
        public string COST_STATUS { get; set; }
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

        public BAS_BRANDINFO BRAND { get; set; }
        public BAS_BUYERINFO HSP { get; set; }
        public BAS_TEAMINFO HST { get; set; }
        public F_BAS_UNITS PURPOSENavigation { get; set; }
        public ICollection<H_SAMPLE_DESPATCH_D> H_SAMPLE_DESPATCH_D { get; set; }
    }
}
