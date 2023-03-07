using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Models
{
    public partial class F_SAMPLE_FABRIC_DISPATCH_DETAILS
    {
        public F_SAMPLE_FABRIC_DISPATCH_DETAILS()
        {
            F_SAMPLE_FABRIC_DISPATCH_TRANSACTION = new HashSet<F_SAMPLE_FABRIC_DISPATCH_TRANSACTION>();
            H_SAMPLE_FABRIC_RECEIVING_D = new HashSet<H_SAMPLE_FABRIC_RECEIVING_D>();
        }

        public int DPDID { get; set; }
        public int? DPID { get; set; }
        public int? TRNSID { get; set; }
        [Display(Name = "Brand Name")]
        public int? BRANDID { get; set; }
        [Display(Name = "Marketing Team ID")]
        public int? MKT_TEAMID { get; set; }
        [Display(Name = "Unit")]
        public int? UID { get; set; }
        [Display(Name = "Request Qty")]
        public double? REQ_QTY { get; set; }
        [Display(Name = "Delivery Qty")]
        [Remote(action: "IsValidForUse", controller: "SampleFabricDispatch", AdditionalFields = "REQ_QTY")]
        public double? DEL_QTY { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        [Display(Name = "Buyer Ref.")]
        public string BUYER_REF { get; set; }
        [Display(Name = "Sample Type ID")]
        public int? STYPEID { get; set; }
        [Display(Name = "Att. Person")]
        public string ATT_PERSON { get; set; }
        [Display(Name = "Buyer Name")]
        public int? BYERID { get; set; }

        public F_SAMPLE_FABRIC_DISPATCH_MASTER DP { get; set; }
        public F_SAMPLE_FABRIC_RCV_D FSampleFabricRcvD { get; set; }
        public BAS_BUYERINFO BUYER { get; set; }
        public MKT_TEAM TEAM { get; set; }
        public F_BAS_UNITS UNIT { get; set; }
        public F_SAMPLE_FABRIC_DISPATCH_SAMPLE_TYPE STYPE { get; set; }
        public BAS_BRANDINFO BRAND { get; set; }

        public ICollection<F_SAMPLE_FABRIC_DISPATCH_TRANSACTION> F_SAMPLE_FABRIC_DISPATCH_TRANSACTION { get; set; }
        public ICollection<H_SAMPLE_FABRIC_RECEIVING_D> H_SAMPLE_FABRIC_RECEIVING_D { get; set; }
    }
}
