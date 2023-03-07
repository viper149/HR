using System.ComponentModel.DataAnnotations;

namespace DenimERP.ViewModels
{
    public class RndFabricInfoAndYarnConsumption
    {
        public int COUNTID { get; set; }
        [Display(Name = "Count")]
        public string COUNTNAME { get; set; }
        public string YARNTYPE { get; set; }
        public int? LOTID { get; set; }
        [Display(Name = "Lot")]
        public string LOTNO { get; set; }
        public int? SUPPID { get; set; }
        [Display(Name = "Supplier")]
        public string SUPPNAME { get; set; }
        public int? COLORCODE { get; set; }
        [Display(Name = "Color")]
        public string COLOR { get; set; }
        public double RATIO { get; set; }
        public double NE { get; set; }
        public int? YARNFOR { get; set; }
        public double? AMOUNT { get; set; }
        public int OverHead { get; set; }
    }
}
