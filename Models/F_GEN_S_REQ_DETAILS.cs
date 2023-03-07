using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_GEN_S_REQ_DETAILS : BaseEntity
    {
        public F_GEN_S_REQ_DETAILS()
        {
            F_GEN_S_ISSUE_DETAILS = new HashSet<F_GEN_S_ISSUE_DETAILS>();
        }

        public int GRQID { get; set; }
        public int? GSRID { get; set; }
        [Display(Name = "Product Name")]
        public int? PRODUCTID { get; set; }
        [Display(Name = "Required Quantity")]
        [Range(1, double.MaxValue, ErrorMessage = "The field {0} must be greater than {1}")]
        public double? REQ_QTY { get; set; }
        [Display(Name = "Status")]
        public string STATUS { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }

        public F_GEN_S_REQ_MASTER GSR { get; set; }
        public F_GS_PRODUCT_INFORMATION PRODUCT { get; set; }
        public ICollection<F_GEN_S_ISSUE_DETAILS> F_GEN_S_ISSUE_DETAILS { get; set; }
    }
}
