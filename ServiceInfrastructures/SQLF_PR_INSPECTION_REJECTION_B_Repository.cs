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
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_PR_INSPECTION_REJECTION_B_Repository : BaseRepository<F_PR_INSPECTION_REJECTION_B>, IF_PR_INSPECTION_REJECTION_B
    {
        private readonly IDataProtector _protector;

        public SQLF_PR_INSPECTION_REJECTION_B_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<IEnumerable<F_PR_INSPECTION_REJECTION_B>> GetAllFPrInspectionRejectionBAsync()
        {
            return await DenimDbContext.F_PR_INSPECTION_REJECTION_B
                .Include(d => d.DOFF_.LOOM_NONavigation)
                .Include(d => d.DOFF_.WV_BEAM.WV_PROCESS.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation)
                .Include(d => d.DEFECT_)
                .Include(d => d.SHIFTNavigation)
                .Include(d => d.SECTION_)
                .Select(d => new F_PR_INSPECTION_REJECTION_B
                {
                    IBR_ID = d.IBR_ID,
                    EncryptedId = _protector.Protect(d.IBR_ID.ToString()),
                    TRANS_DATE = d.TRANS_DATE,
                    DOFFING_DATE = d.DOFFING_DATE,
                    DOFFING_LENGTH = d.DOFFING_LENGTH,
                    REDECTION_YDS = d.REDECTION_YDS,
                    REMARKS = d.REMARKS,
                    DOFF_ = new F_PR_WEAVING_PROCESS_DETAILS_B
                    {
                        WV_BEAM = new F_PR_WEAVING_PROCESS_BEAM_DETAILS_B
                        {
                            WV_PROCESS = new F_PR_WEAVING_PROCESS_MASTER_B
                            {
                                SET = new PL_PRODUCTION_SETDISTRIBUTION
                                {
                                    PROG_ = new PL_BULK_PROG_SETUP_D
                                    {
                                        PROG_NO = d.DOFF_.WV_BEAM.WV_PROCESS.SET.PROG_.PROG_NO,
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
                                                            STYLE_NAME = d.DOFF_.WV_BEAM.WV_PROCESS.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.STYLE_NAME
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        },
                        LOOM_NONavigation = new F_LOOM_MACHINE_NO
                        {
                            LOOM_NO = d.DOFF_.LOOM_NONavigation.LOOM_NO
                        }
                    },
                    DEFECT_ = new F_PR_INSPECTION_DEFECTINFO
                    {
                        NAME = d.DEFECT_.NAME
                    },
                    SHIFTNavigation = new F_HR_SHIFT_INFO
                    {
                        SHIFT = d.SHIFTNavigation.SHIFT
                    },
                    SECTION_ = new F_BAS_SECTION
                    {
                        SECNAME = d.SECTION_.SECNAME
                    }
                }).ToListAsync();
        }

        public async Task<FPrInspectionRejectionBViewModel> GetInitObjByAsync(FPrInspectionRejectionBViewModel fPrInspectionRejectionBViewModel)
        {

            int[] x = new[] { 163, 166, 135, 159, 160, 158, 179, 171, 161, 189 };
            int[] shift = new[] {2,3,4 };

            fPrInspectionRejectionBViewModel.FPrInspectionDefectinfoList = await DenimDbContext
                .F_PR_INSPECTION_DEFECTINFO
                .Select(d => new F_PR_INSPECTION_DEFECTINFO
                {
                    DEF_TYPEID = d.DEF_TYPEID,
                    NAME = $"{d.CODE} - {d.NAME}"
                }).ToListAsync();
            fPrInspectionRejectionBViewModel.FHrShiftInfoList = await DenimDbContext.F_HR_SHIFT_INFO
                .Where(c => shift.Contains(c.ID))
                .Select(d => new F_HR_SHIFT_INFO
                {
                    ID = d.ID,
                    SHIFT = d.SHIFT
                }).ToListAsync();
            fPrInspectionRejectionBViewModel.FBasSectionsList = await DenimDbContext.F_BAS_SECTION
                .Where(c=>x.Contains(c.SECID))
                .Select(d => new F_BAS_SECTION
                {
                    SECID = d.SECID,
                    SECNAME = d.SECNAME
                }).ToListAsync();
            fPrInspectionRejectionBViewModel.StyleSetLoomList = await DenimDbContext.F_PR_WEAVING_PROCESS_DETAILS_B
                .Include(c=>c.WV_BEAM.WV_PROCESS.SET.F_PR_INSPECTION_PROCESS_MASTER)
                .Include(d => d.LOOM_NONavigation)
                .Include(d => d.WV_BEAM.WV_PROCESS.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation)
                .Where(c=> DenimDbContext.F_PR_FINISHING_PROCESS_MASTER.Any(m=>m.DOFF_ID.Equals(c.TRNSID)) && c.WV_BEAM.WV_PROCESS.SET.F_PR_INSPECTION_PROCESS_MASTER.Any(v=>v.INSPDATE.Equals(fPrInspectionRejectionBViewModel.FPrInspectionRejectionB.SearchDate)))
                .Select(d => new TypeTableViewModel()
                {
                    ID = d.TRNSID,
                    Name = $"{d.WV_BEAM.WV_PROCESS.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.STYLE_NAME}  ---  {d.WV_BEAM.WV_PROCESS.SET.PROG_.PROG_NO}  ---  {d.LOOM_NONavigation.LOOM_NO} ({d.LENGTH_BULK})"
                }).ToListAsync();

            fPrInspectionRejectionBViewModel.StyleSetLoomListEdit = await DenimDbContext.F_PR_WEAVING_PROCESS_DETAILS_B
                .Include(c=>c.WV_BEAM.WV_PROCESS.SET.F_PR_INSPECTION_PROCESS_MASTER)
                .Include(d => d.LOOM_NONavigation)
                .Include(d => d.WV_BEAM.WV_PROCESS.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation)
                .Where(c=> DenimDbContext.F_PR_FINISHING_PROCESS_MASTER.Any(m=>m.DOFF_ID.Equals(c.TRNSID)))
                .Select(d => new TypeTableViewModel()
                {
                    ID = d.TRNSID,
                    Name = $"{d.WV_BEAM.WV_PROCESS.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.STYLE_NAME}  ---  {d.WV_BEAM.WV_PROCESS.SET.PROG_.PROG_NO}  ---  {d.LOOM_NONavigation.LOOM_NO} ({d.LENGTH_BULK})"
                }).ToListAsync();
            return fPrInspectionRejectionBViewModel;
        }

        public async Task<FPrInspectionRejectionBViewModel> GetDoffByInspectionDate(FPrInspectionRejectionBViewModel fPrInspectionRejectionBViewModel)
        {
            if (fPrInspectionRejectionBViewModel.FPrInspectionRejectionB.SearchDate!=null)
            {
                fPrInspectionRejectionBViewModel.StyleSetLoomList = await DenimDbContext.F_PR_WEAVING_PROCESS_DETAILS_B
                    .Include(c => c.WV_BEAM.WV_PROCESS.SET.F_PR_INSPECTION_PROCESS_MASTER)
                    .Include(d => d.LOOM_NONavigation)
                    .Include(d => d.WV_BEAM.WV_PROCESS.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation)
                    .Where(c => DenimDbContext.F_PR_FINISHING_PROCESS_MASTER.Any(m => m.DOFF_ID.Equals(c.TRNSID)) && c.WV_BEAM.WV_PROCESS.SET.F_PR_INSPECTION_PROCESS_MASTER.Any(v => v.INSPDATE.Equals(fPrInspectionRejectionBViewModel.FPrInspectionRejectionB.SearchDate)))
                    .Select(d => new TypeTableViewModel()
                    {
                        ID = d.TRNSID,
                        Name = $"{d.WV_BEAM.WV_PROCESS.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.STYLE_NAME}  ---  {d.WV_BEAM.WV_PROCESS.SET.PROG_.PROG_NO}  ---  {d.LOOM_NONavigation.LOOM_NO} ({d.LENGTH_BULK})"
                    }).ToListAsync();
            }
            else
            {

                fPrInspectionRejectionBViewModel.StyleSetLoomList = await DenimDbContext.F_PR_WEAVING_PROCESS_DETAILS_B
                    .Include(c => c.WV_BEAM.WV_PROCESS.SET.F_PR_INSPECTION_PROCESS_MASTER)
                    .Include(d => d.LOOM_NONavigation)
                    .Include(d => d.WV_BEAM.WV_PROCESS.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation)
                    .Where(c => DenimDbContext.F_PR_FINISHING_PROCESS_MASTER.Any(m => m.DOFF_ID.Equals(c.TRNSID)))
                    .Select(d => new TypeTableViewModel()
                    {
                        ID = d.TRNSID,
                        Name = $"{d.WV_BEAM.WV_PROCESS.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.STYLE_NAME}  ---  {d.WV_BEAM.WV_PROCESS.SET.PROG_.PROG_NO}  ---  {d.LOOM_NONavigation.LOOM_NO} ({d.LENGTH_BULK})"
                    }).ToListAsync();
            }
            return fPrInspectionRejectionBViewModel;
        }

        public async Task<F_PR_WEAVING_PROCESS_DETAILS_B> GetAllBywdIdAsync(int wdId)
        {
            try
            {
                var r = await DenimDbContext.F_PR_WEAVING_PROCESS_DETAILS_B
                    .Include(d => d.F_PR_FINISHING_PROCESS_MASTER)
                    .ThenInclude(d => d.FABRICINFO.COLORCODENavigation)
                    .Include(d => d.F_PR_FINISHING_PROCESS_MASTER)
                    .ThenInclude(d => d.F_PR_FINISHING_FNPROCESS)
                    .ThenInclude(d => d.PROCESS_BYNavigation)
                    .Include(d => d.F_PR_FINISHING_PROCESS_MASTER)
                    .ThenInclude(d => d.F_PR_FINISHING_FNPROCESS)
                    .ThenInclude(d => d.FN_MACHINE)
                    .Include(c=>c.LOOM_NONavigation)
                    .Where(d => d.TRNSID.Equals(wdId))
                    .Select(d => new F_PR_WEAVING_PROCESS_DETAILS_B
                    {
                        DOFF_TIME = d.DOFF_TIME,
                        LENGTH_BULK = d.LENGTH_BULK,
                        LOOM_NONavigation = new F_LOOM_MACHINE_NO
                        {
                            LOOM_NO = d.LOOM_NONavigation.LOOM_NO
                        },
                        WV_BEAM = new F_PR_WEAVING_PROCESS_BEAM_DETAILS_B
                        {
                            WV_PROCESS = new F_PR_WEAVING_PROCESS_MASTER_B
                            {
                                SET = new PL_PRODUCTION_SETDISTRIBUTION
                                {
                                    PROG_ = new PL_BULK_PROG_SETUP_D
                                    {
                                        PROG_NO = d.WV_BEAM.WV_PROCESS.SET.PROG_.PROG_NO
                                    }
                                }
                            }
                        },
                        F_PR_FINISHING_PROCESS_MASTER = d.F_PR_FINISHING_PROCESS_MASTER
                            .Where(f => f.F_PR_FINISHING_FNPROCESS.Count > 0)
                            .Select(f => new F_PR_FINISHING_PROCESS_MASTER
                            {
                                FABRICINFO = new RND_FABRICINFO
                                {
                                    COLORCODENavigation = f.FABRICINFO != null ? new BAS_COLOR
                                    {
                                        COLOR = f.FABRICINFO.COLORCODENavigation.COLOR
                                    } : new BAS_COLOR()
                                },
                                F_PR_FINISHING_FNPROCESS = f.F_PR_FINISHING_FNPROCESS
                                .Where(g => g.FIN_PRO_TYPEID.Equals(17))
                                .Select(g => new F_PR_FINISHING_FNPROCESS
                                {
                                    PROCESS_BYNavigation = new F_HRD_EMPLOYEE
                                    {
                                        FIRST_NAME = $"{g.PROCESS_BYNavigation.FIRST_NAME} {g.PROCESS_BYNavigation.LAST_NAME}"
                                    },
                                    FN_MACHINE = new F_PR_FN_MACHINE_INFO
                                    {
                                        NAME = g.FN_MACHINE.NAME
                                    },
                                    FIN_PROCESSDATE = g.FIN_PROCESSDATE
                                }).ToList()
                            }).ToList()
                    }).FirstOrDefaultAsync();

                return r;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
