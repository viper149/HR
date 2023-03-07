using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_YS_YARN_RECEIVE_DETAILS : IBaseService<F_YS_YARN_RECEIVE_DETAILS>
    {
        Task<YarnReceiveViewModel> GetDetailsData(YarnReceiveViewModel yarnReceiveViewModel);
        Task<IEnumerable<F_YS_YARN_RECEIVE_DETAILS>> GetYarnIndentDetails(int countId,int indentType,DateTime? issueDate);
        Task<F_YS_YARN_RECEIVE_DETAILS> GetAllByRcvdId(int id);
        Task<IEnumerable<F_YS_YARN_RECEIVE_DETAILS>> GetCountAsync(DateTime? issueDate);
    }
}
