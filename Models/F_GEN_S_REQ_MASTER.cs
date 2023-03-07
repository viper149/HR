using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_GEN_S_REQ_MASTER : BaseEntity
    {
        public F_GEN_S_REQ_MASTER()
        {
            F_GEN_S_ISSUE_MASTER = new HashSet<F_GEN_S_ISSUE_MASTER>();
            F_GEN_S_REQ_DETAILS = new HashSet<F_GEN_S_REQ_DETAILS>();
        }

        public int GSRID { get; set; }
        [Display(Name = "Requirement No.")]
        public string GSRNO { get; set; }
        [Display(Name = "Trans. Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? GSRDATE { get; set; }
        [Display(Name = "Requisition By")]
        [Required(ErrorMessage = "{0} must be selected.")]
        public int? REQUISITIONBY { get; set; }
        [Display(Name = "Department")]
        public int? DEPTID { get; set; }
        [Display(Name = "Section")]
        public int? SECID { get; set; }
        [Display(Name = "Sub Section")]
        public int? SSECID { get; set; }
        [Display(Name = "Status")]
        public bool? STATUS { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }

        [NotMapped]
        public string EncryptedId { get; set; }
        [NotMapped]
        public bool IsLocked { get; set; }
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

        public ICollection<F_GEN_S_ISSUE_MASTER> F_GEN_S_ISSUE_MASTER { get; set; }
        public ICollection<F_GEN_S_REQ_DETAILS> F_GEN_S_REQ_DETAILS { get; set; }
    }
}
