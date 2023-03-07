using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Models
{
    public partial class RND_FINISHTYPE
    {
        public RND_FINISHTYPE()
        {
            RND_FABRICINFO = new HashSet<RND_FABRICINFO>();
            COS_PRECOSTING_MASTER = new HashSet<COS_PRECOSTING_MASTER>();
            MKT_SDRF_INFO = new HashSet<MKT_SDRF_INFO>();
            RND_ANALYSIS_SHEET = new HashSet<RND_ANALYSIS_SHEET>();
            MKT_SWATCH_CARD = new HashSet<MKT_SWATCH_CARD>();
            RND_BOM = new HashSet<RND_BOM>();
            
        }
        public int FINID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [Display(Name = "Finish Type Name"), Remote("IsFinishTypeInUse", "RndFinishType")]
        public string TYPENAME { get; set; }
        public double COST { get; set; }
        public string REMARKS { get; set; }
        public ICollection<RND_FABRICINFO> RND_FABRICINFO { get; set; }
        public ICollection<COS_PRECOSTING_MASTER> COS_PRECOSTING_MASTER { get; set; }
        public ICollection<MKT_SDRF_INFO> MKT_SDRF_INFO { get; set; }
        public ICollection<RND_ANALYSIS_SHEET> RND_ANALYSIS_SHEET { get; set; }
        public ICollection<MKT_SWATCH_CARD> MKT_SWATCH_CARD { get; set; }
        public ICollection<RND_BOM> RND_BOM { get; set; }
       
    }
}
