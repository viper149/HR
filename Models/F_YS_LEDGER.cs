using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_YS_LEDGER : BaseEntity
    {
        public F_YS_LEDGER()
        {
            F_YS_YARN_RECEIVE_DETAILS = new HashSet<F_YS_YARN_RECEIVE_DETAILS>();
            F_YS_YARN_RECEIVE_DETAILS_S = new HashSet<F_YS_YARN_RECEIVE_DETAILS_S>();
            F_YS_GP_DETAILS = new HashSet<F_YS_GP_DETAILS>();
            F_YS_YARN_RECEIVE_DETAILS2 = new HashSet<F_YS_YARN_RECEIVE_DETAILS2>();
        }

        public int LEDID { get; set; }
        [Display(Name = "Ledger Name")]
        public string LEDNAME { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }

        public ICollection<F_YS_YARN_RECEIVE_DETAILS> F_YS_YARN_RECEIVE_DETAILS { get; set; }
        public ICollection<F_YS_YARN_RECEIVE_DETAILS_S> F_YS_YARN_RECEIVE_DETAILS_S { get; set; }
        public ICollection<F_YS_GP_DETAILS> F_YS_GP_DETAILS { get; set; }
        public ICollection<F_YS_YARN_RECEIVE_DETAILS2> F_YS_YARN_RECEIVE_DETAILS2 { get; set; }
    }
}
