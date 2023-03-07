using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLBAS_SUPP_CATEGORY_Repository : BaseRepository<BAS_SUPP_CATEGORY>, IBAS_SUPP_CATEGORY
    {
        public SQLBAS_SUPP_CATEGORY_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public bool FindBySupplierCategoryName(string categoryName)
        {
            var catNames = DenimDbContext.BAS_SUPP_CATEGORY.Where(sc => sc.CATNAME.Equals(categoryName)).Select(e => e.CATNAME);

            if (catNames.Any())
                return false;
            else
                return true;
        }

        public async Task<IEnumerable<BAS_SUPP_CATEGORY>> GetBasSupplierCategoriesWithPaged(int pageNumber = 1, int pageSize = 5)
        {
            try
            {
                int ExcludeResult = (pageSize * pageNumber) - pageSize;

                var result = await DenimDbContext.BAS_SUPP_CATEGORY
                     .OrderByDescending(sc => sc.SCATID)
                    .Skip(ExcludeResult)
                    .Take(pageSize)
                    .ToListAsync();
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> DeleteCategory(int id)
        {

            try
            {
                var isFoundSupplierCategory = await DenimDbContext.BAS_SUPP_CATEGORY.FindAsync(id);
                if (isFoundSupplierCategory != null)
                {
                    var supplierCategory = await DenimDbContext.BAS_SUPP_CATEGORY.Where(pi => pi.SCATID == isFoundSupplierCategory.SCATID).ToListAsync();
                    if (supplierCategory.Count() > 0)
                    {
                        DenimDbContext.BAS_SUPP_CATEGORY.RemoveRange(supplierCategory);
                        //SaveChanges();
                    }
                    DenimDbContext.BAS_SUPP_CATEGORY.Remove(isFoundSupplierCategory);
                    await SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                throw new System.InvalidOperationException("Failed!");
            }
        }
    }
}
