using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_BAS_VEHICLE_INFO : BaseEntity
    {
        public F_BAS_VEHICLE_INFO()
        {
            F_SAMPLE_DESPATCH_MASTER = new HashSet<F_SAMPLE_DESPATCH_MASTER>();
            H_SAMPLE_RECEIVING_M = new HashSet<H_SAMPLE_RECEIVING_M>();
            F_FS_DELIVERYCHALLAN_PACK_MASTER = new HashSet<F_FS_DELIVERYCHALLAN_PACK_MASTER>();
            F_GS_GATEPASS_INFORMATION_M = new HashSet<F_GS_GATEPASS_INFORMATION_M>();
            F_SAMPLE_FABRIC_DISPATCH_MASTER = new HashSet<F_SAMPLE_FABRIC_DISPATCH_MASTER>();
            F_FS_FABRIC_LOADING_BILL = new HashSet<F_FS_FABRIC_LOADING_BILL>();
        }

        public int VID { get; set; }
        [Display(Name = "Vehicle Number")]
        public string VNUMBER { get; set; }
        [Display(Name = "Vehicle Type")]
        public int? VEHICLE_TYPE { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        [Display(Name = "Driver")]
        public int? VDID { get; set; }

        [NotMapped]
        public string EncryptedId { get; set; }

        public ICollection<F_SAMPLE_DESPATCH_MASTER> F_SAMPLE_DESPATCH_MASTER { get; set; }
        public ICollection<H_SAMPLE_RECEIVING_M> H_SAMPLE_RECEIVING_M { get; set; }
        public ICollection<F_FS_DELIVERYCHALLAN_PACK_MASTER> F_FS_DELIVERYCHALLAN_PACK_MASTER { get; set; }
        public ICollection<F_GS_GATEPASS_INFORMATION_M> F_GS_GATEPASS_INFORMATION_M { get; set; }
        public ICollection<F_SAMPLE_FABRIC_DISPATCH_MASTER> F_SAMPLE_FABRIC_DISPATCH_MASTER { get; set; }

        public VEHICLE_TYPE VEHICLE_TYPENavigation { get; set; }
        public F_BAS_DRIVERINFO VD { get; set; }

        public ICollection<F_FS_FABRIC_LOADING_BILL> F_FS_FABRIC_LOADING_BILL { get; set; }

    }
}
