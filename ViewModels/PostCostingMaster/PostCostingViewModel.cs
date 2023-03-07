using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels.PostCostingMaster
{
    public class PostCostingViewModel
    {
        public PostCostingViewModel()
        {
            CosPostCostingChemDetailsList = new List<COS_POSTCOSTING_CHEMDETAILS>();
            CosPostCostingYarnDetailsList = new List<COS_POSTCOSTING_YARNDETAILS>();
            SOList = new List<TypeTableViewModel>();
            UnitList = new List<F_BAS_UNITS>();
            ChemicalList = new List<F_CHEM_STORE_PRODUCTINFO>();
            CountList = new List<BAS_YARN_COUNTINFO>();
            LotList = new List<BAS_YARN_LOTINFO>();
        }

        public COS_POSTCOSTING_MASTER CosPostcostingMaster { get; set; }
        public COS_POSTCOSTING_CHEMDETAILS CosPostCostingChemDetails { get; set; }
        public COS_POSTCOSTING_YARNDETAILS CosPostCostingYarnDetails { get; set; }

        public List<COS_POSTCOSTING_CHEMDETAILS> CosPostCostingChemDetailsList { get; set; }
        public List<COS_POSTCOSTING_YARNDETAILS> CosPostCostingYarnDetailsList { get; set; }
        public List<TypeTableViewModel> SOList { get; set; }
        public List<F_BAS_UNITS> UnitList { get; set; }
        public List<F_CHEM_STORE_PRODUCTINFO> ChemicalList { get; set; }
        public List<BAS_YARN_COUNTINFO> CountList { get; set; }
        public List<BAS_YARN_LOTINFO> LotList { get; set; }
    }
  
}
