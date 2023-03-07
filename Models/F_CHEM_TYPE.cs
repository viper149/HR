using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_CHEM_TYPE : BaseEntity
    {
        public F_CHEM_TYPE()
        {
            F_CHEM_STORE_PRODUCTINFO = new HashSet<F_CHEM_STORE_PRODUCTINFO>();
        }

        public int CTID { get; set; }
        [Display(Name = "Chemical Type")]
        public string CTYPE { get; set; }
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }

        public ICollection<F_CHEM_STORE_PRODUCTINFO> F_CHEM_STORE_PRODUCTINFO { get; set; }
    }
}
