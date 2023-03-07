using System;
using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels
{
    public class RndBomViewModel
    {
        public RndBomViewModel()
        {
            RndBom = new RND_BOM()
            {
                TRNSDATE = DateTime.Now
            };
            RndBomMaterialsDetailsList = new List<RND_BOM_MATERIALS_DETAILS>();
        }

        public RND_BOM RndBom { get; set; }
        public RND_BOM_MATERIALS_DETAILS RndBomMaterialsDetails { get; set; }
        public List<RND_BOM_MATERIALS_DETAILS> RndBomMaterialsDetailsList { get; set; }


        public List<RND_FABRICINFO> RndFabricInfos { get; set; }
        public List<F_CHEM_STORE_PRODUCTINFO> FChemStoreProductInfo { get; set; }
        public List<F_BAS_SECTION> FBasSections { get; set; }
        

        //public List<PL_PRODUCTION_SETDISTRIBUTION> PlProductionSetdistributionList { get; set; }
        //public List<F_PR_FINISHING_FNPROCESS> FPrFinishingFnprocessList { get; set; }
        //public List<F_HRD_EMPLOYEE> FHrEmployeeList { get; set; }
        //public List<F_HR_SHIFT_INFO> FHrShiftInfoList { get; set; }
        //public List<F_BAS_TESTMETHOD> FBasTestmethodList { get; set; }
    }
}