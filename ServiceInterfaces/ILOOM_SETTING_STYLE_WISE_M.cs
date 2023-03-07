using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels.LoomSetting;

namespace DenimERP.ServiceInterfaces
{
    public interface ILOOM_SETTING_STYLE_WISE_M:IBaseService<LOOM_SETTING_STYLE_WISE_M>
    {
        //Task<IEnumerable<LOOM_SETTING_STYLE_WISE_M>> GetAllAsync();
        Task<LoomSettingStyleWiseViewModel> GetInitObjects(LoomSettingStyleWiseViewModel loomSettingStyleWiseViewModel, bool edit = false);
        Task<RND_FABRICINFO> GetStyleDetails(int fabcode);
        Task<RND_FABRIC_COUNTINFO> GetCountDetails(int countId);

        Task<IEnumerable<LOOM_SETTING_STYLE_WISE_M>> GetAllLoomSettingStyleWiseAsync();
    }
}
