using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces
{
    public interface IRND_YARNCONSUMPTION : IBaseService<RND_YARNCONSUMPTION>
    {
        Task<IEnumerable<RND_YARNCONSUMPTION>> FindByFabCodeIAsync(int fabCode);
        Task<double> GetConsumptionByCountIdAndFabCodeAsync(int countId, int fabCode,int yarnFor);
        Task<RND_YARNCONSUMPTION> GetPrimaryKeyByCountIdAndFabCodeAsync(int countId, int fabCode,int yarnFor,int? color);
    }
}
