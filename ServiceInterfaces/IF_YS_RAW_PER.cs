using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_YS_RAW_PER: IBaseService<F_YS_RAW_PER>
    {
        Task<List<F_YS_RAW_PER>> GetAllAsync();
    }
}
