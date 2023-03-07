using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces
{
    public interface ICOM_EX_CERTIFICATE_MANAGEMENT : IBaseService<COM_EX_CERTIFICATE_MANAGEMENT>
    {

        Task<IEnumerable<COM_EX_CERTIFICATE_MANAGEMENT>> GetAllComExCertificateManagement();
        Task<ComExCertificateManagementViewModel> GetInitObjByAsync(ComExCertificateManagementViewModel comExCertificateManagementViewModel,bool filter);

        Task<COM_EX_INVOICEMASTER> GetAllByIdAsync(int id);


    }
}
