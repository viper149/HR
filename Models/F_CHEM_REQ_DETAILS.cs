using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_CHEM_REQ_DETAILS : BaseEntity
    {
        public F_CHEM_REQ_DETAILS()
        {
            F_CHEM_ISSUE_DETAILS = new HashSet<F_CHEM_ISSUE_DETAILS>();
        }

        public int CRQID { get; set; }
        public int? CSRID { get; set; }
        [Display(Name = "Chemical Name")]
        public int? PRODUCTID { get; set; }
        [Display(Name = "Required Quantity")]
        [Range(0, double.MaxValue, ErrorMessage = "The field {0} must be greater than {1}")]
        public double? REQ_QTY { get; set; }
        [Display(Name = "Status")]
        public string STATUS { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }

        public F_CHEM_REQ_MASTER CSR { get; set; }
        public F_CHEM_STORE_PRODUCTINFO PRODUCT { get; set; }
        public ICollection<F_CHEM_ISSUE_DETAILS> F_CHEM_ISSUE_DETAILS { get; set; }
    }
}
