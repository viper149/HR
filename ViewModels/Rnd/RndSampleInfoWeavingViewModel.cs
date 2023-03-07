using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels.Rnd
{
    public class RndSampleInfoWeavingViewModel
    {
        public RndSampleInfoWeavingViewModel()
        {
            RndSampleInfoWeavingDetails = new RND_SAMPLE_INFO_WEAVING_DETAILS();
            RndSampleInfoWeavingDetailses = new List<RND_SAMPLE_INFO_WEAVING_DETAILS>();
            RndWeaves = new List<RND_WEAVE>();
            LoomTypes = new List<LOOM_TYPE>();
        }

        public RND_SAMPLE_INFO_WEAVING RndSampleInfoWeaving { get; set; }
        public RND_SAMPLE_INFO_WEAVING_DETAILS RndSampleInfoWeavingDetails { get; set; }
        public List<RND_SAMPLE_INFO_WEAVING_DETAILS> RndSampleInfoWeavingDetailses { get; set; }
        public List<BAS_YARN_COUNTINFO> BasYarnCountinfos { get; set; }
        public List<BAS_COLOR> BasColors { get; set; }
        public List<BAS_YARN_LOTINFO> BasYarnLotinfos { get; set; }
        public List<BAS_SUPPLIERINFO> BasSupplierinfos { get; set; }
        public List<RND_SAMPLE_INFO_DYEING> RndSampleInfoDyeings { get; set; }
        public List<PL_SAMPLE_PROG_SETUP> PlSampleProgSetups { get; set; }
        public List<RND_WEAVE> RndWeaves { get; set; }
        public List<LOOM_TYPE> LoomTypes { get; set; }
        public List<BAS_BUYERINFO> BuyerInfos { get; set; }
        public List<F_HRD_EMPLOYEE> RndConcerns { get; set; }
        public List<MKT_TEAM> MktTeams { get; set; }
        public List<PL_PRODUCTION_SETDISTRIBUTION> PlProductionSetDistributions { get; set; }
        public List<YARNFOR> Yarnfors { get; set; }
        public int RemoveIndex { get; set; }
        public bool IsDelete { get; set; }
    }
}
