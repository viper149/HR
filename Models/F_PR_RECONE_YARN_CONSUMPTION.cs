using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_PR_RECONE_YARN_CONSUMPTION : BaseEntity
    {
        public int TRANSID { get; set; }
        [Display(Name = "Recone")]
        public int? RECONE_ID { get; set; }
        [Display(Name = "Count Name")]
        public int? COUNTID { get; set; }
        [Display(Name = "Consumption")]
        public double? CONSUMP { get; set; }
        [Display(Name = "Waste")]
        public double? WASTE { get; set; }
        [Display(Name = "Waste Percentage(%)")]
        public double? WASTE_PERCENT { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
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

        public F_PR_RECONE_MASTER RECONE_ { get; set; }
    }
}
