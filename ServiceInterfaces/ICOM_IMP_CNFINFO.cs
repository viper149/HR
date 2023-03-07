using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels.Com.CnfInfo;

namespace DenimERP.ServiceInterfaces
{
    public interface ICOM_IMP_CNFINFO : IBaseService<COM_IMP_CNFINFO>
    {
        Task<IEnumerable<COM_IMP_CNFINFO>> GetAllForDataTables();
        Task<ComImpCnfInfoViewModel> FindByIdIncludeAllAsync(int cnfId);
    }
}
