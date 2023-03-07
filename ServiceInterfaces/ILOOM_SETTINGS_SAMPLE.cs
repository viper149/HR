using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces
{
    public interface ILOOM_SETTINGS_SAMPLE : IBaseService<LOOM_SETTINGS_SAMPLE>
    {
        Task<IEnumerable<LOOM_SETTINGS_SAMPLE>> GetAllLoomSettingsSampleAsync();
        Task<LoomSettingsSampleViewModel> GetInitObjByAsync(LoomSettingsSampleViewModel loomSettingsSampleViewModel);
        Task<dynamic> GetAllByDevIdAsync(int devId);
    }
}
