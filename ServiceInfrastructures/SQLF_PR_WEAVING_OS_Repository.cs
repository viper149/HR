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
    public class SQLF_PR_WEAVING_OS_Repository : BaseRepository<F_PR_WEAVING_OS>,
        IF_PR_WEAVING_OS
    {

        private readonly IDataProtector _protector;

        public SQLF_PR_WEAVING_OS_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<IEnumerable<F_PR_WEAVING_OS>> GetAllFPrWeavingOs()
        {
            return await DenimDbContext.F_PR_WEAVING_OS
                .Include(e=>e.COUNT)
                .Include(e=>e.LOT)
                .Include(e=>e.PO.SO)

                .Select(e => new F_PR_WEAVING_OS
                {
                    OSID=e.OSID,
                    EncryptedId = _protector.Protect(e.OSID.ToString()),
                    OS=e.OS,
                    OSDATE=e.OSDATE,
                    REMARKS=e.REMARKS,

                    COUNT= new BAS_YARN_COUNTINFO()
                    {
                        RND_COUNTNAME=e.COUNT.RND_COUNTNAME
                    },
                    LOT= new BAS_YARN_LOTINFO()
                    {
                        LOTNO=e.LOT.LOTNO
                    },
                    PO= new RND_PRODUCTION_ORDER()
                    {
                        SO=new COM_EX_PI_DETAILS()
                        {
                            SO_NO=e.PO.SO.SO_NO
                        }
                    }

                }).ToListAsync();
        }

        public async Task<FPrWeavingOsViewModel> GetInitObjByAsync(FPrWeavingOsViewModel fPrWeavingOsViewModel)
        {
            fPrWeavingOsViewModel.PosoViewModels = await DenimDbContext.RND_PRODUCTION_ORDER
               .Include(e => e.SO)
               .Select(e => new POSOViewModel
               {
                   Poid = e.POID,
                   SO_NO = e.SO.SO_NO
               })
               .Where(e => !string.IsNullOrEmpty(e.SO_NO))
               .OrderBy(e => e.SO_NO).ToListAsync();

            fPrWeavingOsViewModel.PlProductionSetDistributions = await DenimDbContext.PL_PRODUCTION_SETDISTRIBUTION.Include(c => c.PROG_).Select(e => new TypeTableViewModel
            {
                ID = e.SETID,
                Name = e.PROG_.PROG_NO
            }).OrderByDescending(e => e.Name).ToListAsync();

            fPrWeavingOsViewModel.LotList = await DenimDbContext.BAS_YARN_LOTINFO.Select(e => new BAS_YARN_LOTINFO
            {
                LOTID = e.LOTID,
                LOTNO = e.LOTNO
            }).OrderBy(e => e.LOTNO).ToListAsync();

            fPrWeavingOsViewModel.CountList= await DenimDbContext.BAS_YARN_COUNTINFO
                .Select(e=> new BAS_YARN_COUNTINFO
                {
                    COUNTID=e.COUNTID,
                    RND_COUNTNAME=e.RND_COUNTNAME
                }).OrderBy(e => e.RND_COUNTNAME).ToListAsync();

            return fPrWeavingOsViewModel;
        }
    }
}
