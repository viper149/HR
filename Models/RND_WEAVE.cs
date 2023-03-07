using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Models
{
    public partial class RND_WEAVE
    {
            public RND_WEAVE()
            {
                RND_FABRICINFO = new HashSet<RND_FABRICINFO>();
                COS_PRECOSTING_MASTER = new HashSet<COS_PRECOSTING_MASTER>();
                RND_ANALYSIS_SHEET = new HashSet<RND_ANALYSIS_SHEET>();
                RND_SAMPLE_INFO_WEAVING = new HashSet<RND_SAMPLE_INFO_WEAVING>();
        }
            public int WID { get; set; }
            [NotMapped]
            public string EncryptedId { get; set; }
            [Display(Name = "RnD Weave Name"),Remote("IsWeaveInUse", "RndWeave")]
            public string NAME { get; set; }
            public string REMARKS { get; set; }

            public ICollection<COS_PRECOSTING_MASTER> COS_PRECOSTING_MASTER { get; set; }
            public ICollection<RND_ANALYSIS_SHEET> RND_ANALYSIS_SHEET { get; set; }
            public ICollection<RND_FABRICINFO> RND_FABRICINFO { get; set; }
            public ICollection<RND_SAMPLE_INFO_WEAVING> RND_SAMPLE_INFO_WEAVING { get; set; }
    }
}
