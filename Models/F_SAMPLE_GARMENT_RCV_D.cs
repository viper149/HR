using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DenimERP.Models
{
    public partial class F_SAMPLE_GARMENT_RCV_D
    {
        public F_SAMPLE_GARMENT_RCV_D()
        {
            F_SAMPLE_DESPATCH_DETAILS = new HashSet<F_SAMPLE_DESPATCH_DETAILS>();
            H_SAMPLE_RECEIVING_D = new HashSet<H_SAMPLE_RECEIVING_D>();
        }

        public int TRNSID { get; set; }

        public int? SGRID { get; set; }
        public int? SITEMID { get; set; }
        [Display(Name = "Development No.")]
        public string DEV_NO { get; set; }
        public int? COLORID { get; set; }
        public int? FABCODE { get; set; }
        [Display(Name = "Quantity")]
        public double? QTY { get; set; }
        public int? LOCID { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }
        public int? BUYERID { get; set; }
        [Display(Name = "Barcode", Prompt = "Scan your barcode here.")]
        public string BARCODE { get; set; }

        public BAS_COLOR COLOR { get; set; }
        public RND_FABRICINFO FABCODENavigation { get; set; }
        public F_SAMPLE_LOCATION LOC { get; set; }
        public F_SAMPLE_GARMENT_RCV_M SGR { get; set; }
        public F_SAMPLE_ITEM_DETAILS SITEM { get; set; }
        public BAS_BUYERINFO BUYER { get; set; }
        public ICollection<F_SAMPLE_DESPATCH_DETAILS> F_SAMPLE_DESPATCH_DETAILS { get; set; }
        public ICollection<H_SAMPLE_RECEIVING_D> H_SAMPLE_RECEIVING_D { get; set; }
    }
}
