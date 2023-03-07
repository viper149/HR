using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class COM_IMP_WORK_ORDER_MASTER : BaseEntity
    {
        public COM_IMP_WORK_ORDER_MASTER()
        {
            COM_IMP_WORK_ORDER_DETAILS = new HashSet<COM_IMP_WORK_ORDER_DETAILS>();
        }

        public int WOID { get; set; }
        [Display(Name = "Work Order Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? WODATE { get; set; }
        [Display(Name = "Contract No.")]
        public string CONTNO { get; set; }
        [Display(Name = "Indent No.")]
        [Required(ErrorMessage = "{0} must be selected")]
        public int? INDID { get; set; }
        [Display(Name = "Seller")]
        [Required(ErrorMessage = "{0} must be selected")]
        public int? SUPPID { get; set; }
        [Display(Name = "Supplier's Note")]
        public string REMARKS { get; set; }
        [Display(Name = "Validity Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? VALDATE { get; set; }
        [Display(Name = "Head of MKT Approve")]
        public bool BTL_APPROVE { get; set; }
        [Display(Name = "Costing Approve")]
        public bool FIN_APPROVE { get; set; }
        [Display(Name = "Revised")]
        public bool IS_REVISED { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        [Display(Name = "Buyer's Notes")]
        public string OPT3 { get; set; }

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

        public BAS_SUPPLIERINFO SUPP { get; set; }
        public F_YS_INDENT_MASTER IND { get; set; }
        public ICollection<COM_IMP_WORK_ORDER_DETAILS> COM_IMP_WORK_ORDER_DETAILS { get; set; }
    }
}
