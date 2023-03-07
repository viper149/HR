using System;
using System.ComponentModel.DataAnnotations;

namespace DenimERP.Models
{
    public partial class F_CHEM_QC_APPROVE
    {
        public int CQCA { get; set; }
        [Display(Name = "Receive No")]
        public int? CRDID { get; set; }
        [Display(Name = "Approved By")]
        public string APPROVED_BY { get; set; }
        [Display(Name = "QC Approved Date")]
        public DateTime? CQCADATE { get; set; }
        public string CREATED_BY { get; set; }
        public string UPDATED_BY { get; set; }
        public DateTime? CREATED_AT { get; set; }
        public DateTime? UPDATED_AT { get; set; }

        public F_CHEM_STORE_RECEIVE_DETAILS CRD { get; set; }
    }
}
