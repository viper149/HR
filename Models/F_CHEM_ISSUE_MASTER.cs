using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_CHEM_ISSUE_MASTER : BaseEntity
    {
        public F_CHEM_ISSUE_MASTER()
        {
            F_CHEM_ISSUE_DETAILS = new HashSet<F_CHEM_ISSUE_DETAILS>();
        }

        [Display(Name = "Issue Number")]
        public int CISSUEID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Issue Date")]
        public DateTime? CISSUEDATE { get; set; }
        [Display(Name = "Issue Type")]
        [Required(ErrorMessage = "The filed {0} can not be empty.")]
        public int? ISSUEID { get; set; }
        [Display(Name = "Issue By")]
        public int? ISSUEBY { get; set; }
        [Display(Name = "Receive By")]
        public int? RECEIVEBY { get; set; }
        [Display(Name = "Requirement No.")]
        //[Required(ErrorMessage = "The filed {0} can not be empty.")]
        public int? CSRID { get; set; }
        [Display(Name = "Issue To")]
        public string ISSUETO { get; set; }
        [Display(Name = "Purpose")]
        public string PURPOSE { get; set; }
        [Display(Name = "Is Returnable")]
        public bool ISRETURNABLE { get; set; }
        [Display(Name = "Remarks")]
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

        public F_BAS_ISSUE_TYPE CISSUE { get; set; }
        public F_CHEM_REQ_MASTER CSR { get; set; }
        public F_HRD_EMPLOYEE IssueFHrdEmployee { get; set; }
        public F_HRD_EMPLOYEE ReceiveFHrdEmployee { get; set; }

        public ICollection<F_CHEM_ISSUE_DETAILS> F_CHEM_ISSUE_DETAILS { get; set; }
    }
}