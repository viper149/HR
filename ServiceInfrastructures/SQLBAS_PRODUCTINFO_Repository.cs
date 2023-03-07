using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels.Basic;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLBAS_PRODUCTINFO_Repository : BaseRepository<BAS_PRODUCTINFO>, IBAS_PRODUCTINFO
    {
        public SQLBAS_PRODUCTINFO_Repository(DenimDbContext denimDbContext)
            : base(denimDbContext)
        {
        }

        public async Task<IEnumerable<BAS_PRODUCTINFO>> GetProductInfoList()
        {
            var result = await DenimDbContext.BAS_PRODUCTINFO.Include(pc => pc.CAT).Where(pi => pi.CATID == pi.CAT.CATID).ToListAsync();
            return result;
        }

        public async Task<IEnumerable<BAS_PRODUCTINFO>> GetProductInfoListAllAsync()
        {
            var result = await DenimDbContext.BAS_PRODUCTINFO
                .Include(d => d.CAT)
                .Include(d=>d.UNITNavigation)
                .ToListAsync();

            return result;
        }

        public async Task<int> TotalNumberOfProducts()
        {
            var result = await DenimDbContext.BAS_PRODUCTINFO.Include(pc => pc.CAT).Where(pi => pi.CATID == pi.CAT.CATID).ToListAsync();
            return result.Count();
        }

        public async Task<BAS_PRODUCTINFO> FindProductInfoByAsync(int id)
        {
            try
            {
                var productInformation = await DenimDbContext.BAS_PRODUCTINFO.Include(pc => pc.CAT).Where(pi => pi.CATID == pi.CAT.CATID && pi.PRODID == id).FirstOrDefaultAsync();
                return productInformation;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<BasProductInfoViewModel> GetInfo(BasProductInfoViewModel basProductInfoViewModel)
        {
            basProductInfoViewModel.BAS_PRODCATEGORies = await DenimDbContext.BAS_PRODCATEGORY.ToListAsync();
            return basProductInfoViewModel;
        }
    }
}
