using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_CHEM_REQ_MASTER : BaseEntity
    {
        public F_CHEM_REQ_MASTER()
        {
            F_CHEM_REQ_DETAILS = new HashSet<F_CHEM_REQ_DETAILS>();
            F_CHEM_ISSUE_MASTER = new HashSet<F_CHEM_ISSUE_MASTER>();
        }

        public int CSRID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [Display(Name = "Requirement No.")]
        public string CSRNO { get; set; }
        [Display(Name = "Rec. Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? CSRDATE { get; set; }
        [Display(Name = "Requisition By")]
        public int? REQUISITIONBY { get; set; }
        [Display(Name = "Department")]
        //[Required(ErrorMessage = "The field {0} can not be empty.")]
        public int? DEPTID { get; set; }
        [Display(Name = "Section No")]
        //[Required(ErrorMessage = "The field {0} can not be empty.")]
        public int? SECID { get; set; }
        [Display(Name = "Sub Section Id")]
        public int? SSECID { get; set; }
        [Required]
        [Display(Name = "SR(Khata) No")]
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }

        [NotMapped]
        public string DateFormat
        {
            get
            {
                var dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper();
                return !string.IsNullOrEmpty(dateTimeFormat) ? dateTimeFormat : "DD-MMM-YY";
            }
        }
        
        [NotMapped]
        public bool IsLocked { get; set; }

        public F_BAS_DEPARTMENT DEPT { get; set; }
        public F_BAS_SECTION FBasSection { get; set; }
        public F_BAS_SUBSECTION FBasSubsection { get; set; }
        public F_HRD_EMPLOYEE RequisitionEmployee { get; set; }

        public ICollection<F_CHEM_REQ_DETAILS> F_CHEM_REQ_DETAILS { get; set; }
        public ICollection<F_CHEM_ISSUE_MASTER> F_CHEM_ISSUE_MASTER { get; set; }
    }
}
