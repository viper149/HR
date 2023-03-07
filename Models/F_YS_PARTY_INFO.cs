using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DenimERP.Models
{
    public partial class F_YS_PARTY_INFO
    {
        public F_YS_PARTY_INFO()
        {
            F_YS_GP_MASTER = new HashSet<F_YS_GP_MASTER>();
        }
       
        public int PARTY_ID { get; set; }

        [Display(Name = "Party Name")]
        public string PARTY_NAME { get; set; }
        [Display(Name = "Address")]
        public string ADDRESS { get; set; }
        [Display(Name = "Contract Person")]
        public string CONTRACT_PERSON { get; set; }
        [Display(Name = "Cell")]
        public string CELL_NO { get; set; }
        public string REMARKS { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }

        public ICollection<F_YS_GP_MASTER> F_YS_GP_MASTER { get; set; }
    }
}
