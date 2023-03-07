using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_CHEM_PURCHASE_REQUISITION_MASTER : BaseEntity
    {
        public F_CHEM_PURCHASE_REQUISITION_MASTER()
        {
            F_CHEM_STORE_INDENTMASTER = new HashSet<F_CHEM_STORE_INDENTMASTER>();
            F_CHEM_STORE_INDENTDETAILS = new HashSet<F_CHEM_STORE_INDENTDETAILS>();
        }

        [Display(Name = "Requisition Number")]
        public int INDSLID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [Display(Name = "Requisition Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? INDSLDATE { get; set; }
        [Display(Name = "Department")]
        [Required(ErrorMessage = "The filed {0} can not be empty.")]
        public int? DEPTID { get; set; }
        [Display(Name = "Section")]
        [Required(ErrorMessage = "The field {0} can not be empty.")]
        public int? SECID { get; set; }
        [Display(Name = "Sub Section Id")]
        [Required(ErrorMessage = "The field {0} can not be empty.")]
        public int? SSECID { get; set; }
        [Display(Name = "Employee")]
        public int? EMPID { get; set; }
        [Display(Name = "Concern Person")]
        public int? CN_PERSON { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        [Display(Name = "Status")]
        public bool STATUS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }
        public string OPT4 { get; set; }

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

        public F_BAS_DEPARTMENT FBasDepartment { get; set; }
        public F_BAS_SECTION FBasSection { get; set; }
        public F_BAS_SUBSECTION FBasSubsection { get; set; }
        public F_HRD_EMPLOYEE Employee { get; set; }
        public F_HRD_EMPLOYEE ConcernEmployee { get; set; }
        public ICollection<F_CHEM_STORE_INDENTMASTER> F_CHEM_STORE_INDENTMASTER { get; set; }
        public ICollection<F_CHEM_STORE_INDENTDETAILS> F_CHEM_STORE_INDENTDETAILS { get; set; }
    }
}
