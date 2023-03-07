using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels
{
    public class CosPreCostingMasterViewModel
    {

        public CosPreCostingMasterViewModel()
        {
            //FabricInfos = new List<RND_FABRICINFO>();
            //Colors = new List<BAS_COLOR>();
            //Weaves = new List<RND_WEAVE>();
            //FinishTypes = new List<RND_FINISHTYPE>();
            //CosPreCostingMaster = new COS_PRECOSTING_MASTER();
            //CosPreCostingDetails = new COS_PRECOSTING_DETAILS();
            //FixedCost = new COS_FIXEDCOST();
            //StandardCons = new COS_STANDARD_CONS();
            //YarnCountInfos = new List<BAS_YARN_COUNTINFO>();
            CosPreCostingDetailsList = new List<COS_PRECOSTING_DETAILS>();
        }

        public COS_PRECOSTING_MASTER CosPreCostingMaster{ get; set; }
        public COS_PRECOSTING_DETAILS CosPreCostingDetails{ get; set; }
        public List<COS_PRECOSTING_DETAILS> CosPreCostingDetailsList{ get; set; }
        public COS_FIXEDCOST FixedCost{ get; set; }
        public COS_STANDARD_CONS StandardCons{ get; set; }
        public List<BAS_COLOR> Colors { get; set; }
        public List<RND_FABRICINFO> FabricInfos { get; set; }
        public List<RND_WEAVE> Weaves { get; set; }
        public List<LOOM_TYPE> LoomTypes { get; set; }
        public List<RND_FINISHTYPE> FinishTypes { get; set; }
        public List<BAS_YARN_COUNTINFO> YarnCountInfos { get; set; }
        public List<COM_TENOR> ComTenors { get; set; }
        public List<COS_CERTIFICATION_COST> CosCertificationCosts { get; set; }
        public List<BAS_SUPPLIERINFO> SupplierInfos { get; set; }
        public List<YARNFOR> YarnFors { get; set; }

    }
}
