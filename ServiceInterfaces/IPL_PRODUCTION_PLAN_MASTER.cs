using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels.Planning;

namespace DenimERP.ServiceInterfaces
{
    public interface IPL_PRODUCTION_PLAN_MASTER: IBaseService<PL_PRODUCTION_PLAN_MASTER>
    {
        Task<PlProductionGroupViewModel> GetInitObjects(PlProductionGroupViewModel plProductionGroupViewModel);
        Task<int> InsertAndGetIdAsync(PL_PRODUCTION_PLAN_MASTER plProductionPlanMaster);
        Task<int> GetGroupNo();
        Task<PL_BULK_PROG_SETUP_D> GetProgramLength(int progId);
        Task<RND_PRODUCTION_ORDER> GetPoDetails(int soNo);
        Task<IEnumerable<PL_PRODUCTION_PLAN_MASTER>> GetAllAsync(string type);

        Task<bool> DateSerialCheck(PL_PRODUCTION_PLAN_MASTER plProductionPlanMaster);

        Task<List<PL_PRODUCTION_PLAN_MASTER>> getPlanListSerial(int? serialNo, int groupId);
        Task<PlProductionGroupViewModel> GetPlanDetails(PL_BULK_PROG_SETUP_D plBulkProgSetupD);
        Task<PlProductionGroupViewModel> FindAllByIdAsync(int id);

        Task<List<PL_PRODUCTION_SETDISTRIBUTION>> GetAllPendingAsync();
    }
}
