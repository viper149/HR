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
    public class SQLF_FS_WASTAGE_ISSUE_M_Repository : BaseRepository<F_FS_WASTAGE_ISSUE_M>, IF_FS_WASTAGE_ISSUE_M
    {
        private readonly IDataProtector _protector;

        public SQLF_FS_WASTAGE_ISSUE_M_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<IEnumerable<F_FS_WASTAGE_ISSUE_M>> GetAllFFsWastageIssueAsync()
        {
            return await DenimDbContext.F_FS_WASTAGE_ISSUE_M
                .Include(d => d.FFsWastageParty)
                .Select(d => new F_FS_WASTAGE_ISSUE_M()
                {
                    WIID = d.WIID,
                    EncryptedId = _protector.Protect(d.WIID.ToString()),
                    WIDATE = d.WIDATE,
                    GPNO = d.GPNO,
                    GPDATE = d.GPDATE,
                    VEHICLENO = d.VEHICLENO,
                    THROUGH = d.THROUGH,
                    REMARKS = d.REMARKS,

                    FFsWastageParty = new F_FS_WASTAGE_PARTY
                    {
                        PID = d.FFsWastageParty.PID,
                        PNAME = d.FFsWastageParty.PNAME,
                    }
                }).ToListAsync();

        }

        public async Task<FFsWastageIssueViewModel> GetInitObjByAsync(FFsWastageIssueViewModel fFsWastageIssueViewModel)
        {
            fFsWastageIssueViewModel.FFsWastagePartyList = await DenimDbContext.F_FS_WASTAGE_PARTY
                .Select(d => new F_FS_WASTAGE_PARTY
                {
                    PID = d.PID,
                    PNAME = d.PNAME
                })
                .ToListAsync();
            fFsWastageIssueViewModel.FWasteProductList = await DenimDbContext.F_WASTE_PRODUCTINFO
                .Select(d => new F_WASTE_PRODUCTINFO
                {
                    WPID = d.WPID,
                    PRODUCT_NAME = d.PRODUCT_NAME
                })
                .ToListAsync();

            return fFsWastageIssueViewModel;

        }

        public async Task<FFsWastageIssueViewModel> FindByIdIncludeAllAsync(int fwiId)
        {
            return await DenimDbContext.F_FS_WASTAGE_ISSUE_M
                .Include(d => d.F_FS_WASTAGE_ISSUE_D)
                .ThenInclude(d => d.WP)
                .Where(d => d.WIID.Equals(fwiId))
                .Select(d => new FFsWastageIssueViewModel
                {
                    FFsWastageIssueM = new F_FS_WASTAGE_ISSUE_M
                    {
                        WIID = d.WIID,
                        EncryptedId = _protector.Protect(d.WIID.ToString()),
                        WIDATE = d.WIDATE,
                        PID = d.PID,
                        GPNO = d.GPNO,
                        GPDATE = d.GPDATE,
                        VEHICLENO = d.VEHICLENO,
                        THROUGH = d.THROUGH,
                        REMARKS = d.REMARKS
                    },
                    FFsWastageIssueDList = d.F_FS_WASTAGE_ISSUE_D.Select(e => new F_FS_WASTAGE_ISSUE_D
                    {
                        TRNSID = e.TRNSID,
                        WPID = e.WPID,
                        TRNSDATE = e.TRNSDATE,
                        ISSUE_QTY = e.ISSUE_QTY,
                        REMARKS = e.REMARKS,
                        WP = new F_WASTE_PRODUCTINFO
                        {
                            PRODUCT_NAME = e.WP.PRODUCT_NAME
                        }
                    }).ToList()
                }).FirstOrDefaultAsync();
        }
    }
}
