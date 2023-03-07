using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class RND_SAMPLE_INFO_WEAVING : BaseEntity
    {
        public RND_SAMPLE_INFO_WEAVING()
        {
            RND_FABRICINFO = new HashSet<RND_FABRICINFO>();
            F_PR_WEAVING_PROCESS_DETAILS_B = new HashSet<F_PR_WEAVING_PROCESS_DETAILS_B>();
            RND_SAMPLE_INFO_WEAVING_DETAILS = new HashSet<RND_SAMPLE_INFO_WEAVING_DETAILS>();
            LOOM_SETTINGS_SAMPLE = new HashSet<LOOM_SETTINGS_SAMPLE>();
        }

        public int WVID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [Display(Name = "RS Code")]
        public int? SDID { get; set; }
        [Display(Name = "Program Id"),
        Required(ErrorMessage = "Can Not Be Empty.")]
        public int? PROG_ID { get; set; }
        [Display(Name = "Style/Dev. No")]
        public string FABCODE { get; set; }
        [Display(Name = "Development No")]
        public string DEV_NO { get; set; }
        [Display(Name = "Concept")]
        public string CONCEPT { get; set; }
        [Display(Name = "Beam No")]
        public string BEAMNO { get; set; }
        [Display(Name = "Total Ends")]
        public double? TOTAL_ENDS { get; set; }
        [Display(Name = "Reed Space")]
        public double? REED_SPACE { get; set; }
        [Display(Name = "Finish PPI")]
        public double? FNPPI { get; set; }
        [Display(Name = "Reed Count")]
        public double? REED_COUNT { get; set; }
        [Display(Name = "Reed Dent")]
        public double? REED_DENT { get; set; }
        [Display(Name = "Greige EPI")]
        public double? GR_EPI { get; set; }
        [Display(Name = "Greige PPI")]
        public double? GR_PPI { get; set; }
        [Display(Name = "Weave")]
        public int? WEAVE { get; set; }
        [Display(Name = "Beam")]
        public int? BEAMID { get; set; }
        [Display(Name = "Beam")]
        public int? SBEAMID { get; set; }
        [Display(Name = "Set/Program No.")]
        public int? SETNO { get; set; }
        [Display(Name = "Loom Type")]
        public int? LOOMID { get; set; }
        [Display(Name = "Buyer")]
        public int? BUYERID { get; set; }
        [Display(Name = "Marketing Person")]
        public int? MKT_PERSON { get; set; }
        [Display(Name = "R&D Concern")]
        public int? RND_CONCERN { get; set; }
        [Display(Name = "Length (M)")]
        public double? LENGTH_MTR { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Close Date")]
        public DateTime? CLOSE_DATE { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }
        public string OPT4 { get; set; }
        [DisplayName("Is OK?")]
        public bool STATUS { get; set; }

        public LOOM_TYPE LOOM { get; set; }
        public BAS_BUYERINFO BUYER { get; set; }
        public F_HRD_EMPLOYEE Employee { get; set; }
        public MKT_TEAM MKTPerson { get; set; }
        public PL_SAMPLE_PROG_SETUP PROG_ { get; set; }
        public RND_WEAVE WEAVENavigation { get; set; }
        public F_PR_SIZING_PROCESS_ROPE_DETAILS SizingBeam { get; set; }
        public F_PR_SLASHER_DYEING_DETAILS SlasherBeamDetails { get; set; }
        public PL_PRODUCTION_SETDISTRIBUTION Set { get; set; }

        public ICollection<RND_FABRICINFO> RND_FABRICINFO { get; set; }
        public ICollection<F_PR_WEAVING_PROCESS_DETAILS_B> F_PR_WEAVING_PROCESS_DETAILS_B { get; set; }
        public ICollection<RND_SAMPLE_INFO_WEAVING_DETAILS> RND_SAMPLE_INFO_WEAVING_DETAILS { get; set; }
        public ICollection<LOOM_SETTINGS_SAMPLE> LOOM_SETTINGS_SAMPLE { get; set; }
    }
}
