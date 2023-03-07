using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DenimERP.Models
{
    public partial class COM_EX_PI_DETAILS
    {
        public COM_EX_PI_DETAILS()
        {
            F_FS_DELIVERYCHALLAN_PACK_MASTER = new HashSet<F_FS_DELIVERYCHALLAN_PACK_MASTER>();
            F_FS_FABRIC_RCV_DETAILS = new HashSet<F_FS_FABRIC_RCV_DETAILS>();
            RND_PRODUCTION_ORDER = new HashSet<RND_PRODUCTION_ORDER>();
            COM_EX_INVDETAILS = new HashSet<COM_EX_INVDETAILS>();
            ACC_EXPORT_DODETAILS = new HashSet<ACC_EXPORT_DODETAILS>();
            ACC_LOCAL_DODETAILS = new HashSet<ACC_LOCAL_DODETAILS>();
            F_PR_INSPECTION_FABRIC_D_DETAILS = new HashSet<F_PR_INSPECTION_FABRIC_D_DETAILS>();
            COM_EX_ADV_DELIVERY_SCH_DETAILS = new HashSet<COM_EX_ADV_DELIVERY_SCH_DETAILS>();
        }

        public int TRNSID { get; set; }
        [Display(Name = "PI No")]
        public int PIID { get; set; }
        [Display(Name = "PI No")]
        public string PINO { get; set; }
        [Display(Name = "So No")]
        public string SO_NO { get; set; }
        [Display(Name = "Style")]
        [Required(ErrorMessage = "The field {0} can not be empty.")]
        public int? STYLEID { get; set; }
        [Display(Name = "Unit")]
        public int? UNIT { get; set; }
        [Display(Name = "Cost Referance")]
        public int? COSTID { get; set; }
        [NotMapped]
        public string COSTREF { get; set; }
        [Display(Name = "PI Quantity")]
        public double? QTY { get; set; }
        [Display(Name = "Original/Initial Quantity")]
        public double? INITIAL_QTY { get; set; }
        public double? INTIAL_QTY_2S { get; set; }
        public int? REV_TRACK { get; set; }
        [Display(Name = "Unit Price")]
        public double? UNITPRICE { get; set; }
        [Display(Name = "Total")]
        public double? TOTAL { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        public string ISDELETE { get; set; }
        public bool SO_STATUS { get; set; }

        public COM_EX_FABSTYLE STYLE { get; set; }
        public COM_EX_PIMASTER PIMASTER { get; set; }
        public COS_PRECOSTING_MASTER PRECOSTING { get; set; }
        public F_BAS_UNITS F_BAS_UNITS { get; set; }

        public ICollection<F_FS_DELIVERYCHALLAN_PACK_MASTER> F_FS_DELIVERYCHALLAN_PACK_MASTER { get; set; }
        public ICollection<F_FS_FABRIC_RCV_DETAILS> F_FS_FABRIC_RCV_DETAILS { get; set; }
        public ICollection<RND_PRODUCTION_ORDER> RND_PRODUCTION_ORDER { get; set; }
        public ICollection<COM_EX_INVDETAILS> COM_EX_INVDETAILS { get; set; }
        public ICollection<ACC_EXPORT_DODETAILS> ACC_EXPORT_DODETAILS { get; set; }
        public ICollection<ACC_LOCAL_DODETAILS> ACC_LOCAL_DODETAILS { get; set; }
        public ICollection<F_PR_INSPECTION_FABRIC_D_DETAILS> F_PR_INSPECTION_FABRIC_D_DETAILS { get; set; }
        public ICollection<COM_EX_ADV_DELIVERY_SCH_DETAILS> COM_EX_ADV_DELIVERY_SCH_DETAILS { get; set; }
    }
}
