using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_FS_DELIVERYCHALLAN_PACK_DETAILS : BaseEntity
    {
        public int D_CHALLAN_D_ID { get; set; }
        public int? D_CHALLANID { get; set; }
        [Display(Name = "Roll Number")]
        public int? ROLL_NO { get; set; }
        [Display(Name = "Packing List Sl.")]
        public int? PACKING_LIST_ID { get; set; }
        [Display(Name = "Shade Group")]
        public string SHADE_GROUP { get; set; }
        [Display(Name = "Length 1")]
        public double? LENGTH1 { get; set; }
        [Display(Name = "Length 2")]
        public double? LENGTH2 { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        [NotMapped]
        public string ROLL_NO_N { get; set; }
        [NotMapped]
        public bool ISMANUAL { get; set; }

        public F_FS_DELIVERYCHALLAN_PACK_MASTER D_CHALLAN { get; set; }
        public F_FS_FABRIC_RCV_DETAILS ROLL { get; set; }
    }
}
