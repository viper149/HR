namespace DenimERP.Models
{
    public partial class COM_FABSTYLE
    {
        public int STYLEID { get; set; }
        public string STYLENAME { get; set; }
        public int? BRANDID { get; set; }
        public string FABCODE { get; set; }
        public string COMM_CONST { get; set; }
        public string RND_CONST { get; set; }
        public string HSCODE { get; set; }
        public string STATUS { get; set; }
        public string BCI { get; set; }
        public string ORG_GOTS { get; set; }
        public string ORG_OCS { get; set; }
        public string RCS { get; set; }
        public string GRS { get; set; }
        public string CMIA { get; set; }
        public string OPTION1 { get; set; }
        public string OPTION2 { get; set; }
        public string OPTION3 { get; set; }
        public string REMARKS { get; set; }
        public string USRID { get; set; }

        public virtual BAS_BRANDINFO BRAND { get; set; }
        public virtual RND_FABRICINFO FABCODENavigation { get; set; }
    }
}
