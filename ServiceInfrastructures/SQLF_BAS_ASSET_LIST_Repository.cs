using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_BAS_ASSET_LIST_Repository : BaseRepository<F_BAS_ASSET_LIST>, IF_BAS_ASSET_LIST

    {
        private readonly IDataProtector _protector;

        public SQLF_BAS_ASSET_LIST_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<IEnumerable<F_BAS_ASSET_LIST>> GetAllFBasAssetListAsync()
        {
            return await DenimDbContext.F_BAS_ASSET_LIST
                .Include(e => e.SEC)
                .Select(d => new F_BAS_ASSET_LIST
                {
                    ASSET_ID = d.ASSET_ID,
                    EncryptedId = _protector.Protect(d.ASSET_ID.ToString()),
                    ASSET_NAME = d.ASSET_NAME,
                    SEC_CODE = d.SEC_CODE,
                    REMARKS = d.REMARKS,
                    SEC = new F_BAS_SECTION
                    {
                        SECNAME = d.SEC.SECNAME
                    }
                }).ToListAsync();
        }

        public async Task<FBasAssetListViewModel> GetInitObjByAsync(FBasAssetListViewModel fBasAssetListViewModel)
        {
            fBasAssetListViewModel.SectionList = await DenimDbContext.F_BAS_SECTION
                    .Select(d => new F_BAS_SECTION
                    {
                        SECID = d.SECID,
                        SECNAME = d.SECNAME
                    }).ToListAsync();

            return fBasAssetListViewModel;
        }
        
        public async Task<bool> FindByAssetName(string assetName, int? secId)
        {
            return !await DenimDbContext.F_BAS_ASSET_LIST.AnyAsync(d => d.ASSET_NAME.Equals(assetName) && d.SEC_CODE.Equals(secId));
        }
    }
}
