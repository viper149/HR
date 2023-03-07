using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_BAS_DRIVERINFO : BaseEntity
    {
        public F_BAS_DRIVERINFO()
        {
            F_SAMPLE_DESPATCH_MASTER = new HashSet<F_SAMPLE_DESPATCH_MASTER>();
            H_SAMPLE_RECEIVING_M = new HashSet<H_SAMPLE_RECEIVING_M>();
            F_SAMPLE_FABRIC_DISPATCH_MASTER = new HashSet<F_SAMPLE_FABRIC_DISPATCH_MASTER>();
            F_BAS_VEHICLE_INFO = new HashSet<F_BAS_VEHICLE_INFO>();
        }

        public int DRID { get; set; }
        [Display(Name = "Driver Name")]
        public string DRIVER_NAME { get; set; }
        [Display(Name = "Details")]
        public string DETAILS { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        public string DCONTACT { get; set; }

        public ICollection<F_SAMPLE_DESPATCH_MASTER> F_SAMPLE_DESPATCH_MASTER { get; set; }
        public ICollection<H_SAMPLE_RECEIVING_M> H_SAMPLE_RECEIVING_M { get; set; }
        public ICollection<F_SAMPLE_FABRIC_DISPATCH_MASTER> F_SAMPLE_FABRIC_DISPATCH_MASTER { get; set; }

        public ICollection<F_BAS_VEHICLE_INFO> F_BAS_VEHICLE_INFO { get; set; }
    }
}
