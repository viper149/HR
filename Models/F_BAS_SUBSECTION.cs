using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Models
{
    public partial class F_BAS_SUBSECTION
    {
        public F_BAS_SUBSECTION()
        {
            F_HR_EMP_OFFICIALINFO = new HashSet<F_HR_EMP_OFFICIALINFO>();
            FChemPurchaseRequisitionMasters = new HashSet<F_CHEM_PURCHASE_REQUISITION_MASTER>();
            FChemReqMasters = new HashSet<F_CHEM_REQ_MASTER>();
            F_YARN_REQ_MASTER = new HashSet<F_YARN_REQ_MASTER>();
            F_YS_YARN_RECEIVE_MASTER = new HashSet<F_YS_YARN_RECEIVE_MASTER>();
            F_YS_YARN_RECEIVE_MASTER2 = new HashSet<F_YS_YARN_RECEIVE_MASTER2>();
        }

        public int SSECID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        public int? SECID { get; set; }
        [Display(Name = "Sub-Section Name"), Required(ErrorMessage = "Sub-Section name can not be empty."), Remote(controller: "FBasSubSection", action: "IsSubSectionNameInUse")]
        public string SSECNAME { get; set; }
        public string OPN1 { get; set; }
        public string OPN2 { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }

        public F_BAS_SECTION SEC { get; set; }
        public ICollection<F_HR_EMP_OFFICIALINFO> F_HR_EMP_OFFICIALINFO { get; set; }
        public ICollection<F_CHEM_PURCHASE_REQUISITION_MASTER> FChemPurchaseRequisitionMasters { get; set; }
        public ICollection<F_CHEM_REQ_MASTER> FChemReqMasters { get; set; }
        public ICollection<F_YARN_REQ_MASTER> F_YARN_REQ_MASTER { get; set; }
        public ICollection<F_YS_YARN_RECEIVE_MASTER> F_YS_YARN_RECEIVE_MASTER { get; set; }
        public ICollection<F_YS_YARN_RECEIVE_MASTER2> F_YS_YARN_RECEIVE_MASTER2 { get; set; }

    }
}
