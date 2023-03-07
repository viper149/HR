using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;
using DenimERP.ViewModels.GeneralStore;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_GS_PRODUCT_INFORMATION : IBaseService<F_GS_PRODUCT_INFORMATION>
    {
        Task<F_GS_PRODUCT_INFORMATION> GetSingleProductByProductId(int id);
        Task<IEnumerable<F_GS_PRODUCT_INFORMATION>> GetAllProductInformationAsync();
        Task<bool> FindByProdName(string prodName);
        Task<FGsProductInformationViewModel> GetInitObjByAsync(FGsProductInformationViewModel fGsProductInformationViewModel, bool edit = false);
        Task<FGenSRequisitionViewModel> GetInitObjForDetailsByAsync(FGenSRequisitionViewModel fGenSRequisitionViewModel);
        Task<F_GS_PRODUCT_INFORMATION> GetIndentListByPId(int productId);
        Task<FGsProductInformationViewModel> FindByIdIncludeAllAsync(int prodId);
    }
}
