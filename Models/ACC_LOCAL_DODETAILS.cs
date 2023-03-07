using System;
using System.ComponentModel.DataAnnotations;

namespace DenimERP.Models
{
    public partial class ACC_LOCAL_DODETAILS
    {
        public int TRNSID { get; set; } 
        [Display(Name = "Date")]
        public DateTime TRNSDATE { get; set; }
        [Display(Name = "DO No")]
        public int? DONO { get; set; }
        [Display(Name = "Style")]
        public int? STYLEID { get; set; }
        [Display(Name = "PI Details Id")]
        public int? PI_TRNSID { get; set; }
        [Display(Name = "Qty.")]
        public decimal? QTY { get; set; }
        [Display(Name = "Rate")]
        public double? RATE { get; set; }
        [Display(Name = "Amount")]
        public double? AMOUNT { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        [Display(Name = "Author")]
        public string USRID { get; set; }
        
        public ACC_LOCAL_DOMASTER DO { get; set; }
        public COM_EX_SCDETAILS STYLE { get; set; }
        public COM_EX_PI_DETAILS ComExPiDetails { get; set; }
    }
}
