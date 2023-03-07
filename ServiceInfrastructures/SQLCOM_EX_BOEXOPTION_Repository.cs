using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLCOM_EX_BOEXOPTION_Repository : BaseRepository<COM_EX_BOEXOPTION>, ICOM_EX_BOEXOPTION
    {
        public SQLCOM_EX_BOEXOPTION_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
    }
}
