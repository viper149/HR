namespace DenimERP.ServiceInfrastructures
{
    internal class H_GS_ITEMSUB_CATEGORY
    {
        public int SCATID { get; set; }
        public object EncryptedId { get; set; }
        public string SCATNAME { get; set; }
        public string DESCRIPTION { get; set; }
        public string REMARKS { get; set; }
        public H_GS_ITEMCATEGORY CAT { get; set; }
    }
}