using System.Collections.Generic;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_YS_SLUB_CODE : BaseEntity
    {
        public F_YS_SLUB_CODE()
        {
            F_YS_INDENT_DETAILS = new HashSet<F_YS_INDENT_DETAILS>();
        }

        public int ID { get; set; }
        public string NAME { get; set; }
        public string REMARKS { get; set; }
        public string OPT4 { get; set; }
        public string OPT3 { get; set; }
        public string OPT2 { get; set; }
        public string OPT1 { get; set; }
        public int? OLD_CODE { get; set; }

        public ICollection<F_YS_INDENT_DETAILS> F_YS_INDENT_DETAILS { get; set; }
    }
}
