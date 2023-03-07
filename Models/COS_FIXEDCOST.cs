using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DenimERP.Models
{
    public class COS_FIXEDCOST
    {
        public COS_FIXEDCOST()
        {
            COS_PRECOSTING_MASTER = new HashSet<COS_PRECOSTING_MASTER>();
        }

        public int FCID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [Display(Name = "Dyeing Cost")]
        public double? DYEINGCOST { get; set; }
        [Display(Name = "UP Charge")]
        public double? UPCHARGE { get; set; }
        [Display(Name = "Printing Cost")]
        public double? PRINTINGCOST { get; set; }
        [Display(Name = "Overhead Cost")]
        public double? OVERHEADCOST { get; set; }
        [Display(Name = "Sample Cost")]
        public double? SAMPLECOST { get; set; }
        [Display(Name = "Sizing Cost")]
        public double? SIZINGCOST { get; set; }
        [Display(Name = "CFR Cost")]
        public double? CFRCOST { get; set; }
        [Display(Name = "Com. Sample Cost")]
        public double? COMSAMPLECOST { get; set; }
        [Display(Name = "Rejection Rate")]
        public double? REJECTION { get; set; }
        [Display(Name = "Option 1")]
        public string OPTION1 { get; set; }
        [Display(Name = "Option 2")]
        public string OPTION2 { get; set; }
        [Display(Name = "Status")]
        public bool? STATUS { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        [Display(Name = "Author")]
        public string USERID { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Created At")]
        public DateTime? CREATED_AT { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Updated At")]
        public DateTime? UPDATED_AT { get; set; }
        
        [Display(Name = "Author")]
        [NotMapped]
        public string USERNAME { get; set; }
        public virtual ICollection<COS_PRECOSTING_MASTER> COS_PRECOSTING_MASTER { get; set; }

    }
}
