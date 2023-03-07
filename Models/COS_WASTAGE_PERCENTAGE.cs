using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DenimERP.Models
{
    public partial class COS_WASTAGE_PERCENTAGE
    {
        public int WESTAGE_ID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [Display(Name = "Description")]
        public string DESCRIPTION { get; set; }
        [Display(Name = "Value")]
        public double? VALUE { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
    }
}
