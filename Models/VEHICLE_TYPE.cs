using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DenimERP.Models
{
    public partial class VEHICLE_TYPE
    {
        public VEHICLE_TYPE()
        {
            F_BAS_VEHICLE_INFO = new HashSet<F_BAS_VEHICLE_INFO>();
        }

        public int ID { get; set; }
        [Display (Name = "Vehicle Type")]
        public string TYPE_NAME { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }

        public ICollection<F_BAS_VEHICLE_INFO> F_BAS_VEHICLE_INFO { get; set; }
    }
}
