using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_SAMPLE_DESPATCH_MASTER : BaseEntity
    {
        public F_SAMPLE_DESPATCH_MASTER()
        {
            F_SAMPLE_DESPATCH_DETAILS = new HashSet<F_SAMPLE_DESPATCH_DETAILS>();
            H_SAMPLE_RECEIVING_M = new HashSet<H_SAMPLE_RECEIVING_M>();
        }

        public int DPID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [Display(Name = "Gate Pass Number")]
        [Required]
        public int GPNO { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Required(ErrorMessage = "Please choose a specific date.")]
        [Display(Name = "Gate Pass Date")]
        public DateTime? GPDATE { get; set; }
        [Required(ErrorMessage = "Gate Pass Type Name Is Required")]
        public int? GPTYPEID { get; set; }
        [Required(ErrorMessage = "Driver Name Is Required")]
        public int? DRID { get; set; }
        [Required(ErrorMessage = "Vehicle Number Is Required")]
        public int? VID { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        public int? TYPEID { get; set; }

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
        public F_SAMPLE_DESPATCH_MASTER_TYPE FSampleDespatchMasterType { get; set; }

        public ICollection<F_SAMPLE_DESPATCH_DETAILS> F_SAMPLE_DESPATCH_DETAILS { get; set; }
        public ICollection<H_SAMPLE_RECEIVING_M> H_SAMPLE_RECEIVING_M { get; set; }
    }
}
