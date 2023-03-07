using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels.LoomSetting;

namespace DenimERP.ServiceInterfaces
{
    public interface ILOOM_SETTING_CHANNEL_INFO:IBaseService<LOOM_SETTING_CHANNEL_INFO>
    {
        Task<LoomSettingStyleWiseViewModel> GetInitChannelData(LoomSettingStyleWiseViewModel loomSettingStyleWiseViewModel);
    }
}
