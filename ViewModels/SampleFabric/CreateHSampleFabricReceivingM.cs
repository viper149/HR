using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels.SampleFabric
{
    public class CreateHSampleFabricReceivingM
    {
        public CreateHSampleFabricReceivingM()
        {
            HSampleFabricReceivingDs = new List<H_SAMPLE_FABRIC_RECEIVING_D>();
            FSampleFabricDispatchMasters = new List<F_SAMPLE_FABRIC_DISPATCH_MASTER>();
        }

        public H_SAMPLE_FABRIC_RECEIVING_M HSampleFabricReceivingM { get; set; }
        public H_SAMPLE_FABRIC_RECEIVING_D HSampleFabricReceivingD { get; set; }

        public List<F_SAMPLE_FABRIC_DISPATCH_MASTER> FSampleFabricDispatchMasters { get; set; }
        public List<H_SAMPLE_FABRIC_RECEIVING_D> HSampleFabricReceivingDs { get; set; }

        public bool IsDelete { get; set; }
        public int RemoveIndex { get; set; }
    }
}
