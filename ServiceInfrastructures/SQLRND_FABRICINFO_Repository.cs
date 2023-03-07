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
using DenimERP.ViewModels.Rnd;
using DenimERP.ViewModels.Rnd.Fabric;
using DenimERP.ViewModels.Rnd.Finish;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLRND_FABRICINFO_Repository : BaseRepository<RND_FABRICINFO>, IRND_FABRICINFO
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDataProtector _protector;

        public SQLRND_FABRICINFO_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            IHttpContextAccessor httpContextAccessor
        )
            : base(denimDbContext)
        {
            _httpContextAccessor = httpContextAccessor;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<IEnumerable<RndFabricCountinfoAndRndYarnConsumptionViewModel>> GetAllRndFabricAndYarnConsumptionList(int fabCode)
        {
            try
            {
                var result = await DenimDbContext.RND_FABRIC_COUNTINFO
                    .Include(e => e.COUNT)
                    .Include(e => e.LOT)
                    .Include(e => e.SUPP)
                    .Include(e => e.Color)
                    .Include(c => c.FABCODENavigation)
                    .ThenInclude(c => c.RND_YARNCONSUMPTION)
                    .Select(e =>
                        new RndFabricCountinfoAndRndYarnConsumptionViewModel()
                        {
                            rND_FABRIC_COUNTINFO = new RND_FABRIC_COUNTINFO
                            {
                                TRNSID = e.TRNSID,
                                EncryptedId = _protector.Protect(e.TRNSID.ToString()),
                                FABCODE = e.FABCODE,
                                COUNTID = e.COUNTID,
                                YARNTYPE = e.YARNTYPE,
                                DESCRIPTION = e.DESCRIPTION,
                                LOTID = e.LOTID,
                                SUPPID = e.SUPPID,
                                RATIO = e.RATIO,
                                NE = e.NE,
                                YARNFOR = e.YARNFOR,
                                COUNT = new BAS_YARN_COUNTINFO
                                {
                                    COUNTID = e.COUNT.COUNTID,
                                    COUNTNAME = e.COUNT.COUNTNAME
                                },
                                LOT = new BAS_YARN_LOTINFO
                                {
                                    LOTID = e.LOT.LOTID,
                                    LOTNO = e.LOT.LOTNO
                                },
                                SUPP = new BAS_SUPPLIERINFO
                                {
                                    SUPPID = e.SUPP.SUPPID,
                                    SUPPNAME = e.SUPP.SUPPNAME
                                },
                                Color = new BAS_COLOR
                                {
                                    COLORCODE = e.Color.COLORCODE,
                                    COLOR = e.Color.COLOR
                                }
                            },
                            rND_YARNCONSUMPTION = new RND_YARNCONSUMPTION
                            {
                                TRNSID = e.FABCODENavigation.RND_YARNCONSUMPTION.Where(f => f.COUNTID.Equals(e.COUNTID) && f.FABCODE.Equals(e.FABCODE) && f.YARNFOR.Equals(e.YARNFOR)).Select(f => f.TRNSID).FirstOrDefault(),
                                EncryptedId = _protector.Protect(e.FABCODENavigation.RND_YARNCONSUMPTION.Where(f => f.COUNTID.Equals(e.COUNTID) && f.FABCODE.Equals(e.FABCODE) && f.YARNFOR.Equals(e.YARNFOR)).Select(f => f.TRNSID).FirstOrDefault().ToString()),
                                FABCODE = e.FABCODENavigation.RND_YARNCONSUMPTION.Where(f => f.COUNTID.Equals(e.COUNTID) && f.FABCODE.Equals(e.FABCODE) && f.YARNFOR.Equals(e.YARNFOR)).Select(f => f.FABCODE).FirstOrDefault(),
                                COUNTID = e.FABCODENavigation.RND_YARNCONSUMPTION.Where(f => f.COUNTID.Equals(e.COUNTID) && f.FABCODE.Equals(e.FABCODE) && f.YARNFOR.Equals(e.YARNFOR)).Select(f => f.COUNTID).FirstOrDefault(),
                                YARNFOR = e.FABCODENavigation.RND_YARNCONSUMPTION.Where(f => f.COUNTID.Equals(e.COUNTID) && f.FABCODE.Equals(e.FABCODE) && f.YARNFOR.Equals(e.YARNFOR)).Select(f => f.YARNFOR).FirstOrDefault(),
                                AMOUNT = e.FABCODENavigation.RND_YARNCONSUMPTION.Where(f => f.COUNTID.Equals(e.COUNTID) && f.FABCODE.Equals(e.FABCODE) && f.YARNFOR.Equals(e.YARNFOR)).Select(f => f.AMOUNT).FirstOrDefault()
                                //,
                                //COUNT = new BAS_YARN_COUNTINFO
                                //{
                                //    COUNTID = e.COUNT.COUNTID,
                                //    COUNTNAME = e.COUNT.COUNTNAME
                                //}
                            },
                            RndYarnconsumptions = e.FABCODENavigation.RND_YARNCONSUMPTION.ToList()
                        }
                        )
                    .Where(e => e.rND_FABRIC_COUNTINFO.FABCODE.Equals(fabCode) && e.rND_YARNCONSUMPTION.FABCODE.Equals(fabCode))
                    .ToListAsync();

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<RND_FABRICINFO> FindByFabCodeIAsync(int fabCode, bool create = false, bool update = false, bool delete = false, bool details = false)
        {
            try
            {
                if (details)
                {
                    var result = await DenimDbContext.RND_FABRICINFO
                        .Include(c=>c.RND_FABRIC_COUNTINFO)
                        .Include(c=>c.RND_YARNCONSUMPTION)
                        .Include(rfi => rfi.BUYER)
                        .Include(rfi => rfi.COLORCODENavigation)
                        .Include(rfi => rfi.D)
                        .Include(c => c.RND_FINISHTYPE)
                        .Include(c => c.RND_WEAVE)
                        .Include(c => c.RND_FINISHMC)
                        .Include(c => c.WV)
                        .AsNoTracking()
                        .Where(rfi => rfi.FABCODE.Equals(fabCode))
                        .FirstOrDefaultAsync();
                    return result;
                }
                else
                {
                    var result = await DenimDbContext.RND_FABRICINFO
                        .Include(c => c.RND_FABRIC_COUNTINFO)
                        .Include(c => c.RND_YARNCONSUMPTION)
                        .Include(rfi => rfi.BUYER)
                        .Include(rfi => rfi.COLORCODENavigation)
                        .Include(rfi => rfi.D)
                        .Include(c => c.RND_FINISHTYPE)
                        .Include(c => c.RND_WEAVE)
                        .Include(c => c.RND_FINISHMC)
                        .Include(c => c.WV)
                        .AsNoTracking()
                        .Where(c => c.FABCODE.Equals(fabCode) && !DenimDbContext.RND_FABRICINFO_APPROVAL_DETAILS.Any(d => d.FABCODE.Equals(c.FABCODE)))
                        .FirstOrDefaultAsync();
                    return result;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<bool> FindByRndFabricInfoFabCodeByAsync(int fabCode)
        {
            var isExistFabCode = await DenimDbContext.RND_FABRICINFO.AnyAsync(fi => fi.FABCODE.Equals(fabCode));
            return isExistFabCode;
        }

        //public async Task<DataTableObject<RND_FABRICINFO>> GetForDataTableByAsync(string sortColumn, string sortColumnDirection, string searchValue, string draw, int skip, int pageSize)
        public async Task<List<RND_FABRICINFO>> GetForDataTableByAsync()
        {
            try
            {
                //var navigationPropertyStrings = new[] { "COLORCODENavigation", "BUYER", "D", "LOOM", "WV" };
                var rndFabricInfoListAllAsync = await DenimDbContext.RND_FABRICINFO
                    .Include(e => e.BUYER)
                    .Include(e => e.COLORCODENavigation)
                    .Include(e => e.D)
                    .Include(e => e.LOOM)
                    //.Include(e => e.WV)
                    .Where(c => c.IS_DELETE.Equals(false))
                    .Select(e => new RND_FABRICINFO
                    {
                        FABCODE = e.FABCODE,
                        STYLE_NAME = e.STYLE_NAME,
                        EncryptedId = _protector.Protect(e.FABCODE.ToString()),
                        DEVID = e.DEVID,
                        PROGNO = e.PROGNO,
                        D = e.D,
                        REED_COUNT = e.REED_COUNT,
                        REED_SPACE = e.REED_SPACE,
                        CRIMP_PERCENTAGE = e.CRIMP_PERCENTAGE,
                        COLORCODENavigation = e.COLORCODENavigation,
                        LOOM = new LOOM_TYPE
                        {
                            LOOM_TYPE_NAME = e.LOOM != null ? e.LOOM.LOOM_TYPE_NAME : ""
                        },
                        BUYER = e.BUYER,
                        WV = new RND_SAMPLE_INFO_WEAVING
                        {
                            WVID = e.WV != null ? e.WV.WVID : 0,
                            FABCODE = e.WV != null ? e.WV.FABCODE : ""
                        },
                        APPROVED = e.APPROVED
                    }).ToListAsync();
                return rndFabricInfoListAllAsync;
                //if (_httpContextAccessor.HttpContext.User.IsInRole("Marketing(DGM)") || _httpContextAccessor.HttpContext.User.IsInRole("Marketing"))
                //{
                //    rndFabricInfoListAllAsync = rndFabricInfoListAllAsync.Where(c => c.APPROVED);
                //}

                //if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                //{
                //    rndFabricInfoListAllAsync = OrderedResult<RND_FABRICINFO>.GetOrderedResult(sortColumnDirection, sortColumn, navigationPropertyStrings, rndFabricInfoListAllAsync);
                //}

                //if (!string.IsNullOrEmpty(searchValue))
                //{
                //    rndFabricInfoListAllAsync = rndFabricInfoListAllAsync.Where(m => m.FABCODE.ToString().ToUpper().Contains(searchValue)
                //        || m.DEVID != null && m.DEVID.Contains(searchValue)
                //        || m.PROGNO != null && m.PROGNO.ToUpper().Contains(searchValue)
                //        || m.D.DTYPE != null && m.D.DTYPE.ToUpper().Contains(searchValue)
                //        || m.COLORCODENavigation.COLOR != null && m.COLORCODENavigation.COLOR.ToUpper().Contains(searchValue)
                //        || m.LOOM.LOOM_TYPE_NAME != null && m.LOOM.LOOM_TYPE_NAME.ToUpper().Contains(searchValue)
                //        || m.BUYER.BUYER_NAME != null && m.BUYER.BUYER_NAME.ToUpper().Contains(searchValue)
                //        || m.STYLE_NAME != null && m.STYLE_NAME.ToUpper().Contains(searchValue));

                //    rndFabricInfoListAllAsync = OrderedResult<RND_FABRICINFO>.GetOrderedResult(sortColumnDirection, sortColumn, navigationPropertyStrings, rndFabricInfoListAllAsync);
                //}

                //var recordsTotal = await rndFabricInfoListAllAsync.CountAsync();
                ////rndFabricInfoListAllAsync = rndFabricInfoListAllAsync.ToList();
                //return new DataTableObject<RND_FABRICINFO>(draw, recordsTotal, recordsTotal, rndFabricInfoListAllAsync.Skip(skip).Take(pageSize).ToList());

            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return null;
            }
        }

        public async Task<int> TotalNumberOfFabricInfoExcludingAllByAsync()
        {
            return await DenimDbContext.RND_FABRICINFO.CountAsync();
        }

        public async Task<RndFabricInfoViewModel> GetInitObjects(RndFabricInfoViewModel rndFabricInfoViewModel)
        {
            rndFabricInfoViewModel.rND_DYEING_TYPEs = await DenimDbContext.RND_DYEING_TYPE.Select(e => new RND_DYEING_TYPE
            {
                DID = e.DID,
                DTYPE = e.DTYPE
            }).OrderBy(e => e.DTYPE).ToListAsync();

            rndFabricInfoViewModel.bAS_YARN_COUNTINFOs = await DenimDbContext.BAS_YARN_COUNTINFO
                .Include(c => c.YARN_CAT_)
                .Select(e => new BAS_YARN_COUNTINFO
                {
                    COUNTID = e.COUNTID,
                    COUNTNAME = e.COUNTNAME,
                    YARN_CAT_ID = e.YARN_CAT_ID
                })
                .Where(c => !c.YARN_CAT_ID.Equals(8102699))
                .OrderBy(e => e.COUNTNAME)
                .ToListAsync();

            rndFabricInfoViewModel.bAS_YARN_LOTINFOs = await DenimDbContext.BAS_YARN_LOTINFO.Select(e => new BAS_YARN_LOTINFO
            {
                LOTID = e.LOTID,
                LOTNO = e.LOTNO
            }).OrderBy(e => e.LOTNO).ToListAsync();

            rndFabricInfoViewModel.bAS_SUPPLIERINFOs = await DenimDbContext.BAS_SUPPLIERINFO.Select(e => new BAS_SUPPLIERINFO
            {
                SUPPID = e.SUPPID,
                SUPPNAME = e.SUPPNAME
            }).OrderBy(e => e.SUPPNAME).ToListAsync();

            rndFabricInfoViewModel.BasColors = await DenimDbContext.BAS_COLOR.Select(e => new BAS_COLOR
            {
                COLORCODE = e.COLORCODE,
                COLOR = e.COLOR
            }).OrderBy(e => e.COLOR).ToListAsync();

            rndFabricInfoViewModel.bAS_BUYERINFOs = await DenimDbContext.BAS_BUYERINFO.Select(e => new BAS_BUYERINFO
            {
                BUYERID = e.BUYERID,
                BUYER_NAME = e.BUYER_NAME
            }).OrderBy(e => e.BUYER_NAME).ToListAsync();

            rndFabricInfoViewModel.RndFinishtypes = await DenimDbContext.RND_FINISHTYPE.Select(e => new RND_FINISHTYPE
            {
                FINID = e.FINID,
                TYPENAME = e.TYPENAME
            }).OrderBy(e => e.TYPENAME).ToListAsync();

            rndFabricInfoViewModel.RndWeaves = await DenimDbContext.RND_WEAVE.Select(e => new RND_WEAVE
            {
                WID = e.WID,
                NAME = e.NAME
            }).OrderBy(e => e.NAME).ToListAsync();

            rndFabricInfoViewModel.RndFinishmcs = await DenimDbContext.RND_FINISHMC.Select(e => new RND_FINISHMC
            {
                MCID = e.MCID,
                NAME = e.NAME
            }).OrderBy(e => e.NAME).ToListAsync();

            rndFabricInfoViewModel.RndSampleInfoWeavings = await DenimDbContext.RND_SAMPLE_INFO_WEAVING
                .Where(e => !DenimDbContext.RND_FABRICINFO.Any(g => g.WVID.Equals(e.WVID)))
                .Select(e => new RND_SAMPLE_INFO_WEAVING
                {
                    WVID = e.WVID,
                    FABCODE = e.FABCODE
                }).OrderBy(e => e.FABCODE).ToListAsync();

            rndFabricInfoViewModel.RndSampleInfoFinishings = await DenimDbContext.RND_SAMPLEINFO_FINISHING
                .Include(c => c.LTG.PROG)
                .Where(e => !e.LTG.PROG.PROG_NO.Equals("1006/0000") && !DenimDbContext.RND_FABRICINFO.Any(g => g.SFINID.Equals(e.SFINID)))
                .Select(e => new RND_SAMPLEINFO_FINISHING
                {
                    SFINID = e.SFINID,
                    STYLE_NAME = e.STYLE_NAME
                })
                .OrderBy(e => e.STYLE_NAME)
                .ToListAsync();

            rndFabricInfoViewModel.FabTestSamples = await DenimDbContext.RND_FABTEST_SAMPLE
                .Where(c => c.LABNO != null)
                .Select(c => new RND_FABTEST_SAMPLE
                {
                    LABNO = c.LABNO,
                    LTSID = c.LTSID
                }).ToListAsync();

            rndFabricInfoViewModel.FabTestGreyList = await DenimDbContext.RND_FABTEST_GREY
                .Where(c => c.LAB_NO != null)
                .Select(c => new RND_FABTEST_GREY
                {
                    LAB_NO = c.LAB_NO,
                    LTGID = c.LTGID
                }).ToListAsync();

            rndFabricInfoViewModel.AgeGroupViewModels = await DenimDbContext.AGEGROUP.Select(e => new AgeGroupViewModel
            {
                AgeGroupId = e.ID,
                AgeGroupName = e.AGEGROUPNAME
            }).ToListAsync();

            rndFabricInfoViewModel.TargetGenderViewModels = await DenimDbContext.TARGETGENDER.Select(e => new TargetGenderViewModel
            {
                GenderId = e.Id,
                GenderName = e.GENDERNAME
            }).ToListAsync();

            rndFabricInfoViewModel.TargetCharacterViewModels = await DenimDbContext.TARGETCHARACTER.Select(e => new TargetCharacterViewModel
            {
                CharacterId = e.Id,
                CharacterName = e.CHARACTERNAME
            }).ToListAsync();

            rndFabricInfoViewModel.TargetPriceSegmentViewModels = await DenimDbContext.TARGETPRICESEGMENT.Select(e => new TargetPriceSegmentViewModel
            {
                PriceSegmentId = e.Id,
                PriceSegmentType = e.PRICESEGMENTTYPE
            }).ToListAsync();

            rndFabricInfoViewModel.TargetFitStyleViewModels = await DenimDbContext.TARGETFITSTYLE.Select(e => new TargetFitStyleViewModel
            {
                FitStyleId = e.Id,
                FitStyleName = e.FITSTYLENAME
            }).ToListAsync();

            rndFabricInfoViewModel.SegmentSeasonViewModels = await DenimDbContext.SEGMENTSEASON.Select(e => new SegmentSeasonViewModel
            {
                SeasonId = e.Id,
                SeasonName = e.SEASONNAME
            }).ToListAsync();

            rndFabricInfoViewModel.ComSegmentViewModels = await DenimDbContext.SEGMENTCOMSEGMENT.Select(e => new ComSegmentViewModel
            {
                ComSegmentId = e.Id,
                ComSegmentName = e.COMSEGMENTNAME
            }).ToListAsync();

            rndFabricInfoViewModel.OtherSimilarViewModels = await DenimDbContext.SEGMENTOTHERSIMILARNAME.Select(e => new OtherSimilarViewModel
            {
                OtherSimilarId = e.Id,
                OtherSimilarName = e.OTHERSIMILARNAME
            }).ToListAsync();


            var FHrEmployees = await DenimDbContext.F_HR_EMP_OFFICIALINFO
                .Include(d => d.EMP)
                .Where(d => d.DEPTID.Equals(81) && !d.OPN2.Equals("Y"))  //R&D Dept
                .ToListAsync();

            rndFabricInfoViewModel.FHrEmployeesList = FHrEmployees.Select(d => new F_HRD_EMPLOYEE
            {
                EMPID = d.EMP.EMPID,
                FIRST_NAME = $"{d.EMP.EMPNO} - {d.EMP.FIRST_NAME} {d.EMP.LAST_NAME}"
            }).ToList();

            rndFabricInfoViewModel.LoomTypes = await DenimDbContext.LOOM_TYPE
                .Select(d => new LOOM_TYPE
                {
                    LOOMID = d.LOOMID,
                    LOOM_TYPE_NAME = d.LOOM_TYPE_NAME
                }).ToListAsync();

            return rndFabricInfoViewModel;
        }

        public async Task<ExtendRndSampleInfoWeavingViewModel> GetAssociateObjectsByWvIdAsync(int wvId)
        {
            try
            {
                var result = await DenimDbContext.RND_SAMPLE_INFO_WEAVING
                .Include(e => e.WEAVENavigation)
                .Include(e => e.PROG_)
                .ThenInclude(e => e.SD)
                .GroupJoin(DenimDbContext.RND_SAMPLE_INFO_DYEING.OrderBy(e => e.SDID)
                    .Include(e => e.SDRF),
                f1 => f1.SDID,
                f2 => f2.SDID,
                (f1, f2) => new ExtendRndSampleInfoWeavingViewModel
                {
                    RndSampleInfoWeaving = f1,
                    RndSampleInfoDyeing = f2.FirstOrDefault()
                })
                .GroupJoin(DenimDbContext.RND_ANALYSIS_SHEET.OrderBy(e => e.AID),
                f5 => f5.RndSampleInfoWeaving.WVID,
                f6 => f6.WID,
                (f5, f6) => new ExtendRndSampleInfoWeavingViewModel
                {
                    RndSampleInfoWeaving = f5.RndSampleInfoWeaving,
                    RndSampleInfoDyeing = f5.RndSampleInfoDyeing,
                    RndAnalysisSheet = f6.FirstOrDefault()
                })
                .GroupJoin(DenimDbContext.RND_FABTEST_GREY.OrderBy(e => e.LTGDATE),
                f7 => f7.RndSampleInfoWeaving.SDID,
                f8 => f8.PROG.SDID,
                (f7, f8) => new ExtendRndSampleInfoWeavingViewModel
                {
                    RndSampleInfoWeaving = f7.RndSampleInfoWeaving,
                    RndSampleInfoDyeing = f7.RndSampleInfoDyeing,
                    RndAnalysisSheet = f7.RndAnalysisSheet,
                    RndFabtestGrey = f8.FirstOrDefault()
                })
                .GroupJoin(DenimDbContext.RND_FABTEST_SAMPLE.OrderBy(e => e.LTSDATE),
                f9 => f9.RndSampleInfoWeaving.WVID,
                f10 => f10.SFINID,
                (f9, f10) => new ExtendRndSampleInfoWeavingViewModel
                {
                    RndSampleInfoWeaving = f9.RndSampleInfoWeaving,
                    RndSampleInfoDyeing = f9.RndSampleInfoDyeing,
                    RndAnalysisSheet = f9.RndAnalysisSheet,
                    RndFabtestGrey = f9.RndFabtestGrey,
                    RndFabtestSample = f10.FirstOrDefault()
                })
                .Where(e => e.RndSampleInfoWeaving.WVID.Equals(wvId))
                .Select(e => new ExtendRndSampleInfoWeavingViewModel
                {
                    RndSampleInfoWeaving = new RND_SAMPLE_INFO_WEAVING
                    {
                        WVID = e.RndSampleInfoWeaving.WVID,
                        SDID = e.RndSampleInfoWeaving.SDID,
                        FABCODE = e.RndSampleInfoWeaving.FABCODE,
                        CONCEPT = e.RndSampleInfoWeaving.CONCEPT,
                        BEAMNO = e.RndSampleInfoWeaving.BEAMNO,
                        TOTAL_ENDS = e.RndSampleInfoWeaving.TOTAL_ENDS,
                        REED_DENT = e.RndSampleInfoWeaving.REED_DENT,
                        GR_EPI = e.RndSampleInfoWeaving.GR_EPI,
                        GR_PPI = e.RndSampleInfoWeaving.GR_PPI,
                        FNPPI = e.RndSampleInfoWeaving.FNPPI,
                        WEAVE = e.RndSampleInfoWeaving.WEAVE,
                        LENGTH_MTR = e.RndSampleInfoWeaving.LENGTH_MTR,
                        CLOSE_DATE = e.RndSampleInfoWeaving.CLOSE_DATE,
                        REED_COUNT = e.RndSampleInfoWeaving.REED_COUNT,
                        OPT2 = e.RndSampleInfoWeaving.OPT2,
                        OPT3 = e.RndSampleInfoWeaving.OPT3,
                        OPT4 = e.RndSampleInfoWeaving.OPT4,
                        REMARKS = e.RndSampleInfoWeaving.REMARKS,
                        WEAVENavigation = new RND_WEAVE
                        {
                            WID = e.RndSampleInfoWeaving.WEAVENavigation.WID,
                            NAME = e.RndSampleInfoWeaving.WEAVENavigation.NAME
                        },
                        PROG_ = new PL_SAMPLE_PROG_SETUP
                        {
                            SD = new RND_SAMPLE_INFO_DYEING
                            {
                                REED_SPACE = e.RndSampleInfoWeaving.PROG_.SD.REED_SPACE,
                                DYEINGCODE = e.RndSampleInfoWeaving.PROG_.SD.DYEINGCODE,
                                NO_OF_ROPE = e.RndSampleInfoWeaving.PROG_.SD.NO_OF_ROPE,
                                TOTAL_ENDS = e.RndSampleInfoWeaving.PROG_.SD.TOTAL_ENDS,
                                D = new RND_DYEING_TYPE
                                {
                                    DID = e.RndSampleInfoWeaving.PROG_.SD.D.DID,
                                    DTYPE = e.RndSampleInfoWeaving.PROG_.SD.D.DTYPE
                                },
                                LOOM = new LOOM_TYPE
                                {
                                    LOOMID = e.RndSampleInfoWeaving.PROG_.SD.LOOM.LOOMID,
                                    LOOM_TYPE_NAME = e.RndSampleInfoWeaving.PROG_.SD.LOOM.LOOM_TYPE_NAME
                                },
                                SDRF = e.RndSampleInfoDyeing.SDRF
                            }
                        }
                    },
                    RndSampleInfoDyeing = new RND_SAMPLE_INFO_DYEING
                    {
                        LOOM = new LOOM_TYPE
                        {
                            LOOMID = e.RndSampleInfoDyeing.LOOM.LOOMID,
                            LOOM_TYPE_NAME = e.RndSampleInfoDyeing.LOOM.LOOM_TYPE_NAME
                        }
                    },
                    RndAnalysisSheet = new RND_ANALYSIS_SHEET
                    {
                        FN_EPI_PPI = e.RndAnalysisSheet.FN_EPI_PPI,
                        WASH_EPI_PPI = e.RndAnalysisSheet.WASH_EPI_PPI
                    },
                    FnEpiPpi = new FnEpiPpi
                    {
                        FnEpi = !string.IsNullOrEmpty(e.RndAnalysisSheet.FN_EPI_PPI) ? e.RndAnalysisSheet.FN_EPI_PPI.ToLower().Split(new char[] { 'x' })[0] : string.Empty,
                        Fnpi = !string.IsNullOrEmpty(e.RndAnalysisSheet.FN_EPI_PPI) ? e.RndAnalysisSheet.FN_EPI_PPI.ToLower().Split(new char[] { 'x' })[1] : string.Empty,
                        WashEpi = !string.IsNullOrEmpty(e.RndAnalysisSheet.WASH_EPI_PPI) ? e.RndAnalysisSheet.WASH_EPI_PPI.ToLower().Split(new char[] { 'x' })[0] : string.Empty,
                        WashPpi = !string.IsNullOrEmpty(e.RndAnalysisSheet.WASH_EPI_PPI) ? e.RndAnalysisSheet.WASH_EPI_PPI.ToLower().Split(new char[] { 'x' })[1] : string.Empty,
                    },
                    RndFabtestGrey = new RND_FABTEST_GREY
                    {
                        WGGRBW = e.RndFabtestGrey.WGGRBW,
                        WGGRAW = e.RndFabtestGrey.WGGRAW,
                        WIGRBW = e.RndFabtestGrey.WIGRBW,
                        WIGRAW = e.RndFabtestGrey.WIGRAW,
                        SRGRWRAP = e.RndFabtestGrey.SRGRWRAP,
                        SRGRWEFT = e.RndFabtestGrey.SRGRWEFT
                    },
                    RndFabtestSample = e.RndFabtestSample
                }).FirstOrDefaultAsync();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }


        public async Task<RND_SAMPLEINFO_FINISHING> GetAssociateObjectsBySFinId(int sFinId)
        {
            try
            {
                var result = await DenimDbContext.RND_SAMPLEINFO_FINISHING
                    .Include(c => c.LTG.PROG.RND_SAMPLE_INFO_WEAVING)
                    .ThenInclude(c => c.WEAVENavigation)
                    .Include(c => c.LTG.PROG.RND_SAMPLE_INFO_WEAVING)
                    .ThenInclude(c => c.LOOM)
                    .Include(c => c.LTG.PROG.SD.D)
                    .Include(c => c.LTG.PROG.SD.SDRF.RND_ANALYSIS_SHEET)
                    .Include(c => c.RND_FABTEST_SAMPLE)
                    .Include(c => c.COLOR)
                    .Where(e => e.SFINID.Equals(sFinId) && e.STATUS)
                    .Select(e => new RND_SAMPLEINFO_FINISHING
                    {
                        STYLE_NAME = e.STYLE_NAME,
                        RND_FABTEST_SAMPLE = e.RND_FABTEST_SAMPLE,
                        FINISH_ROUTE = e.FINISH_ROUTE,
                        FINISHDATE = e.FINISHDATE,
                        FNCONST = e.FNCONST,
                        PROCESSED_LENGTH = e.PROCESSED_LENGTH,
                        WASHPICK = e.WASHPICK,
                        COLORCODE = e.COLORCODE,
                        COLOR = new BAS_COLOR
                        {
                            COLOR = e.COLOR.COLOR
                        },
                        LTG = new RND_FABTEST_GREY
                        {
                            WGGRBW = e.LTG.WGGRBW,
                            WGGRAW = e.LTG.WGGRAW,
                            WIGRBW = e.LTG.WIGRBW,
                            WIGRAW = e.LTG.WIGRAW,
                            SRGRWRAP = e.LTG.SRGRWRAP,
                            SRGRWEFT = e.LTG.SRGRWEFT,
                            GREPI = e.LTG.GREPI,
                            GRPPI = e.LTG.GRPPI,
                            LAB_NO = e.LTG.LAB_NO,
                            LTGID = e.LTG.LTGID,
                            PROG = new PL_SAMPLE_PROG_SETUP
                            {
                                SD = new RND_SAMPLE_INFO_DYEING
                                {
                                    REED_SPACE = e.LTG.PROG.SD.REED_SPACE,
                                    DYEINGCODE = e.LTG.PROG.SD.DYEINGCODE,
                                    DYEING_REF = e.LTG.PROG.SD.DYEING_REF,
                                    NO_OF_ROPE = e.LTG.PROG.SD.NO_OF_ROPE,
                                    TOTAL_ENDS = e.LTG.PROG.SD.TOTAL_ENDS,
                                    D = new RND_DYEING_TYPE
                                    {
                                        DID = e.LTG.PROG.SD.D.DID,
                                        DTYPE = e.LTG.PROG.SD.D.DTYPE
                                    }
                                    //,
                                    //SDRF = new MKT_SDRF_INFO
                                    //{
                                    //    SDRFID = e.LTG.PROG.SD.SDRF.SDRFID,
                                    //    SDRF_NO = e.LTG.PROG.SD.SDRF.SDRF_NO,
                                    //    MKT_PERSON_ID = e.LTG.PROG.SD.SDRF.MKT_PERSON_ID,
                                    //    TEAMID = e.LTG.PROG.SD.SDRF.TEAMID,
                                    //    AID = e.LTG.PROG.SD.SDRF.AID,
                                    //    DEV_ID = e.LTG.PROG.SD.SDRF.DEV_ID,
                                    //    BUYERID = e.LTG.PROG.SD.SDRF.BUYERID,
                                    //    BRANDID = e.LTG.PROG.SD.SDRF.BRANDID,
                                    //    PRIORITY = e.LTG.PROG.SD.SDRF.PRIORITY,
                                    //    BUYER_REF = e.LTG.PROG.SD.SDRF.BUYER_REF,
                                    //    FORTYPE = e.LTG.PROG.SD.SDRF.FORTYPE,
                                    //    REWORK_REASON = e.LTG.PROG.SD.SDRF.REWORK_REASON,
                                    //    SEASON = e.LTG.PROG.SD.SDRF.SEASON,
                                    //    CONSTRUCTION = e.LTG.PROG.SD.SDRF.CONSTRUCTION,
                                    //    WIDTH = e.LTG.PROG.SD.SDRF.WIDTH,
                                    //    WEIGHT_BW = e.LTG.PROG.SD.SDRF.WEIGHT_BW,
                                    //    WEIGHT_AW = e.LTG.PROG.SD.SDRF.WEIGHT_AW,
                                    //    GSM_BW = e.LTG.PROG.SD.SDRF.GSM_BW,
                                    //    GSM_AW = e.LTG.PROG.SD.SDRF.GSM_AW,
                                    //    COLOR = e.LTG.PROG.SD.SDRF.COLOR,
                                    //    RND_ANALYSIS_SHEET = e.LTG.PROG.SD.SDRF.RND_ANALYSIS_SHEET
                                    //}
                                },
                                RND_SAMPLE_INFO_WEAVING = e.LTG.PROG.RND_SAMPLE_INFO_WEAVING.Select(g => new RND_SAMPLE_INFO_WEAVING
                                {
                                    WVID = g.WVID,
                                    SDID = g.SDID,
                                    FABCODE = g.FABCODE,
                                    CONCEPT = g.CONCEPT,
                                    BEAMNO = g.BEAMNO,
                                    TOTAL_ENDS = g.TOTAL_ENDS,
                                    REED_DENT = g.REED_DENT,
                                    GR_EPI = g.GR_EPI,
                                    GR_PPI = g.GR_PPI,
                                    FNPPI = g.FNPPI,
                                    WEAVE = g.WEAVE,
                                    LENGTH_MTR = g.LENGTH_MTR,
                                    CLOSE_DATE = g.CLOSE_DATE,
                                    REED_COUNT = g.REED_COUNT,
                                    OPT2 = g.OPT2,
                                    OPT3 = g.OPT3,
                                    OPT4 = g.OPT4,
                                    REMARKS = g.REMARKS,
                                    LOOM = new LOOM_TYPE
                                    {
                                        LOOMID = g.LOOM.LOOMID,
                                        LOOM_TYPE_NAME = g.LOOM.LOOM_TYPE_NAME
                                    },
                                    WEAVENavigation = g.WEAVENavigation
                                }).ToList()
                            }
                        }
                    }).FirstOrDefaultAsync();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }


        public async Task<RND_FABTEST_SAMPLE> GetLabTestResult(int ltsId)
        {
            try
            {
                var result = await DenimDbContext.RND_FABTEST_SAMPLE
                    .Where(e => e.LTSID.Equals(ltsId))
                    .FirstOrDefaultAsync();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }


        //public async Task<DyeingWeavingDetailsListViewModel> GetDyeingWeavingDetailsByWvIdAsync(int wvId)
        //{
        //    try
        //    {

        //        RndFabricInfoViewModel rndFabricInfoViewModel = new RndFabricInfoViewModel();

        //        var result = await _denimDbContext.RND_SAMPLE_INFO_WEAVING
        //            .Include(c => c.RND_SAMPLE_INFO_WEAVING_DETAILS)
        //            .ThenInclude(e => e.COLORCODENavigation)
        //            .Include(c => c.RND_SAMPLE_INFO_WEAVING_DETAILS)
        //            .ThenInclude(e => e.COUNT)
        //            .Include(c => c.RND_SAMPLE_INFO_WEAVING_DETAILS)
        //            .ThenInclude(e => e.LOT)
        //            .Include(c => c.RND_SAMPLE_INFO_WEAVING_DETAILS)
        //            .ThenInclude(e => e.SUPP)
        //            .Include(c => c.RND_SAMPLE_INFO_WEAVING_DETAILS)
        //            .ThenInclude(e => e.YARN)
        //            .Include(c => c.PROG_.SD.RND_SAMPLE_INFO_DETAILS)
        //            .ThenInclude(c=>c.COLORCODENavigation)
        //            .Include(c => c.PROG_.SD.RND_SAMPLE_INFO_DETAILS)
        //            .ThenInclude(c=>c.COUNT)
        //            .Include(c => c.PROG_.SD.RND_SAMPLE_INFO_DETAILS)
        //            .ThenInclude(c=>c.LOT)
        //            .Include(c => c.PROG_.SD.RND_SAMPLE_INFO_DETAILS)
        //            .ThenInclude(c=>c.SUPP)
        //            .Include(c => c.PROG_.SD.RND_SAMPLE_INFO_DETAILS)
        //            .ThenInclude(c=>c.YARN)
        //            .Where(e => e.WVID.Equals(wvId))
        //            //.Select(c=> new DyeingWeavingDetailsListViewModel
        //            //{
        //            //    RndSampleInfoWeaving = new RND_SAMPLE_INFO_WEAVING
        //            //    {
        //            //        WVID = c.WVID,
        //            //        SDID = c.SDID
        //            //    },
        //            //    RndSampleInfoWeavingDetailses = c.RND_SAMPLE_INFO_WEAVING_DETAILS.Select(e=>new RND_SAMPLE_INFO_WEAVING_DETAILS
        //            //    {
        //            //        TYPE = e.TYPE,
        //            //        RATIO = e.RATIO,
        //            //        NE = e.NE,
        //            //        DESCRIPTION = e.DESCRIPTION,
        //            //        BGT = e.BGT,
        //            //        YARNTYPE = e.YARNTYPE,
        //            //        YARNID = e.YARNID,
        //            //        COUNTID = e.COUNTID,
        //            //        COUNT = e.COUNT,
        //            //        LOT = e.LOT,
        //            //        SUPP = e.SUPP,
        //            //        YARN = e.YARN,
        //            //        COLORCODE = e.COLORCODE,
        //            //        COLORCODENavigation = e.COLORCODENavigation
        //            //    }).ToList(),
        //            //    RndDyeingSampleInfoDetailses = c.PROG_.SD.RND_SAMPLE_INFO_DETAILS.Select(e=>new RND_SAMPLE_INFO_DETAILS
        //            //    {
        //            //        TYPE = e.TYPE,
        //            //        RATIO = e.RATIO,
        //            //        NE = e.NE,
        //            //        DESCRIPTION = e.DESCRIPTION,
        //            //        BGT = e.BGT,
        //            //        YARNTYPE = e.YARNTYPE,
        //            //        YARNID = e.YARNID,
        //            //        COUNTID = e.COUNTID,
        //            //        COUNT = e.COUNT,
        //            //        LOT = e.LOT,
        //            //        SUPP = e.SUPP,
        //            //        YARN = e.YARN,
        //            //        COLORCODE = e.COLORCODE,
        //            //        COLORCODENavigation = e.COLORCODENavigation
        //            //    }).ToList()
        //            //})
        //            .FirstOrDefaultAsync();



        //        foreach (var e in result.RND_SAMPLE_INFO_WEAVING_DETAILS)
        //        {

        //            TYPE = e.TYPE,
        //            RATIO = e.RATIO,
        //            NE = e.NE,
        //            DESCRIPTION = e.DESCRIPTION,
        //            BGT = e.BGT,
        //            YARNTYPE = e.YARNTYPE,
        //            YARNID = e.YARNID,
        //            COUNTID = e.COUNTID,
        //            COUNT = e.COUNT,
        //            LOT = e.LOT,
        //            SUPP = e.SUPP,
        //            YARN = e.YARN,
        //            COLORCODE = e.COLORCODE,
        //            COLORCODENavigation = e.COLORCODENavigation
        //        }



        //        //var dyeingWeavingDetailsListViewModels = await _denimDbContext.RND_SAMPLE_INFO_WEAVING
        //        //            .GroupJoin(
        //        //                await _denimDbContext.RND_SAMPLE_INFO_WEAVING_DETAILS
        //        //            .Include(e => e.COLORCODENavigation)
        //        //            .Include(e => e.COUNT)
        //        //            .Include(e => e.LOT)
        //        //            .Include(e => e.SUPP)
        //        //            .Include(e => e.YARN).ToListAsync(),
        //        //        f1 => f1.SDID,
        //        //        f2 => f2.SDID,
        //        //        (f1, f2) => new DyeingWeavingDetailsListViewModel
        //        //        {
        //        //            RndSampleInfoWeaving = new RND_SAMPLE_INFO_WEAVING
        //        //            {
        //        //                WVID = f1.WVID,
        //        //                SDID = f1.SDID
        //        //            },
        //        //            RndSampleInfoWeavingDetailses = f2.ToList()
        //        //        })
        //        //        .GroupJoin(await _denimDbContext.RND_SAMPLE_INFO_DETAILS
        //        //        .Include(e => e.COLORCODENavigation)
        //        //        .Include(e => e.COUNT)
        //        //        .Include(e => e.LOT)
        //        //        .Include(e => e.SUPP)
        //        //        .Include(e => e.YARN)
        //        //        .Select(e => new RND_SAMPLE_INFO_DETAILS
        //        //        {
        //        //            TRNSID = e.TRNSID,
        //        //            SDID = e.SDID,
        //        //            COUNTID = e.COUNTID,
        //        //            TYPE = e.TYPE,
        //        //            DESCRIPTION = e.DESCRIPTION,
        //        //            COLORCODE = e.COLORCODE,
        //        //            LOTID = e.LOTID,
        //        //            SUPPID = e.SUPPID,
        //        //            RATIO = e.RATIO,
        //        //            NE = e.NE,
        //        //            BGT = e.BGT,
        //        //            YARNTYPE = e.YARNTYPE,
        //        //            YARNID = e.YARNID,
        //        //            REMARKS = e.REMARKS,
        //        //            COLORCODENavigation = new BAS_COLOR
        //        //            {
        //        //                COLORCODE = e.COLORCODENavigation.COLORCODE,
        //        //                COLOR = e.COLORCODENavigation.COLOR
        //        //            },
        //        //            COUNT = new BAS_YARN_COUNTINFO
        //        //            {
        //        //                COUNTID = e.COUNT.COUNTID,
        //        //                COUNTNAME = e.COUNT.COUNTNAME
        //        //            },
        //        //            LOT = new BAS_YARN_LOTINFO
        //        //            {
        //        //                LOTID = e.LOT.LOTID,
        //        //                LOTNO = e.LOT.LOTNO
        //        //            },
        //        //            SUPP = new BAS_SUPPLIERINFO
        //        //            {
        //        //                SUPPID = e.SUPP.SUPPID,
        //        //                SUPPNAME = e.SUPP.SUPPNAME
        //        //            },
        //        //            YARN = new YARNFOR
        //        //            {
        //        //                YARNID = e.YARN.YARNID,
        //        //                YARNNAME = e.YARN.YARNNAME
        //        //            }
        //        //        }).ToListAsync(),
        //        //        f3 => f3.RndSampleInfoWeaving.SDID,
        //        //        f4 => f4.SDID,
        //        //        (f3, f4) => new DyeingWeavingDetailsListViewModel
        //        //        {
        //        //            RndSampleInfoWeaving = new RND_SAMPLE_INFO_WEAVING
        //        //            {
        //        //                WVID = f3.RndSampleInfoWeaving.WVID,
        //        //                SDID = f3.RndSampleInfoWeaving.SDID
        //        //            },
        //        //            RndSampleInfoWeavingDetailses = f3.RndSampleInfoWeavingDetailses,
        //        //            RndDyeingSampleInfoDetailses = f4.ToList()
        //        //        })
        //        //        .Where(e => e.RndSampleInfoWeaving.WVID.Equals(wvId))
        //        //        .FirstOrDefaultAsync();

        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.Write(ex);
        //        return null;
        //    }
        //}


        public async Task<RndFabricInfoViewModel> GetDyeingWeavingDetailsByWvIdAsync(RndFabricInfoViewModel rndFabricInfoViewModel)
        {
            try
            {

                var result = await DenimDbContext.RND_SAMPLE_INFO_WEAVING
                    .Include(c => c.RND_SAMPLE_INFO_WEAVING_DETAILS)
                    .ThenInclude(e => e.COLORCODENavigation)
                    .Include(c => c.RND_SAMPLE_INFO_WEAVING_DETAILS)
                    .ThenInclude(e => e.COUNT)
                    .Include(c => c.RND_SAMPLE_INFO_WEAVING_DETAILS)
                    .ThenInclude(e => e.LOT)
                    .Include(c => c.RND_SAMPLE_INFO_WEAVING_DETAILS)
                    .ThenInclude(e => e.SUPP)
                    .Include(c => c.RND_SAMPLE_INFO_WEAVING_DETAILS)
                    .ThenInclude(e => e.YARN)
                    .Include(c => c.PROG_.SD.RND_SAMPLE_INFO_DETAILS)
                    .ThenInclude(c => c.COLORCODENavigation)
                    .Include(c => c.PROG_.SD.RND_SAMPLE_INFO_DETAILS)
                    .ThenInclude(c => c.COUNT)
                    .Include(c => c.PROG_.SD.RND_SAMPLE_INFO_DETAILS)
                    .ThenInclude(c => c.LOT)
                    .Include(c => c.PROG_.SD.RND_SAMPLE_INFO_DETAILS)
                    .ThenInclude(c => c.SUPP)
                    .Include(c => c.PROG_.SD.RND_SAMPLE_INFO_DETAILS)
                    .ThenInclude(c => c.YARN)
                    .Where(e => e.WVID.Equals(rndFabricInfoViewModel.rND_FABRICINFO.WVID))
                    .FirstOrDefaultAsync();



                foreach (var e in result.RND_SAMPLE_INFO_WEAVING_DETAILS)
                {
                    rndFabricInfoViewModel.RndFabricCountInfos.Add(new RND_FABRIC_COUNTINFO
                    {
                        COUNTID = e.COUNTID,
                        YARNTYPE = e.YARNTYPE,
                        DESCRIPTION = e.DESCRIPTION,
                        COLORCODE = e.COLORCODE,
                        LOTID = e.LOTID,
                        SUPPID = e.SUPPID,
                        RATIO = e.RATIO,
                        NE = e.NE,
                        YARNFOR = e.YARNID,

                        COUNT = e.COUNT,
                        YarnFor = e.YARN,
                        Color = e.COLORCODENavigation,
                        LOT = e.LOT,
                        SUPP = e.SUPP,
                    });

                    rndFabricInfoViewModel.RndYarnConsumptions.Add(new RND_YARNCONSUMPTION
                    {
                        COUNTID = e.COUNTID,
                        AMOUNT = e.BGT,
                        YARNFOR = e.YARNID
                    });
                }

                foreach (var e in result.PROG_.SD.RND_SAMPLE_INFO_DETAILS)
                {
                    rndFabricInfoViewModel.RndFabricCountInfos.Add(new RND_FABRIC_COUNTINFO
                    {
                        COUNTID = e.COUNTID,
                        YARNTYPE = e.YARNTYPE,
                        DESCRIPTION = e.DESCRIPTION,
                        COLORCODE = e.COLORCODE,
                        LOTID = e.LOTID,
                        SUPPID = e.SUPPID,
                        RATIO = e.RATIO,
                        NE = e.NE,
                        YARNFOR = e.YARNID,

                        COUNT = e.COUNT,
                        YarnFor = e.YARN,
                        Color = e.COLORCODENavigation,
                        LOT = e.LOT,
                        SUPP = e.SUPP,
                    });

                    rndFabricInfoViewModel.RndYarnConsumptions.Add(new RND_YARNCONSUMPTION
                    {
                        COUNTID = e.COUNTID,
                        AMOUNT = e.BGT,
                        YARNFOR = e.YARNID
                    });
                }
                return rndFabricInfoViewModel;
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                return null;
            }
        }

        public async Task<int> GetWeavingIdBySFinId(int sFinId)
        {
            try
            {

                if (sFinId == 0)
                {
                    return 0;
                }

                var result = await DenimDbContext.RND_SAMPLEINFO_FINISHING
                    .Include(c => c.LTG.PROG.RND_SAMPLE_INFO_WEAVING)
                    .Where(c => c.SFINID.Equals(sFinId))
                    .Select(c => c.LTG.PROG.RND_SAMPLE_INFO_WEAVING.FirstOrDefault().WVID)
                    .FirstOrDefaultAsync();

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<RndFabricInfoViewModel> GetEditInfo(RndFabricInfoViewModel rndFabricInfoViewModel)
        {
            try
            {
                var fabCode = rndFabricInfoViewModel.rND_FABRICINFO.FABCODE;
                //var rndFabricCountinfoAndRndYarnConsumptionViewModels = (List<RndFabricCountinfoAndRndYarnConsumptionViewModel>)await GetAllRndFabricAndYarnConsumptionList(fabCode);

                rndFabricInfoViewModel.RndFabricCountInfos = await DenimDbContext.RND_FABRIC_COUNTINFO
                    .Include(e => e.COUNT)
                    .Include(c => c.Color)
                    .Include(c => c.SUPP)
                    .Include(c => c.LOT)
                    .Include(c => c.YarnFor)
                    .Where(e => e.FABCODE.Equals(fabCode))
                    .Select(e => new RND_FABRIC_COUNTINFO
                    {
                        TRNSID = e.TRNSID,
                        EncryptedId = _protector.Protect(e.TRNSID.ToString()),
                        FABCODE = e.FABCODE,
                        COUNTID = e.COUNTID,
                        YARNTYPE = e.YARNTYPE,
                        DESCRIPTION = e.DESCRIPTION,
                        LOTID = e.LOTID,
                        SUPPID = e.SUPPID,
                        COLORCODE = e.COLORCODE,
                        RATIO = e.RATIO,
                        NE = e.NE,
                        YARNFOR = e.YARNFOR,
                        SUPP = e.SUPP,
                        LOT = e.LOT,
                        YarnFor = e.YarnFor,
                        Color = e.Color,
                        COUNT = e.COUNT
                    })
                    .OrderBy(e => e.FABCODE)
                    .ToListAsync(); ;

                rndFabricInfoViewModel.RndYarnConsumptions = await DenimDbContext.RND_YARNCONSUMPTION
                    .Include(e => e.COUNT)
                    .Where(e => e.FABCODE.Equals(fabCode))
                    .Select(e => new RND_YARNCONSUMPTION
                    {
                        TRNSID = e.TRNSID,
                        EncryptedId = _protector.Protect(e.TRNSID.ToString()),
                        FABCODE = e.FABCODE,
                        COUNTID = e.COUNTID,
                        YARNFOR = e.YARNFOR,
                        AMOUNT = e.AMOUNT,
                        COLOR = e.COLOR,
                        COUNT = new BAS_YARN_COUNTINFO
                        {
                            COUNTID = e.COUNT != null ? e.COUNT.COUNTID : 0,
                            COUNTNAME = $"{e.COUNT.COUNTNAME}"
                        }
                    })
                    .OrderBy(e => e.FABCODE)
                    .ToListAsync(); ;


                foreach (var item in rndFabricInfoViewModel.RndFabricCountInfos)
                {
                    item.AMOUNT = rndFabricInfoViewModel.RndYarnConsumptions
                        .FirstOrDefault(c => c.COUNTID.Equals(item.COUNTID) && c.YARNFOR.Equals(item.YARNFOR) && c.FABCODE.Equals(item.FABCODE) && c.COLOR.Equals(item.COLORCODE))
                        ?.AMOUNT;
                    //item.AMOUNT = Math.Round((double)rndFabricInfoViewModel.RndYarnConsumptions
                    //    .FirstOrDefault(c => c.COUNTID.Equals(item.COUNTID) && c.YARNFOR.Equals(item.YARNFOR))?.AMOUNT,4);
                }

                //for (var i = 0; i < rndFabricInfoViewModel.RndFabricCountInfos.Count(); i++)
                //{
                //    if ((rndFabricInfoViewModel.RndYarnConsumptions[i].COUNTID ==
                //         rndFabricInfoViewModel.RndFabricCountInfos[i].COUNTID) && (rndFabricInfoViewModel.RndYarnConsumptions[i].YARNFOR ==
                //            rndFabricInfoViewModel.RndFabricCountInfos[i].YARNFOR))
                //    {
                //        rndFabricInfoViewModel.RndFabricCountInfos[i].AMOUNT =
                //            rndFabricInfoViewModel.RndYarnConsumptions[i].AMOUNT;
                //    }

                //}

                rndFabricInfoViewModel.rND_DYEING_TYPEs = await DenimDbContext.RND_DYEING_TYPE.Select(e => new RND_DYEING_TYPE { DID = e.DID, DTYPE = e.DTYPE }).OrderBy(e => e.DTYPE).ToListAsync();

                rndFabricInfoViewModel.bAS_YARN_COUNTINFOs = await DenimDbContext.BAS_YARN_COUNTINFO
                    .Select(e => new BAS_YARN_COUNTINFO
                    {
                        COUNTID = e.COUNTID,
                        COUNTNAME = e.RND_COUNTNAME,
                        YARN_CAT_ID = e.YARN_CAT_ID
                    })
                    .Where(c => !c.YARN_CAT_ID.Equals(8102699))
                    .OrderBy(e => e.RND_COUNTNAME)
                    .ToListAsync();


                rndFabricInfoViewModel.RndSampleInfoWeavings = await DenimDbContext.RND_SAMPLE_INFO_WEAVING
                    .Where(e => !DenimDbContext.RND_FABRICINFO.Any(g => g.WVID.Equals(e.WVID)))
                    .Select(e => new RND_SAMPLE_INFO_WEAVING
                    {
                        WVID = e.WVID,
                        FABCODE = e.FABCODE
                    }).OrderBy(e => e.FABCODE).ToListAsync();

                rndFabricInfoViewModel.RndSampleInfoFinishings = await DenimDbContext.RND_SAMPLEINFO_FINISHING
                    .Include(c => c.LTG.PROG)
                    .Where(e => !e.LTG.PROG.PROG_NO.Equals("1006/0000") && !DenimDbContext.RND_FABRICINFO.Any(g => g.SFINID.Equals(e.SFINID)))
                    .Select(e => new RND_SAMPLEINFO_FINISHING
                    {
                        SFINID = e.SFINID,
                        STYLE_NAME = e.STYLE_NAME
                    })
                    .OrderBy(e => e.STYLE_NAME)
                    .ToListAsync();

                rndFabricInfoViewModel.FabTestSamples = await DenimDbContext.RND_FABTEST_SAMPLE
                    .Where(c => c.LABNO != null)
                    .Select(c => new RND_FABTEST_SAMPLE
                    {
                        LABNO = c.LABNO,
                        LTSID = c.LTSID
                    }).ToListAsync();

                rndFabricInfoViewModel.FabTestGreyList = await DenimDbContext.RND_FABTEST_GREY
                    .Where(c => c.LAB_NO != null)
                    .Select(c => new RND_FABTEST_GREY
                    {
                        LAB_NO = c.LAB_NO,
                        LTGID = c.LTGID
                    }).ToListAsync();

                rndFabricInfoViewModel.bAS_SUPPLIERINFOs = await DenimDbContext.BAS_SUPPLIERINFO.Select(e => new BAS_SUPPLIERINFO { SUPPID = e.SUPPID, SUPPNAME = e.SUPPNAME }).OrderBy(e => e.SUPPNAME).ToListAsync();

                rndFabricInfoViewModel.BasColors = await DenimDbContext.BAS_COLOR.Select(e => new BAS_COLOR { COLORCODE = e.COLORCODE, COLOR = e.COLOR }).OrderBy(e => e.COLOR).ToListAsync();

                rndFabricInfoViewModel.bAS_BUYERINFOs = await DenimDbContext.BAS_BUYERINFO.Select(e => new BAS_BUYERINFO { BUYERID = e.BUYERID, BUYER_NAME = e.BUYER_NAME }).OrderBy(e => e.BUYER_NAME).ToListAsync();
                rndFabricInfoViewModel.bAS_YARN_LOTINFOs = await DenimDbContext.BAS_YARN_LOTINFO.Select(e => new BAS_YARN_LOTINFO { LOTID = e.LOTID, LOTNO = e.LOTNO }).OrderBy(e => e.LOTNO).ToListAsync();
                rndFabricInfoViewModel.RndFinishtypes = await DenimDbContext.RND_FINISHTYPE.Select(e => new RND_FINISHTYPE { FINID = e.FINID, TYPENAME = e.TYPENAME }).OrderBy(e => e.TYPENAME).ToListAsync();
                rndFabricInfoViewModel.RndWeaves = await DenimDbContext.RND_WEAVE.Select(e => new RND_WEAVE { WID = e.WID, NAME = e.NAME }).OrderBy(e => e.NAME).ToListAsync();
                rndFabricInfoViewModel.RndFinishmcs = await DenimDbContext.RND_FINISHMC.Select(e => new RND_FINISHMC { MCID = e.MCID, NAME = e.NAME }).OrderBy(e => e.NAME).ToListAsync();
                rndFabricInfoViewModel.LoomTypes = await DenimDbContext.LOOM_TYPE.Select(e => new LOOM_TYPE { LOOMID = e.LOOMID, LOOM_TYPE_NAME = e.LOOM_TYPE_NAME }).OrderBy(e => e.LOOM_TYPE_NAME).ToListAsync();

                rndFabricInfoViewModel.rND_FABRICINFO = await DenimDbContext.RND_FABRICINFO
                    .Include(e => e.LOOM)
                    .Where(e => e.FABCODE.Equals(fabCode))
                    .FirstOrDefaultAsync();
                rndFabricInfoViewModel.rND_FABRICINFO.EncryptedId = _protector.Protect(rndFabricInfoViewModel.rND_FABRICINFO.FABCODE.ToString());
                //if (rndFabricCountinfoAndRndYarnConsumptionViewModels.Any())
                //{
                //    rndFabricInfoViewModel.rndFabricCountinfoAndRndYarnConsumptionViewModels = rndFabricCountinfoAndRndYarnConsumptionViewModels;
                //}

                // AGE GROUP
                rndFabricInfoViewModel.AgeGroupViewModels = await DenimDbContext.AGEGROUP.Select(e => new AgeGroupViewModel
                {
                    AgeGroupId = e.ID,
                    AgeGroupName = e.AGEGROUPNAME,
                    IsSelected = DenimDbContext.AGEGROUPRNDFABRICS.Any(f => f.AGEGROUPID.Equals(e.ID) && f.FABCODE.Equals(rndFabricInfoViewModel.rND_FABRICINFO.FABCODE))
                }).ToListAsync();

                // GENDER
                rndFabricInfoViewModel.TargetGenderViewModels = await DenimDbContext.TARGETGENDER.Select(e => new TargetGenderViewModel
                {
                    GenderId = e.Id,
                    GenderName = e.GENDERNAME,
                    IsSelected = DenimDbContext.TARGETGENDERRNDFABRICS.Any(f => f.GENDERID.Equals(e.Id) && f.FABCODE.Equals(rndFabricInfoViewModel.rND_FABRICINFO.FABCODE))
                }).ToListAsync();

                // CHARACTER
                rndFabricInfoViewModel.TargetCharacterViewModels = await DenimDbContext.TARGETCHARACTER.Select(e => new TargetCharacterViewModel
                {
                    CharacterId = e.Id,
                    CharacterName = e.CHARACTERNAME,
                    IsSelected = DenimDbContext.TARGETCHARACTERRNDFABRICS.Any(f => f.CHARACTERID.Equals(e.Id) && f.FABCODE.Equals(rndFabricInfoViewModel.rND_FABRICINFO.FABCODE))
                }).ToListAsync();

                // PRICE SEGMENT
                rndFabricInfoViewModel.TargetPriceSegmentViewModels = await DenimDbContext.TARGETPRICESEGMENT.Select(e => new TargetPriceSegmentViewModel
                {
                    PriceSegmentId = e.Id,
                    PriceSegmentType = e.PRICESEGMENTTYPE,
                    IsSelected = DenimDbContext.TARGETPRICESEGMENTRNDFABRICS.Any(f => f.PRICESEGMENTID.Equals(e.Id) && f.FABCODE.Equals(rndFabricInfoViewModel.rND_FABRICINFO.FABCODE))
                }).ToListAsync();

                // FIT STYLE
                rndFabricInfoViewModel.TargetFitStyleViewModels = await DenimDbContext.TARGETFITSTYLE.Select(e => new TargetFitStyleViewModel
                {
                    FitStyleId = e.Id,
                    FitStyleName = e.FITSTYLENAME,
                    IsSelected = DenimDbContext.TARGETFITSTYLERNDFABRICS.Any(f => f.FITSTYLEID.Equals(e.Id) && f.FABCODE.Equals(rndFabricInfoViewModel.rND_FABRICINFO.FABCODE))
                }).ToListAsync();

                // SEASON
                rndFabricInfoViewModel.SegmentSeasonViewModels = await DenimDbContext.SEGMENTSEASON.Select(e => new SegmentSeasonViewModel
                {
                    SeasonId = e.Id,
                    SeasonName = e.SEASONNAME,
                    IsSelected = DenimDbContext.SEGMENTSEASONRNDFABRICS.Any(f => f.SEASONID.Equals(e.Id) && f.FABCODE.Equals(rndFabricInfoViewModel.rND_FABRICINFO.FABCODE))
                }).ToListAsync();

                // COM. SEGMENT
                rndFabricInfoViewModel.ComSegmentViewModels = await DenimDbContext.SEGMENTCOMSEGMENT.Select(e => new ComSegmentViewModel
                {
                    ComSegmentId = e.Id,
                    ComSegmentName = e.COMSEGMENTNAME,
                    IsSelected = DenimDbContext.SEGMENTCOMSEGMENTRNDFABRICS.Any(f => f.COMSEGMENTID.Equals(e.Id) && f.FABCODE.Equals(rndFabricInfoViewModel.rND_FABRICINFO.FABCODE))
                }).ToListAsync();

                // OTHER SIMILAR
                rndFabricInfoViewModel.OtherSimilarViewModels = await DenimDbContext.SEGMENTOTHERSIMILARNAME.Select(e => new OtherSimilarViewModel
                {
                    OtherSimilarId = e.Id,
                    OtherSimilarName = e.OTHERSIMILARNAME,
                    Input = DenimDbContext.SEGMENTOTHERSIMILARRNDFABRICS.FirstOrDefault(f => f.OTHERSIMILARID.Equals(e.Id) && f.FABCODE.Equals(rndFabricInfoViewModel.rND_FABRICINFO.FABCODE)).INPUT,
                    IsSelected = DenimDbContext.SEGMENTOTHERSIMILARRNDFABRICS.Any(f => f.OTHERSIMILARID.Equals(e.Id) && f.FABCODE.Equals(rndFabricInfoViewModel.rND_FABRICINFO.FABCODE))
                }).ToListAsync();

                return rndFabricInfoViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<int?> GetPrimaryKeyDuringInsertByAsync(RND_FABRICINFO rndFabricinfo)
        {
            try
            {
                var entityEntry = await DenimDbContext.RND_FABRICINFO.AddAsync(rndFabricinfo);
                await SaveChanges();
                return entityEntry.Entity.FABCODE;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return null;
            }
        }

        public async Task<RndFabricInfoCountAndYarnConsumptionViewModel> GetSelectedTargetSegmentAsync(RndFabricInfoCountAndYarnConsumptionViewModel rndFabricInfoCountAndYarnConsumptionViewModel)
        {
            // AGE GROUP
            rndFabricInfoCountAndYarnConsumptionViewModel.AgeGroupViewModels = await DenimDbContext.AGEGROUP.Select(e => new AgeGroupViewModel
            {
                AgeGroupId = e.ID,
                AgeGroupName = e.AGEGROUPNAME,
                IsSelected = DenimDbContext.AGEGROUPRNDFABRICS.Any(f => f.AGEGROUPID.Equals(e.ID) && f.FABCODE.Equals(rndFabricInfoCountAndYarnConsumptionViewModel.rND_FABRICINFO.FABCODE))
            }).ToListAsync();

            // GENDER
            rndFabricInfoCountAndYarnConsumptionViewModel.TargetGenderViewModels = await DenimDbContext.TARGETGENDER.Select(e => new TargetGenderViewModel
            {
                GenderId = e.Id,
                GenderName = e.GENDERNAME,
                IsSelected = DenimDbContext.TARGETGENDERRNDFABRICS.Any(f => f.GENDERID.Equals(e.Id) && f.FABCODE.Equals(rndFabricInfoCountAndYarnConsumptionViewModel.rND_FABRICINFO.FABCODE))
            }).ToListAsync();

            // CHARACTER
            rndFabricInfoCountAndYarnConsumptionViewModel.TargetCharacterViewModels = await DenimDbContext.TARGETCHARACTER.Select(e => new TargetCharacterViewModel
            {
                CharacterId = e.Id,
                CharacterName = e.CHARACTERNAME,
                IsSelected = DenimDbContext.TARGETCHARACTERRNDFABRICS.Any(f => f.CHARACTERID.Equals(e.Id) && f.FABCODE.Equals(rndFabricInfoCountAndYarnConsumptionViewModel.rND_FABRICINFO.FABCODE))
            }).ToListAsync();

            // PRICE SEGMENT
            rndFabricInfoCountAndYarnConsumptionViewModel.TargetPriceSegmentViewModels = await DenimDbContext.TARGETPRICESEGMENT.Select(e => new TargetPriceSegmentViewModel
            {
                PriceSegmentId = e.Id,
                PriceSegmentType = e.PRICESEGMENTTYPE,
                IsSelected = DenimDbContext.TARGETPRICESEGMENTRNDFABRICS.Any(f => f.PRICESEGMENTID.Equals(e.Id) && f.FABCODE.Equals(rndFabricInfoCountAndYarnConsumptionViewModel.rND_FABRICINFO.FABCODE))
            }).ToListAsync();

            // FIT STYLE
            rndFabricInfoCountAndYarnConsumptionViewModel.TargetFitStyleViewModels = await DenimDbContext.TARGETFITSTYLE.Select(e => new TargetFitStyleViewModel
            {
                FitStyleId = e.Id,
                FitStyleName = e.FITSTYLENAME,
                IsSelected = DenimDbContext.TARGETFITSTYLERNDFABRICS.Any(f => f.FITSTYLEID.Equals(e.Id) && f.FABCODE.Equals(rndFabricInfoCountAndYarnConsumptionViewModel.rND_FABRICINFO.FABCODE))
            }).ToListAsync();

            // SEASON
            rndFabricInfoCountAndYarnConsumptionViewModel.SegmentSeasonViewModels = await DenimDbContext.SEGMENTSEASON.Select(e => new SegmentSeasonViewModel
            {
                SeasonId = e.Id,
                SeasonName = e.SEASONNAME,
                IsSelected = DenimDbContext.SEGMENTSEASONRNDFABRICS.Any(f => f.SEASONID.Equals(e.Id) && f.FABCODE.Equals(rndFabricInfoCountAndYarnConsumptionViewModel.rND_FABRICINFO.FABCODE))
            }).ToListAsync();

            // COM. SEGMENT
            rndFabricInfoCountAndYarnConsumptionViewModel.ComSegmentViewModels = await DenimDbContext.SEGMENTCOMSEGMENT.Select(e => new ComSegmentViewModel
            {
                ComSegmentId = e.Id,
                ComSegmentName = e.COMSEGMENTNAME,
                IsSelected = DenimDbContext.SEGMENTCOMSEGMENTRNDFABRICS.Any(f => f.COMSEGMENTID.Equals(e.Id) && f.FABCODE.Equals(rndFabricInfoCountAndYarnConsumptionViewModel.rND_FABRICINFO.FABCODE))
            }).ToListAsync();

            // OTHER SIMILAR
            rndFabricInfoCountAndYarnConsumptionViewModel.OtherSimilarViewModels = await DenimDbContext.SEGMENTOTHERSIMILARNAME.Select(e => new OtherSimilarViewModel
            {
                OtherSimilarId = e.Id,
                OtherSimilarName = e.OTHERSIMILARNAME,
                Input = DenimDbContext.SEGMENTOTHERSIMILARRNDFABRICS.FirstOrDefault(f => f.OTHERSIMILARID.Equals(e.Id) && f.FABCODE.Equals(rndFabricInfoCountAndYarnConsumptionViewModel.rND_FABRICINFO.FABCODE)).INPUT,
                IsSelected = DenimDbContext.SEGMENTOTHERSIMILARRNDFABRICS.Any(f => f.OTHERSIMILARID.Equals(e.Id) && f.FABCODE.Equals(rndFabricInfoCountAndYarnConsumptionViewModel.rND_FABRICINFO.FABCODE))
            }).ToListAsync();

            return rndFabricInfoCountAndYarnConsumptionViewModel;
        }

        public async Task<RND_FABRICINFO> GetFabInfoWithCount(int id)
        {
            try
            {
                var result = await DenimDbContext.RND_FABRICINFO
                    .Include(c => c.RND_FABRIC_COUNTINFO)
                    .ThenInclude(c => c.COUNT)
                    .Include(c => c.RND_YARNCONSUMPTION)
                    .Where(c => c.FABCODE.Equals(id)).FirstOrDefaultAsync();

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

    }
}