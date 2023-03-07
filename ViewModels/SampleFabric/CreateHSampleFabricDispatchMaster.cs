using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels.SampleFabric
{
    public class CreateHSampleFabricDispatchMaster
    {
        public CreateHSampleFabricDispatchMaster()
        {
            HSampleFabricDispatchDetailses = new List<H_SAMPLE_FABRIC_DISPATCH_DETAILS>();
            BasBuyerinfos = new List<BAS_BUYERINFO>();
            BasBrandinfos = new List<BAS_BRANDINFO>();
            Merchandisers = new List<MERCHANDISER>();
            HSampleFabricReceivingDs = new List<H_SAMPLE_FABRIC_RECEIVING_D>();
        }

        public H_SAMPLE_FABRIC_DISPATCH_MASTER HSampleFabricDispatchMaster { get; set; }
        public H_SAMPLE_FABRIC_DISPATCH_DETAILS HSampleFabricDispatchDetails { get; set; }

        public List<H_SAMPLE_FABRIC_DISPATCH_DETAILS> HSampleFabricDispatchDetailses { get; set; }
        public List<BAS_BUYERINFO> BasBuyerinfos { get; set; }
        public List<BAS_BRANDINFO> BasBrandinfos { get; set; }
        public List<MERCHANDISER> Merchandisers { get; set; }
        public List<H_SAMPLE_FABRIC_RECEIVING_D> HSampleFabricReceivingDs { get; set; }

        public bool IsDelete { get; set; }
        public int RemoveIndex { get; set; }
    }
}
