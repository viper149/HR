using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DenimERP.Models;

namespace DenimERP.ViewModels.Rnd
{
    public class RndFabricCountInfoViewModel
    {
        public RND_FABRIC_COUNTINFO RndFabricCountinfo { get; set; }
        public F_PR_WEAVING_WEFT_YARN_CONSUM_DETAILS FPrWeavingWeftYarnConsumDetails { get; set; }

        [DisplayName("Yarn For")]
        public int? YARNFOR { get; set; }
        [Display(Name = "Amount")]
        public double? AMOUNT { get; set; }
        public bool IsConsumption { get; set; }
    }
}
