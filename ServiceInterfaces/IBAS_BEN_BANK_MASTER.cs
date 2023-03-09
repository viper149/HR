using System.Collections.Generic;
using System.Threading.Tasks;
using HRMS.Models;
using HRMS.ServiceInterfaces.BaseInterfaces;

namespace HRMS.ServiceInterfaces
{
    public interface IBAS_BEN_BANK_MASTER : IBaseService<BAS_BEN_BANK_MASTER>
    {
        Task<IEnumerable<BAS_BEN_BANK_MASTER>> GetAllBasBenBankAsync();
        Task<bool> FindByBenBankAsync(string bank);
        Task<List<BAS_BEN_BANK_MASTER>> GetAllBenBanksAsync();
    }
}
