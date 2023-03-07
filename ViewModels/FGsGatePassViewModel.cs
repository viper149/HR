using System;
using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels
{
    public class FGsGatePassViewModel
    {
        public FGsGatePassViewModel()
        {
            FGsGatepassInformationMList = new List<F_GS_GATEPASS_INFORMATION_M>();
            FGsGatepassInformationDList = new List<F_GS_GATEPASS_INFORMATION_D>();
            FBasDepartmentList = new List<F_BAS_DEPARTMENT>();
            FBasSectionList = new List<F_BAS_SECTION>();
            FHrEmployeeList = new List<F_HRD_EMPLOYEE>();
            FGatepassTypeList = new List<F_GATEPASS_TYPE>();
            FBasVehicleInfoList = new List<F_BAS_VEHICLE_INFO>();
            FGsProductInformationList = new List<F_GS_PRODUCT_INFORMATION>();

            FGsGatepassInformationM = new F_GS_GATEPASS_INFORMATION_M
            {
                GPDATE = DateTime.Now
            };
        }

        public F_GS_GATEPASS_INFORMATION_M FGsGatepassInformationM { get; set; }
        public F_GS_GATEPASS_INFORMATION_D FGsGatepassInformationD { get; set; }

        public List<F_GS_GATEPASS_INFORMATION_M> FGsGatepassInformationMList { get; set; }
        public List<F_GS_GATEPASS_INFORMATION_D> FGsGatepassInformationDList { get; set; }
        public List<F_BAS_DEPARTMENT> FBasDepartmentList { get; set; }
        public List<F_BAS_SECTION> FBasSectionList { get; set; }
        public List<F_HRD_EMPLOYEE> FHrEmployeeList { get; set; }
        public List<F_GATEPASS_TYPE> FGatepassTypeList { get; set; }
        public List<F_BAS_VEHICLE_INFO> FBasVehicleInfoList { get; set; }
        public List<F_GS_PRODUCT_INFORMATION> FGsProductInformationList { get; set; }
        public bool IsDelete { get; set; }
        public int RemoveIndex { get; set; }
    }
}
