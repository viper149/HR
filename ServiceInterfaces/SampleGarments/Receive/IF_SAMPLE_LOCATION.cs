using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces.SampleGarments.Receive
{
    public interface IF_SAMPLE_LOCATION : IBaseService<F_SAMPLE_LOCATION>
    {
        Task<DataTableObject<F_SAMPLE_LOCATION>> GetForDataTableByAsync(string sortColumn, string sortColumnDirection, string searchValue, string draw, int skip, int pageSize);
    }
}
