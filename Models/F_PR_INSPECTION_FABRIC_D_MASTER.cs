using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_PR_INSPECTION_FABRIC_D_MASTER : BaseEntity
    {
        public F_PR_INSPECTION_FABRIC_D_MASTER()
        {
            F_PR_INSPECTION_FABRIC_D_DETAILS = new HashSet<F_PR_INSPECTION_FABRIC_D_DETAILS>();
        }

        public int DID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Dispatch Date")]
        public DateTime? DDATE { get; set; }
        public int? SECID { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        [Display(Name = "FFTR No.")]
        public int? FFTR_NO { get; set; }
        public string OPT3 { get; set; }
        [Display(Name = "No of Rolls")]
        public int? NO_ROLL { get; set; }

        [NotMapped]
        public string DateFormat
        {
            get
            {
                var dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper();
                return !string.IsNullOrEmpty(dateTimeFormat) ? dateTimeFormat : "DD-MMM-YY";
            }
        }

        public F_BAS_SECTION SEC { get; set; }
        public ICollection<F_PR_INSPECTION_FABRIC_D_DETAILS> F_PR_INSPECTION_FABRIC_D_DETAILS { get; set; }

    }
}
