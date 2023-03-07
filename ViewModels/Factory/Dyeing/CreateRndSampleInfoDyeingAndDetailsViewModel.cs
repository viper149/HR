using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels.Factory.Dyeing
{
    public class CreateRndSampleInfoDyeingAndDetailsViewModel
    {
        public CreateRndSampleInfoDyeingAndDetailsViewModel()
        {
            RndSampleInfoDyeing = new RND_SAMPLE_INFO_DYEING();
            RndSampleInfoDetailses = new List<RND_SAMPLE_INFO_DETAILS>();
            PlSampleProgSetupList = new List<PL_SAMPLE_PROG_SETUP>();
            MktSdrfInfos = new List<MKT_SDRF_INFO>();
            FHrEmployeesList = new List<F_HRD_EMPLOYEE>();
        }

        public RND_SAMPLE_INFO_DYEING RndSampleInfoDyeing { get; set; }
        public RND_SAMPLE_INFO_DETAILS RndSampleInfoDetails { get; set; }
        public List<RND_SAMPLE_INFO_DETAILS> RndSampleInfoDetailses { get; set; }
        public List<BAS_YARN_COUNTINFO> BasYarnCountinfos { get; set; }
        public List<BAS_COLOR> BasColors { get; set; }
        public List<BAS_BUYERINFO> BasBuyerInfos { get; set; }
        public List<BAS_YARN_LOTINFO> BasYarnLotinfos { get; set; }
        public List<BAS_SUPPLIERINFO> BasSupplierinfos { get; set; }
        public List<YARNFOR> Yarnfors { get; set; }
        public List<RND_DYEING_TYPE> DyeingTypes { get; set; }
        public List<BAS_COLOR> Colors { get; set; }
        public List<LOOM_TYPE> LoomTypes { get; set; }
        public PL_SAMPLE_PROG_SETUP PlSampleProgSetup { get; set; }
        public List<PL_SAMPLE_PROG_SETUP> PlSampleProgSetupList { get; set; }
        public List<MKT_SDRF_INFO> MktSdrfInfos { get; set; }
        public List<F_HRD_EMPLOYEE> FHrEmployeesList { get; set; }
        public int RemoveIndex { get; set; }
        public bool IsDelete { get; set; }
    }
}
