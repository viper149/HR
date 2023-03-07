using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_GS_GATEPASS_INFORMATION_M : BaseEntity
    {
        public F_GS_GATEPASS_INFORMATION_M()
        {
            F_GS_GATEPASS_INFORMATION_D = new HashSet<F_GS_GATEPASS_INFORMATION_D>();
            F_GS_RETURNABLE_GP_RCV_M = new HashSet<F_GS_RETURNABLE_GP_RCV_M>();
            F_GS_GATEPASS_RETURN_RCV_MASTER = new HashSet<F_GS_GATEPASS_RETURN_RCV_MASTER>();
        }

        public int GPID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Trans. Date")]
        public DateTime? GPDATE { get; set; }
        [Display(Name = "Gate Pass Issue No.")]
        public string GPNO { get; set; }
        [Display(Name = "Department")]
        public int? DEPTID { get; set; }
        [Display(Name = "Section")]
        public int? SECID { get; set; }
        [Display(Name = "Concern Person")]
        public int? EMPID { get; set; }
        [Display(Name = "Send To")]
        public string SENDTO { get; set; }
        [Display(Name = "Address")]
        public string ADDRESS { get; set; }
        [Display(Name = "Requested By")]
        public int? REQ_BY { get; set; }
        [Display(Name = "Gate Pass Type")]
        public int? GPTID { get; set; }
        [Display(Name = "Vehicle No.")]
        public int? VID { get; set; }
        [Display(Name = "Returnable")]
        public bool IS_RETURNABLE { get; set; }
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

        public F_HRD_EMPLOYEE EMP { get; set; }
        public F_HRD_EMPLOYEE EMP_REQBYNavigation { get; set; }
        public F_BAS_SECTION SEC { get; set; }
        public F_BAS_DEPARTMENT DEPT { get; set; }
        public F_BAS_VEHICLE_INFO V { get; set; }
        public F_GATEPASS_TYPE GPT { get; set; }
        public ICollection<F_GS_GATEPASS_INFORMATION_D> F_GS_GATEPASS_INFORMATION_D { get; set; }
        public ICollection<F_GS_RETURNABLE_GP_RCV_M> F_GS_RETURNABLE_GP_RCV_M { get; set; }
        public ICollection<F_GS_GATEPASS_RETURN_RCV_MASTER> F_GS_GATEPASS_RETURN_RCV_MASTER { get; set; }
    }
}
