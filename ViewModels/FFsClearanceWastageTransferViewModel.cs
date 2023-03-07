using System;
using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels
{
    public class FFsClearanceWastageTransferViewModel
    {
        public FFsClearanceWastageTransferViewModel()
        {
            FFsClearanceWastageTransfer = new F_FS_CLEARANCE_WASTAGE_TRANSFER
            {
                TRANSDATE = DateTime.Now
            };

            FFsClearanceWastageItemsList = new List<F_FS_CLEARANCE_WASTAGE_ITEM>();
            FHrEmployeesList = new List<F_HRD_EMPLOYEE>();
        }

        public F_FS_CLEARANCE_WASTAGE_TRANSFER FFsClearanceWastageTransfer { get; set; }

        public List<F_FS_CLEARANCE_WASTAGE_ITEM> FFsClearanceWastageItemsList { get; set; }
        public List<F_HRD_EMPLOYEE> FHrEmployeesList { get; set; }
    }
}
