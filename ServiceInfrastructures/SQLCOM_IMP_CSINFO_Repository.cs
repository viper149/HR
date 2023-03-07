using System;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLCOM_IMP_CSINFO_Repository:BaseRepository<COM_IMP_CSINFO>, ICOM_IMP_CSINFO
    {
        public SQLCOM_IMP_CSINFO_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
        public async Task<int> InsertAndGetIdAsync(COM_IMP_CSINFO comImpCsInfo)
        {
            try
            {
                await DenimDbContext.COM_IMP_CSINFO.AddAsync(comImpCsInfo);
                await SaveChanges();
                return comImpCsInfo.CSID;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw;
            }
        }
    }
}
