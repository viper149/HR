using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces
{
    public interface ICOM_EX_LCDETAILS : IBaseService<COM_EX_LCDETAILS>
    {
        Task<IEnumerable<COM_EX_LCDETAILS>> FindByLcIdIsDeleteAsync(int lcId);
        Task<COM_EX_LCDETAILS> FindByLcNoAndPiIdIsDeleteAsync(int piId, int lcId);
        Task<bool> IsAdvisingBankMatched(int? piId, int? bankId);
    }
}
