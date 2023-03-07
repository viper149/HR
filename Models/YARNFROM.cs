using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DenimERP.Models
{
    public partial class YARNFROM
    {
        public YARNFROM()
        {
            F_YS_INDENT_DETAILS = new HashSet<F_YS_INDENT_DETAILS>();
        }

        public int YFID { get; set; }
        [Display(Name = "Yarn From")]
        public string TYPENAME { get; set; }

        public ICollection<F_YS_INDENT_DETAILS> F_YS_INDENT_DETAILS { get; set; }
    }
}
