using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DenimERP.Models
{
    public partial class F_SAMPLE_LOCATION
    {
        public F_SAMPLE_LOCATION()
        {
            F_SAMPLE_GARMENT_RCV_D = new HashSet<F_SAMPLE_GARMENT_RCV_D>();
        }

        public int LOCID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [Display(Name = "Location Name", Prompt = "Type the location name")]
        [Required(ErrorMessage = "Location name can not be empty")]
        public string NAME { get; set; }
        [Display(Name = "Description", Prompt = "Add your description here")]
        public string DESCRIPTION { get; set; }
        [Display(Name = "Remarks", Prompt = "Remarks")]
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }

        public ICollection<F_SAMPLE_GARMENT_RCV_D> F_SAMPLE_GARMENT_RCV_D { get; set; }
    }
}
