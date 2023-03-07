using System;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels;
using DenimERP.ViewModels.Factory.Dyeing;
using DenimERP.ViewModels.Rnd.Dyeing;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLRND_SAMPLE_INFO_DYEING_Repository : BaseRepository<RND_SAMPLE_INFO_DYEING>, IRND_SAMPLE_INFO_DYEING
    {
        private readonly IDataProtector _protector;

        public SQLRND_SAMPLE_INFO_DYEING_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<CreateRndSampleInfoDyeingAndDetailsViewModel> GetInitObjects(CreateRndSampleInfoDyeingAndDetailsViewModel createRndSampleInfoDyeingAndDetailsViewModel)
        {
            createRndSampleInfoDyeingAndDetailsViewModel.RndSampleInfoDyeing.SIDATE = DateTime.Now;

            createRndSampleInfoDyeingAndDetailsViewModel.BasYarnCountinfos = await DenimDbContext.BAS_YARN_COUNTINFO
                .Select(e => new BAS_YARN_COUNTINFO
                {
                    COUNTID = e.COUNTID,
                    COUNTNAME = e.RND_COUNTNAME
                })
                .Where(c => c.YARN_CAT_ID != 8102699)
                .OrderBy(e => e.COUNTNAME).ToListAsync();

            createRndSampleInfoDyeingAndDetailsViewModel.BasColors = await DenimDbContext.BAS_COLOR.Select(e => new BAS_COLOR { COLORCODE = e.COLORCODE, COLOR = e.COLOR }).OrderBy(e => e.COLOR).ToListAsync();
            createRndSampleInfoDyeingAndDetailsViewModel.BasBuyerInfos = await DenimDbContext.BAS_BUYERINFO
                .Select(e => new BAS_BUYERINFO { BUYERID = e.BUYERID, BUYER_NAME = e.BUYER_NAME }).OrderBy(e => e.BUYER_NAME).ToListAsync();

            createRndSampleInfoDyeingAndDetailsViewModel.BasYarnLotinfos = await DenimDbContext.BAS_YARN_LOTINFO.Select(e => new BAS_YARN_LOTINFO { LOTID = e.LOTID, LOTNO = e.LOTNO }).OrderBy(e => e.LOTNO).ToListAsync();

            createRndSampleInfoDyeingAndDetailsViewModel.BasSupplierinfos = await DenimDbContext.BAS_SUPPLIERINFO.Select(e => new BAS_SUPPLIERINFO { SUPPID = e.SUPPID, SUPPNAME = e.SUPPNAME }).OrderBy(e => e.SUPPNAME).ToListAsync();

            createRndSampleInfoDyeingAndDetailsViewModel.MktSdrfInfos = await DenimDbContext.MKT_SDRF_INFO
                .Where(c => c.MKT_DGM_APPROVE && c.PLN_APPROVE && c.RND_APPROVE && c.PLANT_HEAD_APPROVE)
                .Select(e => new MKT_SDRF_INFO { SDRFID = e.SDRFID, SDRF_NO = e.SDRF_NO }).ToListAsync();

            createRndSampleInfoDyeingAndDetailsViewModel.Yarnfors = await DenimDbContext.YARNFOR.Select(e => new YARNFOR
            {
                YARNID = e.YARNID,
                YARNNAME = e.YARNNAME
            })
                .Where(e => e.YARNNAME.Contains("warp"))
                .OrderBy(e => e.YARNNAME)
                .ToListAsync();

            createRndSampleInfoDyeingAndDetailsViewModel.DyeingTypes = await DenimDbContext.RND_DYEING_TYPE.Select(e => new RND_DYEING_TYPE { DID = e.DID, DTYPE = e.DTYPE }).OrderBy(e => e.DTYPE).ToListAsync();
            createRndSampleInfoDyeingAndDetailsViewModel.LoomTypes = await DenimDbContext.LOOM_TYPE.Select(e => new LOOM_TYPE { LOOMID = e.LOOMID, LOOM_TYPE_NAME = e.LOOM_TYPE_NAME }).OrderBy(e => e.LOOM_TYPE_NAME).ToListAsync();
            createRndSampleInfoDyeingAndDetailsViewModel.PlSampleProgSetup = new PL_SAMPLE_PROG_SETUP { PROG_DATE = DateTime.Now };

            createRndSampleInfoDyeingAndDetailsViewModel.FHrEmployeesList = await DenimDbContext.F_HRD_EMPLOYEE
                .Where(c => c.EMPNO.Contains("RD"))
                .Select(e => new F_HRD_EMPLOYEE
                {
                    EMPID = e.EMPID,
                    FIRST_NAME = $"{e.EMPNO} - {e.FIRST_NAME}"
                }).OrderBy(e => e.EMPNO).ToListAsync();

            return createRndSampleInfoDyeingAndDetailsViewModel;
        }

        public async Task<CreateRndSampleInfoDyeingAndDetailsViewModel> GetInitObjectsWithDetails(CreateRndSampleInfoDyeingAndDetailsViewModel createRndSampleInfoDyeingAndDetailsViewModel)
        {
            foreach (var item in createRndSampleInfoDyeingAndDetailsViewModel.RndSampleInfoDetailses)
            {
                item.COLORCODENavigation = await DenimDbContext.BAS_COLOR.FindAsync(item.COLORCODE);
                item.COUNT = await DenimDbContext.BAS_YARN_COUNTINFO.FindAsync(item.COUNTID);
                item.LOT = await DenimDbContext.BAS_YARN_LOTINFO.FindAsync(item.LOTID);
                item.SUPP = await DenimDbContext.BAS_SUPPLIERINFO.FindAsync(item.SUPPID);
                item.YARN = await DenimDbContext.YARNFOR.FindAsync(item.YARNID);
            }

            return createRndSampleInfoDyeingAndDetailsViewModel;
        }

        public async Task<DataTableObject<RND_SAMPLE_INFO_DYEING>> GetForDataTableByAsync(string sortColumn, string sortColumnDirection, string searchValue, string draw, int skip, int pageSize)
        {
            var navigationPropertyStrings = new[] { "SDRF" };

            var rndSampleInfoDyeings = DenimDbContext.RND_SAMPLE_INFO_DYEING
                .Include(e => e.SDRF)
                .Select(e => new RND_SAMPLE_INFO_DYEING
                {
                    EncryptedId = _protector.Protect(e.SDID.ToString()),
                    SDID = e.SDID,
                    STYLEREF = e.STYLEREF,
                    DYEINGCODE = e.DYEINGCODE,
                    REMARKS = e.REMARKS,
                    SDRF = e.SDRF,
                    OPT1 = e.OPT1
                });

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                rndSampleInfoDyeings = OrderedResult<RND_SAMPLE_INFO_DYEING>.GetOrderedResult(sortColumnDirection, sortColumn, navigationPropertyStrings, rndSampleInfoDyeings);
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                rndSampleInfoDyeings = rndSampleInfoDyeings
                    .Where(m => m.SDID.ToString().ToUpper().Contains(searchValue)
                                || m.SDRF.BUYER_REF.ToUpper().Contains(searchValue)
                                || m.DYEINGCODE != null && m.DYEINGCODE.ToUpper().Contains(searchValue)
                                || m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue));

                rndSampleInfoDyeings = OrderedResult<RND_SAMPLE_INFO_DYEING>.GetOrderedResult(sortColumnDirection, sortColumn, navigationPropertyStrings, rndSampleInfoDyeings);
            }

            var recordsTotal = await rndSampleInfoDyeings.CountAsync();
            var sampleInfoDyeings = await rndSampleInfoDyeings.Skip(skip).Take(pageSize).ToListAsync();

            return new DataTableObject<RND_SAMPLE_INFO_DYEING>(draw, recordsTotal, recordsTotal, sampleInfoDyeings);
        }

        public async Task<DetailsRndSampleInfoDyeingViewModel> FindByIdIncludeAllAsync(int id)
        {
            return await DenimDbContext.RND_SAMPLE_INFO_DYEING
                .Include(e => e.D)
                .Include(e => e.LOOM)
                .Include(e => e.SDRF)
                .ThenInclude(e => e.TEAM_R)
                .GroupJoin(DenimDbContext.RND_SAMPLE_INFO_DETAILS,
                    f1 => f1.SDID,
                    f2 => f2.SDID,
                    (f1, f2) => new DetailsRndSampleInfoDyeingViewModel
                    {
                        RndSampleInfoDyeing = new RND_SAMPLE_INFO_DYEING
                        {
                            SDID = f1.SDID,
                            EncryptedId = _protector.Protect(f1.SDID.ToString()),
                            SIDATE = f1.SIDATE,
                            PROG_NO = f1.PROG_NO,
                            SDRFID = f1.SDRFID,
                            DYEINGCODE = f1.DYEINGCODE,
                            STYLEREF = f1.STYLEREF,
                            RNDTEAM = f1.RNDTEAM,
                            RNDPERSON = f1.RNDPERSON,
                            ENDS_ROPE = f1.ENDS_ROPE,
                            NO_OF_ROPE = f1.NO_OF_ROPE,
                            TOTAL_ENDS = f1.TOTAL_ENDS,
                            LENGTH_MTR = f1.LENGTH_MTR,
                            DYEING_REF = f1.DYEING_REF,
                            REED_SPACE = f1.REED_SPACE,
                            LOOMID = f1.LOOMID,
                            DID = f1.DID,
                            WARP_PROG_DATE = f1.WARP_PROG_DATE,
                            COMMITED_DEL_DATE = f1.COMMITED_DEL_DATE,
                            YARN_LOCATION = f1.YARN_LOCATION,
                            REMARKS = f1.REMARKS,
                            OPT1 = f1.OPT1,
                            OPT2 = f1.OPT2,
                            OPT3 = f1.OPT3,
                            OPT4 = f1.OPT4,
                            OPT5 = f1.OPT5,
                            USERID = f1.USERID,
                            D = f1.D,
                            LOOM = f1.LOOM,
                            SDRF = f1.SDRF
                        },
                        RndSampleInfoDetailses = f2.ToList()
                    })
                .OrderByDescending(e => e.RndSampleInfoDyeing.SDID)
                .Where(e => e.RndSampleInfoDyeing.SDID.Equals(id))
                .FirstOrDefaultAsync();
        }

        public async Task<RND_SAMPLE_INFO_DYEING> GetTeamInfo(int sdrfId)
        {
            return await DenimDbContext.RND_SAMPLE_INFO_DYEING
                .Include(e => e.SDRF)
                .ThenInclude(e => e.TEAM_R)
                .Where(e => e.SDRFID.Equals(sdrfId))
                .Select(e => new RND_SAMPLE_INFO_DYEING
                {
                    SDRFID = e.SDRFID,
                    SDRF = new MKT_SDRF_INFO
                    {
                        BUYER_REF = e.SDRF.BUYER_REF,
                        TEAM_R = e.SDRF.TEAM_R
                    }
                }).FirstOrDefaultAsync();
        }

        public async Task<RND_SAMPLE_INFO_DYEING> FindByIdIncludeAssociatesAsync(int id)
        {
            try
            {
                var result = await DenimDbContext.RND_SAMPLE_INFO_DYEING
                    .Include(e => e.D)
                    .Include(e => e.LOOM)
                    .Include(e => e.SDRF)
                    .ThenInclude(e => e.TEAM_R)
                    .Include(c => c.PL_SAMPLE_PROG_SETUP)
                    .Where(e => e.SDID.Equals(id))
                    .FirstOrDefaultAsync();

                result.OPT5 = "";

                foreach (var item in result.PL_SAMPLE_PROG_SETUP)
                {
                    result.OPT5 = item.PROG_NO + " + ";
                }

                result.OPT5 = result.OPT5 != "" ? result.OPT5.Substring(0, result.OPT5.LastIndexOf('+')) : "";
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }



        public string GetLastRSNo(string rsCode)
        {
            try
            {
                var rsNo = "";
                //var result = await _denimDbContext.COM_EX_PI_DETAILS.OrderByDescending(c => c.TRNSID).Select(c => c.SO_NO).FirstOrDefaultAsync();
                var year = DateTime.Now.Year - 2000;

                if (rsCode != null)
                {
                    var resultArray = rsCode.Split("-");
                    rsNo = "RS-" + year + "-" + (int.Parse(resultArray[1])).ToString().PadLeft(4, '0');
                }
                else
                {
                    rsNo = "RS-" + year + "-" + "1".PadLeft(4, '0');
                }

                return rsNo;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
