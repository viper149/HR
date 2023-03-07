using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels.SampleGarments.Fabric
{
    public class CreateFSampleFabricIssueViewModel
    {
        public CreateFSampleFabricIssueViewModel()
        {
            BasBuyerinfos = new List<BAS_BUYERINFO>();
            MktTeams = new List<MKT_TEAM>();
            BasBrandinfos = new List<BAS_BRANDINFO>();
            RndFabricinfos = new List<RND_FABRICINFO>();
            FSampleFabricIssueDetailses = new List<F_SAMPLE_FABRIC_ISSUE_DETAILS>();
        }

        public F_SAMPLE_FABRIC_ISSUE FSampleFabric { get; set; }
        public F_SAMPLE_FABRIC_ISSUE_DETAILS FSampleFabricIssueDetails { get; set; }
        public List<F_SAMPLE_FABRIC_ISSUE_DETAILS> FSampleFabricIssueDetailses { get; set; }
        public List<BAS_BUYERINFO> BasBuyerinfos { get; set; }
        public List<BAS_BRANDINFO> BasBrandinfos { get; set; }
        public List<MKT_TEAM> MktTeams { get; set; }
        public List<RND_FABRICINFO> RndFabricinfos { get; set; }
        
        public int RemoveIndex { get; set; }
        public bool IsDelete { get; set; }
    }
}
