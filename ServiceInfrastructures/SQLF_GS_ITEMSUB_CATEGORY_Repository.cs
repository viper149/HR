using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels.GeneralStore;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_GS_ITEMSUB_CATEGORY_Repository : BaseRepository<F_GS_ITEMSUB_CATEGORY>, IF_GS_ITEMSUB_CATEGORY
    {
        private readonly IDataProtector _protector;

        public SQLF_GS_ITEMSUB_CATEGORY_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<List<F_GS_ITEMSUB_CATEGORY>> GetAllFGsItemSubCategoryAsync()
        {
            return await DenimDbContext.F_GS_ITEMSUB_CATEGORY
                .Include(d => d.CAT)
                .Select(d => new F_GS_ITEMSUB_CATEGORY
                {
                    SCATID = d.SCATID,
                    EncryptedId = _protector.Protect(d.SCATID.ToString()),
                    SCATNAME = d.SCATNAME,
                    DESCRIPTION = d.DESCRIPTION,
                    REMARKS = d.REMARKS,
                    CAT = new F_GS_ITEMCATEGORY
                    {
                        CATNAME = d.CAT.CATNAME
                    }
                }).ToListAsync();
        }

        public async Task<bool> FindBySCatName(string scatName)
        {
            return !await DenimDbContext.F_GS_ITEMSUB_CATEGORY.AnyAsync(d => d.SCATNAME.Equals(scatName));
        }

        public async Task<FGsItemSubCategoryViewModel> GetInitObjByAsync(FGsItemSubCategoryViewModel fGsItemSubCategoryViewModel)
        {
            fGsItemSubCategoryViewModel.FGsItemcategoriesList = await DenimDbContext.F_GS_ITEMCATEGORY
                .Select(d => new F_GS_ITEMCATEGORY
                {
                    CATID = d.CATID,
                    CATNAME = d.CATNAME
                }).ToListAsync();

            return fGsItemSubCategoryViewModel;
        }

        public async Task<IEnumerable<F_GS_ITEMSUB_CATEGORY>> GetSubCatByCatId(int catId)
        {
            return await DenimDbContext.F_GS_ITEMSUB_CATEGORY
                .Where(d => d.CATID.Equals(catId))
                .Select(d=> new F_GS_ITEMSUB_CATEGORY
                {
                    SCATID = d.SCATID,
                    SCATNAME = d.SCATNAME
                })
                .ToListAsync();
        }
    }
}
