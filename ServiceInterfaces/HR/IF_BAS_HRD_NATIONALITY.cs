using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels.HR;

namespace DenimERP.ServiceInterfaces.HR
{
    public interface IF_BAS_HRD_NATIONALITY : IBaseService<F_BAS_HRD_NATIONALITY>
    {
        Task<FBasHrdNationalityViewModel> GetInitObjByAsync(FBasHrdNationalityViewModel fBasHrdNationalityViewModel);
        Task<IEnumerable<F_BAS_HRD_NATIONALITY>> GetAllFBasHrdNationalityAsync();
        Task<bool> FindByNationalityAsync(string nationality, bool isBn= false);
        Task<bool> FindByCountryAsync(string country, bool isBn = false);
    }
}
