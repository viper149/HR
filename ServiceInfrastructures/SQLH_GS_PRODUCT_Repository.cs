using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLH_GS_PRODUCT_Repository : BaseRepository<H_GS_PRODUCT>, IH_GS_PRODUCT
    {
        private readonly IDataProtector _protector;
        private readonly H_GS_PRODUCT hGsProduct;

        public SQLH_GS_PRODUCT_Repository(DenimDbContext denimDbContext, IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<H_GS_PRODUCT> GetSingleProductByProductId(int id)
        {
            var fGsProductInformation = await DenimDbContext.H_GS_PRODUCT
                .Include(e => e.UNITNavigation)
                .Where(e => e.PRODID.Equals(id))
                .Select(d => new H_GS_PRODUCT
                {
                    PRODID = d.PRODID,
                    PRODNAME = d.PRODNAME,
                    PARTNO = d.PARTNO,
                    UNITNavigation = new F_BAS_UNITS
                    {
                        UID = d.UNITNavigation.UID,
                        UNAME = d.UNITNavigation.UNAME
                    }
                }).FirstOrDefaultAsync();

            return fGsProductInformation;
        }

        public async Task<List<H_GS_PRODUCT>> GetAllProductInformationAsync()
        {
            try
            {
                var hGsProduct = await DenimDbContext.H_GS_PRODUCT
                .Include(e => e.UNITNavigation)
                .Select(d => new H_GS_PRODUCT
                {
                    PRODID = d.PRODID,
                    PRODNAME = d.PRODNAME,
                    PARTNO = d.PARTNO,
                    UNITNavigation = new F_BAS_UNITS
                    {
                        UID = d.UNITNavigation.UID,
                        UNAME = d.UNITNavigation.UNAME
                    }
                }).ToListAsync();

                return hGsProduct;

            }
            catch(Exception ex)
            {
                return null;
            }
        }
    }
}
