using System;
using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels
{
    public class FFsFabricLoadingBillViewModel
    {
        public FFsFabricLoadingBillViewModel()
        {
            VehicleList = new List<F_BAS_VEHICLE_INFO>();

            FFsFabricLoadingBill = new F_FS_FABRIC_LOADING_BILL
            {
                TRANSDATE = DateTime.Now,
            };
        }

        public F_FS_FABRIC_LOADING_BILL FFsFabricLoadingBill { get; set; }
        public List<F_BAS_VEHICLE_INFO> VehicleList { get; set; }
    }

}
