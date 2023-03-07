using System.Collections.Generic;

namespace DenimERP.Models
{
    public partial class IMP_PRODCATEGORY
    {
        public IMP_PRODCATEGORY()
        {
            IMP_PRODUCTINFO = new HashSet<IMP_PRODUCTINFO>();
        }

        public int CATID { get; set; }
        public string CATEGORY { get; set; }
        public string RAMARKS { get; set; }

        public virtual ICollection<IMP_PRODUCTINFO> IMP_PRODUCTINFO { get; set; }
    }
}
