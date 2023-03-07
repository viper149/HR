using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using DenimERP.Models.BaseModels;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Models
{
    public partial class MKT_SDRF_INFO : BaseEntity
    {
        public MKT_SDRF_INFO()
        {
            RND_SAMPLE_INFO_DYEING = new HashSet<RND_SAMPLE_INFO_DYEING>();
            RND_PURCHASE_REQUISITION_MASTER = new HashSet<RND_PURCHASE_REQUISITION_MASTER>();
        }
        public int SDRFID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "SDRF Date")]
        [DataType(DataType.Date)]
        public DateTime? TRANSDATE { get; set; }
        [Remote(action: "IsSdrfNoInUse", controller: "MktSdrfInfo")]
        [Display(Name = "SDRF No")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Please enter a valid {0}.")]
        public string SDRF_NO { get; set; }
        [Display(Name = "Related Marketing Person")]
        public int? MKT_PERSON_ID { get; set; }
        [Display(Name = "Marketing Team")]
        public int? TEAMID { get; set; }
        [Display(Name = "Query No.")]
        public int? AID { get; set; }
        [Display(Name = "Development Type")]
        public int? DEV_ID { get; set; }
        [Display(Name = "Vendor")]
        public int? BUYERID { get; set; }
        [Display(Name = "Factory")]
        public int? BRANDID { get; set; }
        [Display(Name = "Development Priority")]
        public string PRIORITY { get; set; }
        [Display(Name = "Buyer Ref./Style No.")]
        public string BUYER_REF { get; set; }
        [Display(Name = "For")]
        public string FORTYPE { get; set; }
        [Display(Name = "Reason For Rework")]
        public string REWORK_REASON { get; set; }
        [Display(Name = "Season")]
        public string SEASON { get; set; }
        [Display(Name = "Construction")]
        public string CONSTRUCTION { get; set; }
        [Display(Name = "Width")]
        public string WIDTH { get; set; }
        [Display(Name = "Weight(BW)")]
        public string WEIGHT_BW { get; set; }
        [Display(Name = "Weight(AW)")]
        public string WEIGHT_AW { get; set; }
        [Display(Name = "GSP(BW)")]
        public string GSM_BW { get; set; }
        [Display(Name = "GSP(AW)")]
        public string GSM_AW { get; set; }
        [Display(Name = "Color")]
        public string COLOR { get; set; }
        [Display(Name = "Buyer's Target Price ($)")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public string TARGET_PRICE { get; set; }
        [Display(Name = "Projected Order Qty.")]
        public string ORDER_QTY { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Buyer Required Date")]
        [DataType(DataType.Date)]
        public DateTime? REQUIRED_DATE { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Optional Date")]
        [DataType(DataType.Date)]
        public DateTime? OPTIONAL_DATE { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Dyeing Date")]
        [DataType(DataType.Date)]
        public DateTime? ACTUAL_DATE { get; set; }
        [Display(Name = "Comments")]
        public string REMARKS { get; set; }
        [Display(Name = "Marketing DGM Approve")]
        public bool MKT_DGM_APPROVE { get; set; }
        [Display(Name = "Marketing(DGM) Comments")]
        public string Marketing_DGM_REMARKS { get; set; }
        [Display(Name = "Planning Approve")]
        public bool PLN_APPROVE { get; set; }
        [Display(Name = "Is Material Available")]
        public bool MATERIAL_AVAILABLE { get; set; }
        [Display(Name = "Planning Comments")]
        public string PLANNING_REMARKS { get; set; }
        [Display(Name = "RND Approve")]
        public bool RND_APPROVE { get; set; }
        [Display(Name = "RND Comments")]
        public string RND_REMARKS { get; set; }
        [Display(Name = "Plant Head Approve")]
        public bool PLANT_HEAD_APPROVE { get; set; }
        [Display(Name = "Plant Head Comments")]
        public string PLANT_HEAD_REMARKS { get; set; }
        [Display(Name = "RnD Team")]
        public int? RND_TEAM_ID { get; set; }
        [Display(Name = "Buyer Origin")]
        public int BUYER_ORIGIN { get; set; }
        [Display(Name = "Buyer Previous Dev. Qty")]
        public double BUYER_PREVIOUS_QTY { get; set; }
        [Display(Name = "Sample Qty.")]
        public double? SAMPLE_QTY { get; set; }
        [Display(Name = "Is Buyer Visited Factory")]
        public bool IS_BUYER_VISITED_FACTORY { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Tentative Submitted From Factory")]
        [DataType(DataType.Date)]
        public DateTime? SUBMIT_DATE_FACTORY { get; set; }
        [Display(Name = "Finish Type")]
        public int FINID { get; set; }
        public string OPTION1 { get; set; }
        public string OPTION2 { get; set; }
        public string OPTION3 { get; set; }
        public string OPTION4 { get; set; }
        [Display(Name = "Query No. (Manual)")]
        public string OPTION5 { get; set; }

        [NotMapped]
        public string DateFormat
        {
            get
            {
                var dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper();
                return !string.IsNullOrEmpty(dateTimeFormat) ? dateTimeFormat : "DD-MMM-YY";
            }
        }

        public BAS_BUYERINFO BUYER { get; set; }
        public BAS_BRANDINFO BasBrandinfo { get; set; }
        public MKT_DEV_TYPE DEV_ { get; set; }
        public MKT_TEAM MKT_PERSON { get; set; }
        public BAS_TEAMINFO TEAM_M { get; set; }
        public BAS_TEAMINFO TEAM_R { get; set; }
        public COUNTRIES COUNTRIES { get; set; }
        public RND_FINISHTYPE FINISHTYPE { get; set; }
        public RND_ANALYSIS_SHEET RND_ANALYSIS_SHEET { get; set; }

        public ICollection<RND_SAMPLE_INFO_DYEING> RND_SAMPLE_INFO_DYEING { get; set; }
        public ICollection<RND_PURCHASE_REQUISITION_MASTER> RND_PURCHASE_REQUISITION_MASTER { get; set; }
    }
}
