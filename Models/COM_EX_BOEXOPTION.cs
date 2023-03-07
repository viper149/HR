using System.ComponentModel.DataAnnotations;

namespace DenimERP.Models
{
    public partial class COM_EX_BOEXOPTION
    {
        public int OPTIONID { get; set; }
        [Display(Name = "Bill of Exe Option")]
        public string OPTIONNAME { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
    }
}
