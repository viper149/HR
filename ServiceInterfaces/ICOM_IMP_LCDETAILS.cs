using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels.Com;

namespace DenimERP.ServiceInterfaces
{
    public interface ICOM_IMP_LCDETAILS : IBaseService<COM_IMP_LCDETAILS>
    {
        Task<IEnumerable<COM_IMP_LCDETAILS>> FindByLcNoAsync(string lcNo);
        Task<COM_IMP_LCDETAILS> FindByLcNoAndProdIdAsync(string lcNo, int? prodId, string piNo);
        Task<ComImpLcInformationForCreateViewModel> GetInitObjForDetailsTable(ComImpLcInformationForCreateViewModel comImpLcInformationForCreateViewModel);
        Task<COM_IMP_LCDETAILS> GetLcNoByTransId(int id);
    }
}
