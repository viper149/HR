using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces
{
    public interface ICOM_IMP_CSITEM_DETAILS: IBaseService<COM_IMP_CSITEM_DETAILS>
    {
        Task<int> InsertAndGetIdAsync(COM_IMP_CSITEM_DETAILS comImpCsItemDetails);
    }
}
