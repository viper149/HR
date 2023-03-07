using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_YARN_REQ_DETAILS : IBaseService<F_YARN_REQ_DETAILS>
    {
        Task<IEnumerable<POSOViewModel>> GetSoList();
        Task<YarnRequirementViewModel> GetCountIdList(int poId,int sec);
        Task<IEnumerable<RND_FABRIC_COUNTINFO>> GetCountIdList2();
        YarnRequirementViewModel GetCountConsumpList(int setId,int sec);
        dynamic GetSetList(int poId);
        Task<IEnumerable<F_YARN_REQ_DETAILS>> GetYarnReqCountList(int orderno, int ysrid=0);
        Task<F_YARN_REQ_DETAILS> GetSingleYarnReqDetails(int countId, double qty);
        Task<IEnumerable<F_YARN_TRANSACTION>> GetYarnLotDetails(int countId);
        Task<IEnumerable<F_YARN_TRANSACTION>> GetYarnLotDetailsByCount(int countId,int indentType);
        Task<IEnumerable<F_YARN_TRANSACTION>> GetYarnLotDetailsByIndentType(int countId,int indentType);
        Task<YarnRequirementViewModel> GetInitObjectsOfSelectedItems(YarnRequirementViewModel yarnRequirement);
        Task<RND_SAMPLE_INFO_DYEING> GetStyleByRSNO(int rsId);
        Task<IEnumerable<BAS_YARN_COUNTINFO>> GetCountIdList2T();
        Task<int> GetCountIdByReqDId(int reqId);
    }
}
