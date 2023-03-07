using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_CHEM_STORE_RECEIVE_DETAILS : BaseEntity
    {
        public F_CHEM_STORE_RECEIVE_DETAILS()
        {
            F_CHEM_QC_APPROVE = new HashSet<F_CHEM_QC_APPROVE>();
            F_CS_CHEM_RECEIVE_REPORT = new HashSet<F_CS_CHEM_RECEIVE_REPORT>();
            F_CHEM_TRANSECTION = new HashSet<F_CHEM_TRANSECTION>();
            F_CHEM_ISSUE_DETAILS = new HashSet<F_CHEM_ISSUE_DETAILS>();
        }

        public int TRNSID { get; set; }
        [Display(Name = "Trans. Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? TRNSDATE { get; set; }
        [Display(Name = "Receive No.")]
        public int? CHEMRCVID { get; set; }
        [Display(Name = "Chemical Name")]
        public int? PRODUCTID { get; set; }
        [Display(Name = "Adjusted With")]
        public int? ADJUSTED_WITH { get; set; }
        [NotMapped]
        [Display(Name = "QC Approve")]
        public string QC_APPROVE { get; set; }
        [NotMapped]
        [Display(Name = "MRR Create")]
        public string MRR_CREATE { get; set; }
        [Display(Name = "Unit")]
        public int? UNIT { get; set; }
        [Display(Name = "Indent No")]
        public int? CINDID { get; set; }
        [Display(Name = "Indent Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? CINDDATE { get; set; }
        [Display(Name = "Invoice Quantity")]
        public double? INVQTY { get; set; }
        [Display(Name = "Rate")]
        public double? RATE { get; set; }
        [Display(Name = "Currency")]
        public string CURRENCY { get; set; }
        [Display(Name = "Amount")]
        public double? AMOUNT { get; set; }
        [Display(Name = "Batch No")]
        public string BATCHNO { get; set; }
        [Display(Name = "Manufacture Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? MNGDATE { get; set; }
        [Display(Name = "Expire Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? EXDATE { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        public bool? ISQC { get; set; }
        [Display(Name = "MRR No")]
        public string MRRNO { get; set; }
        [Display(Name = "Fresh Quantity")]
        //[Required(ErrorMessage = "The field {0} can not be empty.")]
        //[Range(1, double.MaxValue, ErrorMessage = "The field {0} must between {1} ~ {2}")]
        public double? FRESH_QTY { get; set; }
        [Display(Name = "Rejection Quantity")]
        //[Range(1, double.MaxValue, ErrorMessage = "The field {0} must between {1} ~ {2}")]
        public double? REJ_QTY { get; set; }
        [Display(Name = "MRR Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? MRRDATE { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }

        public F_CHEM_STORE_RECEIVE_MASTER CHEMRCV { get; set; }
        public F_BAS_UNITS FBasUnits { get; set; }
        public F_CHEM_STORE_PRODUCTINFO FChemStoreProductinfo { get; set; }
        public ICollection<F_CHEM_QC_APPROVE> F_CHEM_QC_APPROVE { get; set; }
        public ICollection<F_CS_CHEM_RECEIVE_REPORT> F_CS_CHEM_RECEIVE_REPORT { get; set; }
        public ICollection<F_CHEM_TRANSECTION> F_CHEM_TRANSECTION { get; set; }
        public ICollection<F_CHEM_ISSUE_DETAILS> F_CHEM_ISSUE_DETAILS { get; set; }
    }
}
