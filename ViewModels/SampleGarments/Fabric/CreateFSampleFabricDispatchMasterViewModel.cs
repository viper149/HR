using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels.SampleGarments.Fabric
{
    public class CreateFSampleFabricDispatchMasterViewModel
    {
        public CreateFSampleFabricDispatchMasterViewModel()
        {
            FSampleFabricDispatchDetailses = new List<F_SAMPLE_FABRIC_DISPATCH_DETAILS>();
            FSampleFabricDispatchTransactions = new List<F_SAMPLE_FABRIC_DISPATCH_TRANSACTION>();
            FSampleFabricRcvDs = new List<F_SAMPLE_FABRIC_RCV_D>();
            FSampleDespatchMasterTypes = new List<F_SAMPLE_DESPATCH_MASTER_TYPE>();
            GatepassTypes = new List<GATEPASS_TYPE>();
            FBasDriverinfos = new List<F_BAS_DRIVERINFO>();
            FBasVehicleInfos = new List<F_BAS_VEHICLE_INFO>();
            MktTeams = new List<MKT_TEAM>();
            BasBuyerinfos = new List<BAS_BUYERINFO>();
            FBasUnitses = new List<F_BAS_UNITS>();
            FSampleFabricDispatchSampleTypes = new List<F_SAMPLE_FABRIC_DISPATCH_SAMPLE_TYPE>();
            BasBrandinfos = new List<BAS_BRANDINFO>();
        }

        public F_SAMPLE_FABRIC_DISPATCH_MASTER FSampleFabricDispatchMaster { get; set; }
        public F_SAMPLE_FABRIC_DISPATCH_DETAILS FSampleFabricDispatchDetails { get; set; }
        public List<F_SAMPLE_FABRIC_DISPATCH_DETAILS> FSampleFabricDispatchDetailses { get; set; }
        public List<F_SAMPLE_FABRIC_DISPATCH_TRANSACTION> FSampleFabricDispatchTransactions { get; set; }
        public List<F_SAMPLE_FABRIC_RCV_D> FSampleFabricRcvDs { get; set; }
        public List<F_SAMPLE_DESPATCH_MASTER_TYPE> FSampleDespatchMasterTypes { get; set; }
        public List<GATEPASS_TYPE> GatepassTypes { get; set; }
        public List<F_BAS_DRIVERINFO> FBasDriverinfos { get; set; }
        public List<F_BAS_VEHICLE_INFO> FBasVehicleInfos { get; set; }
        public List<MKT_TEAM> MktTeams { get; set; }
        public List<BAS_BUYERINFO> BasBuyerinfos { get; set; }
        public List<F_BAS_UNITS> FBasUnitses { get; set; }
        public List<F_SAMPLE_FABRIC_DISPATCH_SAMPLE_TYPE> FSampleFabricDispatchSampleTypes { get; set; }
        public List<BAS_BRANDINFO> BasBrandinfos { get; set; }
        
        public bool IsDeletable { get; set; }
        public int RemoveIndex { get; set; }
    }
}
