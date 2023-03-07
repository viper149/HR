using System;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLCOM_IMP_CSITEM_DETAILS_Repository:BaseRepository<COM_IMP_CSITEM_DETAILS>, ICOM_IMP_CSITEM_DETAILS
    {
        public SQLCOM_IMP_CSITEM_DETAILS_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
        public async Task<int> InsertAndGetIdAsync(COM_IMP_CSITEM_DETAILS comImpCsItemDetails)
        {
            try
            {
                await DenimDbContext.COM_IMP_CSITEM_DETAILS.AddAsync(comImpCsItemDetails);
                await SaveChanges();
                return comImpCsItemDetails.CSITEMID;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw;
            }
        }
    }
}
