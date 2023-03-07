using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class MKT_TEAM : BaseEntity
    {
        public MKT_TEAM()
        {
            COM_EX_LCINFO = new HashSet<COM_EX_LCINFO>();
            MKT_SDRF_INFO = new HashSet<MKT_SDRF_INFO>();
            RND_ANALYSIS_SHEET = new HashSet<RND_ANALYSIS_SHEET>();
            MKT_SWATCH_CARD = new HashSet<MKT_SWATCH_CARD>();
            COM_EX_PIMASTER = new HashSet<COM_EX_PIMASTER>();
            RND_SAMPLE_INFO_WEAVING = new HashSet<RND_SAMPLE_INFO_WEAVING>();
            F_SAMPLE_FABRIC_ISSUE = new HashSet<F_SAMPLE_FABRIC_ISSUE>();
            F_SAMPLE_FABRIC_DISPATCH_DETAILS = new HashSet<F_SAMPLE_FABRIC_DISPATCH_DETAILS>();
            H_SAMPLE_FABRIC_DISPATCH_MASTER = new HashSet<H_SAMPLE_FABRIC_DISPATCH_MASTER>();
        }

        public int MKT_TEAMID { get; set; }
        [Display(Name = "Marketing Person Name")]
        public string PERSON_NAME { get; set; }
        public string PERSON_FULL_NAME { get; set; }
        public int? TEAMID { get; set; }
        public bool? CODE_LEVEL { get; set; }
        public string PHONE { get; set; }
        public string EMAIL { get; set; }
        public string DESID { get; set; }
        public string OLD_ID { get; set; }

        public BAS_TEAMINFO BasTeamInfo { get; set; }

        public ICollection<COM_EX_LCINFO> COM_EX_LCINFO { get; set; }
        public ICollection<MKT_SDRF_INFO> MKT_SDRF_INFO { get; set; }
        public ICollection<RND_ANALYSIS_SHEET> RND_ANALYSIS_SHEET { get; set; }
        public ICollection<MKT_SWATCH_CARD> MKT_SWATCH_CARD { get; set; }
        public ICollection<COM_EX_PIMASTER> COM_EX_PIMASTER { get; set; }
        public ICollection<RND_SAMPLE_INFO_WEAVING> RND_SAMPLE_INFO_WEAVING { get; set; }
        public ICollection<F_SAMPLE_FABRIC_ISSUE> F_SAMPLE_FABRIC_ISSUE { get; set; }
        public ICollection<F_SAMPLE_FABRIC_DISPATCH_DETAILS> F_SAMPLE_FABRIC_DISPATCH_DETAILS { get; set; }
        public ICollection<H_SAMPLE_FABRIC_DISPATCH_MASTER> H_SAMPLE_FABRIC_DISPATCH_MASTER { get; set; }
    }
}
