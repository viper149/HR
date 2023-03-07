using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_YARN_REQ_DETAILS_S : IBaseService<F_YARN_REQ_DETAILS_S>
    {
        Task<IEnumerable<POSOViewModel>> GetSoList();
        Task<FYarnReqSViewModel> GetCountIdList(int poId, int sec);
        FYarnReqSViewModel GetCountConsumpList(int setId, int sec);
        dynamic GetSetList(int poId);
        Task<IEnumerable<F_YARN_REQ_DETAILS_S>> GetYarnReqCountList(int orderno, int ysrid = 0);
        Task<F_YARN_REQ_DETAILS_S> GetSingleYarnReqDetails(int countId, double qty);
        Task<IEnumerable<F_YARN_TRANSACTION_S>> GetYarnLotDetails(int countId);
        Task<FYarnReqSViewModel> GetInitObjectsOfSelectedItems(FYarnReqSViewModel fYarnReqSViewModel);
    }
}
