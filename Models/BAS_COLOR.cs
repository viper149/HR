using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DenimERP.Models
{
    public partial class BAS_COLOR
    {
        public BAS_COLOR()
        {
            RND_FABRICINFO = new HashSet<RND_FABRICINFO>();
            COS_PRECOSTING_MASTER = new HashSet<COS_PRECOSTING_MASTER>();
            RND_SAMPLE_INFO_DETAILS = new HashSet<RND_SAMPLE_INFO_DETAILS>();
            RND_SAMPLE_INFO_WEAVING_DETAILS = new HashSet<RND_SAMPLE_INFO_WEAVING_DETAILS>();
            F_SAMPLE_GARMENT_RCV_D = new HashSet<F_SAMPLE_GARMENT_RCV_D>();
            BAS_YARN_COUNTINFO = new HashSet<BAS_YARN_COUNTINFO>();
            MKT_SWATCH_CARD = new HashSet<MKT_SWATCH_CARD>();
            RND_FABRIC_COUNTINFO = new HashSet<RND_FABRIC_COUNTINFO>();
            F_QA_YARN_TEST_INFORMATION_POLYESTER = new HashSet<F_QA_YARN_TEST_INFORMATION_POLYESTER>();
            RND_SAMPLE_INFO_DYEING = new HashSet<RND_SAMPLE_INFO_DYEING>();
            RND_SAMPLEINFO_FINISHING = new HashSet<RND_SAMPLEINFO_FINISHING>();
            RND_BOM = new HashSet<RND_BOM>();
        }

        public int COLORCODE { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [Required(ErrorMessage = "Color can not be empty")]
        [Display(Name = "Color Name")]
        public string COLOR { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }

        public ICollection<RND_FABRICINFO> RND_FABRICINFO { get; set; }
        public ICollection<COS_PRECOSTING_MASTER> COS_PRECOSTING_MASTER { get; set; }
        public ICollection<RND_SAMPLE_INFO_DETAILS> RND_SAMPLE_INFO_DETAILS { get; set; }
        public ICollection<RND_SAMPLE_INFO_WEAVING_DETAILS> RND_SAMPLE_INFO_WEAVING_DETAILS { get; set; }
        public ICollection<F_SAMPLE_GARMENT_RCV_D> F_SAMPLE_GARMENT_RCV_D { get; set; }
        public ICollection<BAS_YARN_COUNTINFO> BAS_YARN_COUNTINFO { get; set; }
        public ICollection<MKT_SWATCH_CARD> MKT_SWATCH_CARD { get; set; }
        public ICollection<RND_FABRIC_COUNTINFO> RND_FABRIC_COUNTINFO { get; set; }
        public ICollection<F_QA_YARN_TEST_INFORMATION_POLYESTER> F_QA_YARN_TEST_INFORMATION_POLYESTER { get; set; }
        public ICollection<RND_SAMPLE_INFO_DYEING> RND_SAMPLE_INFO_DYEING { get; set; }
        public ICollection<RND_SAMPLEINFO_FINISHING> RND_SAMPLEINFO_FINISHING { get; set; }
        public ICollection<RND_BOM> RND_BOM { get; set; }
    }
}
