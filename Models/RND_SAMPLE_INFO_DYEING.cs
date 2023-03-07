using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class RND_SAMPLE_INFO_DYEING : BaseEntity
    {
        public RND_SAMPLE_INFO_DYEING()
        {
            PL_SAMPLE_PROG_SETUP = new HashSet<PL_SAMPLE_PROG_SETUP>();
            RND_PRODUCTION_ORDER = new HashSet<RND_PRODUCTION_ORDER>();
            RND_SAMPLE_INFO_DETAILS = new HashSet<RND_SAMPLE_INFO_DETAILS>();
        }
        
        public int SDID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? SIDATE { get; set; }
        [Display(Name = "Program No")]
        public string PROG_NO { get; set; }
        [RegularExpression(@"^(((\d{1})*))$", ErrorMessage = "Negative Value Not Acceptable.")]
        [Display(Name = "SDRF No")]
        public int? SDRFID { get; set; }
        [Display(Name = "Buyer Name")]
        public int? BUYERID { get; set; }
        [Display(Name = "RS Code")]
        public string DYEINGCODE { get; set; }
        [Display(Name = "RS Order")]
        public string RSOrder { get; set; }
        [Display(Name = "Style Ref.")]
        public string STYLEREF { get; set; }
        [Display(Name = "RnD Team")]
        public string RNDTEAM { get; set; }
        [Display(Name = "Rnd Person")]
        public int? RNDPERSON { get; set; }
        [Display(Name = "Ends Rope")]
        public string ENDS_ROPE { get; set; }
        [Display(Name = "No. of Rope")]
        public string NO_OF_ROPE { get; set; }
        [Display(Name = "Total Ends")]
        public int? TOTAL_ENDS { get; set; }
        [Display(Name = "Length (Mtr)")]
        public int? LENGTH_MTR { get; set; }
        [Display(Name = "Dyeing Reference")]
        public string DYEING_REF { get; set; }
        [Display(Name = "Reed Space")]
        public string REED_SPACE { get; set; }
        [Display(Name = "Loom Type")]
        public int? LOOMID { get; set; }
        [Display(Name = "Dyeing Type")]
        public int? DID { get; set; }
        [Display(Name = "Color")]
        public int? COLORCODE { get; set; }
        [Display(Name = "Warp Program Date")]
        public DateTime? WARP_PROG_DATE { get; set; }
        [Display(Name = "Committed Delivery Date")]
        public DateTime? COMMITED_DEL_DATE { get; set; }
        [Display(Name = "Yarn Location")]
        public string YARN_LOCATION { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        [Display(Name = "SDRF NO(Manual)")]
        public string OPT1 { get; set; }
        [Display(Name = "Option 2")]
        public string OPT2 { get; set; }
        [Display(Name = "Option 2")]
        public string OPT3 { get; set; }
        [Display(Name = "Option 4")]
        public string OPT4 { get; set; }
        [Display(Name = "Program No")]
        public string OPT5 { get; set; }
        [Display(Name = "User Id")]
        public string USERID { get; set; }
        [DisplayName("Is OK?")]
        public bool STATUS { get; set; }

        public RND_DYEING_TYPE D { get; set; }
        public LOOM_TYPE LOOM { get; set; }
        public BAS_COLOR COLOR { get; set; }
        public MKT_SDRF_INFO SDRF { get; set; }
        public BAS_BUYERINFO BUYER { get; set; }
        public F_HRD_EMPLOYEE RNDPERSONNavigation { get; set; }

        public ICollection<PL_SAMPLE_PROG_SETUP> PL_SAMPLE_PROG_SETUP { get; set; }
        public ICollection<RND_PRODUCTION_ORDER> RND_PRODUCTION_ORDER { get; set; }
        public ICollection<RND_SAMPLE_INFO_DETAILS> RND_SAMPLE_INFO_DETAILS { get; set; }
        public ICollection<F_YARN_REQ_DETAILS> F_YARN_REQ_DETAILS { get; set; }
    }
}
