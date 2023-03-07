using System;

namespace DenimERP.Models
{
    public partial class F_PR_SLASHER_CHEM_CONSM
    {
        public int SL_CHEMID { get; set; }
        public int? SLID { get; set; }
        public int? CHEM_PRODID { get; set; }
        public string TYPE { get; set; }
        public string QTY { get; set; }
        public int? UNIT { get; set; }
        public string FOR_ { get; set; }
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }
        public string OPT4 { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? CREATED_AT { get; set; }
        public string UPDATED_BY { get; set; }
        public DateTime? UPDATED_AT { get; set; }

        public F_CHEM_STORE_PRODUCTINFO CHEM_PROD { get; set; }
        public F_PR_SLASHER_DYEING_MASTER SL { get; set; }
        public F_BAS_UNITS UNITNavigation { get; set; }
    }
}
