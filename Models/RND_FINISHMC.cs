using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Models
{
    public partial class RND_FINISHMC
    {
        public RND_FINISHMC()
        {
            RND_FABRICINFO = new HashSet<RND_FABRICINFO>();
        }
        public int MCID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [Remote("IsFinishMcInUse", "RndFinishMc")]
        public string NAME { get; set; }
        public string REMARKS { get; set; }
        public virtual ICollection<RND_FABRICINFO> RND_FABRICINFO { get; set; }
    }
}
