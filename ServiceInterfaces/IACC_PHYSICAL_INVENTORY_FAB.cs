using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces
{
    public interface IACC_PHYSICAL_INVENTORY_FAB : IBaseService<ACC_PHYSICAL_INVENTORY_FAB>
    {
        Task<IEnumerable<ACC_PHYSICAL_INVENTORY_FAB>> GetAllAsync(string userId);
        Task<int> GetRcvIdByRollNoAsync(string rollNo);
        Task<bool> FindReceivedByRoll(string roll);
        Task<bool> FindDuplicateByRoll(string roll);
    }
}
