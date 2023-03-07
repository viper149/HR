using System;
using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels
{
    public class FYarnReqSViewModel
    {
        public FYarnReqSViewModel()
        {
            FYarnRequirementMasterSList = new List<F_YARN_REQ_MASTER_S>();
            FYarnRequirementDetailsSList = new List<F_YARN_REQ_DETAILS_S>();
            DepartmentList = new List<F_BAS_DEPARTMENT>();
            FBasSectionList = new List<F_BAS_SECTION>();
            Count = new List<RND_FABRIC_COUNTINFO>();
            BasCount = new List<BAS_YARN_COUNTINFO>();
            Lot = new List<BAS_YARN_LOTINFO>();
            BasUnits = new List<F_BAS_UNITS>();
            RndFabricCountinfos = new List<RND_FABRIC_COUNTINFO>();

            FYarnRequirementMasterS = new F_YARN_REQ_MASTER_S
            {
                YSRDATE = DateTime.Now
            };

            FYarnRequirementDetailsS = new F_YARN_REQ_DETAILS_S
            {
                TRNSDATE = DateTime.Now
            };
        }

        public object Dynamic { get; set; }
        public List<PL_BULK_PROG_SETUP_M> SetList { get; set; }
        public F_YARN_REQ_DETAILS_S FYarnRequirementDetailsS { get; set; }
        public F_YARN_REQ_MASTER_S FYarnRequirementMasterS { get; set; }
        public List<RND_FABRIC_COUNTINFO> RndFabricCountinfos { get; set; }
        public List<F_YARN_REQ_MASTER_S> FYarnRequirementMasterSList { get; set; }
        public List<F_YARN_REQ_DETAILS_S> FYarnRequirementDetailsSList { get; set; }
        public List<F_BAS_DEPARTMENT> DepartmentList { get; set; }
        public List<F_BAS_SECTION> FBasSectionList { get; set; }
        public List<POSOViewModel> PosoViewModels { get; set; }
        public List<RND_FABRIC_COUNTINFO> Count { get; set; }
        public List<BAS_YARN_COUNTINFO> BasCount { get; set; }
        public List<TypeTableViewModel> PlProductionSetDistributions { get; set; }
        public List<BAS_YARN_LOTINFO> Lot { get; set; }
        public List<F_BAS_UNITS> BasUnits { get; set; }
        public COM_EX_PI_DETAILS PiDetails { get; set; }
        public int RemoveIndex { get; set; }
        public bool IsDeletable { get; set; }
    }
}
