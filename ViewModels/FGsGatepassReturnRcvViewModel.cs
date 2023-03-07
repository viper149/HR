using System;
using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels
{
    public class FGsGatepassReturnRcvViewModel
    {
        public FGsGatepassReturnRcvViewModel()
        {
            FGsGatepassInformationMList = new List<F_GS_GATEPASS_INFORMATION_M>();
            FHrEmployeesList = new List<F_HRD_EMPLOYEE>();
            FGsProductInformationList = new List<F_GS_PRODUCT_INFORMATION>();
            FGsGatepassReturnRcvDetailsList = new List<F_GS_GATEPASS_RETURN_RCV_DETAILS>();

            FGsGatepassReturnRcvMaster = new F_GS_GATEPASS_RETURN_RCV_MASTER
            {
                RCVDATE = DateTime.Now
            };
        }

        public F_GS_GATEPASS_RETURN_RCV_MASTER FGsGatepassReturnRcvMaster { get; set; }
        public F_GS_GATEPASS_RETURN_RCV_DETAILS FGsGatepassReturnRcvDetails  { get; set; }

        public List<F_GS_GATEPASS_INFORMATION_M> FGsGatepassInformationMList { get; set; }
        public List<F_HRD_EMPLOYEE> FHrEmployeesList { get; set; }
        public List<F_GS_PRODUCT_INFORMATION> FGsProductInformationList { get; set; }
        public List<F_GS_GATEPASS_RETURN_RCV_DETAILS> FGsGatepassReturnRcvDetailsList { get; set; }

        public bool IsDelete { get; set; }
        public int RemoveIndex { get; set; }
    }
}
