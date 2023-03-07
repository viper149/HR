using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class H_SAMPLE_RECEIVING_D : BaseEntity
    {
        public H_SAMPLE_RECEIVING_D()
        {
            H_SAMPLE_DESPATCH_D = new HashSet<H_SAMPLE_DESPATCH_D>();
        }

        public int RCVDID { get; set; }
        public int? RCVID { get; set; }
        public int? TRNSID { get; set; }
        public int? BUYERID { get; set; }
        [Display(Name = "Unit")]
        public int? UID { get; set; }
        [Display(Name = "Quantity")]
        public double? QTY { get; set; }

        public BAS_BUYERINFO BUYER { get; set; }
        public H_SAMPLE_RECEIVING_M RCV { get; set; }
        public F_SAMPLE_GARMENT_RCV_D TRNS { get; set; }
        public F_BAS_UNITS U { get; set; }
        public ICollection<H_SAMPLE_DESPATCH_D> H_SAMPLE_DESPATCH_D { get; set; }
    }
}
