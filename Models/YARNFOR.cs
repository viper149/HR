using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class YARNFOR : BaseEntity
    {
        public YARNFOR()
        {
            RND_SAMPLE_INFO_DETAILS = new HashSet<RND_SAMPLE_INFO_DETAILS>();
            RND_SAMPLE_INFO_WEAVING_DETAILS = new HashSet<RND_SAMPLE_INFO_WEAVING_DETAILS>();
            PL_ORDERWISE_LOTINFO = new HashSet<PL_ORDERWISE_LOTINFO>();
            PL_BULK_PROG_SETUP_D = new HashSet<PL_BULK_PROG_SETUP_D>();
            F_YS_INDENT_DETAILS = new HashSet<F_YS_INDENT_DETAILS>();
            RND_FABRIC_COUNTINFO = new HashSet<RND_FABRIC_COUNTINFO>();
            COS_PRECOSTING_DETAILS = new HashSet<COS_PRECOSTING_DETAILS>();
            F_YS_YARN_RECEIVE_DETAILS = new HashSet<F_YS_YARN_RECEIVE_DETAILS>();
            COS_POSTCOSTING_YARNDETAILS = new HashSet<COS_POSTCOSTING_YARNDETAILS>();
        }

        public int YARNID { get; set; }
        [Display(Name = "Yarn For")]
        public string YARNNAME { get; set; }

        public ICollection<RND_SAMPLE_INFO_DETAILS> RND_SAMPLE_INFO_DETAILS { get; set; }
        public ICollection<RND_SAMPLE_INFO_WEAVING_DETAILS> RND_SAMPLE_INFO_WEAVING_DETAILS { get; set; }
        public ICollection<PL_ORDERWISE_LOTINFO> PL_ORDERWISE_LOTINFO { get; set; }
        public ICollection<PL_BULK_PROG_SETUP_D> PL_BULK_PROG_SETUP_D { get; set; }
        public ICollection<F_YS_INDENT_DETAILS> F_YS_INDENT_DETAILS { get; set; }
        public ICollection<RND_FABRIC_COUNTINFO> RND_FABRIC_COUNTINFO { get; set; }
        public ICollection<COS_PRECOSTING_DETAILS> COS_PRECOSTING_DETAILS { get; set; }
        public ICollection<F_YS_YARN_RECEIVE_DETAILS> F_YS_YARN_RECEIVE_DETAILS { get; set; }
        public ICollection<COS_POSTCOSTING_YARNDETAILS> COS_POSTCOSTING_YARNDETAILS { get; set; }
    }
}