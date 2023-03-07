using System;
using System.ComponentModel.DataAnnotations;

namespace DenimERP.Models
{
    public partial class COM_IMP_CSRAT_DETAILS
    {
        public int CSRID { get; set; }
        public int? CSITEMID { get; set; }
        [Display(Name = "Supplier")]
        public int? SUPPID { get; set; }
        [Display(Name = "Brand & Origin")]
        public string ORIGIN { get; set; }
        [Display(Name = "Rate")]
        public double? RATE { get; set; }
        [Display(Name = "Total")]
        public double? TOTAL { get; set; }
        public DateTime CREATED_AT { get; set; }

        public COM_IMP_CSITEM_DETAILS CSITEM { get; set; }
        public BAS_SUPPLIERINFO SUPP { get; set; }
    }
}
