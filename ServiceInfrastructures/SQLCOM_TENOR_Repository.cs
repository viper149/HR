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
    public class SQLCOM_TENOR_Repository : BaseRepository<COM_TENOR>, ICOM_TENOR
    {
        private readonly IDataProtector _protector;

        public SQLCOM_TENOR_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<bool> FindByTypeName(string name)
        {
            try
            {
                return await DenimDbContext.COM_TENOR.Where(sc => sc.NAME.Equals(name)).AnyAsync();
            }
            catch (Exception ex)
            {
                throw new System.InvalidOperationException(ex.Message);
            }
        }

        public async Task<IEnumerable<COM_TENOR>> GetAllForDataTableByAsync()
        {
            return await DenimDbContext.COM_TENOR.Select(e => new COM_TENOR
            {
                EncryptedId = _protector.Protect(e.TID.ToString()),
                NAME = e.NAME,
                COST = e.COST,
                REMARKS = e.REMARKS
            }).ToListAsync();
        }
    }
}
