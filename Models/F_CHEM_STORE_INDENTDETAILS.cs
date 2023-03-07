using System;
using System.ComponentModel.DataAnnotations;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_CHEM_STORE_INDENTDETAILS : BaseEntity
    {
        public int TRNSID { get; set; }
        [Display(Name = "Trans. Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? TRNSDATE { get; set; }
        [Display(Name = "Chem. Indent No.")]
        public int? CINDID { get; set; }
        [Display(Name = "Indent No.")]
        public int? INDSLID { get; set; }
        [Display(Name = "Chemical Name")]
        public int? PRODUCTID { get; set; }
        [Display(Name = "Unit")]
        public int? UNIT { get; set; }
        [Display(Name = "Quantity"/*, Prompt = "0.00"*/)]
        //[Range(1.0, double.MaxValue, ErrorMessage = "The field {0} must be greater than {1}")]
        [Required(ErrorMessage = "The field {0} can not be empty.")]
        public double? QTY { get; set; }
        [Display(Name = "Validity")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? VALIDITY { get; set; }
        [Display(Name = "Full Quantity")]
        public string FULL_QTY { get; set; }
        [Display(Name = "Add Quantity")]
        public string ADD_QTY { get; set; }
        [Display(Name = "Balance")]
        public string BAL_QTY { get; set; }
        [Display(Name = "Location")]
        public string LOCATION { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }

        public F_BAS_UNITS FBasUnits { get; set; }
        public F_CHEM_STORE_PRODUCTINFO PRODUCT { get; set; }
        public F_CHEM_STORE_INDENTMASTER TRNS { get; set; }
        public F_CHEM_PURCHASE_REQUISITION_MASTER INDSL { get; set; }
    }
}