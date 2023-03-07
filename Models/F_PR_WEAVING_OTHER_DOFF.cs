using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DenimERP.Models
{
    public class F_PR_WEAVING_OTHER_DOFF
    {
        public F_PR_WEAVING_OTHER_DOFF()
        {
            F_PR_WEAVING_PROCESS_DETAILS_B = new HashSet<F_PR_WEAVING_PROCESS_DETAILS_B>();
        }
        public int ID { get; set; }
        [Display(Name = "Name")]
        public string NAME { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        public virtual ICollection<F_PR_WEAVING_PROCESS_DETAILS_B> F_PR_WEAVING_PROCESS_DETAILS_B { get; set; }
    }
}
