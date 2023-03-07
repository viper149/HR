using System;
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
    public class SQLF_GS_WASTAGE_ISSUE_M_Repository : BaseRepository<F_GS_WASTAGE_ISSUE_M>, IF_GS_WASTAGE_ISSUE_M
    {
        private readonly IDataProtector _protector;

        public SQLF_GS_WASTAGE_ISSUE_M_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<IEnumerable<F_GS_WASTAGE_ISSUE_M>> GetAllFGsWastageIssueAsync()
        {
            try
            {
                return await DenimDbContext.F_GS_WASTAGE_ISSUE_M
                    .Include(d=>d.FGsWastageParty)
                    .Select(d => new F_GS_WASTAGE_ISSUE_M()
                    {
                        WIID = d.WIID,
                        EncryptedId = _protector.Protect(d.WIID.ToString()),
                        WIDATE = d.WIDATE,
                        GPNO = d.GPNO,
                        GPDATE = d.GPDATE,
                        VEHICLENO = d.VEHICLENO,
                        THROUGH = d.THROUGH,
                        REMARKS = d.REMARKS,

                        FGsWastageParty = new F_GS_WASTAGE_PARTY
                        {
                            PID = d.FGsWastageParty.PID,
                            PNAME = d.FGsWastageParty.PNAME,
                        }
                    }).ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<FGsWastageIssueViewModel> GetInitObjByAsync(FGsWastageIssueViewModel fGsWastageIssueViewModel)
        {
            fGsWastageIssueViewModel.FGsWastagePartyList = await DenimDbContext.F_GS_WASTAGE_PARTY
                .Select(d => new F_GS_WASTAGE_PARTY
                {
                    PID = d.PID,
                    PNAME = d.PNAME
                })
                .ToListAsync();
            fGsWastageIssueViewModel.FWasteProductList = await DenimDbContext.F_WASTE_PRODUCTINFO
                .Select(d => new F_WASTE_PRODUCTINFO
                {
                    WPID = d.WPID,
                    PRODUCT_NAME = d.PRODUCT_NAME
                })
                .ToListAsync();

            return fGsWastageIssueViewModel;
        }

        public async Task<FGsWastageIssueViewModel> FindByIdIncludeAllAsync(int wiId)
        {
            return await DenimDbContext.F_GS_WASTAGE_ISSUE_M
                .Include(d => d.F_GS_WASTAGE_ISSUE_D)
                .ThenInclude(d=> d.WP)
                .Where(d => d.WIID.Equals(wiId))
                .Select(d => new FGsWastageIssueViewModel
                {
                    FGsWastageIssueM = new F_GS_WASTAGE_ISSUE_M
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
                    FGsWastageIssueDList = d.F_GS_WASTAGE_ISSUE_D.Select(e => new F_GS_WASTAGE_ISSUE_D
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
