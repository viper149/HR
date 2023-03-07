using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_PR_FINISHING_MACHINE_PREPARATION:IBaseService<F_PR_FINISHING_MACHINE_PREPARATION>
    {
        Task<IEnumerable<F_PR_FINISHING_MACHINE_PREPARATION>> GetAllAsync();

        Task<FPrFinishingMachineCreatePreparationViewModel> GetInitObjects(
            FPrFinishingMachineCreatePreparationViewModel fPrFinishingMachineCreatePreparationViewModel);

        Task<FPrFinishingMachineCreatePreparationViewModel> GetEditData(int machineId);

        Task<IEnumerable<dynamic>> GetStyleDetailsAsync(int fabcode);
        Task<dynamic> GetDoffDetails(FPrFinishingMachineCreatePreparationViewModel fPrFinishingMachineCreatePreparationViewModel);
    }
}
