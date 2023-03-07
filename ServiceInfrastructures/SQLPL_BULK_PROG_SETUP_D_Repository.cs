using System;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels.Planning;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLPL_BULK_PROG_SETUP_D_Repository: BaseRepository<PL_BULK_PROG_SETUP_D>, IPL_BULK_PROG_SETUP_D
    {
        public SQLPL_BULK_PROG_SETUP_D_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
        public async Task<int> InsertAndGetIdAsync(PL_BULK_PROG_SETUP_D plBulkProgSetupD)
        {
            try
            {
                await DenimDbContext.PL_BULK_PROG_SETUP_D.AddAsync(plBulkProgSetupD);
                await SaveChanges();
                return plBulkProgSetupD.PROG_ID;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw;
            }
        }
        public async Task<bool> FindByProgNoInUseAsync(string ProgNo)
        {
            return await DenimDbContext.PL_BULK_PROG_SETUP_D.Where(c => c.PROG_NO == ProgNo).AnyAsync();
        }

        public async Task<PlBulkProgSetupViewModel> GetBulkProgList(PlBulkProgSetupViewModel plBulkProgSetupViewModel)
        {
            try
            {
                var result = await DenimDbContext.RND_PRODUCTION_ORDER
                    .Include(c => c.RS.RND_SAMPLE_INFO_DETAILS)
                    .ThenInclude(c=>c.YARN)
                    .Include(c => c.RS.RND_SAMPLE_INFO_DETAILS)
                    .ThenInclude(c=>c.SUPP)
                    .Include(c => c.SO.STYLE.FABCODENavigation.RND_FABRIC_COUNTINFO)
                    .ThenInclude(c=>c.COUNT)
                    .Include(c => c.SO.STYLE.FABCODENavigation.RND_FABRIC_COUNTINFO)
                    .ThenInclude(c=>c.YarnFor)
                    .Include(c => c.SO.STYLE.FABCODENavigation.RND_FABRIC_COUNTINFO)
                    .ThenInclude(c=>c.SUPP)
                    .Where(c => c.POID.Equals(plBulkProgSetupViewModel.PlBulkProgSetupM.ORDERNO))
                    .FirstOrDefaultAsync();


                plBulkProgSetupViewModel.PlBulkProgSetupD.PlBulkProgYarnDList = result.SO==null? 
                    result.RS.RND_SAMPLE_INFO_DETAILS.Select(c => new PL_BULK_PROG_YARN_D
                {
                    SCOUNTID = c.TRNSID,
                    OPT1 = c.YARN.YARNNAME,
                    OPT2 = c.SUPP.SUPPNAME,
                    LOTID = c.LOTID
                }).ToList() : 
                    result.SO.STYLE.FABCODENavigation
                    .RND_FABRIC_COUNTINFO.OrderBy(c=>c.YARNFOR).ThenByDescending(c=>c.RATIO).Select(c => new PL_BULK_PROG_YARN_D
                    {
                        COUNTID = c.TRNSID,
                        OPT1 = c.YarnFor.YARNNAME,
                        OPT2 = c.SUPP.SUPPNAME,
                        LOTID = c.LOTID
                    }).ToList();

                if (plBulkProgSetupViewModel.PlBulkProgSetupDList.Any())
                {
                    foreach (var item in plBulkProgSetupViewModel.PlBulkProgSetupD.PlBulkProgYarnDList)
                    {
                        item.LOTID = plBulkProgSetupViewModel.PlBulkProgSetupDList.FirstOrDefault().PlBulkProgYarnDList
                            .FirstOrDefault(c => c.COUNTID.Equals(item.COUNTID) && c.OPT1.Equals(item.OPT1)).LOTID;
                    }
                }
                

                return plBulkProgSetupViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        public async Task<string> GetLastSetNo(PlBulkProgSetupViewModel plBulkProgSetupViewModel)
        {
            try
            {
                var lastSet = await DenimDbContext.PL_BULK_PROG_SETUP_D.Where(c=>c.PROG_NO.StartsWith(plBulkProgSetupViewModel.PlBulkProgSetupD.PROG_NO)).OrderByDescending(c=>c.PROG_ID).FirstOrDefaultAsync();

                //lastSet.OPT1 = (int.Parse(lastSet.PROG_NO.Split("/")[1]) + 1).ToString();
                return lastSet.PROG_NO;
                //return plBulkProgSetupViewModel.PlBulkProgSetupD.PROG_NO+"/"+lastSet.OPT1;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
