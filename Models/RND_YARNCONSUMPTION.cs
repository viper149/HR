using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DenimERP.Models
{
    public partial class RND_YARNCONSUMPTION
    {
        public int TRNSID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [DisplayName("Fabric Code")]
        public int? FABCODE { get; set; }
        [DisplayName("Count Name")]
        public int? COUNTID { get; set; }
        [DisplayName("Color Name")]
        public int? COLOR { get; set; }
        [DisplayName("Yarn For")]
        public int? YARNFOR { get; set; }
        [Display(Name = "Amount")]
        public double? AMOUNT { get; set; }
        public double? OLD_CONSUMPTION { get; set; }

        public BAS_YARN_COUNTINFO COUNT { get; set; }
        public RND_FABRICINFO FABCODENavigation { get; set; }
    }
}
