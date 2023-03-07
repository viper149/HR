using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace DenimERP.Models
{
    public partial class F_PR_WEAVING_OS
    {
        public int OSID { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? OSDATE { get; set; }
        [Display(Name = "Order No")]
        public int? POID { get; set; }
        [Display(Name = "Set No")]
        public int? SETID { get; set; }
        [Display(Name = "Count Name")]
        public int? COUNTID { get; set; }
        [Display(Name = "Lot")]
        public int? LOTID { get; set; }
        [Display(Name = "Opening Stock")]
        public string OS { get; set; }
        public string REMARKS { get; set; }
        public int? OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? CREATED_AT { get; set; }
        public string UPDATED_BY { get; set; }
        public DateTime? UPDATED_AT { get; set; }

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
        public BAS_YARN_COUNTINFO COUNT { get; set; }
        public BAS_YARN_LOTINFO LOT { get; set; }
        public RND_PRODUCTION_ORDER PO { get; set; }
        public PL_PRODUCTION_SETDISTRIBUTION SET { get; set; }
    }
}
