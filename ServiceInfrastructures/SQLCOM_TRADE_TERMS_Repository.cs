using System;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLCOM_TRADE_TERMS_Repository:BaseRepository<COM_TRADE_TERMS>, ICOM_TRADE_TERMS
    {
        public SQLCOM_TRADE_TERMS_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
        public async Task<bool> FindByTypeName(string name)
        {
            try
            {
                return await DenimDbContext.COM_TRADE_TERMS.Where(sc => sc.TRADE_TERMS.Equals(name)).AnyAsync();
            }
            catch (Exception ex)
            {
                throw new System.InvalidOperationException(ex.Message);
            }
        }
    }
}
