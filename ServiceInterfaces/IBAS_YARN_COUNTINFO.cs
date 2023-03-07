using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;
using DenimERP.ViewModels.Basic.YarnCountInfo;

namespace DenimERP.ServiceInterfaces
{
    public interface IBAS_YARN_COUNTINFO : IBaseService<BAS_YARN_COUNTINFO>
    {
        Task<bool> FindByCountnameByAsync(string countName);
        Task<string> FindCountNameByIdAsync(int? id);
        Task<List<COS_PRECOSTING_DETAILS>> GetCountDetailsByIdAsync(List<COS_PRECOSTING_DETAILS> cosPreCostingDetailsList);
        Task<IEnumerable<BAS_YARN_COUNTINFO>> GetForSelectItemsByAsync();
        Task<DataTableObject<BAS_YARN_COUNTINFO>> GetForDataTableByAsync(string sortColumn, string sortColumnDirection, string searchValue, string draw, int skip, int pageSize);
        Task<CreateBasYarnCountInfoViewModel> GetInitObjects(CreateBasYarnCountInfoViewModel createBasYarnCountInfoViewModel);
        Task<YarnCountUpdateViewModel> GetInitYarnObjects(YarnCountUpdateViewModel yarnCountUpdateViewModel);
        Task<CreateBasYarnCountInfoViewModel> FindByCountIdAsync(int countId);
        Task<int> InsertByAndGetIdAsync(BAS_YARN_COUNTINFO basYarnCountInfo);
        Task<IEnumerable<BAS_YARN_COUNT_LOT_INFO>> GetLotList(int countId);
        Task<IEnumerable<BAS_YARN_COUNTINFO>> GetCountListByAsync();
        Task<List<COS_PRECOSTING_DETAILS>> GetDetailsByIdAsync(List<COS_PRECOSTING_DETAILS> cosPreCostingDetailsList);
    }
}
