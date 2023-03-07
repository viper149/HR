using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Models
{
    public partial class BAS_SUPPLIERINFO
    {
        public BAS_SUPPLIERINFO()
        {
            COM_IMP_CSRAT_DETAILS = new HashSet<COM_IMP_CSRAT_DETAILS>();
            COM_IMP_LCINFORMATION = new HashSet<COM_IMP_LCINFORMATION>();
            RND_FABRIC_COUNTINFO = new HashSet<RND_FABRIC_COUNTINFO>();
            RND_SAMPLE_INFO_DETAILS = new HashSet<RND_SAMPLE_INFO_DETAILS>();
            RND_SAMPLE_INFO_WEAVING_DETAILS = new HashSet<RND_SAMPLE_INFO_WEAVING_DETAILS>();
            PL_ORDERWISE_LOTINFO = new HashSet<PL_ORDERWISE_LOTINFO>();
            RND_MSTR_ROLL = new HashSet<RND_MSTR_ROLL>();
            F_PR_WEAVING_WEFT_YARN_CONSUM_DETAILS = new HashSet<F_PR_WEAVING_WEFT_YARN_CONSUM_DETAILS>();
            //COS_PRECOSTING_DETAILS = new HashSet<COS_PRECOSTING_DETAILS>();
            LOOM_SETTING_CHANNEL_INFO = new HashSet<LOOM_SETTING_CHANNEL_INFO>();
            FYsYarnReceiveMasters = new List<F_YS_YARN_RECEIVE_MASTER>();
            FChemStoreReceiveMasters = new HashSet<F_CHEM_STORE_RECEIVE_MASTER>();
            F_GEN_S_RECEIVE_MASTER = new HashSet<F_GEN_S_RECEIVE_MASTER>();
            COM_IMP_WORK_ORDER_MASTER = new HashSet<COM_IMP_WORK_ORDER_MASTER>();
            F_QA_FIRST_MTR_ANALYSIS_D = new HashSet<F_QA_FIRST_MTR_ANALYSIS_D>();
            F_YS_YARN_RECEIVE_MASTER_S = new HashSet<F_YS_YARN_RECEIVE_MASTER_S>();
            PROC_WORKORDER_MASTER = new HashSet<PROC_WORKORDER_MASTER>();
            F_YS_YARN_RECEIVE_MASTER2 = new HashSet<F_YS_YARN_RECEIVE_MASTER2>();
        }

        public int SUPPID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [Remote(action: "IsSupplierInfoInUse", controller: "BasicSupplier")]
        [Display(Name = "Supplier Name" )]
        public string SUPPNAME { get; set; }
        [Display(Name = "Supplier Category")]
        public int SCATID { get; set; }
        public string ADDRESS { get; set; }
        public string PHONE { get; set; }
        public string EMAIL { get; set; }
        public string CPERSON { get; set; }
        public string REMARKS { get; set; }

        public BAS_SUPP_CATEGORY SCAT { get; set; }

        public ICollection<COM_IMP_CSRAT_DETAILS> COM_IMP_CSRAT_DETAILS { get; set; }
        public ICollection<COM_IMP_LCINFORMATION> COM_IMP_LCINFORMATION { get; set; }
        public ICollection<RND_FABRIC_COUNTINFO> RND_FABRIC_COUNTINFO { get; set; }
        public ICollection<RND_SAMPLE_INFO_DETAILS> RND_SAMPLE_INFO_DETAILS { get; set; }
        public ICollection<RND_SAMPLE_INFO_WEAVING_DETAILS> RND_SAMPLE_INFO_WEAVING_DETAILS { get; set; }
        public ICollection<PL_ORDERWISE_LOTINFO> PL_ORDERWISE_LOTINFO { get; set; }
        public ICollection<RND_MSTR_ROLL> RND_MSTR_ROLL { get; set; }
        public ICollection<F_PR_WEAVING_WEFT_YARN_CONSUM_DETAILS> F_PR_WEAVING_WEFT_YARN_CONSUM_DETAILS { get; set; }
        //public ICollection<COS_PRECOSTING_DETAILS> COS_PRECOSTING_DETAILS { get; set; }
        public ICollection<LOOM_SETTING_CHANNEL_INFO> LOOM_SETTING_CHANNEL_INFO { get; set; }
        public ICollection<F_YS_YARN_RECEIVE_MASTER> FYsYarnReceiveMasters { get; set; }
        public ICollection<F_CHEM_STORE_RECEIVE_MASTER> FChemStoreReceiveMasters { get; set; }
        public ICollection<F_GEN_S_RECEIVE_MASTER> F_GEN_S_RECEIVE_MASTER { get; set; }
        public ICollection<COM_IMP_WORK_ORDER_MASTER> COM_IMP_WORK_ORDER_MASTER { get; set; }
        public ICollection<F_QA_FIRST_MTR_ANALYSIS_D> F_QA_FIRST_MTR_ANALYSIS_D { get; set; }
        public ICollection<F_YS_YARN_RECEIVE_MASTER_S> F_YS_YARN_RECEIVE_MASTER_S { get; set; }
        public ICollection<PROC_WORKORDER_MASTER> PROC_WORKORDER_MASTER { get; set; }
        public ICollection<F_YS_YARN_RECEIVE_MASTER2> F_YS_YARN_RECEIVE_MASTER2 { get; set; }
    }
}