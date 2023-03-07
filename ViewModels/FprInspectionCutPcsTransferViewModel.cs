using System;
using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels
{

    public class FprInspectionCutPcsTransferViewModel
    {
        public FprInspectionCutPcsTransferViewModel()
        {
            FPrInspectionCutpcsTransfer = new F_PR_INSPECTION_CUTPCS_TRANSFER
            {
                TRNS_DATE = DateTime.Now,
            };

            ShiftList = new List<F_HR_SHIFT_INFO>();
        }

        public F_PR_INSPECTION_CUTPCS_TRANSFER FPrInspectionCutpcsTransfer { get; set; }

        public List<F_HR_SHIFT_INFO> ShiftList { get; set; }
    }
}

