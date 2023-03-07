using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels.AccountFinance
{
    public class DetailsAccExportDoMasterViewModel
    {
        public ExtendAccExportDoMasterViewModel ExtendAccExportDoMasterViewModel { get; set; }
        public List<ACC_EXPORT_DODETAILS> AccExportDodetailses { get; set; }
    }
}
