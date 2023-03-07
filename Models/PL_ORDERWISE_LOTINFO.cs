using System;
using System.ComponentModel.DataAnnotations;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class PL_ORDERWISE_LOTINFO : BaseEntity
    {
        public int TRNSID { get; set; }
        [Display(Name = "Trans. Date")]
        public DateTime? TRNSDATE { get; set; }
        public int? POID { get; set; }
        [Display(Name = "Lot No.")]
        public int? LOTID { get; set; }
        [Display(Name = "Yarn Type")]
        public int? YARNTYPE { get; set; }
        [Display(Name = "Supplier Name")]
        public int? SUPPID { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }

        public BAS_YARN_LOTINFO LOT { get; set; }
        public BAS_SUPPLIERINFO SUPP { get; set; }
        public YARNFOR YARNFOR { get; set; }
    }
}
