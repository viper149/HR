using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class RND_FABRIC_COUNTINFO : BaseEntity
    {
        public RND_FABRIC_COUNTINFO()
        {
            F_YARN_REQ_DETAILS = new HashSet<F_YARN_REQ_DETAILS>();
            F_PR_WEAVING_WEFT_YARN_CONSUM_DETAILS = new HashSet<F_PR_WEAVING_WEFT_YARN_CONSUM_DETAILS>();
            //F_YS_YARN_ISSUE_DETAILS = new HashSet<F_YS_YARN_ISSUE_DETAILS>();
            LOOM_SETTING_CHANNEL_INFO = new HashSet<LOOM_SETTING_CHANNEL_INFO>();
            F_PR_WARPING_PROCESS_DW_DETAILS = new HashSet<F_PR_WARPING_PROCESS_DW_DETAILS>();
            F_PR_WARPING_PROCESS_DW_YARN_CONSUM_DETAILS = new HashSet<F_PR_WARPING_PROCESS_DW_YARN_CONSUM_DETAILS>();
            PL_BULK_PROG_YARN_D = new HashSet<PL_BULK_PROG_YARN_D>();
            F_PR_WARPING_PROCESS_ECRU_DETAILS = new HashSet<F_PR_WARPING_PROCESS_ECRU_DETAILS>();
            F_PR_WARPING_PROCESS_ECRU_YARN_CONSUM_DETAILS = new HashSet<F_PR_WARPING_PROCESS_ECRU_YARN_CONSUM_DETAILS>();
            F_PR_WARPING_PROCESS_SW_DETAILS = new HashSet<F_PR_WARPING_PROCESS_SW_DETAILS>();
            F_PR_WARPING_PROCESS_SW_YARN_CONSUM_DETAILS = new HashSet<F_PR_WARPING_PROCESS_SW_YARN_CONSUM_DETAILS>();
            F_YARN_REQ_DETAILS_S = new HashSet<F_YARN_REQ_DETAILS_S>();
        }
        public int TRNSID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [Display(Name = "Fabric Code")]
        public int FABCODE { get; set; }
        //[Required(ErrorMessage = "Count cannot be empty.")]
        [Display(Name = "Yarn Count")]
        public int? COUNTID { get; set; }
        [Display(Name = "Type")]
        public string YARNTYPE { get; set; }
        [Display(Name = "Description")]
        public string DESCRIPTION { get; set; }
        [Display(Name = "Color")]
        public int? COLORCODE { get; set; }
        [Display(Name = "Lot")]
        public int? LOTID { get; set; }
        [Display(Name = "Supplier")]
        public int? SUPPID { get; set; }
        [Display(Name = "Ratio")]
        public double? RATIO { get; set; }
        [Display(Name = "Ne")]
        public double? NE { get; set; }
        [Display(Name = "Yarn For")]
        public int? YARNFOR { get; set; }
        [NotMapped]
        [Display(Name = "Amount")]
        public double? AMOUNT { get; set; }

        public BAS_YARN_COUNTINFO COUNT { get; set; }
        public RND_FABRICINFO FABCODENavigation { get; set; }
        public BAS_YARN_LOTINFO LOT { get; set; }
        public BAS_SUPPLIERINFO SUPP { get; set; }
        public YARNFOR YarnFor { get; set; }
        public BAS_COLOR Color { get; set; }

        public ICollection<F_YARN_REQ_DETAILS> F_YARN_REQ_DETAILS { get; set; }
        public ICollection<F_PR_WEAVING_WEFT_YARN_CONSUM_DETAILS> F_PR_WEAVING_WEFT_YARN_CONSUM_DETAILS { get; set; }
        //public ICollection<F_YS_YARN_ISSUE_DETAILS> F_YS_YARN_ISSUE_DETAILS { get; set; }
        public ICollection<LOOM_SETTING_CHANNEL_INFO> LOOM_SETTING_CHANNEL_INFO { get; set; }
        public ICollection<F_PR_WARPING_PROCESS_DW_DETAILS> F_PR_WARPING_PROCESS_DW_DETAILS { get; set; }
        public ICollection<F_PR_WARPING_PROCESS_DW_YARN_CONSUM_DETAILS> F_PR_WARPING_PROCESS_DW_YARN_CONSUM_DETAILS { get; set; }
        public ICollection<PL_BULK_PROG_YARN_D> PL_BULK_PROG_YARN_D { get; set; }
        public ICollection<F_PR_WARPING_PROCESS_ECRU_DETAILS> F_PR_WARPING_PROCESS_ECRU_DETAILS { get; set; }
        public ICollection<F_PR_WARPING_PROCESS_ECRU_YARN_CONSUM_DETAILS> F_PR_WARPING_PROCESS_ECRU_YARN_CONSUM_DETAILS { get; set; }
        public ICollection<F_PR_WARPING_PROCESS_SW_DETAILS> F_PR_WARPING_PROCESS_SW_DETAILS { get; set; }
        public ICollection<F_PR_WARPING_PROCESS_SW_YARN_CONSUM_DETAILS> F_PR_WARPING_PROCESS_SW_YARN_CONSUM_DETAILS { get; set; }
        public ICollection<F_YARN_REQ_DETAILS_S> F_YARN_REQ_DETAILS_S { get; set; }

        public ICollection<F_PR_RECONE_YARN_CONSUMPTION> F_PR_RECONE_YARN_CONSUMPTION { get; set; }

    }
}
