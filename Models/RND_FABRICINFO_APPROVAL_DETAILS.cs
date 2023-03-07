using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class RND_FABRICINFO_APPROVAL_DETAILS : BaseEntity
    {
        public int RFAID { get; set; }
        public int FABCODE { get; set; }
        public string APPROVED_BY { get; set; }
        public string APPROVAL_ROLE { get; set; }
        public RND_FABRICINFO FABCODENavigation { get; set; }
    }
}
