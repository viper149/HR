using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Models
{
    public partial class F_BAS_SECTION
    {
        public F_BAS_SECTION()
        {
            F_BAS_SUBSECTION = new HashSet<F_BAS_SUBSECTION>();
            F_HR_EMP_OFFICIALINFO = new HashSet<F_HR_EMP_OFFICIALINFO>();
            F_SAMPLE_GARMENT_RCV_M = new HashSet<F_SAMPLE_GARMENT_RCV_M>();
            F_PR_FINISHING_FNPROCESS = new HashSet<F_PR_FINISHING_FNPROCESS>();
            F_PR_INSPECTION_PROCESS_DETAILS = new HashSet<F_PR_INSPECTION_PROCESS_DETAILS>();
            F_FS_FABRIC_RCV_MASTER = new HashSet<F_FS_FABRIC_RCV_MASTER>();
            F_GS_GATEPASS_INFORMATION_M = new HashSet<F_GS_GATEPASS_INFORMATION_M>();
            FChemPurchaseRequisitionMasters = new HashSet<F_CHEM_PURCHASE_REQUISITION_MASTER>();
            FChemReqMasters = new HashSet<F_CHEM_REQ_MASTER>();
            F_PR_INSPECTION_REJECTION_B = new HashSet<F_PR_INSPECTION_REJECTION_B>();
            F_YARN_REQ_MASTER = new HashSet<F_YARN_REQ_MASTER>();
            F_SAMPLE_FABRIC_RCV_M = new HashSet<F_SAMPLE_FABRIC_RCV_M>();
            RND_BOM_MATERIALS_DETAILS = new HashSet<RND_BOM_MATERIALS_DETAILS>();
            F_GS_WASTAGE_RECEIVE_M = new HashSet<F_GS_WASTAGE_RECEIVE_M>();
            F_FS_WASTAGE_RECEIVE_M = new HashSet<F_FS_WASTAGE_RECEIVE_M>();
            F_PR_INSPECTION_FABRIC_D_MASTER = new HashSet<F_PR_INSPECTION_FABRIC_D_MASTER>();
            F_BAS_ASSET_LIST = new HashSet<F_BAS_ASSET_LIST>();
            F_YS_YARN_RECEIVE_MASTER= new HashSet<F_YS_YARN_RECEIVE_MASTER>();
            F_YS_YARN_RECEIVE_MASTER2 = new HashSet<F_YS_YARN_RECEIVE_MASTER2>();


        }

        public int SECID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [Display(Name = "Department")]
        public int? DEPTID { get; set; }
        [Display(Name = "Section")]
        [Remote(controller: "FBasSection", action: "IsSectionNameInUse")]
        public string SECNAME { get; set; }
        [Display(Name = "Option 1")]
        public string OPN1 { get; set; }
        [Display(Name = "Option 2")]
        public string OPN2 { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }

        public F_BAS_DEPARTMENT DEPT { get; set; }

        public ICollection<F_BAS_SUBSECTION> F_BAS_SUBSECTION { get; set; }
        public ICollection<F_HR_EMP_OFFICIALINFO> F_HR_EMP_OFFICIALINFO { get; set; }
        public ICollection<F_YS_INDENT_DETAILS> F_YS_INDENT_DETAILS { get; set; }
        public ICollection<F_SAMPLE_GARMENT_RCV_M> F_SAMPLE_GARMENT_RCV_M { get; set; }
        public ICollection<F_PR_FINISHING_FNPROCESS> F_PR_FINISHING_FNPROCESS { get; set; }
        public ICollection<F_PR_INSPECTION_PROCESS_DETAILS> F_PR_INSPECTION_PROCESS_DETAILS { get; set; }
        public ICollection<F_FS_FABRIC_RCV_MASTER> F_FS_FABRIC_RCV_MASTER { get; set; }
        public ICollection<F_GS_GATEPASS_INFORMATION_M> F_GS_GATEPASS_INFORMATION_M { get; set; }
        public ICollection<F_CHEM_PURCHASE_REQUISITION_MASTER> FChemPurchaseRequisitionMasters { get; set; }
        public ICollection<F_CHEM_REQ_MASTER> FChemReqMasters { get; set; }
        public ICollection<F_PR_INSPECTION_REJECTION_B> F_PR_INSPECTION_REJECTION_B { get; set; }
        public ICollection<F_YARN_REQ_MASTER> F_YARN_REQ_MASTER { get; set; }
        public ICollection<F_SAMPLE_FABRIC_RCV_M> F_SAMPLE_FABRIC_RCV_M { get; set; }
        public ICollection<RND_BOM_MATERIALS_DETAILS> RND_BOM_MATERIALS_DETAILS { get; set; }
        public ICollection<F_GS_WASTAGE_RECEIVE_M> F_GS_WASTAGE_RECEIVE_M { get; set; }
        public ICollection<F_FS_WASTAGE_RECEIVE_M> F_FS_WASTAGE_RECEIVE_M { get; set; }
        public ICollection<F_PR_INSPECTION_FABRIC_D_MASTER> F_PR_INSPECTION_FABRIC_D_MASTER { get; set; }
        public ICollection<F_BAS_ASSET_LIST> F_BAS_ASSET_LIST { get; set; }

        public ICollection<F_YS_YARN_RECEIVE_MASTER> F_YS_YARN_RECEIVE_MASTER { get; set; }
        public ICollection<F_YS_YARN_RECEIVE_MASTER2> F_YS_YARN_RECEIVE_MASTER2 { get; set; }

    }
}