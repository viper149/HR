using System.Collections.Generic;

namespace DenimERP.Models
{
    public partial class BAS_IMP_PRODCATEGORY
    {
        public BAS_IMP_PRODCATEGORY()
        {
            BAS_IMP_PRODUCTINFO = new HashSet<BAS_IMP_PRODUCTINFO>();
        }

        public int CATID { get; set; }
        public string CATEGORY { get; set; }
        public string RAMARKS { get; set; }

        public virtual ICollection<BAS_IMP_PRODUCTINFO> BAS_IMP_PRODUCTINFO { get; set; }
    }
}
