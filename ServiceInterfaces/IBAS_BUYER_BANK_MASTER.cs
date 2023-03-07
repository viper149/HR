using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces
{
    public interface IBAS_BUYER_BANK_MASTER : IBaseService<BAS_BUYER_BANK_MASTER>
    {
        Task<IEnumerable<BAS_BUYER_BANK_MASTER>> GetBasBuyerBanksWithPaged(int pageNumber = 1, int pageSize = 5);
        Task<bool> DeleteBank(int id);
        bool FindByBuyerBankName(string bankName);
    }
}
