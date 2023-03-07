using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces
{
    public interface IRND_MSTR_ROLL:IBaseService<RND_MSTR_ROLL>
    {
        Task<RND_MSTR_ROLL> FindByIdAllAsync(int mid);
    }
}
