using System.ComponentModel.DataAnnotations;

namespace DenimERP.Models
{
    public partial class UPAS
    {
        public int Id { get; set; }
        [Display(Name = "UPAS Name")]
        public string NAME { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
    }
}
