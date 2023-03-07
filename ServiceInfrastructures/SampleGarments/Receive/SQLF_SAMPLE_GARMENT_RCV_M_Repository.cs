using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces.SampleGarments.Receive;
using DenimERP.ViewModels;
using DenimERP.ViewModels.SampleGarments.Receive;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures.SampleGarments.Receive
{
    public class SQLF_SAMPLE_GARMENT_RCV_M_Repository : BaseRepository<F_SAMPLE_GARMENT_RCV_M>, IF_SAMPLE_GARMENT_RCV_M
    {
        private readonly IDataProtector _protector;

        public SQLF_SAMPLE_GARMENT_RCV_M_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<CreateFSampleGarmentRcvMViewModel> GetInitObjectsByAsync(CreateFSampleGarmentRcvMViewModel createFSampleGarmentRcvMViewModel)
        {
            if (createFSampleGarmentRcvMViewModel.FSampleGarmentRcvM is { SECID: { } })
            {
                createFSampleGarmentRcvMViewModel.FBasSections = await DenimDbContext.F_BAS_SECTION
                    .OrderBy(e => e.SECNAME)
                    .Where(e => e.SECID.Equals(createFSampleGarmentRcvMViewModel.FSampleGarmentRcvM.SECID))
                    .Select(e => new F_BAS_SECTION
                    {
                        SECID = e.SECID,
                        SECNAME = e.SECNAME
                    }).ToListAsync();
            }

            if (createFSampleGarmentRcvMViewModel.FSampleGarmentRcvM is { EMPID: { } })
            {
                createFSampleGarmentRcvMViewModel.FHrEmployees = await DenimDbContext.F_HRD_EMPLOYEE
                    .OrderBy(e => e.EMPNO)
                    .Where(e => e.EMPID.Equals(createFSampleGarmentRcvMViewModel.FSampleGarmentRcvM.EMPID))
                    .Select(e => new F_HRD_EMPLOYEE
                    {
                        EMPID = e.EMPID,
                        FIRST_NAME = string.Join("; ", e.EMPNO, string.Join(" ", e.FIRST_NAME, e.LAST_NAME)),
                    }).ToListAsync();
            }

            createFSampleGarmentRcvMViewModel.RndFabricinfos = await DenimDbContext.RND_FABRICINFO
                .Include(e => e.WV)
                .Select(e => new RND_FABRICINFO
                {
                    FABCODE = e.FABCODE,
                    WV = new RND_SAMPLE_INFO_WEAVING
                    {
                        FABCODE = e.WV.FABCODE
                    },
                    STYLE_NAME = e.STYLE_NAME
                }).OrderBy(e => e.STYLE_NAME).ToListAsync();

            createFSampleGarmentRcvMViewModel.BasColors = await DenimDbContext.BAS_COLOR
                .Where(e => !string.IsNullOrEmpty(e.COLOR))
                .Select(e => new BAS_COLOR
                {
                    COLORCODE = e.COLORCODE,
                    COLOR = e.COLOR
                }).OrderBy(e => e.COLOR).ToListAsync();

            createFSampleGarmentRcvMViewModel.FSampleItemDetailses = await DenimDbContext.F_SAMPLE_ITEM_DETAILS.Select(e => new F_SAMPLE_ITEM_DETAILS
            {
                SITEMID = e.SITEMID,
                NAME = e.NAME
            }).OrderBy(e => e.NAME).ToListAsync();

            createFSampleGarmentRcvMViewModel.FSampleLocations = await DenimDbContext.F_SAMPLE_LOCATION.Select(e => new F_SAMPLE_LOCATION
            {
                LOCID = e.LOCID,
                NAME = e.NAME
            }).OrderBy(e => e.NAME).ToListAsync();

            createFSampleGarmentRcvMViewModel.BasBuyerinfos = await DenimDbContext.BAS_BUYERINFO.Select(e => new BAS_BUYERINFO
            {
                BUYERID = e.BUYERID,
                BUYER_NAME = e.BUYER_NAME
            }).OrderBy(e => e.BUYER_NAME).ToListAsync();

            return createFSampleGarmentRcvMViewModel;
        }

        public async Task<CreateFSampleGarmentRcvMViewModel> GetInitObjectsOfSelectedItems(CreateFSampleGarmentRcvMViewModel createFSampleGarmentRcvMViewModel)
        {
            foreach (var item in createFSampleGarmentRcvMViewModel.FSampleGarmentRcvDs)
            {
                item.SITEM = await DenimDbContext.F_SAMPLE_ITEM_DETAILS.FirstOrDefaultAsync(f => f.SITEMID.Equals(item.SITEMID));
                item.FABCODENavigation = await DenimDbContext.RND_FABRICINFO
                    .Include(e => e.COLORCODENavigation)
                    .Include(f => f.WV)
                    .FirstOrDefaultAsync(g => g.FABCODE.Equals(item.FABCODE));
                item.LOC = await DenimDbContext.F_SAMPLE_LOCATION.FirstOrDefaultAsync(f => f.LOCID.Equals(item.LOCID));
                item.COLOR = await DenimDbContext.BAS_COLOR.FirstOrDefaultAsync(f => f.COLORCODE.Equals(item.COLORID));
                item.BUYER = await DenimDbContext.BAS_BUYERINFO.FirstOrDefaultAsync(e => e.BUYERID.Equals(item.BUYERID));
            }

            return createFSampleGarmentRcvMViewModel;
        }

        public async Task<DataTableObject<F_SAMPLE_GARMENT_RCV_M>> GetForDataTableByAsync(string sortColumn, string sortColumnDirection, string searchValue, string draw, int skip, int pageSize)
        {
            try
            {
                var navigationPropertyStrings = new[] { "SEC", "EMP" };
                var fSampleGarmentRcvMs = DenimDbContext.F_SAMPLE_GARMENT_RCV_M
                    .Include(e => e.SEC)
                    .Include(e => e.EMP)
                    .Select(e => new F_SAMPLE_GARMENT_RCV_M
                    {
                        SGRID = e.SGRID,
                        EncryptedId = _protector.Protect(e.SGRID.ToString()),
                        SGRDATE = e.SGRDATE,
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
                    fSampleGarmentRcvMs = OrderedResult<F_SAMPLE_GARMENT_RCV_M>.GetOrderedResult(sortColumnDirection, sortColumn, navigationPropertyStrings, fSampleGarmentRcvMs);
                }

                if (!string.IsNullOrEmpty(searchValue))
                {
                    fSampleGarmentRcvMs = fSampleGarmentRcvMs
                        .Where(m => m.SGRDATE.ToString().ToUpper(CultureInfo.InvariantCulture).Contains(searchValue)
                                    || m.SEC.SECNAME != null && m.SEC.SECNAME.ToUpper().Contains(searchValue)
                                    || m.EMP.FIRST_NAME != null && m.EMP.FIRST_NAME.ToUpper().Contains(searchValue)
                                    || m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue));

                    fSampleGarmentRcvMs = OrderedResult<F_SAMPLE_GARMENT_RCV_M>.GetOrderedResult(sortColumnDirection, sortColumn, navigationPropertyStrings, fSampleGarmentRcvMs);
                }

                var recordsTotal = await fSampleGarmentRcvMs.CountAsync();

                return new DataTableObject<F_SAMPLE_GARMENT_RCV_M>(draw, recordsTotal, recordsTotal, await fSampleGarmentRcvMs.Skip(skip).Take(pageSize).ToListAsync());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<CreateFSampleGarmentRcvMViewModel> FindBySrIdIncludeAllAsync(int srId)
        {
            var createFSampleGarmentRcvMViewModel = await DenimDbContext.F_SAMPLE_GARMENT_RCV_M
                .Include(e => e.SEC)
                .Include(e => e.EMP)
                .GroupJoin(DenimDbContext.F_SAMPLE_GARMENT_RCV_D.OrderByDescending(f => f.BARCODE)
                        .Include(e => e.SITEM)
                        .Include(e => e.COLOR)
                        .Include(e => e.BUYER)
                        .Include(e => e.LOC)
                        .Include(e => e.FABCODENavigation)
                        .ThenInclude(e => e.WV)
                        .Include(e => e.FABCODENavigation)
                        .ThenInclude(e => e.COLORCODENavigation),
                    f1 => f1.SGRID,
                    f2 => f2.SGRID,
                    (f1, f2) => new CreateFSampleGarmentRcvMViewModel
                    {
                        FSampleGarmentRcvM = f1,
                        FSampleGarmentRcvDs = f2.ToList()
                    }).FirstOrDefaultAsync(e => e.FSampleGarmentRcvM.SGRID.Equals(srId));

            createFSampleGarmentRcvMViewModel.FSampleGarmentRcvM.EncryptedId = _protector.Protect(createFSampleGarmentRcvMViewModel.FSampleGarmentRcvM.SGRID.ToString());

            return createFSampleGarmentRcvMViewModel;
        }

        public async Task<RND_FABRICINFO> FindByFabCodeAsync(int fabCode)
        {
            return await DenimDbContext.RND_FABRICINFO
                .Include(e => e.WV.WEAVENavigation)
                .Include(e => e.COLORCODENavigation)
                .FirstOrDefaultAsync(e => e.FABCODE.Equals(fabCode));
        }

        public async Task<CreateFSampleGarmentRcvMViewModel> GetEmployeesByAsync(string search, int page = 1)
        {
            var createFSampleGarmentRcvMViewModel = new CreateFSampleGarmentRcvMViewModel();

            if (!string.IsNullOrEmpty(search))
            {
                createFSampleGarmentRcvMViewModel.FHrEmployees = await DenimDbContext.F_HRD_EMPLOYEE
                    .OrderBy(e => e.EMPNO)
                    .Select(e => new F_HRD_EMPLOYEE
                    {
                        EMPID = e.EMPID,
                        FIRST_NAME = string.Join("; ", e.EMPNO, string.Join(" ", e.FIRST_NAME, e.LAST_NAME)),
                    }).Where(e => e.FIRST_NAME.ToLower().Contains(search.ToLower())).ToListAsync();
            }
            else
            {
                createFSampleGarmentRcvMViewModel.FHrEmployees = await DenimDbContext.F_HRD_EMPLOYEE
                    .OrderBy(e => e.EMPNO)
                    .Select(e => new F_HRD_EMPLOYEE
                    {
                        EMPID = e.EMPID,
                        FIRST_NAME = string.Join("; ", e.EMPNO, string.Join(" ", e.FIRST_NAME, e.LAST_NAME)),
                    }).ToListAsync();
            }


            return createFSampleGarmentRcvMViewModel;
        }

        public async Task<CreateFSampleGarmentRcvMViewModel> GetSampleItemsByAsync(string search, int page = 1)
        {
            var createFSampleGarmentRcvMViewModel = new CreateFSampleGarmentRcvMViewModel();

            if (!string.IsNullOrEmpty(search))
            {
                createFSampleGarmentRcvMViewModel.FSampleItemDetailses = await DenimDbContext.F_SAMPLE_ITEM_DETAILS
                    .OrderBy(e => e.NAME)
                    .Select(e => new F_SAMPLE_ITEM_DETAILS
                    {
                        SITEMID = e.SITEMID,
                        NAME = e.NAME,
                    }).Where(e => e.NAME.ToLower().Contains(search.ToLower())).ToListAsync();
            }
            else
            {
                createFSampleGarmentRcvMViewModel.FSampleItemDetailses = await DenimDbContext.F_SAMPLE_ITEM_DETAILS
                    .OrderBy(e => e.NAME)
                    .Select(e => new F_SAMPLE_ITEM_DETAILS
                    {
                        SITEMID = e.SITEMID,
                        NAME = e.NAME,
                    }).ToListAsync();
            }
            
            return createFSampleGarmentRcvMViewModel;
        }

        public async Task<CreateFSampleGarmentRcvMViewModel> GetRndFabricsByAsync(string search, int page = 1)
        {
            var createFSampleGarmentRcvMViewModel = new CreateFSampleGarmentRcvMViewModel();

            if (!string.IsNullOrEmpty(search))
            {
                createFSampleGarmentRcvMViewModel.RndFabricinfos = await DenimDbContext.RND_FABRICINFO
                    .OrderBy(e => e.STYLE_NAME)
                    .Select(e => new RND_FABRICINFO
                    {
                        FABCODE = e.FABCODE,
                        STYLE_NAME = e.STYLE_NAME,
                    }).Where(e => e.STYLE_NAME.ToLower().Contains(search.ToLower())).ToListAsync();
            }
            else
            {
                createFSampleGarmentRcvMViewModel.RndFabricinfos = await DenimDbContext.RND_FABRICINFO
                    .OrderBy(e => e.STYLE_NAME)
                    .Select(e => new RND_FABRICINFO
                    {
                        FABCODE = e.FABCODE,
                        STYLE_NAME = e.STYLE_NAME,
                    }).ToListAsync();
            }

            return createFSampleGarmentRcvMViewModel;
        }

        public async Task<CreateFSampleGarmentRcvMViewModel> GetSectionsByAsync(string search, int page = 1)
        {
            var createFSampleGarmentRcvMViewModel = new CreateFSampleGarmentRcvMViewModel();

            if (!string.IsNullOrEmpty(search))
            {
                createFSampleGarmentRcvMViewModel.FBasSections = await DenimDbContext.F_BAS_SECTION
                    .OrderBy(e => e.SECNAME)
                    .Select(e => new F_BAS_SECTION
                    {
                        SECID = e.SECID,
                        SECNAME = e.SECNAME,
                    }).Where(e => e.SECNAME.ToLower().Contains(search.ToLower())).ToListAsync();
            }
            else
            {
                createFSampleGarmentRcvMViewModel.FBasSections = await DenimDbContext.F_BAS_SECTION
                    .OrderBy(e => e.SECNAME)
                    .Select(e => new F_BAS_SECTION
                    {
                        SECID = e.SECID,
                        SECNAME = e.SECNAME,
                    }).ToListAsync();
            }

            return createFSampleGarmentRcvMViewModel;
        }

        public async Task<CreateFSampleGarmentRcvMViewModel> GetDetailsFormInspectionByAsync(CreateFSampleGarmentRcvMViewModel createFSampleGarmentRcvMViewModel)
        {
            createFSampleGarmentRcvMViewModel.FSampleGarmentRcvDs.AddRange(await DenimDbContext.F_PR_INSPECTION_PROCESS_MASTER
                .Include(e => e.F_PR_INSPECTION_PROCESS_DETAILS)
                .ThenInclude(e => e.TROLLEYNONavigation.F_PR_INSPECTION_PROCESS_DETAILS)
                .Include(e => e.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.WV)
                .Include(e => e.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.COLORCODENavigation)
                .Where(e => e.INSPDATE.Equals(createFSampleGarmentRcvMViewModel.ProductionDate))
                .Select(e => new F_SAMPLE_GARMENT_RCV_D
                {
                    FABCODE = e.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.FABCODE,
                    COLORID = e.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.COLORCODENavigation.COLORCODE,
                    BUYERID = e.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.PIMASTER.BUYERID,
                    BUYER = e.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.PIMASTER.BUYER,
                    FABCODENavigation = new RND_FABRICINFO
                    {
                        FABCODE = e.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.FABCODE,
                        COLORCODENavigation = new BAS_COLOR
                        {
                            COLOR = $"{e.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.COLORCODENavigation.COLOR}"
                        },
                        WV = new RND_SAMPLE_INFO_WEAVING
                        {
                            FABCODE = $"{e.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.WV.FABCODE}"
                        }
                    },
                    QTY = e.F_PR_INSPECTION_PROCESS_DETAILS.Sum(f => f.TROLLEYNONavigation.F_PR_INSPECTION_PROCESS_DETAILS.Sum(g => g.LENGTH_YDS))
                }).ToListAsync());

            return createFSampleGarmentRcvMViewModel;
        }
    }
}
