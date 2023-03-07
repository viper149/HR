using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_BAS_ASSET_LIST : BaseEntity
    {
        [Display(Name = "Asset Code")]
        public int ASSET_ID { get; set; }
        [Display(Name = "Asset Name")]
        [Required(ErrorMessage = "{0} can not be empty.")]
        public string ASSET_NAME { get; set; }
        [Display(Name = "Asset Location")]
        [Required(ErrorMessage = "Asset Location Must Be selected")]
        public int? SEC_CODE { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }

        [NotMapped]
        public string EncryptedId { get; set; }

        public F_BAS_SECTION SEC { get; set; }
    }
}
