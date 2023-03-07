using System;
using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels
{
    public class FGenSRequirementViewModel
    {
        public FGenSRequirementViewModel()
        {
            FGsProductInformationsList = new List<F_GS_PRODUCT_INFORMATION>();
            FGenSReqDetailsesList = new List<F_GEN_S_REQ_DETAILS>();
            FRndDyeingTypesList = new List<RND_DYEING_TYPE>();
            BasUnits = new List<F_BAS_UNITS>();
            ReqEmployees = new List<F_HRD_EMPLOYEE>();
            
            FGenSReqMaster = new F_GEN_S_REQ_MASTER
            {
                GSRDATE = DateTime.Now
            };
        }

        public F_GEN_S_REQ_DETAILS FGenSReqDetails { get; set; }
        public F_GEN_S_REQ_MASTER FGenSReqMaster { get; set; }

        public List<F_GS_PRODUCT_INFORMATION> FGsProductInformationsList { get; set; }
        public List<RND_DYEING_TYPE> FRndDyeingTypesList { get; set; }
        public List<F_BAS_UNITS> BasUnits { get; set; }
        public List<F_GEN_S_REQ_DETAILS> FGenSReqDetailsesList { get; set; }
        public List<F_HRD_EMPLOYEE> ReqEmployees { get; set; }

        public bool IsDelete { get; set; }
        public int RemoveIndex { get; set; }
    }
}
