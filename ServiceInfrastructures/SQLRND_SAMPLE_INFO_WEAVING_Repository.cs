using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels;
using DenimERP.ViewModels.Factory.Dyeing;
using DenimERP.ViewModels.Rnd;
using DenimERP.ViewModels.Rnd.Weaving;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLRND_SAMPLE_INFO_WEAVING_Repository : BaseRepository<RND_SAMPLE_INFO_WEAVING>, IRND_SAMPLE_INFO_WEAVING
    {
        private readonly IDataProtector _protector;

        public SQLRND_SAMPLE_INFO_WEAVING_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<RndSampleInfoWeavingViewModel> GetInitObjects(RndSampleInfoWeavingViewModel createRndSampleInfoWeavingViewModel)
        {
            try
            {
                createRndSampleInfoWeavingViewModel.BasYarnCountinfos = await DenimDbContext.BAS_YARN_COUNTINFO
                    .Select(e => new BAS_YARN_COUNTINFO
                    {
                        COUNTID = e.COUNTID,
                        COUNTNAME = e.RND_COUNTNAME
                    })
                    .Where(c => c.YARN_CAT_ID != 8102699)
                    .OrderBy(e => e.COUNTNAME).ToListAsync();
                createRndSampleInfoWeavingViewModel.BasColors = await DenimDbContext.BAS_COLOR.Select(e => new BAS_COLOR { COLORCODE = e.COLORCODE, COLOR = e.COLOR }).OrderBy(e => e.COLOR).ToListAsync();
                createRndSampleInfoWeavingViewModel.BasYarnLotinfos = await DenimDbContext.BAS_YARN_LOTINFO.Select(e => new BAS_YARN_LOTINFO { LOTID = e.LOTID, LOTNO = e.LOTNO }).OrderBy(e => e.LOTNO).ToListAsync();
                createRndSampleInfoWeavingViewModel.BasSupplierinfos = await DenimDbContext.BAS_SUPPLIERINFO.Select(e => new BAS_SUPPLIERINFO { SUPPID = e.SUPPID, SUPPNAME = e.SUPPNAME }).OrderBy(e => e.SUPPNAME).ToListAsync();
                createRndSampleInfoWeavingViewModel.Yarnfors = await DenimDbContext.YARNFOR.Select(e => new YARNFOR { YARNID = e.YARNID, YARNNAME = e.YARNNAME }).Where(e => e.YARNNAME.ToLower().Equals("weft")).OrderBy(e => e.YARNNAME).ToListAsync();
                createRndSampleInfoWeavingViewModel.RndSampleInfoDyeings = await DenimDbContext.RND_SAMPLE_INFO_DYEING
                    //.Where(e => !_denimDbContext.RND_SAMPLE_INFO_WEAVING.Any(f => f.SDID.Equals(e.SDID)))
                    .Select(e => new RND_SAMPLE_INFO_DYEING { SDID = e.SDID, PROG_NO = e.PROG_NO })
                    .OrderBy(e => e.PROG_NO)
                    .ToListAsync();
                createRndSampleInfoWeavingViewModel.PlSampleProgSetups = await DenimDbContext.PL_SAMPLE_PROG_SETUP
                    .Include(c=>c.SD)
                    .Select(e => new PL_SAMPLE_PROG_SETUP
                    {
                        TRNSID = e.TRNSID,
                        PROG_NO = $"{e.PROG_NO} - {e.SD.DYEINGCODE}"
                    }).ToListAsync();
                createRndSampleInfoWeavingViewModel.RndWeaves = await DenimDbContext.RND_WEAVE.Select(e => new RND_WEAVE
                {
                    WID = e.WID,
                    NAME = e.NAME
                }).OrderBy(e => e.NAME).ToListAsync();

                createRndSampleInfoWeavingViewModel.LoomTypes = await DenimDbContext.LOOM_TYPE.Select(e => new LOOM_TYPE
                {
                    LOOMID = e.LOOMID,
                    LOOM_TYPE_NAME = e.LOOM_TYPE_NAME
                }).OrderBy(e => e.LOOM_TYPE_NAME).ToListAsync();

                createRndSampleInfoWeavingViewModel.MktTeams = await DenimDbContext.MKT_TEAM.Select(e => new MKT_TEAM
                {
                    MKT_TEAMID = e.MKT_TEAMID,
                     PERSON_NAME = e.PERSON_NAME
                }).OrderBy(e => e.PERSON_NAME).ToListAsync();

                createRndSampleInfoWeavingViewModel.BuyerInfos = await DenimDbContext.BAS_BUYERINFO.Select(e => new BAS_BUYERINFO
                {
                    BUYERID = e.BUYERID,
                    BUYER_NAME = e.BUYER_NAME
                }).OrderBy(e => e.BUYER_NAME).ToListAsync();


                var FHrEmployees = await DenimDbContext.F_HR_EMP_OFFICIALINFO
                    .Include(c => c.EMP)
                    .Where(c => c.SECID.Equals(171) && !c.OPN2.Equals("Y"))
                    .ToListAsync();

                createRndSampleInfoWeavingViewModel.RndConcerns = FHrEmployees.Select(c => new F_HRD_EMPLOYEE
                {
                    EMPID = c.EMP.EMPID,
                    FIRST_NAME = c.EMP.FIRST_NAME + " " + c.EMP.LAST_NAME + '-' + c.EMP.EMPNO
                }).ToList();

                createRndSampleInfoWeavingViewModel.PlProductionSetDistributions = await DenimDbContext.PL_PRODUCTION_SETDISTRIBUTION
                    .Include(c=>c.PROG_)
                    .Select(e => new PL_PRODUCTION_SETDISTRIBUTION
                { 
                    SETID = e.SETID,
                    PROG_ = new PL_BULK_PROG_SETUP_D
                    {
                        PROG_NO = e.PROG_.PROG_NO
                    }
                }).ToListAsync();

                return createRndSampleInfoWeavingViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<RndSampleInfoWeavingViewModel> GetInitObjectsWithDetails(RndSampleInfoWeavingViewModel createRndSampleInfoWeavingViewModel)
        {
            foreach (var item in createRndSampleInfoWeavingViewModel.RndSampleInfoWeavingDetailses)
            {
                item.COLORCODENavigation = await DenimDbContext.BAS_COLOR.FindAsync(item.COLORCODE);
                item.COUNT = await DenimDbContext.BAS_YARN_COUNTINFO.FindAsync(item.COUNTID);
                item.LOT = await DenimDbContext.BAS_YARN_LOTINFO.FindAsync(item.LOTID);
                item.SUPP = await DenimDbContext.BAS_SUPPLIERINFO.FindAsync(item.SUPPID);
                item.YARN = await DenimDbContext.YARNFOR.FindAsync(item.YARNID);
            }

            return createRndSampleInfoWeavingViewModel;
        }

        public async Task<DataTableObject<RND_SAMPLE_INFO_WEAVING>> GetForDataTableByAsync(string sortColumn, string sortColumnDirection, string searchValue, string draw, int skip, int pageSize)
        {
            var rndSampleInfoWeavings = DenimDbContext.RND_SAMPLE_INFO_WEAVING
                .Select(e => new RND_SAMPLE_INFO_WEAVING
                {
                    WVID = e.WVID,
                    EncryptedId = _protector.Protect(e.WVID.ToString()),
                    FABCODE = e.FABCODE,
                    TOTAL_ENDS = e.TOTAL_ENDS,
                    REED_DENT = e.REED_DENT,
                    GR_EPI = e.GR_EPI,
                    GR_PPI = e.GR_PPI,
                    REMARKS = e.REMARKS
                });

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                switch (sortColumnDirection)
                {
                    case "asc":

                        rndSampleInfoWeavings = rndSampleInfoWeavings.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty).GetValue(c));
                        break;
                    default:
                        rndSampleInfoWeavings = rndSampleInfoWeavings.OrderByDescending(c => c.GetType().GetProperty(sortColumn ?? string.Empty).GetValue(c));
                        break;
                }
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                rndSampleInfoWeavings = rndSampleInfoWeavings
                    .Where(m => m.FABCODE.ToString().ToUpper().Contains(searchValue)
                                || m.TOTAL_ENDS.ToString().ToUpper().Contains(searchValue)
                                || m.REED_DENT != null && m.REED_DENT.ToString().ToUpper().Contains(searchValue)
                                || m.GR_EPI != null && m.GR_EPI.ToString().ToUpper().Contains(searchValue)
                                || m.GR_PPI != null && m.GR_PPI.ToString().ToUpper().Contains(searchValue)
                                || m.REMARKS != null && m.REMARKS.ToString().ToUpper().Contains(searchValue)
                    );
            }

            var recordsTotal = await rndSampleInfoWeavings.CountAsync();

            return new DataTableObject<RND_SAMPLE_INFO_WEAVING>(draw, recordsTotal, recordsTotal, await rndSampleInfoWeavings.Skip(skip).Take(pageSize).ToListAsync());
        }

        public async Task<RND_SAMPLE_INFO_DYEING> FindBySdIdAsync(int sdId)
        {
            return await DenimDbContext.RND_SAMPLE_INFO_DYEING
                .Where(e => e.SDID.Equals(sdId))
                .FirstOrDefaultAsync();
        }

        public async Task<CreateRndSampleInfoDyeingAndDetailsViewModel> FindBySdIdWithSetAsync(int sdId)
        {
            try
            {
                var res = await DenimDbContext.PL_PRODUCTION_SETDISTRIBUTION
                    .Include(x=>x.PROG_.BLK_PROG_.RndProductionOrder.RS.RND_SAMPLE_INFO_DETAILS)
                    .Where(d=>d.SETID.Equals(sdId))
                    .Select(x=>new RND_SAMPLE_INFO_DYEING
                    {
                        SDID = x.PROG_.BLK_PROG_.RndProductionOrder.RS.SDID,
                        SIDATE = x.PROG_.BLK_PROG_.RndProductionOrder.RS.SIDATE,
                        PROG_NO = x.PROG_.BLK_PROG_.RndProductionOrder.RS.PROG_NO,
                        SDRFID = x.PROG_.BLK_PROG_.RndProductionOrder.RS.SDRFID ?? 0,
                        DYEINGCODE = x.PROG_.BLK_PROG_.RndProductionOrder.RS.DYEINGCODE,
                        STYLEREF = x.PROG_.BLK_PROG_.RndProductionOrder.RS.STYLEREF,
                        RNDTEAM = x.PROG_.BLK_PROG_.RndProductionOrder.RS.RNDTEAM,
                        RNDPERSON = x.PROG_.BLK_PROG_.RndProductionOrder.RS.RNDPERSON ?? 0,
                        ENDS_ROPE = x.PROG_.BLK_PROG_.RndProductionOrder.RS.ENDS_ROPE,
                        NO_OF_ROPE = x.PROG_.BLK_PROG_.RndProductionOrder.RS.NO_OF_ROPE,
                        TOTAL_ENDS = x.PROG_.BLK_PROG_.RndProductionOrder.RS.TOTAL_ENDS ?? 0,
                        LENGTH_MTR = x.PROG_.BLK_PROG_.RndProductionOrder.RS.LENGTH_MTR ?? 0,
                        DYEING_REF = x.PROG_.BLK_PROG_.RndProductionOrder.RS.DYEING_REF,
                        REED_SPACE = x.PROG_.BLK_PROG_.RndProductionOrder.RS.REED_SPACE,
                        LOOMID = x.PROG_.BLK_PROG_.RndProductionOrder.RS.LOOMID ?? 0,
                        DID = x.PROG_.BLK_PROG_.RndProductionOrder.RS.DID ?? 0,
                        WARP_PROG_DATE = x.PROG_.BLK_PROG_.RndProductionOrder.RS.WARP_PROG_DATE,
                        COMMITED_DEL_DATE = x.PROG_.BLK_PROG_.RndProductionOrder.RS.COMMITED_DEL_DATE,
                        YARN_LOCATION = x.PROG_.BLK_PROG_.RndProductionOrder.RS.YARN_LOCATION,
                        REMARKS = x.PROG_.BLK_PROG_.RndProductionOrder.RS.REMARKS,
                        OPT1 = x.PROG_.BLK_PROG_.RndProductionOrder.RS.OPT1,
                        OPT2 = x.PROG_.BLK_PROG_.RndProductionOrder.RS.OPT2,
                        OPT3 = x.PROG_.BLK_PROG_.RndProductionOrder.RS.OPT3,
                        OPT4 = x.PROG_.BLK_PROG_.RndProductionOrder.RS.OPT4,
                        OPT5 = x.PROG_.BLK_PROG_.RndProductionOrder.RS.OPT5,
                        USERID = x.PROG_.BLK_PROG_.RndProductionOrder.RS.USERID,
                        RND_SAMPLE_INFO_DETAILS = x.PROG_.BLK_PROG_.RndProductionOrder.RS.RND_SAMPLE_INFO_DETAILS
                    }).FirstOrDefaultAsync();


                //var result = await _denimDbContext.PL_SAMPLE_PROG_SETUP
                //    .Include(c=>c.SD.RND_SAMPLE_INFO_DETAILS)
                //    .ThenInclude(c=>c.COLORCODENavigation)
                //    .Include(c=>c.SD.RND_SAMPLE_INFO_DETAILS)
                //    .ThenInclude(c=>c.COUNT)
                //    .Include(c=>c.SD.RND_SAMPLE_INFO_DETAILS)
                //    .ThenInclude(c=>c.LOT)
                //    .Include(c=>c.SD.RND_SAMPLE_INFO_DETAILS)
                //    .ThenInclude(c=>c.SUPP)
                //    .Include(c=>c.SD.RND_SAMPLE_INFO_DETAILS)
                //    .ThenInclude(c=>c.YARN)
                //    .Where(e => e.TRNSID.Equals(sdId))
                //    .Select(c=>new RND_SAMPLE_INFO_DYEING
                //    {
                //        SDID = c.SD.SDID,
                //        SIDATE = c.SD.SIDATE,
                //        PROG_NO = c.SD.PROG_NO,
                //        SDRFID = c.SD.SDRFID,
                //        DYEINGCODE = c.SD.DYEINGCODE,
                //        STYLEREF = c.SD.STYLEREF,
                //        RNDTEAM = c.SD.RNDTEAM,
                //        RNDPERSON = c.SD.RNDPERSON,
                //        ENDS_ROPE = c.SD.ENDS_ROPE,
                //        NO_OF_ROPE = c.SD.NO_OF_ROPE,
                //        TOTAL_ENDS = c.SD.TOTAL_ENDS,
                //        LENGTH_MTR = c.SD.LENGTH_MTR,
                //        DYEING_REF = c.SD.DYEING_REF,
                //        REED_SPACE = c.SD.REED_SPACE,
                //        LOOMID = c.SD.LOOMID,
                //        DID = c.SD.DID,
                //        WARP_PROG_DATE = c.SD.WARP_PROG_DATE,
                //        COMMITED_DEL_DATE = c.SD.COMMITED_DEL_DATE,
                //        YARN_LOCATION = c.SD.YARN_LOCATION,
                //        REMARKS = c.SD.REMARKS,
                //        OPT1 = c.SD.OPT1,
                //        OPT2 = c.SD.OPT2,
                //        OPT3 = c.SD.OPT3,
                //        OPT4 = c.SD.OPT4,
                //        OPT5 = c.SD.OPT5,
                //        USERID = c.SD.USERID,
                //        RND_SAMPLE_INFO_DETAILS = c.SD.RND_SAMPLE_INFO_DETAILS
                //    })
                //    .FirstOrDefaultAsync();

                var createRndSampleInfoDyeingAndDetailsViewModel = new CreateRndSampleInfoDyeingAndDetailsViewModel();

                createRndSampleInfoDyeingAndDetailsViewModel.RndSampleInfoDyeing = res;
                createRndSampleInfoDyeingAndDetailsViewModel.RndSampleInfoDetailses =
                    res.RND_SAMPLE_INFO_DETAILS.ToList();

                return createRndSampleInfoDyeingAndDetailsViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<RndSampleInfoWeavingWithDetailsViewModel> FindByWvIdAsync(int wvId)
        {
            try
            {
                return await DenimDbContext.RND_SAMPLE_INFO_WEAVING.GroupJoin(DenimDbContext.RND_SAMPLE_INFO_WEAVING_DETAILS
                            .Include(e => e.COLORCODENavigation)
                            .Include(e => e.COUNT)
                            .Include(e => e.LOT)
                            .Include(e => e.SUPP)
                            .Include(e => e.YARN),
                        f1 => f1.WVID,
                        f2 => f2.WVID_PARENT,
                        (f1, f2) => new RndSampleInfoWeavingWithDetailsViewModel
                        {
                            RndSampleInfoWeaving = f1,
                            RndSampleInfoWeavingDetailses = f2.ToList()
                        })
                    .Where(e => e.RndSampleInfoWeaving.WVID.Equals(wvId)).FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<RND_SAMPLE_INFO_WEAVING_DETAILS>> GetRndSampleInfoWeavingDetailsesBySdIdAsync(int sdId)
        {
            return await DenimDbContext.RND_SAMPLE_INFO_WEAVING_DETAILS.Where(e => e.SDID.Equals(sdId)).ToListAsync();
        }

        public async Task<int> GetParentWvIdInsertByAsync(RND_SAMPLE_INFO_WEAVING rndSampleInfoWeaving)
        {
            var entityEntry = await DenimDbContext.Set<RND_SAMPLE_INFO_WEAVING>().AddAsync(rndSampleInfoWeaving);
            await SaveChanges();
            return entityEntry.Entity.WVID;
        }

        public async Task<PL_PRODUCTION_SETDISTRIBUTION> FindSetWithSetIdAsync(
            int id)
        {
            try
            {
                var plProductionSetDistribution = await DenimDbContext.PL_PRODUCTION_SETDISTRIBUTION
                    .Include(c => c.PROG_)
                    .Include(c => c.F_PR_SIZING_PROCESS_ROPE_MASTER)
                    .ThenInclude(c=>c.F_PR_SIZING_PROCESS_ROPE_DETAILS)
                    .ThenInclude(c=>c.W_BEAM)
                    .Include(c => c.F_PR_SLASHER_DYEING_MASTER)
                    .ThenInclude(c=>c.F_PR_SLASHER_DYEING_DETAILS)
                    .ThenInclude(c=>c.W_BEAM)
                    .Where(c => c.SETID.Equals(id))
                    .FirstOrDefaultAsync();

                return plProductionSetDistribution;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
