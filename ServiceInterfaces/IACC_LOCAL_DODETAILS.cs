using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces
{
    public interface IACC_LOCAL_DODETAILS : IBaseService<ACC_LOCAL_DODETAILS>
    {
       Task<IEnumerable<ACC_LOCAL_DODETAILS>> FindDoDetailsBydoNoAndStyleIdAsync(int doNo, int styleId);
       Task<IEnumerable<ACC_LOCAL_DODETAILS>> FindDoDetailsListByDoNoAsync(int doNo);
    }
}
