using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DenimERP.Models
{
    public partial class F_BAS_DELIVERY_TYPE
    {
        public F_BAS_DELIVERY_TYPE()
        {
            F_FS_DELIVERYCHALLAN_PACK_MASTER = new HashSet<F_FS_DELIVERYCHALLAN_PACK_MASTER>();
        }

        public int ID { get; set; }
        [Display(Name = "Delivery Type")]
        public string DEL_TYPE { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }

        public ICollection<F_FS_DELIVERYCHALLAN_PACK_MASTER> F_FS_DELIVERYCHALLAN_PACK_MASTER { get; set; }
    }
}
