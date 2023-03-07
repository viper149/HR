using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces
{
    public interface IBAS_BEN_BANK_MASTER : IBaseService<BAS_BEN_BANK_MASTER>
    {
        Task<IEnumerable<BAS_BEN_BANK_MASTER>> GetAllBasBenBankAsync();
        Task<bool> FindByBenBankAsync(string bank);
        Task<List<BAS_BEN_BANK_MASTER>> GetAllBenBanksAsync();
    }
}
