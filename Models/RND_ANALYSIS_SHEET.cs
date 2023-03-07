using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class RND_ANALYSIS_SHEET : BaseEntity
    {
        public RND_ANALYSIS_SHEET()
        {
            RND_ANALYSIS_SHEET_DETAILS = new HashSet<RND_ANALYSIS_SHEET_DETAILS>();
            MKT_SDRF_INFO = new HashSet<MKT_SDRF_INFO>();
        }

        public int AID { get; set; }
        public int? SWATCH_ID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [DataType(DataType.Date)]
        [Display(Name = "Date")]
        public DateTime? ADATE { get; set; }
        [Display(Name = "Marketing Person")]
        public int? MKT_PERSON_ID { get; set; }
        [Display(Name = "Marketing Query No.")]
        public string MKT_QUERY_NO { get; set; }
        [Display(Name = "RND Query No.")]
        public string RND_QUERY_NO { get; set; }
        [Display(Name = "Buyer Ref.")]
        public string BUYER_REF { get; set; }
        [Display(Name = "Buyer")]
        public int? BUYERID { get; set; }
        [Display(Name = "Construction")]
        public string CONSTRUCTION { get; set; }
        [Display(Name = "Ratio(%) Warp")]
        public string WARP_RATIO { get; set; }
        [Display(Name = "Ratio(%) Weft")]
        public string WEFT_RATIO { get; set; }
        [Display(Name = "Shrinkage Warp")]
        public string WARP_SHRINKAGE { get; set; }
        [Display(Name = "Shrinkage Weft")]
        public string WEFT_SHRINKAGE { get; set; }
        [Display(Name = "Finish EPI X PPI")]
        public string FN_EPI_PPI { get; set; }
        [Display(Name = "Wash EPI X PPI")]
        public string WASH_EPI_PPI { get; set; }
        [Display(Name = "Weave")]
        public int? WID { get; set; }
        [Display(Name = "Finish Weight")]
        public string FN_WEIGHT { get; set; }
        [Display(Name = "Wash Weight")]
        public string WA_WEIGHT { get; set; }
        [Display(Name = "Finish Width")]
        public string FN_WIDTH { get; set; }
        [Display(Name = "Color")]
        public string COLORCODE { get; set; }
        [Display(Name = "Finish Type")]
        public int? FINID { get; set; }
        [Display(Name = "Fabric Type")]
        public string FAB_TYPE { get; set; }
        [Display(Name = "Fabric Content")]
        public string FAB_CONTENT { get; set; }
        [Display(Name = "Warp Fabric Length")]
        public string WARP_FAB_LENGTH { get; set; }
        [Display(Name = "Weft Fabric Length")]
        public string WEFT_FAB_LENGTH { get; set; }
        [Display(Name = "Warp Crimp%")]
        public string WARP_CRIMP { get; set; }
        [Display(Name = "Weft Crimp%")]
        public string WEFT_CRIMP { get; set; }
        [Display(Name = "On Loom EPI X PPI")]
        public string LOOM_EPI_PPI { get; set; }
        [Display(Name = "Stretch Ability")]
        public string STRETCH_ABILITY { get; set; }
        [Display(Name = "Total Ends", Prompt = "Superior Defined Total Ends. Must Match With The User Defined Total Ends.")]
        public string TOTAL_ENDS { get; set; }
        [Display(Name = "Reed Space", Prompt = "Superior Defined Reed Space. Must Match With The User Defined Reed Space.")]
        public string REED_SPACE { get; set; }
        [Display(Name = "Finish Route")]
        public string FINISH_ROUTE { get; set; }
        [Display(Name = "Checked Comments")]
        public string CHECKED_COMMENTS { get; set; }
        [Display(Name = "Superior Comments")]
        public string HEAD_COMMENTS { get; set; }
        [Display(Name = "Submitted Style")]
        public string SUBMITTED_STYLE { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [DataType(DataType.Date)]
        [Display(Name = "Submitted Date")]
        public DateTime? SUBMITTED_DATE { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [DataType(DataType.Date)]
        [Display(Name = "Swatch Received Date")]
        public DateTime? SWATCH_RCV_DATE { get; set; }
        [Display(Name = "Check By")]
        public string CHECK_BY { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [DataType(DataType.Date)]
        [Display(Name = "Check Date")]
        public DateTime? CHECK_DATE { get; set; }
        [Display(Name = "Head of R&D Approve")]
        public bool RND_HEAD_APPROVE { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [DataType(DataType.Date)]
        [Display(Name = "Approve Date")]
        public DateTime? APPROVE_DATE { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        public int? BRANDID { get; set; }
        [Display(Name = "Total Ends", Prompt = "User Defined Total Ends.")]
        public string UTOTAL_ENDS { get; set; }
        [Display(Name = "Reed Space", Prompt = "User Defined Reed Space.")]
        public string UREED_SPACE { get; set; }

        public BAS_BUYERINFO BUYER { get; set; }
        public MKT_SWATCH_CARD Swatch { get; set; }
        public RND_FINISHTYPE FIN { get; set; }
        public MKT_TEAM MKT_PERSON_ { get; set; }
        public RND_WEAVE W { get; set; }
        public BAS_BRANDINFO BasBrandinfo { get; set; }
        public ICollection<RND_ANALYSIS_SHEET_DETAILS> RND_ANALYSIS_SHEET_DETAILS { get; set; }
        public ICollection<MKT_SDRF_INFO> MKT_SDRF_INFO { get; set; }
    }
}
