using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace DenimERP.Models
{
    public partial class H_SAMPLE_FABRIC_RECEIVING_M
    {
        public H_SAMPLE_FABRIC_RECEIVING_M()
        {
            H_SAMPLE_FABRIC_RECEIVING_D = new HashSet<H_SAMPLE_FABRIC_RECEIVING_D>();
        }

        public int RCVID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [Display(Name = "Gate Pass No")]
        [Required(ErrorMessage = "The field {0} can not be empty")]
        public int DPID { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Receive Date")]
        public DateTime? RCVDATE { get; set; }
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

        public F_SAMPLE_FABRIC_DISPATCH_MASTER FSampleFabricDispatchMaster { get; set; }

        public ICollection<H_SAMPLE_FABRIC_RECEIVING_D> H_SAMPLE_FABRIC_RECEIVING_D { get; set; }
    }
}
