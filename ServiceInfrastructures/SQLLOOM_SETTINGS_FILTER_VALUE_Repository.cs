using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLLOOM_SETTINGS_FILTER_VALUE_Repository:BaseRepository<LOOM_SETTINGS_FILTER_VALUE>, ILOOM_SETTINGS_FILTER_VALUE
    {
        public SQLLOOM_SETTINGS_FILTER_VALUE_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
    }
}
