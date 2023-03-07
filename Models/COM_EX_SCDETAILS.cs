using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DenimERP.Models
{
    public partial class COM_EX_SCDETAILS
    {
        public COM_EX_SCDETAILS()
        {
            ACC_LOCAL_DODETAILS = new HashSet<ACC_LOCAL_DODETAILS>();
        }

        public int TRNSID { get; set; }
        [Display(Name = "Transaction Date")]
        public DateTime TRNSDATE { get; set; }
        [Display(Name = "Seller No")]
        public int? SCNO { get; set; }
        [Display(Name = "Style Name")]
        [Required(AllowEmptyStrings = true)]
        public int STYLEID { get; set; }
        [NotMapped]
        [Display(Name = "Style Name")]
        public string STYLENAME { get; set; }
        [Display(Name = "Unit")]
        public string UNIT { get; set; }
        [Display(Name = "Qty")]
        public decimal? QTY { get; set; }
        [Display(Name = "Rate")]
        public double? RATE { get; set; }
        [Display(Name = "Amount")]
        public double? AMOUNT { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        public string ISDELETE { get; set; }

        public COM_EX_FABSTYLE STYLE { get; set; }
        public COM_EX_SCINFO SC { get; set; }

        public ICollection<ACC_LOCAL_DODETAILS> ACC_LOCAL_DODETAILS { get; set; }
    }
}
