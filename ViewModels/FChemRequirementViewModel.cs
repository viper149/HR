using System;
using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels
{
    public class FChemRequirementViewModel
    {
        public FChemRequirementViewModel()
        {
            FChemStoreProductinfosList = new List<F_CHEM_STORE_PRODUCTINFO>();
            FBasDepartmentsList = new List<F_BAS_DEPARTMENT>();
            FChemReqDetailsList = new List<F_CHEM_REQ_DETAILS>();
            FBasSectionsList = new List<F_BAS_SECTION>();
            FRndDyeingTypesList = new List<RND_DYEING_TYPE>();
            BasUnits = new List<F_BAS_UNITS>();
            ReqEmployees = new List<F_HRD_EMPLOYEE>();
            FBasSubsections = new List<F_BAS_SUBSECTION>();

            FChemReqMaster = new F_CHEM_REQ_MASTER
            {
                CSRDATE = DateTime.Now
            };
        }

        public F_CHEM_REQ_DETAILS FChemReqDetails { get; set; } 
        public F_CHEM_REQ_MASTER FChemReqMaster { get; set; } 
        public List<F_CHEM_STORE_PRODUCTINFO> FChemStoreProductinfosList { get; set; } 
        public List<F_BAS_DEPARTMENT> FBasDepartmentsList { get; set; } 
        public List<F_BAS_SECTION> FBasSectionsList { get; set; } 
        public List<F_BAS_SUBSECTION> FBasSubsections { get; set; } 
        public List<RND_DYEING_TYPE> FRndDyeingTypesList { get; set; } 
        public List<F_BAS_UNITS> BasUnits { get; set; } 
        public List<F_CHEM_REQ_DETAILS> FChemReqDetailsList { get; set; }
        public List<F_HRD_EMPLOYEE> ReqEmployees { get; set; }

        public bool IsDelete { get; set; }
        public int RemoveIndex { get; set; }
    }
}
