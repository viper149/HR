using System;
using System.ComponentModel.DataAnnotations;

namespace DenimERP.Models
{
    public partial class F_DYEING_PROCESS_ROPE_CHEM
    {
        public int CHEM_ID { get; set; }
        public int? ROPE_DID { get; set; }
        [Display(Name = "Chemical Name.")]
        public int? CHEM_PROD_ID { get; set; }
        [Display(Name = "Quantity")]
        public double? QTY { get; set; }
        [Display(Name = "Unit")]
        public string UNIT { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }
        public string OPT4 { get; set; }
        public string OPT5 { get; set; }
        public DateTime? CREATED_AT { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? UPDATED_AT { get; set; }
        public string UPDATED_BY { get; set; }

        public F_CHEM_STORE_PRODUCTINFO CHEM_PROD_ { get; set; }
        public F_DYEING_PROCESS_ROPE_MASTER ROPE_D { get; set; }
    }
}
