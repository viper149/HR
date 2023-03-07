using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DenimERP.Models
{
    public partial class F_SAMPLE_FABRIC_RCV_D
    {
        public F_SAMPLE_FABRIC_RCV_D()
        {
            FSampleFabricDispatchDetailses = new HashSet<F_SAMPLE_FABRIC_DISPATCH_DETAILS>();
        }

        public int TRNSID { get; set; }
        public int? SFRID { get; set; }
        public int? SITEMID { get; set; }
        [Display(Name = "Development No.")]
        public string DEV_NO { get; set; }
        public int? FABCODE { get; set; }
        [NotMapped]
        public int? WVID { get; set; }
        public int? SETID { get; set; }
        [Display(Name = "Fabric Grade")]
        public string FAB_GRADE { get; set; }
        [Display(Name = "Qty")]
        public double? QTY { get; set; }
        [Display(Name = "Roll No")]
        public string ROLLNO { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }
        [Display(Name = "Barcode")]
        public string BARCODE { get; set; }

        [NotMapped] public string StyleNo { get; set; }

        public F_SAMPLE_FABRIC_RCV_M SFR { get; set; }
        public F_SAMPLE_ITEM_DETAILS SITEM { get; set; }
        public RND_FABRICINFO FABCODENavigation { get; set; }
        public PL_PRODUCTION_SETDISTRIBUTION SET { get; set; }

        public ICollection<F_SAMPLE_FABRIC_DISPATCH_DETAILS> FSampleFabricDispatchDetailses { get; set; }
    }
}
