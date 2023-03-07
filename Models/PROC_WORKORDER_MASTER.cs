using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace DenimERP.Models
{
    public partial class PROC_WORKORDER_MASTER
    {
        public PROC_WORKORDER_MASTER()
        {
            PROC_WORKORDER_DETAILS = new HashSet<PROC_WORKORDER_DETAILS>();
        }

        public int WOID { get; set; }
        [Display (Name = "Order Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? WODATE { get; set; }
        [Display(Name = "Party Name")]
        public int? SUPPID { get; set; }
        [Display(Name = "Pay Mode")]
        public string PAYMODE { get; set; }
        [Display(Name = "Unit")]
        public string UNIT { get; set; }
        [Display(Name = "Currency")]
        public string CURRENCY { get; set; }
        [Display(Name = "Carrying Amount")]
        public double? CARRING_AMT { get; set; }
        [Display(Name = "Discount Amount")]
        public double? DISC_AMT { get; set; }
        [Display(Name = "Less Amount")]
        public double? LESS_AMT { get; set; }
        [Display(Name = "Special Discount Amount")]
        public double? SPC_DISC_AMT { get; set; }
        [Display(Name = "Discount Rate %")]
        public double? DISC_RATE { get; set; }
        [Display(Name = "Pay Amount")]
        public double? PAY_AMT { get; set; }
        [Display(Name = "Add Amount")]
        public double? ADD_AMT { get; set; }
        [Display(Name = "Vat %")]
        public double? VAT { get; set; }
        [Display(Name = "Freight Cost")]
        public double? FREIGHT_COST { get; set; }
        [Display(Name = "Freight")]
        public string FREIGHT { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }
        public string OPT4 { get; set; }
        public string OPT5 { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? CREATED_AT { get; set; }
        public string UPDATED_BY { get; set; }
        public DateTime? UPDATED_AT { get; set; }


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
        public ICollection<PROC_WORKORDER_DETAILS> PROC_WORKORDER_DETAILS { get; set; }
    }
}
