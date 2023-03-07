using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_YS_INDENT_DETAILS : IBaseService<F_YS_INDENT_DETAILS>
    {
        Task<dynamic> GetIndentYarnListByIndidAsync(int indId);
        Task<dynamic> GetInvoiceDetailsByINVIDAsync(int invId);
        Task<F_YS_INDENT_DETAILS> GetIndentQtyAsync(int prodId, int indId);

        Task<F_YS_INDENT_DETAILS> FindIndentListByIdAsync(int id,int prdId);
        Task<FYarnIndentComExPiViewModel> FindAllpRODUCTListAsync(int id);
        Task<YarnRequirementViewModel> GetCountIdList(int poId);
        Task<int> GetYarnForByIndProdAsync(int indId, int prodId);
    }
}
