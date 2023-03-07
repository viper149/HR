using System.ComponentModel.DataAnnotations;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class COS_POSTCOSTING_YARNDETAILS : BaseEntity
    {
        public int TRNSID { get; set; }
        public int PCOSTID { get; set; }
        [Display(Name = "Count Name")]
        public int? COUNTID { get; set; }
        [Display(Name = "Lot")]
        public int? LOTID { get; set; }
        [Display(Name = "Yarn For")]
        public int? YARNFOR { get; set; }
        [Display(Name = "Consumption")]
        public double? CONSUMPTION { get; set; }
        [Display(Name = "Rate")]
        public double? RATE { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }

        public BAS_YARN_COUNTINFO COUNT { get; set; }
        public BAS_YARN_LOTINFO LOT { get; set; }
        public YARNFOR YarnFor { get; set; }
        public COS_POSTCOSTING_MASTER PCOST { get; set; }
    }
}
