using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces.SampleGarments.GatePass
{
    public interface IF_BAS_VEHICLE_INFO : IBaseService<F_BAS_VEHICLE_INFO>
    {
        Task<IEnumerable<F_BAS_VEHICLE_INFO>> GetAllFBasVehicleInfoAsync();
        Task<FBasVehicleInfoViewModel> GetInitObjByAsync(FBasVehicleInfoViewModel fBasVehicleInfoViewModel);
    }
}
