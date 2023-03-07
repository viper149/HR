using System.ComponentModel.DataAnnotations;

namespace DenimERP.Models
{
    public partial class RND_ANALYSIS_SHEET_DETAILS
    {
        public int ADID { get; set; }
        public int? AID { get; set; }
        [Display(Name = "Yarn Count")]
        public int? COUNTID { get; set; }
        [Display(Name = "Yarn Length")]
        public string YARN_LENGTH { get; set; }
        [Display(Name = "Yarn Weight")]
        public string YARN_WEIGHT { get; set; }
        [Display(Name = "No of Yarn")]
        public int? NO_OF_YARN { get; set; }
        [Display(Name = "Yarn For")]
        public string YARN_FOR { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }

        public RND_ANALYSIS_SHEET A { get; set; }
        public BAS_YARN_COUNTINFO BasYarnCountinfo { get; set; }
    }
}
