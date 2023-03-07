using System;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_YARN_TRANSACTION : IBaseService<F_YARN_TRANSACTION>
    {
        Task<double?> GetLastBalanceByCountIdAsync(int? countId,int lotId,int INDENT_TYPE);
        Task<double?> GetLastBalanceByIndentAsync(int? RCVDID, int INDENT_TYPE,DateTime? YIssueDate);
        Task<int?> GetLastBagBalanceByCountIdAsync(int? countId,int lotId,int INDENT_TYPE);
        Task<int?> GetLastBagBalanceByIndentAsync(int? RCVDID, int INDENT_TYPE, DateTime? YIssueDate);
        Task<dynamic> GetPoidByRcvId(int id);
        Task<int?> GetPoidByIndId(int id);
    }
}
