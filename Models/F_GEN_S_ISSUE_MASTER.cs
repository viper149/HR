using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_GEN_S_ISSUE_MASTER : BaseEntity
    {
        public F_GEN_S_ISSUE_MASTER()
        {
            F_GEN_S_ISSUE_DETAILS = new HashSet<F_GEN_S_ISSUE_DETAILS>();
        }

        [Display(Name = "Issue Number")]
        public int GISSUEID { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Issue Date")]
        public DateTime? GISSUEDATE { get; set; }
        [Display(Name = "Issue Type")]
        [Required(ErrorMessage = "{0} must be selected.")]
        public int? ISSUEID { get; set; }
        [Display(Name = "Issued By")]
        public int? ISSUEBY { get; set; }
        [Display(Name = "Received By")]
        public int? RECEIVEBY { get; set; }
        [Display(Name = "Requirement No.")]
        public int? GSRID { get; set; }
        [Display(Name = "Issue To")]
        public string ISSUETO { get; set; }
        [Display(Name = "Purpose")]
        public string PURPOSE { get; set; }
        [Display(Name = "Is Returnable?")]
        public bool ISRETURNABLE { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        [Display(Name = "SR No.")]
        public int? SRNO { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "SR Date")]
        public DateTime? SRDATE { get; set; }
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

        [NotMapped]
        public bool IsLocked { get; set; }

        public F_GEN_S_REQ_MASTER GSR { get; set; }
        public F_BAS_ISSUE_TYPE ISSUE { get; set; }
        public F_HRD_EMPLOYEE ISSUEBYNavigation { get; set; }
        public F_HRD_EMPLOYEE RECEIVEBYNavigation { get; set; }

        public ICollection<F_GEN_S_ISSUE_DETAILS> F_GEN_S_ISSUE_DETAILS { get; set; }
    }
}
