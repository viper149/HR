using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DenimERP.Models
{
    public partial class F_YARN_TRANSACTION_TYPE
    {
        public F_YARN_TRANSACTION_TYPE()
        {
            F_YS_GP_DETAILS = new HashSet<F_YS_GP_DETAILS>();
        }
        public int STOCKID { get; set; }
        [Display(Name = "Stock Type")]
        public string NAME { get; set; }

        public ICollection<F_YS_GP_DETAILS> F_YS_GP_DETAILS { get; set; }
    }
}
