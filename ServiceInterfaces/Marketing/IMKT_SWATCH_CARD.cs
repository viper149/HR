using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;
using DenimERP.ViewModels.Marketing;

namespace DenimERP.ServiceInterfaces.Marketing
{
    public interface IMKT_SWATCH_CARD : IBaseService<MKT_SWATCH_CARD>
    {
        Task<CreateMktSwatchCardViewModel> GetInitObjects(CreateMktSwatchCardViewModel createMktSwatchCardViewModel);
        Task<DataTableObject<MKT_SWATCH_CARD>> GetForDataTableByAsync(string sortColumn, string sortColumnDirection, string searchValue, string draw, int skip, int pageSize);
        Task<CreateMktSwatchCardViewModel> FindBySwCdIdAsync(int swCdId);
    }
}
