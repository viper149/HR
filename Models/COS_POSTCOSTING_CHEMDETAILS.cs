using System.ComponentModel.DataAnnotations;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class COS_POSTCOSTING_CHEMDETAILS : BaseEntity
    {
        public int TRANSID { get; set; }
        public int? PCOSTID { get; set; }
        [Display(Name = "Product Name")]
        public int? CHEM_PRODID { get; set; }
        [Display(Name = "Unit")]
        public int? UNIT { get; set; }
        [Display(Name = "Consumption")]
        public double? CONSUMPTION { get; set; }
        [Display(Name = "Rate")]
        public double? RATE { get; set; }
        [Display(Name = "Section")]
        public string SECTION { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }

        public F_CHEM_STORE_PRODUCTINFO CHEM_PROD { get; set; }
        public COS_POSTCOSTING_MASTER PCOST { get; set; }
        public F_BAS_UNITS UNITNavigation { get; set; }
    }
}
