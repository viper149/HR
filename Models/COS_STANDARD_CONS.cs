using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DenimERP.Models
{
    public class COS_STANDARD_CONS
    {
        public COS_STANDARD_CONS()
        {
            COS_PRECOSTING_MASTER = new HashSet<COS_PRECOSTING_MASTER>();
        }

        public int SCID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [Display(Name = "Monthly Cost")]
        public double? MONTHLY_COST { get; set; }
        [Display(Name = "Prod.")]
        public double? PROD { get; set; }
        [Display(Name = "Overhead(৳)")]
        public double? OVERHEAD_BDT { get; set; }
        [Display(Name = "Convert Rate")]
        public double? CONV_RATE { get; set; }
        [Display(Name = "Overhead($)")]
        public double? OVERHEAD_USD { get; set; }
        [Display(Name = "Loomspeed")]
        public double? LOOMSPEED { get; set; }
        [Display(Name = "Efficiency")]
        public double? EFFICIENCY { get; set; }
        [Display(Name = "Greige PPI")]
        public double? GRPPI { get; set; }
        [Display(Name = "Contraction")]
        public double? CONTRACTION { get; set; }
        [Display(Name = "Status")]
        public bool? STATUS { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Created At")]
        public DateTime? CREATED_AT { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Updated At")]
        public DateTime? UPDATED_AT { get; set; }

        [Display(Name = "Author")]
        public string USERID { get; set; }
        [Display(Name = "Author")]
        [NotMapped]
        public string USERNAME { get; set; }
        public virtual ICollection<COS_PRECOSTING_MASTER> COS_PRECOSTING_MASTER { get; set; }
    }
}
