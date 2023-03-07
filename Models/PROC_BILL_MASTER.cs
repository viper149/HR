using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace DenimERP.Models
{
    public partial class PROC_BILL_MASTER
    {
        public PROC_BILL_MASTER()
        {
            PROC_BILL_DETAILS = new HashSet<PROC_BILL_DETAILS>();

        }

        public int BILLID { get; set; }
        [Display(Name="Bill Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? BILLDATE { get; set; }
        [Display(Name = "Challan No.")]
        public int? CHALLANID { get; set; }
        [Display(Name = "Source")]
        public string SOURCE { get; set; }
        [Display(Name = "Pay Mode")]
        public string PAYMODE { get; set; }
        [Display(Name = "Bill Ammount")]
        public double? BILLAMOUNT { get; set; }
        [Display(Name = "Discount %")]
        public double? DISCOUUNT { get; set; }
        [Display(Name = "Vat %")]
        public double? VAT { get; set; }
        [Display(Name = "Carrying Cost")]
        public double? CARRYING_COST { get; set; }
        [Display(Name = "Other Cost")]
        public double? OTHER_COST { get; set; }
        [Display(Name = "Actual Bill")]
        public double? ACTBILL { get; set; }
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

        public ICollection<PROC_BILL_DETAILS> PROC_BILL_DETAILS { get; set; }
        public F_GEN_S_RECEIVE_MASTER CHALLAN { get; set; }
    }
}
