using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_BAS_UNITS : BaseEntity
    {
        public F_BAS_UNITS()
        {
            F_CHEM_STORE_PRODUCTINFO = new HashSet<F_CHEM_STORE_PRODUCTINFO>();
            F_GS_PRODUCT_INFORMATION = new HashSet<F_GS_PRODUCT_INFORMATION>();
            F_SAMPLE_DESPATCH_DETAILS = new HashSet<F_SAMPLE_DESPATCH_DETAILS>();
            H_SAMPLE_RECEIVING_D = new HashSet<H_SAMPLE_RECEIVING_D>();
            H_SAMPLE_DESPATCH_M = new HashSet<H_SAMPLE_DESPATCH_M>();
            FYarnReqDetailses = new HashSet<F_YARN_REQ_DETAILS>();
            F_PR_SLASHER_CHEM_CONSM = new HashSet<F_PR_SLASHER_CHEM_CONSM>();
            FYsIndentDetailses = new HashSet<F_YS_INDENT_DETAILS>();
            FYsYarnIssueDetailses = new HashSet<F_YS_YARN_ISSUE_DETAILS>();
            FChemStoreIndentdetailses = new HashSet<F_CHEM_STORE_INDENTDETAILS>();
            FChemStoreReceiveDetailses = new HashSet<F_CHEM_STORE_RECEIVE_DETAILS>();
            COM_IMP_INVDETAILS = new HashSet<COM_IMP_INVDETAILS>();
            COM_IMP_LCDETAILS = new List<COM_IMP_LCDETAILS>();
            COM_EX_PI_DETAILS = new HashSet<COM_EX_PI_DETAILS>();
            H_GS_PRODUCT = new HashSet<H_GS_PRODUCT>();
            F_YARN_REQ_DETAILS_S = new HashSet<F_YARN_REQ_DETAILS_S>();
            F_YS_YARN_ISSUE_DETAILS_S = new HashSet<F_YS_YARN_ISSUE_DETAILS_S>();
            F_SAMPLE_FABRIC_DISPATCH_DETAILS = new HashSet<F_SAMPLE_FABRIC_DISPATCH_DETAILS>();
            COS_POSTCOSTING_CHEMDETAILS = new HashSet<COS_POSTCOSTING_CHEMDETAILS>();
            BAS_PRODUCTINFO = new HashSet<BAS_PRODUCTINFO>();
        }

        public int UID { get; set; }
        [Display(Name = "Unit")]
        public string UNAME { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }

        public F_WASTE_PRODUCTINFO F_WASTE_PRODUCTINFO { get; set; }

        public ICollection<F_CHEM_STORE_PRODUCTINFO> F_CHEM_STORE_PRODUCTINFO { get; set; }
        public ICollection<F_GS_PRODUCT_INFORMATION> F_GS_PRODUCT_INFORMATION { get; set; }
        public ICollection<F_SAMPLE_DESPATCH_DETAILS> F_SAMPLE_DESPATCH_DETAILS { get; set; }
        public ICollection<H_SAMPLE_RECEIVING_D> H_SAMPLE_RECEIVING_D { get; set; }
        public ICollection<H_SAMPLE_DESPATCH_M> H_SAMPLE_DESPATCH_M { get; set; }
        public ICollection<F_YARN_REQ_DETAILS> FYarnReqDetailses { get; set; }
        public ICollection<F_PR_SLASHER_CHEM_CONSM> F_PR_SLASHER_CHEM_CONSM { get; set; }
        public ICollection<F_YS_INDENT_DETAILS> FYsIndentDetailses { get; set; }
        public ICollection<F_YS_YARN_ISSUE_DETAILS> FYsYarnIssueDetailses { get; set; }
        public ICollection<F_CHEM_STORE_INDENTDETAILS> FChemStoreIndentdetailses { get; set; }
        public ICollection<F_CHEM_STORE_RECEIVE_DETAILS> FChemStoreReceiveDetailses { get; set; }
        public ICollection<COM_IMP_INVDETAILS> COM_IMP_INVDETAILS { get; set; }
        public ICollection<COM_IMP_LCDETAILS> COM_IMP_LCDETAILS { get; set; }
        public ICollection<COM_EX_PI_DETAILS> COM_EX_PI_DETAILS { get; set; }
        public ICollection<H_GS_PRODUCT> H_GS_PRODUCT { get; set; }
        public ICollection<F_YARN_REQ_DETAILS_S> F_YARN_REQ_DETAILS_S { get; set; }
        public ICollection<F_YS_YARN_ISSUE_DETAILS_S> F_YS_YARN_ISSUE_DETAILS_S { get; set; }
        public ICollection<F_SAMPLE_FABRIC_DISPATCH_DETAILS> F_SAMPLE_FABRIC_DISPATCH_DETAILS { get; set; }
        public ICollection<BAS_PRODUCTINFO> BAS_PRODUCTINFO { get; set; }
        public ICollection<COS_POSTCOSTING_CHEMDETAILS> COS_POSTCOSTING_CHEMDETAILS { get; set; }
    }
}
