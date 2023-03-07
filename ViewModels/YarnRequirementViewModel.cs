using System;
using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels
{
    public class YarnRequirementViewModel
    {
        public YarnRequirementViewModel()
        {
            FYarnRequirementMasterList = new List<F_YARN_REQ_MASTER>();
            FYarnRequirementDetailsList = new List<F_YARN_REQ_DETAILS>();
            DepartmentList = new List<F_BAS_DEPARTMENT>();
            FBasSectionList = new List<F_BAS_SECTION>();
            Count = new List<RND_FABRIC_COUNTINFO>();
            BasCount = new List<BAS_YARN_COUNTINFO>();
            Lot = new List<BAS_YARN_LOTINFO>();
            BasUnits = new List<F_BAS_UNITS>();
            RndFabricCountinfos = new List<RND_FABRIC_COUNTINFO>();
            RndSampleInfoDyeingList = new List<RND_SAMPLE_INFO_DYEING>();

            FYarnRequirementMaster = new F_YARN_REQ_MASTER
            {
                YSRDATE = DateTime.Now
            };

            FYarnRequirementDetails = new F_YARN_REQ_DETAILS
            {
                TRNSDATE = DateTime.Now
            };
        }


        public object Dynamic { get; set; }
        
        public F_YARN_REQ_DETAILS FYarnRequirementDetails { get; set; }
        public F_YARN_REQ_MASTER FYarnRequirementMaster { get; set; }
        public RND_SAMPLE_INFO_DYEING RNDSampleInfoDyeing { get; set; }
        public COM_EX_PI_DETAILS PiDetails { get; set; }

        public List<RND_FABRIC_COUNTINFO> RndFabricCountinfos { get; set; }
        public List<F_YARN_REQ_MASTER> FYarnRequirementMasterList { get; set; }
        public List<F_YARN_REQ_DETAILS> FYarnRequirementDetailsList { get; set; }
        public List<F_BAS_DEPARTMENT> DepartmentList { get; set; }
        public List<F_BAS_SECTION> FBasSectionList { get; set; }
        public List<F_BAS_SUBSECTION> FBasSubSectionList { get; set; }
        public List<POSOViewModel> PosoViewModels { get; set; }
        public List<RSNoViewModel> RSNoViewModel { get; set; }
        public List<RND_FABRIC_COUNTINFO> Count { get; set; }
        public List<BAS_YARN_COUNTINFO> BasCount { get; set; }
        public List<TypeTableViewModel> PlProductionSetDistributions  { get; set; }
        public List<BAS_YARN_LOTINFO> Lot { get; set; }
        public List<F_BAS_UNITS> BasUnits { get; set; }
        public List<PL_BULK_PROG_SETUP_M> SetList { get; set; }
        public List<RND_SAMPLE_INFO_DYEING> RndSampleInfoDyeingList { get; set; }
       
        public int RemoveIndex { get; set; }
        public bool IsDeletable { get; set; }
    }
}
