using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DenimERP.Models
{
    public partial class F_FS_CLEARANCE_WASTAGE_ITEM
    {
        public F_FS_CLEARANCE_WASTAGE_ITEM()
        {
            F_FS_CLEARANCE_WASTAGE_TRANSFER = new HashSet<F_FS_CLEARANCE_WASTAGE_TRANSFER>();
        }

        public int IID { get; set; }
        [Display(Name = "Item Name")]
        public string INAME { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }

        public ICollection<F_FS_CLEARANCE_WASTAGE_TRANSFER> F_FS_CLEARANCE_WASTAGE_TRANSFER { get; set; }
    }
}
