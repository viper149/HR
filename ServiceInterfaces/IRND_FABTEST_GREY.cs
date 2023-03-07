using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels.Rnd.Grey;

namespace DenimERP.ServiceInterfaces
{
    public interface IRND_FABTEST_GREY : IBaseService<RND_FABTEST_GREY>
    {
        Task<CreateRndFabTestGreyViewModel> GetInitObjects(CreateRndFabTestGreyViewModel createRndFabTestGreyViewModel);
        Task<PreviousDataRndFabTestGreyViewModel> FindBySdIdAsync(int sdId);
        Task<IEnumerable<RND_FABTEST_GREY>> GetForDataTableByAsync();
        Task<CreateRndFabTestGreyViewModel> GetRndFabTestGreyWithDetailsByAsnc(int ltgId);
        Task<CreateRndFabTestGreyViewModel> GetInitObjectsForEditView(CreateRndFabTestGreyViewModel createRndFabTestGreyViewModel);
        Task<DetailsRndFabTestGreyViewModel> FindByLtgIdAsync(int ltgId);
        Task<bool> FindByLabNo(string labNo);
    }
}
