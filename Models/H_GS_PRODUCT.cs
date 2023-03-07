using System;

namespace DenimERP.Models
{
    public partial class H_GS_PRODUCT
    {
        public int PRODID { get; set; }
        public int? SCATID { get; set; }
        public string PRODNAME { get; set; }
        public string PARTNO { get; set; }
        public int? OLDCODE { get; set; }
        public string PROD_LOC { get; set; }
        public string DESCRIPTION { get; set; }
        public string REMARKS { get; set; }
        public int? UNIT { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? CREATED_AT { get; set; }
        public string UPDATED_BY { get; set; }
        public DateTime? UPDATED_AT { get; set; }

        public H_GS_ITEM_SUBCATEGORY SCAT { get; set; }
        public F_BAS_UNITS UNITNavigation { get; internal set; }
    }
}
