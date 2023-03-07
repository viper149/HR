using System.ComponentModel.DataAnnotations;

namespace DenimERP.ViewModels
{
    public class ColorViewModel
    {
        [Required(ErrorMessage = "Color can not be empty")]
        [Display(Name = "Color Name")]
        public string COLOR { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
    }
}
