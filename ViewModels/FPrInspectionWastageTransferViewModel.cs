using System;
using DenimERP.Models;

namespace DenimERP.ViewModels
{
    public class FPrInspectionWastageTransferViewModel
    {
        public FPrInspectionWastageTransferViewModel()
        {
            FPrInspectionWastageTransfer = new F_PR_INSPECTION_WASTAGE_TRANSFER
            {
                TRANSDATE = DateTime.Now
            };
        }
        public F_PR_INSPECTION_WASTAGE_TRANSFER FPrInspectionWastageTransfer { get; set; }
    }
}
