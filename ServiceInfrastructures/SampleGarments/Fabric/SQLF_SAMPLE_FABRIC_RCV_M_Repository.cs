using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces.SampleGarments.Fabric;
using DenimERP.ViewModels;
using DenimERP.ViewModels.SampleGarments.Fabric;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures.SampleGarments.Fabric
{
    public class SQLF_SAMPLE_FABRIC_RCV_M_Repository : BaseRepository<F_SAMPLE_FABRIC_RCV_M>, IF_SAMPLE_FABRIC_RCV_M
    {
        private readonly IDataProtector _protector;

        public SQLF_SAMPLE_FABRIC_RCV_M_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<CreateFSampleFabricRcvMViewModel> GetInitObjsByAsync(CreateFSampleFabricRcvMViewModel createFSampleFabricRcvMViewModel)
        {
            createFSampleFabricRcvMViewModel.FSampleItemDetailses = await DenimDbContext.F_SAMPLE_ITEM_DETAILS
                .Where(e => createFSampleFabricRcvMViewModel.FSampleItemDetailses.All(f => f.SITEMID.Equals(e.SITEMID)))
                .ToListAsync();

            createFSampleFabricRcvMViewModel.PlProductionSetdistributions = await DenimDbContext.PL_PRODUCTION_SETDISTRIBUTION
                .Include(e => e.PROG_)
                .Where(e => createFSampleFabricRcvMViewModel.PlProductionSetdistributions.All(f => f.SETID.Equals(e.SETID)))
                .ToListAsync();

            if (createFSampleFabricRcvMViewModel.FSampleFabricRcvM is { SECID: { } })
            {
                createFSampleFabricRcvMViewModel.FBasSections = await DenimDbContext.F_BAS_SECTION
                    .OrderBy(e => e.SECNAME)
                    .Where(e => e.SECID.Equals(createFSampleFabricRcvMViewModel.FSampleFabricRcvM.SECID))
                    .Select(e => new F_BAS_SECTION
                    {
                        SECID = e.SECID,
                        SECNAME = e.SECNAME
                    }).ToListAsync();
            }

            if (createFSampleFabricRcvMViewModel.FSampleFabricRcvM is { EMPID: { } })
            {
                createFSampleFabricRcvMViewModel.FHrEmployees = await DenimDbContext.F_HRD_EMPLOYEE
                    .OrderBy(e => e.FIRST_NAME)
                    .Where(e => e.EMPID.Equals(createFSampleFabricRcvMViewModel.FSampleFabricRcvM.EMPID))
                    .Select(e => new F_HRD_EMPLOYEE
                    {
                        EMPID = e.EMPID,
                        FIRST_NAME = string.Join(" ", e.FIRST_NAME, e.LAST_NAME)
                    }).ToListAsync();
            }

            return createFSampleFabricRcvMViewModel;
        }

        public async Task<CreateFSampleFabricRcvMViewModel> GetInitObjectsByAsync(CreateFSampleFabricRcvMViewModel createFSampleFabricRcvMViewModel)
        {
            foreach (var item in createFSampleFabricRcvMViewModel.FSampleFabricRcvDs)
            {
                item.FABCODENavigation = await DenimDbContext.RND_FABRICINFO
                    .Include(e => e.WV)
                    .FirstOrDefaultAsync(e => e.FABCODE.Equals(item.FABCODE));

                item.SITEM = await DenimDbContext.F_SAMPLE_ITEM_DETAILS
                    .FirstOrDefaultAsync(e => e.SITEMID.Equals(item.SITEMID));

                item.SET = await DenimDbContext.PL_PRODUCTION_SETDISTRIBUTION
                    .Include(e => e.PROG_)
                    .FirstOrDefaultAsync(e => e.SETID.Equals(item.SETID));
            }

            createFSampleFabricRcvMViewModel.FSampleItemDetailses = await DenimDbContext.F_SAMPLE_ITEM_DETAILS
                .Select(e => new F_SAMPLE_ITEM_DETAILS
                {
                    SITEMID = e.SITEMID,
                    NAME = e.NAME
                }).OrderBy(e => e.NAME).ToListAsync();

            createFSampleFabricRcvMViewModel.PlProductionSetdistributions = await DenimDbContext.PL_PRODUCTION_SETDISTRIBUTION
                .Include(e => e.PROG_)
                .OrderBy(e => e.PROG_.PROG_NO)
                .Select(e => new PL_PRODUCTION_SETDISTRIBUTION
                {
                    SETID = e.SETID,
                    PROG_ = new PL_BULK_PROG_SETUP_D
                    {
                        PROG_NO = $"{e.PROG_.PROG_NO}"
                    }
                }).ToListAsync();

            return createFSampleFabricRcvMViewModel;
        }

        public async Task<CreateFSampleFabricRcvMViewModel> GetDetailsFormInspectionByAsync(CreateFSampleFabricRcvMViewModel createFSampleFabricRcvMViewModel)
        {
            var processTypes = new[] {
                11, // Bulk to Sample
                13  // Sample (RS)
            };

            createFSampleFabricRcvMViewModel.FSampleFabricRcvDs.AddRange(await DenimDbContext.F_PR_INSPECTION_PROCESS_DETAILS
                .Include(e => e.TROLLEYNONavigation.FN_PROCESS.DOFF.WV.RND_FABRICINFO)
                //.Include(c => c.INSP)
                //.Include(e => e.INSP.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.WV)
                //.Include(e => e.INSP.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.COLORCODENavigation)
                .Where(e => !DenimDbContext.F_SAMPLE_FABRIC_RCV_D
                                .Any(g => g.ROLLNO.Equals(e.ROLLNO)) &&
                            e.ROLL_INSPDATE.Equals(createFSampleFabricRcvMViewModel.ProductionDate) &&
                            processTypes.Any(g => g.Equals(e.PROCESS_TYPE)))
                .Select(e => new F_SAMPLE_FABRIC_RCV_D
                {
                    WVID = e.TROLLEYNONavigation.FN_PROCESS.DOFF.WV.WVID,
                    //FABCODENavigation = new RND_FABRICINFO
                    //{
                    //    FABCODE = e.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.FABCODE,
                    //    COLORCODENavigation = new BAS_COLOR
                    //    {
                    //        COLOR = $"{e.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.COLORCODENavigation.COLOR}"
                    //    },
                    //    WV = new RND_SAMPLE_INFO_WEAVING
                    //    {
                    //        FABCODE = $"{e.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.WV.FABCODE}"
                    //    }
                    //},
                    
                    QTY = e.LENGTH_YDS,
                    ROLLNO = e.ROLLNO,
                    SETID = e.INSP.SETID,
                    FAB_GRADE = e.FAB_GRADE,
                    StyleNo = e.OPT4,
                    DEV_NO = DenimDbContext.RND_FABRICINFO
                        .FirstOrDefault(f => f.STYLE_NAME.Equals(e.OPT4)).DEVID,
                    FABCODE = DenimDbContext.RND_FABRICINFO
                        .FirstOrDefault(f => f.STYLE_NAME.Equals(e.OPT4)).FABCODE

                    //FABCODE = _denimDbContext.RND_FABRICINFO.Include(f => f.WV)
                    //    .FirstOrDefault(f => f.WVID.Equals(e.TROLLEYNONavigation.FN_PROCESS.DOFF.WV.WVID)).FABCODE
                }).ToListAsync());

            //foreach (var item in createFSampleFabricRcvMViewModel.FSampleFabricRcvDs)
            //{
            //    item.FABCODE = Task.Run(async () => await _denimDbContext.RND_FABRICINFO
            //        .Include(e => e.WV)
            //        .FirstOrDefaultAsync(c => c.WVID.Equals(item.WVID))).Result.FABCODE;
            //}

            return createFSampleFabricRcvMViewModel;
        }

        public async Task<CreateFSampleFabricRcvMViewModel> GetProgramsByAsync(string search, int page = 1)
        {
            var createFSampleFabricRcvMViewModel = new CreateFSampleFabricRcvMViewModel();

            if (!string.IsNullOrEmpty(search))
            {
                createFSampleFabricRcvMViewModel.PlProductionSetdistributions = await DenimDbContext.PL_PRODUCTION_SETDISTRIBUTION
                    .Include(e => e.PROG_)
                    .OrderBy(e => e.PROG_.PROG_NO)
                    .Select(e => new PL_PRODUCTION_SETDISTRIBUTION
                    {
                        SETID = e.SETID,
                        PROG_ = new PL_BULK_PROG_SETUP_D
                        {
                            PROG_NO = $"{e.PROG_.PROG_NO}"
                        }
                    }).Where(e => e.PROG_.PROG_NO.ToLower().Contains(search.ToLower())).ToListAsync();
            }
            else
            {
                createFSampleFabricRcvMViewModel.PlProductionSetdistributions = await DenimDbContext.PL_PRODUCTION_SETDISTRIBUTION
                    .Include(e => e.PROG_)
                    .OrderBy(e => e.PROG_.PROG_NO)
                    .Select(e => new PL_PRODUCTION_SETDISTRIBUTION
                    {
                        SETID = e.SETID,
                        PROG_ = new PL_BULK_PROG_SETUP_D
                        {
                            PROG_NO = $"{e.PROG_.PROG_NO}"
                        }
                    }).ToListAsync();
            }

            return createFSampleFabricRcvMViewModel;
        }

        public async Task<CreateFSampleFabricRcvMViewModel> GetDetailsFormClearanceByAsync(CreateFSampleFabricRcvMViewModel createFSampleFabricRcvMViewModel)
        {
            createFSampleFabricRcvMViewModel.FSampleFabricRcvDs.AddRange(await DenimDbContext.F_FS_FABRIC_CLEARANCE_MASTER
                .Where(e => e.TRNSDATE.Equals(createFSampleFabricRcvMViewModel.ProductionDate))
                .Select(e => new F_SAMPLE_FABRIC_RCV_D
                {
                    FABCODE = e.FABCODE,
                    //FABCODENavigation = new RND_FABRICINFO
                    //{
                    //    FABCODE = e.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.FABCODE,
                    //    COLORCODENavigation = new BAS_COLOR
                    //    {
                    //        COLOR = $"{e.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.COLORCODENavigation.COLOR}"
                    //    },
                    //    WV = new RND_SAMPLE_INFO_WEAVING
                    //    {
                    //        FABCODE = $"{e.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.WV.FABCODE}"
                    //    }
                    //},
                    QTY = e.ROLE_QTY
                }).ToListAsync());

            return createFSampleFabricRcvMViewModel;
        }

        public async Task<DataTableObject<F_SAMPLE_FABRIC_RCV_M>> GetForDataTableByAsync(string sortColumn, string sortColumnDirection, string searchValue, string draw, int skip, int pageSize)
        {
            try
            {
                var navigationPropertyStrings = new[] { "SEC", "EMP" };

                var fSampleFabricRcvMs = DenimDbContext.F_SAMPLE_FABRIC_RCV_M
                    .Include(e => e.SEC)
                    .Include(e => e.EMP)
                    .Select(e => new F_SAMPLE_FABRIC_RCV_M
                    {
                        SFRID = e.SFRID,
                        EncryptedId = _protector.Protect(e.SFRID.ToString()),
                        SFRDATE = e.SFRDATE,
                        SEC = new F_BAS_SECTION
                        {
                            SECNAME = e.SEC.SECNAME
                        },
                        EMP = new F_HRD_EMPLOYEE
                        {
                            FIRST_NAME = e.EMP.FIRST_NAME
                        },
                        REMARKS = e.REMARKS
                    }).AsQueryable();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    fSampleFabricRcvMs = OrderedResult<F_SAMPLE_FABRIC_RCV_M>.GetOrderedResult(sortColumnDirection, sortColumn, navigationPropertyStrings, fSampleFabricRcvMs);
                }

                if (!string.IsNullOrEmpty(searchValue))
                {
                    fSampleFabricRcvMs = fSampleFabricRcvMs
                        .Where(m => m.SFRDATE.ToString().ToUpper(CultureInfo.InvariantCulture).Contains(searchValue)
                                    || m.SEC.SECNAME != null && m.SEC.SECNAME.ToUpper().Contains(searchValue)
                                    || m.EMP.FIRST_NAME != null && m.EMP.FIRST_NAME.ToUpper().Contains(searchValue)
                                    || m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue));

                    fSampleFabricRcvMs = OrderedResult<F_SAMPLE_FABRIC_RCV_M>.GetOrderedResult(sortColumnDirection, sortColumn, navigationPropertyStrings, fSampleFabricRcvMs);
                }

                var recordsTotal = await fSampleFabricRcvMs.CountAsync();

                return new DataTableObject<F_SAMPLE_FABRIC_RCV_M>(draw, recordsTotal, recordsTotal, await fSampleFabricRcvMs.Skip(skip).Take(pageSize).ToListAsync());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<CreateFSampleFabricRcvMViewModel> FindBySfrIdIncludeAllAsync(int sfrId)
        {
            try
            {
                var createFSampleFabricRcvMViewModel = await DenimDbContext.F_SAMPLE_FABRIC_RCV_M
                .Include(e => e.SEC)
                .Include(e => e.EMP)
                .Include(e => e.F_SAMPLE_FABRIC_RCV_D)
                .ThenInclude(e => e.SITEM)
                .Include(e => e.F_SAMPLE_FABRIC_RCV_D)
                .ThenInclude(e => e.FABCODENavigation.BUYER)
                .Include(e => e.F_SAMPLE_FABRIC_RCV_D)
                .ThenInclude(e => e.FABCODENavigation.COLORCODENavigation)
                .Include(e => e.F_SAMPLE_FABRIC_RCV_D)
                .ThenInclude(e => e.FABCODENavigation.WV)
                .Where(e => e.SFRID.Equals(sfrId))
                .Select(e => new CreateFSampleFabricRcvMViewModel
                {
                    FSampleFabricRcvM = new F_SAMPLE_FABRIC_RCV_M
                    {
                        SFRID = e.SFRID,
                        EncryptedId = _protector.Protect(e.SFRID.ToString()),
                        SFTRDATE = e.SFTRDATE,
                        SECID = e.SECID,
                        EMPID = e.EMPID,
                        REMARKS = e.REMARKS,
                        SFTRNO = e.SFTRNO,
                        SFRDATE = e.SFRDATE,
                        EMP = new F_HRD_EMPLOYEE
                        {
                            EMPID = e.EMP != null ? e.EMP.EMPID : 0,
                            FIRST_NAME = e.EMP != null ? $"{e.EMP.FIRST_NAME} {e.EMP.LAST_NAME}" : ""
                        },
                        SEC = new F_BAS_SECTION
                        {
                            SECID = e.SEC != null ? e.SEC.SECID : 0,
                            SECNAME = $"{e.SEC.SECNAME}"
                        }
                    },
                    FSampleFabricRcvDs = e.F_SAMPLE_FABRIC_RCV_D.Select(f => new F_SAMPLE_FABRIC_RCV_D
                    {
                        TRNSID = f.TRNSID,
                        SFRID = f.SFRID,
                        SITEMID = f.SITEMID,
                        DEV_NO = f.DEV_NO,
                        FABCODE = f.FABCODE,
                        StyleNo = f.FABCODENavigation.STYLE_NAME,
                        QTY = f.QTY,
                        REMARKS = f.REMARKS,
                        OPT1 = f.OPT1,
                        OPT2 = f.OPT2,
                        OPT3 = f.OPT3,
                        BARCODE = f.BARCODE,
                        SETID = f.SETID,
                        ROLLNO = f.ROLLNO,
                        

                        SITEM = new F_SAMPLE_ITEM_DETAILS
                        {
                            NAME = f.SITEM.NAME
                        },
                        FABCODENavigation = new RND_FABRICINFO
                        {
                            FABCODE = f.FABCODENavigation.FABCODE,
                            STYLE_NAME = f.FABCODENavigation.STYLE_NAME,
                            WV = new RND_SAMPLE_INFO_WEAVING
                            {
                                FABCODE = f.FABCODENavigation.WV.FABCODE
                            },
                            BUYER = new BAS_BUYERINFO
                            {
                                BUYER_NAME = f.FABCODENavigation.BUYER.BUYER_NAME
                            },
                            COLORCODENavigation = new BAS_COLOR
                            {
                                COLOR = f.FABCODENavigation.COLORCODENavigation.COLOR
                            }
                        }
                    }).ToList()
                }).FirstOrDefaultAsync();

                return createFSampleFabricRcvMViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }

        public async Task<CreateFSampleFabricRcvMViewModel> FindBySfrIdForDeleteAsync(int sfrId)
        {
            var fSampleFabricRcvM = await DenimDbContext.F_SAMPLE_FABRIC_RCV_M
                .Include(e => e.F_SAMPLE_FABRIC_RCV_D)
                .FirstOrDefaultAsync(e => e.SFRID.Equals(sfrId));

            return new CreateFSampleFabricRcvMViewModel
            {
                FSampleFabricRcvM = fSampleFabricRcvM,
                FSampleFabricRcvDs = fSampleFabricRcvM.F_SAMPLE_FABRIC_RCV_D.ToList()
            };
        }
    }
}
