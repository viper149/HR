using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;
using DenimERP.ViewModels.Factory.Production;
using DenimERP.ViewModels.Home;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_DYEING_PROCESS_ROPE_MASTER: IBaseService<F_DYEING_PROCESS_ROPE_MASTER>
    {
        Task<IEnumerable<F_DYEING_PROCESS_ROPE_MASTER>> GetAllAsync();
        Task<FDyeingProcessRopeViewModel> GetInitObjectsByAsync(FDyeingProcessRopeViewModel dyeingProcessRopeViewModel);
        Task<PL_PRODUCTION_PLAN_MASTER> GetGroupDetails(int groupId);
        Task<PL_PRODUCTION_PLAN_DETAILS> GetProgramNoDetails(int setId);
        Task<float> GetBallNoDetails(int ballId);
        Task<int> InsertAndGetIdAsync(F_DYEING_PROCESS_ROPE_MASTER fDyeingProcessRopeMaster);
        Task<FDyeingProcessRopeViewModel> FindAllByIdAsync(int sId);
        Task<FDyeingProcessRopeViewModel> GetGroupNumbersByAsync(FDyeingProcessRopeViewModel fDyeingProcessRopeViewModel, string search, int page);
        Task<DashboardViewModel> GetDyeingDateWiseLength();
        Task<List<ChartViewModel>> GetDyeingDateWiseLengthGraph();
    }
}
