using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DenimERP.Models
{
    public partial class COS_CERTIFICATION_COST
    {
        public COS_CERTIFICATION_COST()
        {
            COS_PRECOSTING_MASTER = new HashSet<COS_PRECOSTING_MASTER>();
        }

        public int CID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        public string DESCRIPTION { get; set; }
        public double? VALUE { get; set; }
        public string REMARKS { get; set; }

        public ICollection<COS_PRECOSTING_MASTER> COS_PRECOSTING_MASTER { get; set; }
    }
}
