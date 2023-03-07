using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_FS_WASTAGE_ISSUE_M :BaseEntity
    {
        public F_FS_WASTAGE_ISSUE_M()
        {
            F_FS_WASTAGE_ISSUE_D = new HashSet<F_FS_WASTAGE_ISSUE_D>();
        }

        public int WIID { get; set; }
        [Display(Name = "Wastage Issue Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? WIDATE { get; set; }
        [Display(Name = "Party Name")]
        public int? PID { get; set; }
        [Display(Name = "Gate Pass No.")]
        public int? GPNO { get; set; }
        [Display(Name = "Gate Pass Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? GPDATE { get; set; }
        [Display(Name = "Vehicle No.")]
        public int? VEHICLENO { get; set; }
        [Display(Name = "Through/Bearer")]
        public string THROUGH { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }

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


        public F_FS_WASTAGE_PARTY FFsWastageParty { get; set; }
        public ICollection<F_FS_WASTAGE_ISSUE_D> F_FS_WASTAGE_ISSUE_D { get; set; }
    }
}
