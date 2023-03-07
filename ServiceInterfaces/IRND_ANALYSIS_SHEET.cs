using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels.Rnd;

namespace DenimERP.ServiceInterfaces
{
    public interface IRND_ANALYSIS_SHEET : IBaseService<RND_ANALYSIS_SHEET>
    {
        Task<int> InsertAndGetIdAsync(RND_ANALYSIS_SHEET rndAnalysisSheet);
        Task<RndAnalysisSheetInfoViewModel> GetInitObjects(RndAnalysisSheetInfoViewModel rndAnalysisSheetInfoViewModel);
        Task<RndAnalysisSheetInfoViewModel> GetInitObjectsForAddYarnDetailsByAsync(RndAnalysisSheetInfoViewModel rndAnalysisSheetInfoViewModel);
        Task<MKT_SWATCH_CARD> GetSwatchCardDetails(int swatchId);
        Task<string> GetLastRndQueryNoAsync();
        Task<RND_ANALYSIS_SHEET> FindByIdIncludeAllAsync(int id);
    }
}
