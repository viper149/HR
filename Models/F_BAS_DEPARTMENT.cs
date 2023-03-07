using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Models
{
    public partial class F_BAS_DEPARTMENT
    {
        public F_BAS_DEPARTMENT()
        {
            F_BAS_SECTION = new HashSet<F_BAS_SECTION>();
            F_HR_EMP_OFFICIALINFO = new HashSet<F_HR_EMP_OFFICIALINFO>();
            F_CHEM_REQ_MASTER = new HashSet<F_CHEM_REQ_MASTER>();
            FChemPurchaseRequisitionMasters = new HashSet<F_CHEM_PURCHASE_REQUISITION_MASTER>();
        }

        public int DEPTID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [Display(Name = "Department")]
        [Remote(controller: "FBasDepartment", action: "IsDepartmentNameInUse")]
        public string DEPTNAME { get; set; }
        [Display(Name = "Option 1")]
        public string OPN1 { get; set; }
        [Display(Name = "Option 2")]
        public string OPN2 { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }

        public ICollection<F_BAS_SECTION> F_BAS_SECTION { get; set; }
        public ICollection<F_HR_EMP_OFFICIALINFO> F_HR_EMP_OFFICIALINFO { get; set; }
        public ICollection<F_CHEM_REQ_MASTER> F_CHEM_REQ_MASTER { get; set; }
        public ICollection<F_CHEM_PURCHASE_REQUISITION_MASTER> FChemPurchaseRequisitionMasters { get; set; }
    }
}
