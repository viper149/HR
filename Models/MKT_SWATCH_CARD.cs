using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DenimERP.Models
{
    public partial class MKT_SWATCH_CARD
    {

        public MKT_SWATCH_CARD()
        {
            RND_ANALYSIS_SHEET = new HashSet<RND_ANALYSIS_SHEET>();
        }

        [Display(Name = "Swatch Card Id")]
        public int SWCDID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [Display(Name = "Marketing Query")]
        public string MKTQUERY { get; set; }
        public int? BUYERID { get; set; }
        [Display(Name = "Buyer Reference")]
        public string BUYERREF { get; set; }
        [Display(Name = "Construction")]
        public string CONSTRUCTION { get; set; }
        [Display(Name = "Width")]
        public string WIDTH { get; set; }
        [Display(Name = "Finish Weight")]
        public string FNWEIGHT { get; set; }
        public int? FNID { get; set; }
        [Display(Name = "Color Code")]
        public int? COLORCODE { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        [Display(Name = "Marketing Team")]
        public int? MKTTEAM { get; set; }
        [Display(Name = "Marketing Person")]
        public int? MKTPERSON { get; set; }
        [Display(Name = "Order Type")]
        public string ORDERTYPE { get; set; }

        public BAS_BUYERINFO BUYER { get; set; }
        public MKT_TEAM TEAM { get; set; }
        public BAS_TEAMINFO BasTeamInfo { get; set; }
        public RND_FINISHTYPE FN { get; set; }
        public BAS_COLOR BasColor { get; set; }
        public ICollection<RND_ANALYSIS_SHEET> RND_ANALYSIS_SHEET { get; set; }
    }
}