using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces
{
     public interface IPROC_BILL_MASTER : IBaseService<PROC_BILL_MASTER>
    {
        Task<IEnumerable<PROC_BILL_MASTER>> GetAllProcBillMaster();
        Task<ProcBillMasterViewModel> GetInitObjByAsync(ProcBillMasterViewModel procBillMasterViewModel);
    }
}
