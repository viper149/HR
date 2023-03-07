using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Models
{
    public partial class BAS_BUYERINFO
    {
        public BAS_BUYERINFO()
        {
            COM_EX_INVOICEMASTER = new HashSet<COM_EX_INVOICEMASTER>();
            COM_EX_LCINFO = new HashSet<COM_EX_LCINFO>();
            COM_EX_SCINFO = new HashSet<COM_EX_SCINFO>();
            RND_FABRICINFO = new HashSet<RND_FABRICINFO>();
            RND_ANALYSIS_SHEET = new HashSet<RND_ANALYSIS_SHEET>();
            COM_EX_PIMASTER = new HashSet<COM_EX_PIMASTER>();
            MKT_SWATCH_CARD = new HashSet<MKT_SWATCH_CARD>();
            F_SAMPLE_DESPATCH_DETAILS = new HashSet<F_SAMPLE_DESPATCH_DETAILS>();
            H_SAMPLE_RECEIVING_D = new HashSet<H_SAMPLE_RECEIVING_D>();
            MKT_SDRF_INFO = new HashSet<MKT_SDRF_INFO>();
            F_SAMPLE_GARMENT_RCV_D = new HashSet<F_SAMPLE_GARMENT_RCV_D>();
            H_SAMPLE_DESPATCH_M = new HashSet<H_SAMPLE_DESPATCH_M>();
            RndPurchaseRequisitionMasters = new HashSet<RND_PURCHASE_REQUISITION_MASTER>();
            RND_SAMPLE_INFO_DYEING = new HashSet<RND_SAMPLE_INFO_DYEING>();
            RND_SAMPLE_INFO_WEAVING = new HashSet<RND_SAMPLE_INFO_WEAVING>();
            F_SAMPLE_FABRIC_ISSUE = new HashSet<F_SAMPLE_FABRIC_ISSUE>();
            F_FS_FABRIC_CLEARANCE_MASTER = new HashSet<F_FS_FABRIC_CLEARANCE_MASTER>();
            F_FS_FABRIC_CLEARANCE_MASTER_FACTORY = new HashSet<F_FS_FABRIC_CLEARANCE_MASTER>();
            F_SAMPLE_FABRIC_DISPATCH_DETAILS = new HashSet<F_SAMPLE_FABRIC_DISPATCH_DETAILS>();
            H_SAMPLE_FABRIC_DISPATCH_MASTER = new HashSet<H_SAMPLE_FABRIC_DISPATCH_MASTER>();
            COM_EX_ADV_DELIVERY_SCH_MASTER = new HashSet<COM_EX_ADV_DELIVERY_SCH_MASTER>();
            F_FS_FABRIC_RETURN_RECEIVE = new HashSet<F_FS_FABRIC_RETURN_RECEIVE>();
        }

        public int BUYERID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [Remote(action: "IsBuyerNameInUse", controller: "BasicBuyerInfo")]
        [Display(Name = "Buyer Name")]
        public string BUYER_NAME { get; set; }
        [Display(Name = "Address")]
        public string ADDRESS { get; set; }
        [Display(Name = "Delivery Address")]
        public string DEL_ADDRESS { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        [Display(Name = "BIN Number")]
        public string BIN_NO { get; set; }

        public ICollection<COM_EX_INVOICEMASTER> COM_EX_INVOICEMASTER { get; set; }
        public ICollection<COM_EX_LCINFO> COM_EX_LCINFO { get; set; }
        public ICollection<COM_EX_SCINFO> COM_EX_SCINFO { get; set; }
        public ICollection<RND_FABRICINFO> RND_FABRICINFO { get; set; }
        public ICollection<RND_ANALYSIS_SHEET> RND_ANALYSIS_SHEET { get; set; }
        public ICollection<MKT_SWATCH_CARD> MKT_SWATCH_CARD { get; set; }
        public ICollection<COM_EX_PIMASTER> COM_EX_PIMASTER { get; set; }
        public ICollection<F_SAMPLE_DESPATCH_DETAILS> F_SAMPLE_DESPATCH_DETAILS { get; set; }
        public ICollection<H_SAMPLE_RECEIVING_D> H_SAMPLE_RECEIVING_D { get; set; }
        public ICollection<MKT_SDRF_INFO> MKT_SDRF_INFO { get; set; }
        public ICollection<F_SAMPLE_GARMENT_RCV_D> F_SAMPLE_GARMENT_RCV_D { get; set; }
        public ICollection<H_SAMPLE_DESPATCH_M> H_SAMPLE_DESPATCH_M { get; set; }
        public ICollection<RND_SAMPLE_INFO_DYEING> RND_SAMPLE_INFO_DYEING { get; set; }
        public ICollection<RND_PURCHASE_REQUISITION_MASTER> RndPurchaseRequisitionMasters { get; set; }
        public ICollection<RND_SAMPLE_INFO_WEAVING> RND_SAMPLE_INFO_WEAVING { get; set; }
        public ICollection<F_SAMPLE_FABRIC_ISSUE> F_SAMPLE_FABRIC_ISSUE { get; set; }
        public ICollection<F_FS_FABRIC_CLEARANCE_MASTER> F_FS_FABRIC_CLEARANCE_MASTER { get; set; }
        public ICollection<F_FS_FABRIC_CLEARANCE_MASTER> F_FS_FABRIC_CLEARANCE_MASTER_FACTORY { get; set; }
        public ICollection<F_SAMPLE_FABRIC_DISPATCH_DETAILS> F_SAMPLE_FABRIC_DISPATCH_DETAILS { get; set; }
        public ICollection<H_SAMPLE_FABRIC_DISPATCH_MASTER> H_SAMPLE_FABRIC_DISPATCH_MASTER { get; set; }
        public ICollection<COM_EX_ADV_DELIVERY_SCH_MASTER> COM_EX_ADV_DELIVERY_SCH_MASTER { get; set; }
        public ICollection<F_FS_FABRIC_RETURN_RECEIVE> F_FS_FABRIC_RETURN_RECEIVE { get; set; }

    }
}
