using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels.SampleGarments.HReceive
{
    public class CreateHSampleReceivingMViewModel
    {
        public CreateHSampleReceivingMViewModel()
        {
            HSampleReceivingDs = new List<H_SAMPLE_RECEIVING_D>();
            GatepassTypes = new List<GATEPASS_TYPE>();
            FBasDriverinfos = new List<F_BAS_DRIVERINFO>();
            FBasVehicleInfos = new List<F_BAS_VEHICLE_INFO>();
            BasBuyerinfos = new List<BAS_BUYERINFO>();
            FSampleGarmentRcvDs = new List<F_SAMPLE_GARMENT_RCV_D>();
            FSampleDespatchMasters = new List<F_SAMPLE_DESPATCH_MASTER>();
            FSampleDespatchDetailses = new List<F_SAMPLE_DESPATCH_DETAILS>();
        }

        public H_SAMPLE_RECEIVING_M HSampleReceivingM { get; set; }
        public H_SAMPLE_RECEIVING_D HSampleReceivingD { get; set; }

        public F_SAMPLE_DESPATCH_MASTER FSampleDespatchMaster { get; set; }

        public List<H_SAMPLE_RECEIVING_D> HSampleReceivingDs { get; set; }
        public List<GATEPASS_TYPE> GatepassTypes { get; set; }
        public List<F_BAS_DRIVERINFO> FBasDriverinfos { get; set; }
        public List<F_BAS_VEHICLE_INFO> FBasVehicleInfos { get; set; }
        public List<BAS_BUYERINFO> BasBuyerinfos { get; set; }

        public List<F_SAMPLE_DESPATCH_MASTER> FSampleDespatchMasters { get; set; }
        public List<F_SAMPLE_DESPATCH_DETAILS> FSampleDespatchDetailses { get; set; }
        public List<F_SAMPLE_GARMENT_RCV_D> FSampleGarmentRcvDs { get; set; }
    }
}
