using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces
{
    public interface ICOM_EX_ADV_DELIVERY_SCH_MASTER : IBaseService<COM_EX_ADV_DELIVERY_SCH_MASTER>
    {
        Task<IEnumerable<COM_EX_ADV_DELIVERY_SCH_MASTER>> GetAllAsync();
        Task<string> GetLastDSNoAsync();
        Task<ComExAdvDeliverySchViewModel> GetInitObjByAsync(ComExAdvDeliverySchViewModel comExAdvDeliverySchViewModel);
        Task<ComExAdvDeliverySchViewModel> GetInitDetailsObjByAsync(ComExAdvDeliverySchViewModel fComExAdvDeliverySchViewModel);
        Task<ComExAdvDeliverySchViewModel> FindByIdIncludeAllAsync(int id);
    }
}
