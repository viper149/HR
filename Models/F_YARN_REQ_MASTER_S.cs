using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_YARN_REQ_MASTER_S : BaseEntity
    {
        public F_YARN_REQ_MASTER_S()
        {
            F_YARN_REQ_DETAILS_S = new HashSet<F_YARN_REQ_DETAILS_S>();
            F_YS_YARN_ISSUE_MASTER_S = new HashSet<F_YS_YARN_ISSUE_MASTER_S>();
        }

        public int YSRID { get; set; }
        [Display(Name = "Yarn Req. No.")]
        public string YSRNO { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Trans. Date")]
        public DateTime? YSRDATE { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }

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

        public ICollection<F_YARN_REQ_DETAILS_S> F_YARN_REQ_DETAILS_S { get; set; }
        public ICollection<F_YS_YARN_ISSUE_MASTER_S> F_YS_YARN_ISSUE_MASTER_S { get; set; }
    }
}
