namespace DenimERP.Models
{
    public partial class MKT_SUPPLIER
    {
        public int SUPP_ID { get; set; }
        public string SUPP_NAME { get; set; }
        public string ADDRESS { get; set; }
        public string DEL_ADDRESS { get; set; }
        public string REMARKS { get; set; }
        public string BIN_NO { get; set; }
        public int? OLD_ID { get; set; }
    }
}
