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
    public class SQLF_FS_CLEARANCE_MASTER_SAMPLE_ROLL_Repository : BaseRepository<F_FS_CLEARANCE_MASTER_SAMPLE_ROLL>, IF_FS_CLEARANCE_MASTER_SAMPLE_ROLL
    {
        private readonly IDataProtector _protector;

        public SQLF_FS_CLEARANCE_MASTER_SAMPLE_ROLL_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<FFsClearanceMasterSampleRollViewModel> GetInitObjByAsync(FFsClearanceMasterSampleRollViewModel fFsClearanceMasterSampleRollViewModel)
        {
            fFsClearanceMasterSampleRollViewModel.PlProductionSetdistributionsList = await DenimDbContext.PL_PRODUCTION_SETDISTRIBUTION
                .Include(d => d.PROG_)
                .Where(d=>d.F_PR_INSPECTION_PROCESS_MASTER.Any())
                .Select(d => new PL_PRODUCTION_SETDISTRIBUTION
                {
                    SETID = d.SETID,
                    PROG_ = new PL_BULK_PROG_SETUP_D
                    {
                        PROG_NO = d.PROG_.PROG_NO
                    }
                }).ToListAsync();

            fFsClearanceMasterSampleRollViewModel.FFsClearanceWashTypeList = await DenimDbContext
                .F_FS_CLEARANCE_WASH_TYPE
                .Select(d => new F_FS_CLEARANCE_WASH_TYPE
                {
                    WTID = d.WTID,
                    WTNAME = d.WTNAME
                }).ToListAsync();

            fFsClearanceMasterSampleRollViewModel.FFsClearanceRollTypeList = await DenimDbContext
                .F_FS_CLEARANCE_ROLL_TYPE
                .Select(d => new F_FS_CLEARANCE_ROLL_TYPE
                {
                    RTID = d.RTID,
                    RTNAME = d.RTNAME
                }).ToListAsync();

            fFsClearanceMasterSampleRollViewModel.FPrInspectionProcessDetailseList = await DenimDbContext
                .F_PR_INSPECTION_PROCESS_DETAILS
                .Select(d => new F_PR_INSPECTION_PROCESS_DETAILS
                {
                    ROLL_ID = d.ROLL_ID,
                    ROLLNO = d.ROLLNO
                }).ToListAsync();

            return fFsClearanceMasterSampleRollViewModel;
        }

        public async Task<IEnumerable<F_FS_CLEARANCE_MASTER_SAMPLE_ROLL>> GetAllClearanceMasterSampleRollAsync()
        {
            return await DenimDbContext.F_FS_CLEARANCE_MASTER_SAMPLE_ROLL
                .Include(d => d.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation)
                .Include(d => d.ROLL)
                .Include(d => d.RT)
                .Include(d => d.WT)
                .Select(d => new F_FS_CLEARANCE_MASTER_SAMPLE_ROLL
                {
                    MSRID = d.MSRID,
                    EncryptedId = _protector.Protect(d.MSRID.ToString()),
                    MSRDATE = d.MSRDATE,
                    MAILDATE = d.MAILDATE,
                    SET = new PL_PRODUCTION_SETDISTRIBUTION
                    {
                        PROG_ = new PL_BULK_PROG_SETUP_D
                        {
                            PROG_NO = d.SET.PROG_.PROG_NO,
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
                                                STYLE_NAME = d.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.STYLE_NAME
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    },
                    ROLL = new F_PR_INSPECTION_PROCESS_DETAILS
                    {
                        ROLLNO = d.ROLL.ROLLNO
                    },
                    RT = new F_FS_CLEARANCE_ROLL_TYPE
                    {
                        RTNAME = d.RT.RTNAME
                    },
                    WT = new F_FS_CLEARANCE_WASH_TYPE
                    {
                        WTNAME = d.WT.WTNAME
                    }
                }).ToListAsync();
        }

        public async Task<F_PR_INSPECTION_PROCESS_DETAILS> GetRollDetailsAsync(int rollId)
        {
            return await DenimDbContext.F_PR_INSPECTION_PROCESS_DETAILS
                .Where(d => d.ROLL_ID.Equals(rollId))
                    .Select(d => new F_PR_INSPECTION_PROCESS_DETAILS
                    {
                        BATCH = d.BATCH,
                        LENGTH_YDS = d.LENGTH_YDS
                    }).FirstOrDefaultAsync();
        }

        public async Task<PL_PRODUCTION_SETDISTRIBUTION> GetSetDetailsAsync(int setId)
        {
            var result = await DenimDbContext.PL_PRODUCTION_SETDISTRIBUTION
                    .Include(d => d.F_PR_INSPECTION_PROCESS_MASTER)
                    .ThenInclude(d => d.F_PR_INSPECTION_PROCESS_DETAILS)
                    .Include(d => d.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.RND_WEAVE)
                    .Include(d => d.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.COLORCODENavigation)
                    .Include(d => d.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.FABTEST)
                    .Include(d => d.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.RND_FABRIC_COUNTINFO)
                    .ThenInclude(c=>c.COUNT)
                    .Where(d => d.SETID.Equals(setId))
                    .Select(d => new PL_PRODUCTION_SETDISTRIBUTION
                    {
                        F_PR_INSPECTION_PROCESS_MASTER = d.F_PR_INSPECTION_PROCESS_MASTER.Select(f => new F_PR_INSPECTION_PROCESS_MASTER
                        {
                            F_PR_INSPECTION_PROCESS_DETAILS = f.F_PR_INSPECTION_PROCESS_DETAILS.Select(g => new F_PR_INSPECTION_PROCESS_DETAILS
                            {
                                ROLL_ID = g.ROLL_ID,
                                ROLLNO = g.ROLLNO
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
                                        STYLE = new COM_EX_FABSTYLE
                                        {
                                            FABCODENavigation = new RND_FABRICINFO
                                            {
                                                STYLE_NAME = d.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.STYLE_NAME,
                                                FINISH_ROUTE = d.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.FINISH_ROUTE,
                                                FNEPI = d.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.FNEPI,
                                                FNPPI = d.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.FNPPI,
                                                WIDEC = d.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.WIDEC,
                                                WGDEC = d.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.WGDEC,
                                                SRDECWARP = d.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.SRDECWARP,
                                                SRDECWEFT = d.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.SRDECWEFT,
                                                STDECWARP = d.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.STDECWARP,
                                                STDECWEFT = d.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.STDECWEFT,
                                                SPR_A_DEC = d.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.SPR_A_DEC,
                                                SPR_B_DEC = d.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.SPR_B_DEC,
                                                RND_FABRIC_COUNTINFO = d.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.RND_FABRIC_COUNTINFO.Select(c=>new RND_FABRIC_COUNTINFO
                                                {
                                                    COUNT = new BAS_YARN_COUNTINFO
                                                    {
                                                        RND_COUNTNAME = c.COUNT.RND_COUNTNAME
                                                    },
                                                    YARNFOR = c.YARNFOR
                                                }).ToList()
                                                ,
                                                RND_WEAVE = new RND_WEAVE()
                                                {
                                                    NAME = d.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.RND_WEAVE.NAME
                                                },
                                                COLORCODENavigation = new BAS_COLOR
                                                {
                                                    COLOR = d.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.COLORCODENavigation.COLOR
                                                },
                                                FABTEST = new RND_FABTEST_SAMPLE
                                                {
                                                    SPIRALITY_A = d.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.FABTEST.SPIRALITY_A,
                                                    SPIRALIRTY_B = d.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.FABTEST.SPIRALIRTY_B
                                                }
                                            }
                                        },
                                    }
                                }
                            }
                        }
                    }).FirstOrDefaultAsync();



            result.OPT1 = string.Join(" + ",
                result.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.RND_FABRIC_COUNTINFO.Where(c => c.YARNFOR.Equals(1))
                    .Select(p => p.COUNT.RND_COUNTNAME));
            result.OPT2 = string.Join(" + ",
                result.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.RND_FABRIC_COUNTINFO.Where(c => c.YARNFOR.Equals(2))
                    .Select(p => p.COUNT.RND_COUNTNAME));
            result.OPT1 =
                $"{result.OPT1} X {result.OPT2} / {result.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.FNEPI}X{result.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.FNPPI}";

            return result;
        }
    }
}
