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
    public class SQLBAS_PRODCATEGORY_Repository : BaseRepository<BAS_PRODCATEGORY>, IBAS_PRODCATEGORY
    {
        public SQLBAS_PRODCATEGORY_Repository(DenimDbContext denimDbContext)
            : base(denimDbContext)
        {

        }

        public async Task<bool> DeleteCategory(int id)
        {
            var isFoundProductCategory = await DenimDbContext.BAS_PRODCATEGORY.FindAsync(id);
            if (isFoundProductCategory != null)
            {
                try
                {
                    var products = await DenimDbContext.BAS_PRODUCTINFO.Where(pi => pi.CATID == isFoundProductCategory.CATID).ToListAsync();
                    if (products.Count() > 0)
                    {
                        DenimDbContext.BAS_PRODUCTINFO.RemoveRange(products);
                        //SaveChanges();
                    }
                    DenimDbContext.BAS_PRODCATEGORY.Remove(isFoundProductCategory);
                    await SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    throw new System.InvalidOperationException("Failed!");
                }
            }
            else
            {
                return false;
            }
        }

        public bool FindByProductCategoryName(string categoryName)
        {
            var catNames = DenimDbContext.BAS_PRODCATEGORY.Where(pc => pc.CATEGORY.Equals(categoryName)).Select(e => e.CATEGORY);

            if (catNames.Any())
                return false;
            else
                return true;
        }

        public async Task<IEnumerable<BAS_PRODCATEGORY>> GetProductCategoryInfoWithPaged(int pageNumber = 1, int pageSize = 1)
        {
            try
            {
                int ExcludeResult = (pageSize * pageNumber) - pageSize;

                var result = await DenimDbContext.BAS_PRODCATEGORY
                    .OrderByDescending(pi => pi.CATID)
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

        public async Task<int> TotalNumberOfProductCategory()
        {
            var result = await DenimDbContext.BAS_PRODCATEGORY.ToListAsync();
            return result.Count();
        }
    }
}
