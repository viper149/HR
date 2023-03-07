using System.ComponentModel.DataAnnotations;

namespace DenimERP.ViewModels
{
    public class UpdateCountInfoViewModel
    {
        [Display(Name = "Old Count")]
        public int? OLD_COUNTID { get; set; }
        [Display(Name = "New Count")]
        public int? NEW_COUNTID { get; set; }
    }
}
