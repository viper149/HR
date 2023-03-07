using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLRND_FABTEST_BULK_Repository : BaseRepository<RND_FABTEST_BULK>, IRND_FABTEST_BULK
    {
        private readonly IDataProtector _protector;
        public SQLRND_FABTEST_BULK_Repository(DenimDbContext denimDbContext, IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<IEnumerable<RND_FABTEST_BULK>> GetAllRndFabTestBulkAsync()
        {
            return await DenimDbContext.RND_FABTEST_BULK
                    .Include(d => d.PROG.PROG_)
                    .Include(d => d.FINPROC.TROLLNONavigation)
                    .Include(d => d.PROG.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation)
                    .Include(d => d.PROG.PROG_.BLK_PROG_.RndProductionOrder.SO.PIMASTER.BUYER)
                    .Select(d => new RND_FABTEST_BULK
                    {
                        LTBID = d.LTBID,
                        EncryptedId = _protector.Protect(d.LTBID.ToString()),
                        LAB_NO = d.LAB_NO,
                        LTB_DATE = d.LTB_DATE,
                        REMARKS = d.REMARKS,
                        PROG = new PL_PRODUCTION_SETDISTRIBUTION
                        {
                            PROG_ = new PL_BULK_PROG_SETUP_D
                            {
                                PROG_NO = d.PROG.PROG_.PROG_NO,
                                BLK_PROG_ = new PL_BULK_PROG_SETUP_M
                                {
                                    RndProductionOrder = new RND_PRODUCTION_ORDER
                                    {
                                        SO = new COM_EX_PI_DETAILS
                                        {
                                            STYLE = new COM_EX_FABSTYLE
                                            {
                                                FABCODENavigation = new RND_FABRICINFO
                                                {
                                                    STYLE_NAME = d.PROG.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.STYLE_NAME
                                                }
                                            },
                                            PIMASTER = new COM_EX_PIMASTER
                                            {
                                                BUYER = new BAS_BUYERINFO
                                                {
                                                    BUYER_NAME = d.PROG.PROG_.BLK_PROG_.RndProductionOrder.SO.PIMASTER.BUYER.BUYER_NAME
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        },
                        FINPROC = new F_PR_FINISHING_FNPROCESS
                        {
                            TROLLNONavigation = new F_PR_FIN_TROLLY
                            {
                                NAME = $"{d.FINPROC.TROLLNONavigation.NAME} - {d.FINPROC.LENGTH_OUT} mtr"
                            }
                        }
                    }).ToListAsync();
        }

        public async Task<RndFabTestBulkViewModel> GetInitObjByAsync(RndFabTestBulkViewModel rndFabTestBulkViewModel)
        {
            // MAPPED WITH DB ROWS
            var shift = new Dictionary<int, string>
                {
                    {2, "P/A"},
                    {3, "P/B"},
                    {4, "P/B"}
                };
            rndFabTestBulkViewModel.RndProductionOrderList = await DenimDbContext.RND_PRODUCTION_ORDER
                .Include(d => d.SO)
                .Select(d => new RND_PRODUCTION_ORDER
                {
                    POID = d.POID,
                    SO = new COM_EX_PI_DETAILS
                    {
                        SO_NO = d.SO.SO_NO
                    }
                }).ToListAsync();

            rndFabTestBulkViewModel.PlProductionSetdistributionList = await DenimDbContext.PL_PRODUCTION_SETDISTRIBUTION
                .Include(d => d.PROG_)
                .Select(d => new PL_PRODUCTION_SETDISTRIBUTION
                {
                    SETID = d.SETID,
                    PROG_ = new PL_BULK_PROG_SETUP_D
                    {
                        PROG_NO = d.PROG_.PROG_NO
                    }
                }).ToListAsync();

            rndFabTestBulkViewModel.FPrFinishingFnprocessList = await DenimDbContext.F_PR_FINISHING_FNPROCESS
                    .Include(c=>c.FN_MACHINE)
                    .Include(c=>c.FN_PROCESS.DOFF.LOOM_NONavigation)
                    .Include(d => d.TROLLNONavigation)
                    .Where(c => c.FIN_PRO_TYPEID.Equals(17))
                    .Select(d => new F_PR_FINISHING_FNPROCESS
                    {
                        FIN_PROCESSID = d.FIN_PROCESSID,
                        TROLLNONavigation = new F_PR_FIN_TROLLY
                        {
                            NAME = $"{d.TROLLNONavigation.NAME} - {d.LENGTH_OUT} mtr"
                        }
                    }).ToListAsync();

            var FHrEmployees = await DenimDbContext.F_HR_EMP_OFFICIALINFO
                .Include(d => d.EMP)
                .Where(d => d.SECID.Equals(168) && !d.OPN2.Equals("Y"))  //PHYSICAL LAB
                .ToListAsync();

            rndFabTestBulkViewModel.FHrEmployeeList = FHrEmployees
                .Select(d => new F_HRD_EMPLOYEE
                {
                    EMPID = d.EMP.EMPID,
                    FIRST_NAME = $"{d.EMP.EMPNO} - {d.EMP.FIRST_NAME} {d.EMP.LAST_NAME}"
                }).ToList();

            //rndFabTestBulkViewModel.FHrEmployeeList = await _denimDbContext.F_HRD_EMPLOYEE
            //    .Select(d => new F_HRD_EMPLOYEE
            //    {
            //        EMPID = d.EMPID,
            //        FIRST_NAME = $"{d.FIRST_NAME} {d.LAST_NAME}"
            //    }).ToListAsync();

            rndFabTestBulkViewModel.FHrShiftInfoList = await DenimDbContext.F_HR_SHIFT_INFO
                .Where(d => shift.ContainsKey(d.ID) || shift.ContainsValue(d.SHIFT))
                .Select(d => new F_HR_SHIFT_INFO
                {
                    ID = d.ID,
                    SHIFT = d.SHIFT
                }).ToListAsync();

            rndFabTestBulkViewModel.FBasTestmethodList = await DenimDbContext.F_BAS_TESTMETHOD
                .Select(d => new F_BAS_TESTMETHOD
                {
                    TMID = d.TMID,
                    TMNAME = d.TMNAME
                }).ToListAsync();

            return rndFabTestBulkViewModel;
        }
        public async Task<bool> FindByLabNo(string labNo)
        {
            return !await DenimDbContext.RND_FABTEST_BULK.AnyAsync(d => d.LAB_NO.Equals(labNo));
        }

        public async Task<PL_PRODUCTION_SETDISTRIBUTION> GetSetDetailsAsync(int setId)
        {
            return await DenimDbContext.PL_PRODUCTION_SETDISTRIBUTION
                .Include(d => d.F_PR_FINISHING_PROCESS_MASTER)
                .ThenInclude(d => d.F_PR_FINISHING_FNPROCESS)
                .ThenInclude(d => d.TROLLNONavigation)
                .Include(d => d.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation)
                .Include(d => d.PROG_.BLK_PROG_.RndProductionOrder.SO.PIMASTER.BUYER)
                .Where(d => d.SETID.Equals(setId))
                .Select(d => new PL_PRODUCTION_SETDISTRIBUTION
                {
                    F_PR_FINISHING_PROCESS_MASTER = d.F_PR_FINISHING_PROCESS_MASTER.Select(f => new F_PR_FINISHING_PROCESS_MASTER
                    {
                        F_PR_FINISHING_FNPROCESS = f.F_PR_FINISHING_FNPROCESS
                            .Where(g => g.FIN_PRO_TYPEID.Equals(17)).Select(g => new F_PR_FINISHING_FNPROCESS
                            {
                                FIN_PROCESSID = g.FIN_PROCESSID,
                                TROLLNONavigation = new F_PR_FIN_TROLLY
                                {
                                    NAME = $"{g.TROLLNONavigation.NAME} - {g.LENGTH_OUT} mtr"
                                }
                            }).ToList()
                    }).ToList(),
                    PROG_ = new PL_BULK_PROG_SETUP_D
                    {
                        BLK_PROG_ = new PL_BULK_PROG_SETUP_M
                        {
                            RndProductionOrder = new RND_PRODUCTION_ORDER
                            {
                                SO = new COM_EX_PI_DETAILS
                                {
                                    SO_NO = d.PROG_.BLK_PROG_.RndProductionOrder.SO.SO_NO,
                                    PIMASTER = new COM_EX_PIMASTER
                                    {
                                        BUYER = new BAS_BUYERINFO
                                        {
                                            BUYER_NAME = d.PROG_.BLK_PROG_.RndProductionOrder.SO.PIMASTER.BUYER.BUYER_NAME
                                        }
                                    },
                                    STYLE = new COM_EX_FABSTYLE
                                    {
                                        FABCODENavigation = new RND_FABRICINFO
                                        {
                                            STYLE_NAME = d.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.STYLE_NAME,
                                            FINISH_ROUTE = d.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.FINISH_ROUTE,
                                            FNEPI = d.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.FNEPI,
                                            FNPPI = d.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.FNPPI,
                                            SRDECWARP= d.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.SRDECWARP,
                                            SRDECWEFT = d.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.SRDECWEFT,
                                            STDECWEFT = d.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.STDECWEFT,
                                            WIDEC = d.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.WIDEC,
                                            WGDEC = d.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.WGDEC,
                                            GRDECWARP= d.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.GRDECWARP,
                                            GRDECWEFT = d.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.GRDECWEFT

                                        }
                                    },
                                }
                            }
                        }
                    }
                }).FirstOrDefaultAsync();
        }

        public async Task<F_PR_FINISHING_FNPROCESS> GetFnProcessDetailsAsync(int id)
        {
            return await DenimDbContext.F_PR_FINISHING_FNPROCESS
                .Include(d => d.FN_PROCESS.DOFF.LOOM_NONavigation)
                .Include(d => d.FN_MACHINE)
                .Where(d => d.FIN_PROCESSID.Equals(id))
                .Select(d => new F_PR_FINISHING_FNPROCESS
                {
                    LENGTH_OUT = d.LENGTH_OUT,
                    FN_PROCESS = new F_PR_FINISHING_PROCESS_MASTER
                    {
                        DOFF = new F_PR_WEAVING_PROCESS_DETAILS_B
                        {
                            LOOM_NONavigation = new F_LOOM_MACHINE_NO
                            {
                                LOOM_NO = d.FN_PROCESS.DOFF.LOOM_NONavigation.LOOM_NO
                            }
                        }
                    },
                    FN_MACHINE = new F_PR_FN_MACHINE_INFO
                    {
                        NAME = d.FN_MACHINE.NAME
                    }
                }).FirstOrDefaultAsync();
        }
    }
}
