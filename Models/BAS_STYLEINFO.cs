namespace DenimERP.Models
{
    public partial class BAS_STYLEINFO
    {
        public int STYLEID { get; set; }
        public string STYLENAME { get; set; }
        public int BRANDID { get; set; }
        public int? FABID { get; set; }
        public string STATUS { get; set; }
        public int? BUYERID { get; set; }
        public string BCI { get; set; }
        public string ORG_GOTS { get; set; }
        public string ORG_OCS { get; set; }
        public string RCS { get; set; }
        public string GRS { get; set; }
        public string CMIA { get; set; }
        public string REMARKS { get; set; }
        public string USRID { get; set; }

        public virtual BAS_BRANDINFO BRAND { get; set; }
        public virtual BAS_BUYERINFO BUYER { get; set; }
        public virtual BAS_FABRICINFO FAB { get; set; }
    }
}
