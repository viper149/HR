using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_CHEM_STORE_PRODUCTINFO : IBaseService<F_CHEM_STORE_PRODUCTINFO>
    {
        Task<dynamic> GetSingleProductDetailsByPID(int id);
        Task<dynamic> GetProducts();
        Task<IEnumerable<F_CHEM_STORE_PRODUCTINFO>> GetProductDD();
        Task<FChemicalRequisitionViewModel> GetInitObjForDetailsByAsync(FChemicalRequisitionViewModel requisitionViewModel);
        Task<FChemProductEntryViewModel> GetInitObjByAsync(FChemProductEntryViewModel fChemProductEntryViewModel);
        Task<FChemProductEntryViewModel> FindByIdIncludeAllAsync(int productId);
        Task<bool> FindByProdName(string prodName);
        Task<FChemProductEntryViewModel> FindByIdIncludeAllForDetailsAsync(int productId);
    }
}
