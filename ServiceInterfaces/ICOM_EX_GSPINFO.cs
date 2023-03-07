using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces
{
    public interface ICOM_EX_GSPINFO : IBaseService<COM_EX_GSPINFO>
    {
        Task<IEnumerable<COM_EX_GSPINFO>> GetGspInfoWithPaged(int pageNumber, int pageSize);
        Task<IEnumerable<COM_EX_GSPINFO>> GetAllForDataTableByAsync();
        Task<dynamic> GetForGSPInformationByAsync(int invId);
        Task<IEnumerable<COM_EX_GSPINFO>> GetGSPNo(string GSPNO);
    }
}
