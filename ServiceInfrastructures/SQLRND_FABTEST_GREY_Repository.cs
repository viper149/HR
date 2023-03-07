using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels.Rnd.Grey;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLRND_FABTEST_GREY_Repository : BaseRepository<RND_FABTEST_GREY>, IRND_FABTEST_GREY
    {
        private readonly IDataProtector _protector;

        public SQLRND_FABTEST_GREY_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<CreateRndFabTestGreyViewModel> GetInitObjects(CreateRndFabTestGreyViewModel createRndFabTestGreyViewModel)
        {
            try
            {
                // MAPPED WITH DB ROWS
                var shift = new Dictionary<int, string>
                {
                    {2, "P/A"},
                    {3, "P/B"},
                    {4, "P/B"}
                };

                createRndFabTestGreyViewModel.PlProductionSetdistributionsList = await DenimDbContext.PL_PRODUCTION_SETDISTRIBUTION
                    .Include(d => d.PROG_)
                    .Select(d => new PL_PRODUCTION_SETDISTRIBUTION
                    {
                        SETID = d.SETID,
                        PROG_ = new PL_BULK_PROG_SETUP_D
                        {
                            PROG_NO = d.PROG_.PROG_NO
                        }
                    }).ToListAsync();

                createRndFabTestGreyViewModel.FHrEmployees = await DenimDbContext.F_HRD_EMPLOYEE
                    .Select(d => new F_HRD_EMPLOYEE
                    {
                        EMPID = d.EMPID,
                        FIRST_NAME = $"{d.EMPNO} - {d.FIRST_NAME} {d.LAST_NAME}"
                    }).OrderBy(e => e.FIRST_NAME).ToListAsync();

                createRndFabTestGreyViewModel.Doffs = await DenimDbContext.F_PR_WEAVING_PROCESS_DETAILS_B
                    .Include(c => c.LOOM_NONavigation)
                    .Include(c => c.WV_BEAM.F_PR_SIZING_PROCESS_ROPE_DETAILS.W_BEAM)
                    .Include(c => c.WV_BEAM.F_PR_SLASHER_DYEING_DETAILS.W_BEAM)
                    .Include(c => c.WV_BEAM.WV_PROCESS.SET.PROG_)
                    .Select(c => new F_PR_WEAVING_PROCESS_DETAILS_B
                    {
                        TRNSID = c.TRNSID,
                        OPT1 = c.WV_BEAM.F_PR_SIZING_PROCESS_ROPE_DETAILS == null ? c.WV_BEAM.WV_PROCESS.SET.PROG_.PROG_NO + "-" + c.WV_BEAM.F_PR_SLASHER_DYEING_DETAILS.W_BEAM.BEAM_NO + " " + c.LOOM_NONavigation.LOOM_NO + " (Length-" + c.LENGTH_BULK + ")" : c.WV_BEAM.WV_PROCESS.SET.PROG_.PROG_NO + "-" + c.WV_BEAM.F_PR_SIZING_PROCESS_ROPE_DETAILS.W_BEAM.BEAM_NO + " " + c.LOOM_NONavigation.LOOM_NO + " (Length-" + c.LENGTH_BULK + ")"
                    })
                    .OrderByDescending(c => c.CREATED_AT)
                    .ToListAsync();


                createRndFabTestGreyViewModel.OrderRepeats =
                    await DenimDbContext.RND_ORDER_REPEAT.Select(c => new RND_ORDER_REPEAT
                    {
                        ORPTID = c.ORPTID,
                        ORPTNAME = c.ORPTNAME
                    }).OrderBy(c => c.ORPTNAME).ToListAsync();

                createRndFabTestGreyViewModel.FHrShiftInfoList = await DenimDbContext.F_HR_SHIFT_INFO
                    .Where(d => shift.ContainsKey(d.ID) || shift.ContainsValue(d.SHIFT))
                    .Select(d => new F_HR_SHIFT_INFO
                    {
                        ID = d.ID,
                        SHIFT = d.SHIFT
                    }).ToListAsync();

                return createRndFabTestGreyViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<PreviousDataRndFabTestGreyViewModel> FindBySdIdAsync(int sdId)
        {
            var result = await DenimDbContext.PL_SAMPLE_PROG_SETUP
                .Include(c => c.SD)
                .Include(c => c.RND_SAMPLE_INFO_WEAVING)
                .Where(c => c.TRNSID.Equals(sdId))
                .Select(c => new PreviousDataRndFabTestGreyViewModel
                {
                    RndSampleInfoDyeing = c.SD,
                    RndSampleInfoWeaving = c.RND_SAMPLE_INFO_WEAVING.FirstOrDefault(),
                })
                .FirstOrDefaultAsync();


            //var result = await _denimDbContext.RND_SAMPLE_INFO_DYEING.Where(e => e.SDID.Equals(sdId))
            //    .GroupJoin(_denimDbContext.RND_SAMPLE_INFO_WEAVING.Where(f => f.SDID.Equals(sdId)),
            //        f1 => f1.SDID,
            //        f2 => f2.SDID,
            //        (f1, f2) => new PreviousDataRndFabTestGreyViewModel
            //        {
            //            RndSampleInfoDyeing = f1,
            //            RndSampleInfoWeaving = f2.FirstOrDefault()
            //        })
            //    .FirstOrDefaultAsync();
            return result;
        }

        public async Task<IEnumerable<RND_FABTEST_GREY>> GetForDataTableByAsync()
        {
            try
            {
                var r = await DenimDbContext.RND_FABTEST_GREY
                    .Include(d => d.EMP_WASHEDBY)
                    .Include(d => d.EMP_UNWASHEDBY)
                    .Include(c=>c.PROGN.PROG_)
                    .Select(d => new RND_FABTEST_GREY
                    {
                        LAB_NO = d.LAB_NO,
                        LTGID = d.LTGID,
                        EncryptedId = _protector.Protect(d.LTGID.ToString()),
                        LTGDATE = d.LTGDATE,
                        OPTION1 = d.DEVELOPMENTNO,
                        EMP_WASHEDBY = new F_HRD_EMPLOYEE
                        {
                            FIRST_NAME = d.EMP_WASHEDBY.FIRST_NAME
                        },
                        EMP_UNWASHEDBY = new F_HRD_EMPLOYEE
                        {
                            FIRST_NAME = d.EMP_UNWASHEDBY.FIRST_NAME
                        },
                        PROGN = new PL_PRODUCTION_SETDISTRIBUTION
                        {
                            OPT1 = d.PROGN ==null && d.PROGN.PROG_==null?"": d.PROGN.PROG_.PROG_NO
                        }
                    }).ToListAsync();
                return r;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<CreateRndFabTestGreyViewModel> GetRndFabTestGreyWithDetailsByAsnc(int ltgId)
        {
            try
            {
                var createRndFabTestGreyViewModel = await DenimDbContext.RND_FABTEST_GREY
                .Include(e => e.PROG)
                .GroupJoin(DenimDbContext.RND_SAMPLE_INFO_WEAVING_DETAILS
                        .Include(e => e.COUNT)
                        .Include(e => e.COLORCODENavigation)
                        .Include(e => e.LOT)
                        .Include(e => e.SUPP)
                        .Include(e => e.YARN),
                    f1 => f1.PROG.SDID,
                    f2 => f2.SDID,
                    (f1, f2) => new CreateRndFabTestGreyViewModel
                    {
                        RndFabtestGrey = f1,
                        RndSampleInfoWeavingDetailses = f2.ToList()
                    })
                .GroupJoin(DenimDbContext.RND_SAMPLE_INFO_DETAILS
                        .Include(e => e.COUNT)
                        .Include(e => e.COLORCODENavigation)
                        .Include(e => e.LOT)
                        .Include(e => e.SUPP)
                        .Include(e => e.YARN),
                    f3 => f3.RndFabtestGrey.PROG.SDID,
                    f4 => f4.SDID,
                    (f3, f4) => new CreateRndFabTestGreyViewModel
                    {
                        RndFabtestGrey = f3.RndFabtestGrey,
                        RndSampleInfoWeavingDetailses = f3.RndSampleInfoWeavingDetailses,
                        RndSampleInfoDetailses = f4.ToList()
                    })
                .Where(e => e.RndFabtestGrey.LTGID.Equals(ltgId))
                .FirstOrDefaultAsync();

                createRndFabTestGreyViewModel.FHrEmployees = await DenimDbContext.F_HRD_EMPLOYEE.Select(e => new F_HRD_EMPLOYEE
                {
                    EMPID = e.EMPID,
                    FIRST_NAME = e.FIRST_NAME
                }).OrderBy(e => e.FIRST_NAME).ToListAsync();

                createRndFabTestGreyViewModel.RndFabtestGrey.EncryptedId = _protector.Protect(createRndFabTestGreyViewModel.RndFabtestGrey.LTGID.ToString());

                return createRndFabTestGreyViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<CreateRndFabTestGreyViewModel> GetInitObjectsForEditView(CreateRndFabTestGreyViewModel createRndFabTestGreyViewModel)
        {
            createRndFabTestGreyViewModel.RndSampleInfoDetailses = await DenimDbContext.RND_SAMPLE_INFO_DETAILS
                .Include(e => e.COUNT)
                .Include(e => e.COLORCODENavigation)
                .Include(e => e.LOT)
                .Include(e => e.SUPP)
                .Include(e => e.YARN)
                .Where(e => e.SDID.Equals(createRndFabTestGreyViewModel.RndFabtestGrey.PROG.SDID))
                .ToListAsync();

            createRndFabTestGreyViewModel.RndSampleInfoWeavingDetailses = await DenimDbContext
                .RND_SAMPLE_INFO_WEAVING_DETAILS
                .Include(e => e.COUNT)
                .Include(e => e.COLORCODENavigation)
                .Include(e => e.LOT)
                .Include(e => e.SUPP)
                .Include(e => e.YARN)
                .Where(e => e.SDID.Equals(createRndFabTestGreyViewModel.RndFabtestGrey.PROG.SDID))
                .ToListAsync();

            createRndFabTestGreyViewModel.FHrEmployees = await DenimDbContext.F_HRD_EMPLOYEE.ToListAsync();

            return createRndFabTestGreyViewModel;
        }

        public async Task<DetailsRndFabTestGreyViewModel> FindByLtgIdAsync(int ltgId)
        {
            var detailsRndFabTestGreyViewModels = await DenimDbContext.RND_FABTEST_GREY
                .Include(e => e.PROG)
                .GroupJoin(DenimDbContext.RND_SAMPLE_INFO_WEAVING_DETAILS
                        .Include(e => e.COUNT)
                        .Include(e => e.COLORCODENavigation)
                        .Include(e => e.LOT)
                        .Include(e => e.SUPP)
                        .Include(e => e.YARN),
                    f1 => f1.PROG.SDID,
                    f2 => f2.SDID,
                    (f1, f2) => new
                    {
                        F1 = f1,
                        F2 = f2.ToList()
                    })
                .GroupJoin(DenimDbContext.RND_SAMPLE_INFO_DETAILS
                        .Include(e => e.COUNT)
                        .Include(e => e.COLORCODENavigation)
                        .Include(e => e.LOT)
                        .Include(e => e.SUPP)
                        .Include(e => e.YARN),
                    f3 => f3.F1.PROG.SDID,
                    f4 => f4.SDID,
                    (f3, f4) => new DetailsRndFabTestGreyViewModel
                    {
                        RndFabtestGrey = f3.F1,
                        RndSampleInfoWeavingDetailses = f3.F2,
                        RndSampleInfoDetailses = f4.ToList()
                    })
                .Where(e => e.RndFabtestGrey.LTGID.Equals(ltgId))
                .FirstOrDefaultAsync();

            detailsRndFabTestGreyViewModels.RndFabtestGrey.EncryptedId = _protector.Protect(detailsRndFabTestGreyViewModels.RndFabtestGrey.LTGID.ToString());

            return detailsRndFabTestGreyViewModels;
        }

        public async Task<bool> FindByLabNo(string labNo)
        {
            return !await DenimDbContext.RND_FABTEST_GREY.AnyAsync(d => d.LAB_NO.Equals(labNo));
        }
    }
}
