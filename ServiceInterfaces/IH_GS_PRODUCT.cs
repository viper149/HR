using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces
{
    public interface IH_GS_PRODUCT : IBaseService<H_GS_PRODUCT>
    {
        Task<List<H_GS_PRODUCT>> GetAllProductInformationAsync();
        Task<H_GS_PRODUCT> GetSingleProductByProductId(int id);
    }
}
