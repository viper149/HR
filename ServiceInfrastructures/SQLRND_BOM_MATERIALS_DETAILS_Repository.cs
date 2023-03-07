using System;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLRND_BOM_MATERIALS_DETAILS_Repository : BaseRepository<RND_BOM_MATERIALS_DETAILS>, IRND_BOM_MATERIALS_DETAILS
    {
        public SQLRND_BOM_MATERIALS_DETAILS_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<RndBomViewModel> GetMaterialsNameAsync(RndBomViewModel rndBomViewModel)
        {
            try
            {

                foreach (var item in rndBomViewModel.RndBomMaterialsDetailsList)
                {
                    item.CHEM_PROD_ = await DenimDbContext.F_CHEM_STORE_PRODUCTINFO
                        .Where(c => c.PRODUCTID.Equals(item.CHEM_PROD_ID)).FirstOrDefaultAsync();
                    item.SECTIONNavigation = await DenimDbContext.F_BAS_SECTION.Where(c => c.SECID.Equals(item.SECTION))
                        .FirstOrDefaultAsync();
                }
                return rndBomViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
