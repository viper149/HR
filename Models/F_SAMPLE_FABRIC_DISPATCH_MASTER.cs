using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_SAMPLE_FABRIC_DISPATCH_MASTER : BaseEntity
    {
        public F_SAMPLE_FABRIC_DISPATCH_MASTER()
        {
            F_SAMPLE_FABRIC_DISPATCH_DETAILS = new HashSet<F_SAMPLE_FABRIC_DISPATCH_DETAILS>();
            H_SAMPLE_FABRIC_RECEIVING_M = new HashSet<H_SAMPLE_FABRIC_RECEIVING_M>();
        }

        public int DPID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [Display(Name = "Gate Pass No")]
        public int? GPNO { get; set; }
        [Display(Name = "Gate Pass Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? GPDATE { get; set; }
        public int? GPTYPEID { get; set; }
        public int? DRID { get; set; }
        public int? VID { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        [Display(Name = "Cancel")]
        public bool IS_CANCELLED { get; set; }
        [Display(Name = "Canceled Reason")]
        public string CAUSE_OF_CANCEL { get; set; }

        [NotMapped]
        public string DateFormat
        {
            get
            {
                var dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper();
                return !string.IsNullOrEmpty(dateTimeFormat) ? dateTimeFormat : "DD-MMM-YY";
            }
        }

        public F_BAS_DRIVERINFO DR { get; set; }
        public GATEPASS_TYPE GPTYPE { get; set; }
        public F_BAS_VEHICLE_INFO V { get; set; }

        public ICollection<F_SAMPLE_FABRIC_DISPATCH_DETAILS> F_SAMPLE_FABRIC_DISPATCH_DETAILS { get; set; }
        public ICollection<H_SAMPLE_FABRIC_RECEIVING_M> H_SAMPLE_FABRIC_RECEIVING_M { get; set; }
    }
}
