using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_GS_GATEPASS_RETURN_RCV_MASTER : BaseEntity
    {
        public F_GS_GATEPASS_RETURN_RCV_MASTER()
        {
            F_GS_GATEPASS_RETURN_RCV_DETAILS = new HashSet<F_GS_GATEPASS_RETURN_RCV_DETAILS>();
        }

        public int RCVID { get; set; }
        [Display(Name = "Trans. Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? RCVDATE { get; set; }
        [Display(Name = "Gate Pass Issue No.")]
        [Required(ErrorMessage = "{0} must be selected")]
        public int? GPID { get; set; }
        [Display(Name = "Received By")]
        [Required(ErrorMessage = "{0} must be selected")]
        public int? RCVD_BY { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }

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

        public F_GS_GATEPASS_INFORMATION_M GP { get; set; }
        public F_HRD_EMPLOYEE RCVD_BYNavigation { get; set; }
        public ICollection<F_GS_GATEPASS_RETURN_RCV_DETAILS> F_GS_GATEPASS_RETURN_RCV_DETAILS { get; set; }
    }
}
