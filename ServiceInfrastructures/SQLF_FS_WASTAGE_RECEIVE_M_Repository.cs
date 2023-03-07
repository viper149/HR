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
    public class SQLF_FS_WASTAGE_RECEIVE_M_Repository:BaseRepository<F_FS_WASTAGE_RECEIVE_M>, IF_FS_WASTAGE_RECEIVE_M
    {
        private readonly IDataProtector _protector;

        public SQLF_FS_WASTAGE_RECEIVE_M_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<List<F_FS_WASTAGE_RECEIVE_M>> GetAllFFsWastageReceiveAsync()
        {
            try
            {
                return await DenimDbContext.F_FS_WASTAGE_RECEIVE_M
                    .Include(d => d.SEC)
                    .Select(d => new F_FS_WASTAGE_RECEIVE_M()
                    {
                        WRID = d.WRID,
                        EncryptedId = _protector.Protect(d.WRID.ToString()),
                        WRDATE = d.WRDATE,
                        WTRNO = d.WTRNO,
                        WTRDATE = d.WTRDATE,
                        REMARKS = d.REMARKS,

                        SEC = new F_BAS_SECTION
                        {
                            SECID = d.SEC.SECID,
                            SECNAME = d.SEC.SECNAME,
                        }
                    }).ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<FFsWastageReceiveViewModel> GetInitObjByAsync(FFsWastageReceiveViewModel fFsWastageReceiveViewModel)
        {
            fFsWastageReceiveViewModel.SectionList = await DenimDbContext.F_BAS_SECTION
                .Select(d => new F_BAS_SECTION
                {
                    SECID = d.SECID,
                    SECNAME = d.SECNAME
                })
                .ToListAsync();
            fFsWastageReceiveViewModel.WasteProductList = await DenimDbContext.F_WASTE_PRODUCTINFO
                .Select(d => new F_WASTE_PRODUCTINFO
                {
                    WPID = d.WPID,
                    PRODUCT_NAME = d.PRODUCT_NAME
                })
                .ToListAsync();

            return fFsWastageReceiveViewModel;
        }

        public async Task<FFsWastageReceiveViewModel> FindByIdIncludeAllAsync(int fwrId)
        {
            return await DenimDbContext.F_FS_WASTAGE_RECEIVE_M
                .Include(d => d.F_FS_WASTAGE_RECEIVE_D)
                .ThenInclude(d => d.WP)
                .Where(d => d.WRID.Equals(fwrId))
                .Select(d => new FFsWastageReceiveViewModel
                {
                    FFsWastageReceiveM = new F_FS_WASTAGE_RECEIVE_M
                    {
                        WRID = d.WRID,
                        EncryptedId = _protector.Protect(d.WRID.ToString()),
                        WRDATE = d.WRDATE,
                        WTRNO = d.WTRNO,
                        WTRDATE = d.WTRDATE,
                        SECID = d.SECID,
                        REMARKS = d.REMARKS
                    },
                    FFsWastageReceiveDList = d.F_FS_WASTAGE_RECEIVE_D.Select(e => new F_FS_WASTAGE_RECEIVE_D
                    {
                        TRNSID = e.TRNSID,
                        WPID = e.WPID,
                        TRNSDATE = e.TRNSDATE,
                        RCV_QTY = e.RCV_QTY,
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
