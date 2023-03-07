using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace DenimERP.Models
{
    public partial class F_SAMPLE_FABRIC_RCV_M
    {
        public F_SAMPLE_FABRIC_RCV_M()
        {
            F_SAMPLE_FABRIC_RCV_D = new HashSet<F_SAMPLE_FABRIC_RCV_D>();
        }

        public int SFRID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Required(ErrorMessage = "The field {0} can not be empty.")]
        [Display(Name = "Fabric Receive Date")]
        public DateTime? SFRDATE { get; set; }
        public int? SECID { get; set; }
        public int? EMPID { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }
        [Display(Name = "SFTR No")]
        public string SFTRNO { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "SFTR Date")]
        public DateTime? SFTRDATE { get; set; }

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
        public F_HRD_EMPLOYEE EMP { get; set; }

        public ICollection<F_SAMPLE_FABRIC_RCV_D> F_SAMPLE_FABRIC_RCV_D { get; set; }
    }
}
