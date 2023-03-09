using System.Collections.Generic;
using System.Threading.Tasks;
using HRMS.Models;
using HRMS.ServiceInterfaces.BaseInterfaces;
using HRMS.ViewModels.HR;

namespace HRMS.ServiceInterfaces.HR
{
    public interface IF_BAS_HRD_NATIONALITY : IBaseService<F_BAS_HRD_NATIONALITY>
    {
        Task<FBasHrdNationalityViewModel> GetInitObjByAsync(FBasHrdNationalityViewModel fBasHrdNationalityViewModel);
        Task<IEnumerable<F_BAS_HRD_NATIONALITY>> GetAllFBasHrdNationalityAsync();
        Task<bool> FindByNationalityAsync(string nationality, bool isBn= false);
        Task<bool> FindByCountryAsync(string country, bool isBn = false);
    }
}
