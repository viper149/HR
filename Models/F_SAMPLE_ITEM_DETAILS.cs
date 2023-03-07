using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DenimERP.Models
{
    public partial class F_SAMPLE_ITEM_DETAILS
    {
        public F_SAMPLE_ITEM_DETAILS()
        {
            F_SAMPLE_GARMENT_RCV_D = new HashSet<F_SAMPLE_GARMENT_RCV_D>();
            F_SAMPLE_FABRIC_RCV_D = new HashSet<F_SAMPLE_FABRIC_RCV_D>();
        }

        public int SITEMID { get; set; }
        [Display(Name = "Item Name")]
        public string NAME { get; set; }
        [Display(Name = "Description")]
        public string DESCRIPTION { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }

        public ICollection<F_SAMPLE_GARMENT_RCV_D> F_SAMPLE_GARMENT_RCV_D { get; set; }
        public ICollection<F_SAMPLE_FABRIC_RCV_D> F_SAMPLE_FABRIC_RCV_D { get; set; }
    }
}
