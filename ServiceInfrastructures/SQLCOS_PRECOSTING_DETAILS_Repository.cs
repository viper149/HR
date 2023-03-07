using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLCOS_PRECOSTING_DETAILS_Repository : BaseRepository<COS_PRECOSTING_DETAILS>, ICOS_PRECOSTING_DETAILS
    {
        public SQLCOS_PRECOSTING_DETAILS_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<IEnumerable<COS_PRECOSTING_DETAILS>> FindPreCostDetailsListByFabCodeAndCountIdAsync(int fabCode, int countId)
        {
            try
            {
                var result = await DenimDbContext.COS_PRECOSTING_DETAILS
                    .Where(pi => pi.FABCODE.Equals(fabCode) && pi.COUNTID.Equals(countId)).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<COS_PRECOSTING_DETAILS>> GetAllDetailsAsync(int csId)
        {
            try
            {
                var result = await DenimDbContext.COS_PRECOSTING_DETAILS
                    .Include(c => c.BasYarnCountInfo)
                    
                    .OrderBy(c=>c.YARN_FOR)
                    .ThenByDescending(c=>c.RND_CONSUMP)
                    .Where(c => c.CSID == csId).ToListAsync();

                foreach (var item in result)
                {
                    item.Lot = await DenimDbContext.RND_FABRIC_COUNTINFO
                        .Include(c => c.LOT)
                        .Where(c => c.FABCODE.Equals(item.FABCODE) && c.COUNTID.Equals(item.COUNTID) &&
                                    c.YARNFOR.Equals(item.YARN_FOR))
                        .Select(c=>c.LOT.LOTNO)
                        .FirstOrDefaultAsync();

                    var Type = await DenimDbContext.RND_FABRIC_COUNTINFO
                        .Include(c => c.LOT)
                        .Where(c => c.FABCODE.Equals(item.FABCODE) && c.COUNTID.Equals(item.COUNTID) &&
                                    c.YARNFOR.Equals(item.YARN_FOR))
                        .Select(c => c.YARNTYPE)
                        .FirstOrDefaultAsync();

                    var color = await DenimDbContext.RND_FABRIC_COUNTINFO
                        .Include(c => c.LOT)
                        .Where(c => c.FABCODE.Equals(item.FABCODE) && c.COUNTID.Equals(item.COUNTID) &&
                                    c.YARNFOR.Equals(item.YARN_FOR))
                        .Select(c => c.Color
                            .COLOR)
                        .FirstOrDefaultAsync();

                    item.BasYarnCountInfo.RND_COUNTNAME = color!=null ? item.BasYarnCountInfo.RND_COUNTNAME + ' ' + Type + '(' + color + ')' : item.BasYarnCountInfo.RND_COUNTNAME + ' ' + Type;
                }

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<CosPreCostingMasterViewModel> GetCountList(CosPreCostingMasterViewModel cosPreCostingMasterViewModel)
        {
            try
            {
                var result = await DenimDbContext.RND_FABRIC_COUNTINFO.Include(c => c.COUNT)
                    .Include(c=>c.LOT)
                    .Include(c => c.FABCODENavigation)
                    .Where(c => c.FABCODE == cosPreCostingMasterViewModel.CosPreCostingMaster.FABCODE)
                    .OrderBy(c => c.YARNFOR)
                    .ThenByDescending(c=>c.RATIO)
                    .Select(c => new COS_PRECOSTING_DETAILS
                    {
                        TRNSDATE = DateTime.Now,
                        FABCODE = cosPreCostingMasterViewModel.CosPreCostingMaster.FABCODE,
                        COUNTID = c.COUNTID,
                        YARN_FOR = c.YARNFOR,
                        REMARKS = c.FABCODENavigation.STYLE_NAME.Contains("-CHK-") ? c.COLORCODE.ToString() : null,
                        YPB = "",
                        //COUNTNAME = c.COUNT.COUNTNAME,
                        FABCODENavigation = c.FABCODENavigation,
                        Lot = c.LOT.LOTNO,
                    }).ToListAsync();

                foreach (var item in result)
                {
                    var consumpDetails = await DenimDbContext.RND_YARNCONSUMPTION.Where(c =>
                        c.FABCODE == item.FABCODE && c.COUNTID == item.COUNTID && c.YARNFOR == item.YARN_FOR).FirstOrDefaultAsync();
                    item.RND_CONSUMP = consumpDetails.AMOUNT;
                    var westage = await DenimDbContext.COS_WASTAGE_PERCENTAGE.ToListAsync();

                   // var warpHardWastage = (100-westage.Where(c => c.DESCRIPTION.Equals("Warp Hard Wastage %")).Select(c => c.VALUE).FirstOrDefault())/100;
                    //var weftHardWastage = (100-westage.Where(c => c.DESCRIPTION.Equals("Weft Hard Wastage %")).Select(c => c.VALUE).FirstOrDefault())/100;
                    //var loomCrimp = (100-westage.Where(c => c.DESCRIPTION.Equals("Loom Crimp")).Select(c => c.VALUE).FirstOrDefault())/100;
                    //var rejections = (100-westage.Where(c => c.DESCRIPTION.Equals("Rejection %")).Select(c => c.VALUE).FirstOrDefault())/100;

                    if (item.FABCODENavigation != null)
                    {
                        var crimp = (100 - item.FABCODENavigation.CRIMP_PERCENTAGE??12) / 100;

                        item.COS_CONSUMP = consumpDetails.AMOUNT;
                        switch (item.YARN_FOR)
                        {
                            case 1:
                                item.COS_CONSUMP *= 0.97; //Rejection
                                //item.COS_CONSUMP *= 0.97; //Hard Wastage
                                //item.COS_CONSUMP *= crimp; //crimp%
                                
                                item.COS_CONSUMP /= 0.95;
                                item.COS_CONSUMP /= 0.97;
                                //item.COS_CONSUMP /= loomCrimp;
                                item.COS_CONSUMP = Math.Round(item.COS_CONSUMP??0, 3);
                                break;
                            case 2:
                                item.COS_CONSUMP *= 0.97; //Rejection
                                //item.COS_CONSUMP *= 0.97; //Hard Wastage

                                item.COS_CONSUMP /= 0.95;
                                item.COS_CONSUMP /= 0.97;
                                item.COS_CONSUMP = Math.Round(item.COS_CONSUMP ?? 0, 3);
                                break;
                        }
                    }
                }

                cosPreCostingMasterViewModel.CosPreCostingDetailsList = result.ToList();

                return cosPreCostingMasterViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
