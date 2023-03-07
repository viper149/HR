using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces
{
    public interface IACC_EXPORT_DODETAILS : IBaseService<ACC_EXPORT_DODETAILS>
    {
        
           Task<IEnumerable<ACC_EXPORT_DODETAILS>> FindDoDetailsListByDoNoAsync(int doNo);
           Task<IEnumerable<ACC_EXPORT_DODETAILS>> FindDODetailsBYDONoAndStyleIdAndPIIDAsync(int doNo, int? piID, int styleId);
           Task<COM_EX_PI_DETAILS> GetPiDetailsByStyleIdAsync(AccExportDoMasterViewModel accExportDoMasterViewModel);
           Task<AccExportDoMasterViewModel> GetInitObjForDetailsByAsync(AccExportDoMasterViewModel accExportDoMasterViewModel);
    }
}
