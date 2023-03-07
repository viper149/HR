using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_GEN_S_PURCHASE_REQUISITION_MASTER : BaseEntity
    {
        public F_GEN_S_PURCHASE_REQUISITION_MASTER()
        {
            F_GEN_S_INDENTDETAILS = new HashSet<F_GEN_S_INDENTDETAILS>();
            F_GEN_S_INDENTMASTER = new HashSet<F_GEN_S_INDENTMASTER>();
        }

        [Display(Name = "Requisition Number")]
        public int INDSLID { get; set; }
        [Display(Name = "Requisition Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? INDSLDATE { get; set; }
        [Display(Name = "Department")]
        public int? DEPTID { get; set; }
        [Display(Name = "Section")]
        public int? SECID { get; set; }
        [Display(Name = "Sub Section")]
        public int? SSECID { get; set; }
        [Display(Name = "Employee")]
        [Required(ErrorMessage = "{0} must be selected.")]
        public int? EMPID { get; set; }
        [Display(Name = "Concern Person")]
        [Required(ErrorMessage = "{0} must be selected.")]
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
        [NotMapped]
        public bool IsLocked { get; set; }

        public F_HRD_EMPLOYEE CN_PERSONNavigation { get; set; }
        public F_HRD_EMPLOYEE EMP { get; set; }

        public ICollection<F_GEN_S_INDENTDETAILS> F_GEN_S_INDENTDETAILS { get; set; }
        public ICollection<F_GEN_S_INDENTMASTER> F_GEN_S_INDENTMASTER { get; set; }
    }
}
