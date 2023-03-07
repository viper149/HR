using System.Collections.Generic;
using DenimERP.Models;
using DenimERP.ViewModels.SampleGarments.Receive;

namespace DenimERP.ViewModels.SampleGarments.GatePass
{
    public class CreateFSampleDesPatchMasterViewModel
    {
        public CreateFSampleDesPatchMasterViewModel()
        {
            FSampleDespatchMaster = new F_SAMPLE_DESPATCH_MASTER();
            FSampleDespatchDetails = new F_SAMPLE_DESPATCH_DETAILS();
            FSampleDespatchDetailses = new List<F_SAMPLE_DESPATCH_DETAILS>();
            FBasDriverinfos = new List<F_BAS_DRIVERINFO>();
            FBasVehicleInfos = new List<F_BAS_VEHICLE_INFO>();
            BasBuyerinfos = new List<BAS_BUYERINFO>();
            FSampleGarmentRcvDs = new List<F_SAMPLE_GARMENT_RCV_D>();
            GatepassTypes = new List<GATEPASS_TYPE>();
            FBasUnitses = new List<F_BAS_UNITS>();
            FSampleDespatchMasterTypes = new List<F_SAMPLE_DESPATCH_MASTER_TYPE>();
        }

        public F_SAMPLE_DESPATCH_MASTER FSampleDespatchMaster { get; set; }
        public F_SAMPLE_DESPATCH_DETAILS FSampleDespatchDetails { get; set; }

        public List<F_SAMPLE_DESPATCH_DETAILS> FSampleDespatchDetailses { get; set; }
        public List<F_BAS_DRIVERINFO> FBasDriverinfos { get; set; }
        public List<F_BAS_VEHICLE_INFO> FBasVehicleInfos { get; set; }
        public List<BAS_BUYERINFO> BasBuyerinfos { get; set; }
        public List<F_SAMPLE_GARMENT_RCV_D> FSampleGarmentRcvDs { get; set; }
        public List<ExtendFSampleGarmentRcvD> ExtendFSampleGarmentRcvDs { get; set; }
        public List<GATEPASS_TYPE> GatepassTypes { get; set; }
        public List<F_BAS_UNITS> FBasUnitses { get; set; }
        public List<F_SAMPLE_DESPATCH_MASTER_TYPE> FSampleDespatchMasterTypes { get; set; }
 
        public int RemoveIndex { get; set; }
        public bool IsDeletable { get; set; }
        public bool IsLocked { get; set; }
    }
}
