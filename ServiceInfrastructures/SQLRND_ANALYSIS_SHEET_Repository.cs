using System;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels.Rnd;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLRND_ANALYSIS_SHEET_Repository : BaseRepository<RND_ANALYSIS_SHEET>, IRND_ANALYSIS_SHEET
    {
        public SQLRND_ANALYSIS_SHEET_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<int> InsertAndGetIdAsync(RND_ANALYSIS_SHEET rndAnalysisSheet)
        {
            try
            {
                await DenimDbContext.RND_ANALYSIS_SHEET.AddAsync(rndAnalysisSheet);
                await SaveChanges();
                return rndAnalysisSheet.AID;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw ex;
            }
        }

        public async Task<RndAnalysisSheetInfoViewModel> GetInitObjects(RndAnalysisSheetInfoViewModel rndAnalysisSheetInfoViewModel)
        {
            try
            {
                rndAnalysisSheetInfoViewModel.MktTeams = await DenimDbContext.MKT_TEAM.OrderBy(e => e.PERSON_NAME).ToListAsync();
                rndAnalysisSheetInfoViewModel.BasBuyerInfos = await DenimDbContext.BAS_BUYERINFO.OrderBy(e => e.BUYER_NAME).ToListAsync();
                rndAnalysisSheetInfoViewModel.RndFinishTypes = await DenimDbContext.RND_FINISHTYPE.OrderBy(e => e.TYPENAME).ToListAsync();
                rndAnalysisSheetInfoViewModel.RndWeaves = await DenimDbContext.RND_WEAVE.OrderBy(e => e.NAME).ToListAsync();
                rndAnalysisSheetInfoViewModel.MktSwatchCards = await DenimDbContext.MKT_SWATCH_CARD.OrderBy(e => e.SWCDID).ToListAsync();
                rndAnalysisSheetInfoViewModel.BasBrandinfos = await DenimDbContext.BAS_BRANDINFO.OrderBy(e => e.BRANDNAME).ToListAsync();
                rndAnalysisSheetInfoViewModel.BasYarnCountinfos = await DenimDbContext.BAS_YARN_COUNTINFO.OrderBy(e => e.COUNTNAME).ToListAsync();

                return rndAnalysisSheetInfoViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<RndAnalysisSheetInfoViewModel> GetInitObjectsForAddYarnDetailsByAsync(RndAnalysisSheetInfoViewModel rndAnalysisSheetInfoViewModel)
        {
            foreach (var item in rndAnalysisSheetInfoViewModel.RndAnalysisSheetDetailsList)
            {
                item.BasYarnCountinfo = await DenimDbContext.BAS_YARN_COUNTINFO.FirstOrDefaultAsync(e => e.COUNTID.Equals(item.COUNTID));
            }

            return rndAnalysisSheetInfoViewModel;
        }

        public async Task<MKT_SWATCH_CARD> GetSwatchCardDetails(int swatchId)
        {
            try
            {
                return await DenimDbContext.MKT_SWATCH_CARD
                    .Include(e => e.TEAM)
                    .FirstOrDefaultAsync(c => c.SWCDID.Equals(swatchId));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<string> GetLastRndQueryNoAsync()
        {
            try
            {
                var piNo = "";
                var result = await DenimDbContext.RND_ANALYSIS_SHEET.OrderByDescending(c => c.RND_QUERY_NO).Select(c => c.RND_QUERY_NO).FirstOrDefaultAsync();
                var year = DateTime.Now.Year - 2000;

                if (result != null)
                {
                    var resultArray = result.Split("-");
                    if (int.Parse(resultArray[1]) < year)
                    {
                        piNo = "RD-" + year + "-" + "1".PadLeft(4, '0');
                    }
                    else
                    {
                        piNo = "RD-" + year + "-" + (int.Parse(resultArray[2]) + 1).ToString().PadLeft(4, '0');
                    }
                }
                else
                {
                    piNo = "RD-" + year + "-" + "1".PadLeft(4, '0');
                }

                return piNo;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RND_ANALYSIS_SHEET> FindByIdIncludeAllAsync(int id)
        {
            return await DenimDbContext.RND_ANALYSIS_SHEET
                .Include(e => e.MKT_PERSON_)
                .Include(e => e.Swatch)
                .Include(e => e.BUYER)
                .Include(e => e.RND_ANALYSIS_SHEET_DETAILS)
                .ThenInclude(e => e.BasYarnCountinfo)
                .FirstOrDefaultAsync(e => e.AID.Equals(id));
        }
    }
}
