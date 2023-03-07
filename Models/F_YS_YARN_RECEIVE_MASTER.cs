using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_YS_YARN_RECEIVE_MASTER : BaseEntity
    {
        public F_YS_YARN_RECEIVE_MASTER()
        {
            F_YS_YARN_RECEIVE_DETAILS = new HashSet<F_YS_YARN_RECEIVE_DETAILS>();
            F_QA_YARN_TEST_INFORMATION_COTTON = new HashSet<F_QA_YARN_TEST_INFORMATION_COTTON>();
            F_QA_YARN_TEST_INFORMATION_POLYESTER = new HashSet<F_QA_YARN_TEST_INFORMATION_POLYESTER>();
        }

        public int YRCVID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Yarn Receive Date")]
        public DateTime? YRCVDATE { get; set; }
        [Display(Name = "Receive No")]
        public int? RCVTID { get; set; }
        [Display(Name = "Invoice No.")]
        public int? INVID { get; set; }

        [Display(Name = "Section")]
        public int? SECID { get; set; }
        [Display(Name = "Sub Section")]
        public int? SUBSECID { get; set; }
        [Display(Name = "Challan No.")]
        public string CHALLANNO { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Challan Date")]
        public DateTime? CHALLANDATE { get; set; }
        [Display(Name = "Truck No.")]
        public string TUCK_NO { get; set; }
        [Display(Name = "Supplier")]
        public int? RCVFROM { get; set; }
        [Display(Name = "Is Returnable")]
        public bool ISRETURNABLE { get; set; }
        [Display(Name = "Currier No.")]
        public string COMMENTS { get; set; }
        [Display(Name = "Gate Entry No.")]
        public string G_ENTRY_NO { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Gate Entry Date")]
        public DateTime? G_ENTRY_DATE { get; set; }
        //[Display(Name = "Order No.", Prompt = "Order Number")]
        //public int? ORDER_NO { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }
        [NotMapped]
        public string OPT4 { get; set; }
        [NotMapped]
        public string OPT5 { get; set; }
        [NotMapped]
        public string OPT6 { get; set; }
        [NotMapped]
        public string OPT7 { get; set; }
        [NotMapped]
        public string OPT8 { get; set; }
        public int? INDID { get; set; }
        [Required(ErrorMessage = "Please Select Indent Type First")]
        [Display(Name = "Prod. Type")]
        public int INDENT_TYPE { get; set; }
        
        [NotMapped]
        public string DateFormat
        {
            get
            {
                var dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper();
                return !string.IsNullOrEmpty(dateTimeFormat) ? dateTimeFormat : "DD-MMM-YY";
            }
        }

        public F_YS_INDENT_MASTER IND { get; set; }
        public COM_IMP_INVOICEINFO INV { get; set; }
        public F_BAS_RECEIVE_TYPE RCVT { get; set; }
        public BAS_SUPPLIERINFO SUPP { get; set; }
        public F_BAS_SECTION SEC { get; set; }
        public F_BAS_SUBSECTION SUBSEC { get; set; }

        public ICollection<F_YS_YARN_RECEIVE_DETAILS> F_YS_YARN_RECEIVE_DETAILS { get; set; }
        public ICollection<F_QA_YARN_TEST_INFORMATION_COTTON> F_QA_YARN_TEST_INFORMATION_COTTON { get; set; }
        public ICollection<F_QA_YARN_TEST_INFORMATION_POLYESTER> F_QA_YARN_TEST_INFORMATION_POLYESTER { get; set; }
    }
}
