using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_YS_INDENT_MASTER : BaseEntity
    {
        public F_YS_INDENT_MASTER()
        {
            F_YS_INDENT_DETAILS = new HashSet<F_YS_INDENT_DETAILS>();
            COM_IMP_CSINFO = new HashSet<COM_IMP_CSINFO>();
            F_YS_YARN_RECEIVE_MASTER = new HashSet<F_YS_YARN_RECEIVE_MASTER>();
            COM_IMP_WORK_ORDER_MASTER = new HashSet<COM_IMP_WORK_ORDER_MASTER>();
            F_YS_YARN_RECEIVE_MASTER_S = new HashSet<F_YS_YARN_RECEIVE_MASTER_S>();
            PROC_WORKORDER_DETAILS = new HashSet<PROC_WORKORDER_DETAILS>();
        }

        public int INDID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [Display(Name = "Indent No.")]
        public string INDNO { get; set; }
        [Display(Name = "Indent Sl. No.")]
        public int? INDSLID { get; set; }
        [Display(Name = "Indent Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMMM yyyy}")]
        public DateTime? INDDATE { get; set; }
        [Display(Name = "SO/SDRF No.")]
        public string OPT1 { get; set; }
        [Display(Name = "Style")]
        public string OPT2 { get; set; }
        [Display(Name = "Indent No.")]
        public string OPT3 { get; set; }
        public string OPT4 { get; set; }
        [Display(Name = "Indent No.")]
        public string OPT5 { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        [NotMapped]
        [Display(Name = "Is Revised?")]
        public string IsRevised { get; set; }
        //[NotMapped]
        //[DisplayName("INDENT SL. NO.")]
        //public string INDSLID_STRING_FYSIM { get; set; }

        [NotMapped]
        public string DateFormat
        {
            get
            {
                var dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper();
                return !string.IsNullOrEmpty(dateTimeFormat) ? dateTimeFormat : "DD-MMM-YY";
            }
        }

        public RND_PURCHASE_REQUISITION_MASTER INDSL { get; set; }

        public ICollection<F_YS_INDENT_DETAILS> F_YS_INDENT_DETAILS { get; set; }
        public ICollection<COM_IMP_CSINFO> COM_IMP_CSINFO { get; set; }
        public ICollection<F_YS_YARN_RECEIVE_MASTER> F_YS_YARN_RECEIVE_MASTER { get; set; }
        public ICollection<COM_IMP_WORK_ORDER_MASTER> COM_IMP_WORK_ORDER_MASTER { get; set; }
        public ICollection<F_YS_YARN_RECEIVE_MASTER_S> F_YS_YARN_RECEIVE_MASTER_S { get; set; }
        public ICollection<PROC_WORKORDER_DETAILS> PROC_WORKORDER_DETAILS { get; set; }

    }
}
