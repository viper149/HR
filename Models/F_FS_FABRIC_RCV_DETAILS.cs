using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_FS_FABRIC_RCV_DETAILS : BaseEntity
    {
        public F_FS_FABRIC_RCV_DETAILS()
        {
            F_FS_DELIVERYCHALLAN_PACK_DETAILS = new HashSet<F_FS_DELIVERYCHALLAN_PACK_DETAILS>();
            ACC_PHYSICAL_INVENTORY_FAB = new HashSet<ACC_PHYSICAL_INVENTORY_FAB>();
        }

        public int TRNSID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        public int? RCVID { get; set; }
        [Display(Name = "Style Name")]
        public int? FABCODE { get; set; }
        [Display(Name = "Order No")]
        public int? SO_NO { get; set; }
        [Display(Name = "PO No")]
        public int? PO_NO { get; set; }
        [Display(Name = "Roll No")]
        public int? ROLL_ID { get; set; }
        [Display(Name = "Roll No")]
        [NotMapped]
        public string ROLL_NO { get; set; }
        [Display(Name = "Location")]
        public int? LOCATION { get; set; }
        [Display(Name = "Qty (Yds)")]
        public double? QTY_YARDS { get; set; }
        [Display(Name = "Balance(Yds)")]
        public double? BALANCE_QTY { get; set; }
        [Display(Name = "QC Approved?")]
        public bool IS_QC_APPROVE { get; set; }
        [Display(Name = "QC Approve Date")]
        public DateTime? QC_APPROVE_DATE { get; set; }
        [Display(Name = "QC Reject?")]
        public bool IS_QC_REJECT { get; set; }
        [Display(Name = "Reject Date")]
        public DateTime? QC_REJECT_DATE { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }
        [NotMapped]
        public double Qty { get; set; }

        public RND_FABRICINFO FABCODENavigation { get; set; }
        public COM_EX_PIMASTER PO_NONavigation { get; set; }
        public F_FS_FABRIC_RCV_MASTER RCV { get; set; }
        public F_PR_INSPECTION_PROCESS_DETAILS ROLL_ { get; set; }
        public COM_EX_PI_DETAILS SO_NONavigation { get; set; }
        public F_FS_LOCATION LOCATIONNavigation { get; set; }

        public ICollection<F_FS_DELIVERYCHALLAN_PACK_DETAILS> F_FS_DELIVERYCHALLAN_PACK_DETAILS { get; set; }
        public ICollection<ACC_PHYSICAL_INVENTORY_FAB> ACC_PHYSICAL_INVENTORY_FAB { get; set; }
    }
}