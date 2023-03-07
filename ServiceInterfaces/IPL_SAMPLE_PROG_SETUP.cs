using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces
{
    public interface IPL_SAMPLE_PROG_SETUP : IBaseService<PL_SAMPLE_PROG_SETUP>
    {
        Task<IEnumerable<PL_SAMPLE_PROG_SETUP>> GetAllBySdIdAsync(int sdId);
        Task<PL_SAMPLE_PROG_SETUP> FindByProgIdAsync(int progId);
    }
}
