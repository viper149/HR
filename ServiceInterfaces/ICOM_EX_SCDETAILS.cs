using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces
{
    public interface ICOM_EX_SCDETAILS : IBaseService<COM_EX_SCDETAILS>
    {
        Task<IEnumerable<COM_EX_SCDETAILS>> FindScDetailsByScNoAndStyleIdAsync(int? scNo,int styleId);
        Task<IEnumerable<COM_EX_SCDETAILS>> GetComExScDetailsList(int? scNo);
    }
}
