using System.ComponentModel.DataAnnotations;

namespace DenimERP.ViewModels.Basic
{
    public class BasProductViewModel
    {
        public int CATID { get; set; }
        [Required(ErrorMessage = "Please enter category. Category can no be empty")]
        public string CATEGORY { get; set; }
        public string REMARKS { get; set; }
    }
}
