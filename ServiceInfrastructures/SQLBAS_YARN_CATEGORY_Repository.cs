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
    public class SQLBAS_YARN_CATEGORY_Repository:BaseRepository<BAS_YARN_CATEGORY>, IBAS_YARN_CATEGORY
    {
        public SQLBAS_YARN_CATEGORY_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<bool> FindByYarnCategoryName(string catName)
        {
            try
            {
                var result = await DenimDbContext.BAS_YARN_CATEGORY.AnyAsync(c=>c.CATEGORY_NAME.Equals(catName));
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<bool> FindByYarnCode(int? code)
        {
            try
            {
                var result = await DenimDbContext.BAS_YARN_CATEGORY.AnyAsync(c=>c.YARN_CODE.Equals(code));
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<bool> DeleteCategory(int id)
        {

            try
            {
                var isFoundYarnCategory = await DenimDbContext.BAS_YARN_CATEGORY.FindAsync(id);
                if (isFoundYarnCategory == null) return false;

                var yarnCategory = await DenimDbContext.BAS_YARN_CATEGORY.Where(pi => pi.YARN_CAT_ID.Equals(isFoundYarnCategory.YARN_CAT_ID)).ToListAsync();
                if (yarnCategory.Any())
                {
                    DenimDbContext.BAS_YARN_CATEGORY.RemoveRange(yarnCategory);
                }
                DenimDbContext.BAS_YARN_CATEGORY.Remove(isFoundYarnCategory);
                await SaveChanges();
                return true;
            }
            catch (Exception)
            {
                throw new System.InvalidOperationException("Failed!");
            }
        }
    }
}
