namespace DenimERP.Models
{
    public partial class IMP_PRODUCTINFO
    {
        public int PRODID { get; set; }
        public string PRODNAME { get; set; }
        public string DESCRIPTION { get; set; }
        public int? CATID { get; set; }
        public string UNIT { get; set; }
        public string REMARKS { get; set; }

        public virtual IMP_PRODCATEGORY CAT { get; set; }
    }
}
