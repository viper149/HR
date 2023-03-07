using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels
{
    public class FBasVehicleInfoViewModel
    {
        public FBasVehicleInfoViewModel()
        {
            DriverList = new List<F_BAS_DRIVERINFO>();
            VehicleTypeList = new List<VEHICLE_TYPE>();
        }

        public F_BAS_VEHICLE_INFO VehicleInfo { get; set; }
        public List<F_BAS_DRIVERINFO> DriverList { get; set; }
        public List<VEHICLE_TYPE> VehicleTypeList { get; set; }
    }
}