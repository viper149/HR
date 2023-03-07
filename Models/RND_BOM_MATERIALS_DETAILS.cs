using System.ComponentModel;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class RND_BOM_MATERIALS_DETAILS : BaseEntity
    {
        public int BOM_D_ID { get; set; }
        public int? BOMID { get; set; }
        public int? SECTION { get; set; }
        [DisplayName("Material Description Name")]
        public int? CHEM_PROD_ID { get; set; }
        [DisplayName("Dosing (L/min)")]
        public double? DOSING { get; set; }
        [DisplayName("Conc.(g/L)")]
        public double? CONC { get; set; }
        [DisplayName("Speed(m/min)")]
        public double? SPEED { get; set; }
        [DisplayName("No. of Sets")]
        public double? NO_OF_SETS { get; set; }
        [DisplayName("Required Qty (gm/mtr)")]
        public double? REQ_QTY { get; set; }

        [DisplayName("Add 10% for Box")]
        public double? ADD_10_FOR_BOX { get; set; }
        public string REMARKS { get; set; }
        public string OPT10 { get; set; }
        public string OPT9 { get; set; }
        public string OPT8 { get; set; }
        public string OPT7 { get; set; }
        public string OPT6 { get; set; }
        public string OPT5 { get; set; }
        public string OPT4 { get; set; }
        public string OPT3 { get; set; }
        public string OPT2 { get; set; }
        public string OPT1 { get; set; }
       

        public RND_BOM BOM { get; set; }
        public F_CHEM_STORE_PRODUCTINFO CHEM_PROD_ { get; set; }
        public F_BAS_SECTION SECTIONNavigation { get; set; }
    }
}
