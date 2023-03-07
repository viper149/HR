using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_GEN_S_INDENTMASTER : BaseEntity
    {
        public F_GEN_S_INDENTMASTER()
        {
            F_GEN_S_INDENTDETAILS = new HashSet<F_GEN_S_INDENTDETAILS>();
            F_GEN_S_RECEIVE_DETAILS = new HashSet<F_GEN_S_RECEIVE_DETAILS>();
            F_GEN_S_RECEIVE_MASTER = new HashSet<F_GEN_S_RECEIVE_MASTER>();
        }

        public int GINDID { get; set; }
        [Display(Name = "Indent Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? GINDDATE { get; set; }
        [Display(Name = "Indent No.")]
        public string GINDNO { get; set; }
        [Display(Name = "Indent Sl. No")]
        public int? INDSLID { get; set; }
        [Display(Name = "Indent Type")]
        public int? INDTYPE { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }
        public string OPT4 { get; set; }
        [Display(Name = "Status")]
        public bool STATUS { get; set; }

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


        public F_GEN_S_PURCHASE_REQUISITION_MASTER INDSL { get; set; }
        public F_GEN_S_INDENT_TYPE INDTYPENavigation { get; set; }

        public ICollection<F_GEN_S_INDENTDETAILS> F_GEN_S_INDENTDETAILS { get; set; }
        public ICollection<F_GEN_S_RECEIVE_DETAILS> F_GEN_S_RECEIVE_DETAILS { get; set; }
        public ICollection<F_GEN_S_RECEIVE_MASTER> F_GEN_S_RECEIVE_MASTER { get; set; }
    }
}
