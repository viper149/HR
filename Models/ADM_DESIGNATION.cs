namespace DenimERP.Models
{
    public partial class ADM_DESIGNATION
    {
        public int DESID { get; set; }
        public int DEPID { get; set; }
        public string DESNAME { get; set; }
        public string REMARKS { get; set; }

        public virtual ADM_DEPARTMENT DEP { get; set; }
    }
}
