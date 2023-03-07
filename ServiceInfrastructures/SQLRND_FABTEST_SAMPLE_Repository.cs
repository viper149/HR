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
    public class SQLRND_FABTEST_SAMPLE_Repository : BaseRepository<RND_FABTEST_SAMPLE>, IRND_FABTEST_SAMPLE
    {
        private readonly IDataProtector _protector;

        public SQLRND_FABTEST_SAMPLE_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<RndFabTestSampleViewModel> GetInitObjects(RndFabTestSampleViewModel rndFabTestSampleViewModel)
        {
            // MAPPED WITH DB ROWS
            var shift = new Dictionary<int, string>
                {
                    {2, "P/A"},
                    {3, "P/B"},
                    {4, "P/B"}
                };
            //rndFabTestSampleViewModel.RndFabtestSample.LTSDATE = DateTime.Now;
            var FHrEmployees = await DenimDbContext.F_HR_EMP_OFFICIALINFO
                .Include(d => d.EMP)
                .Where(d => d.SECID.Equals(168) && !d.OPN2.Equals("Y"))  //PHYSICAL LAB
                .ToListAsync();

            rndFabTestSampleViewModel.FHrEmployees = FHrEmployees
                .Select(d => new F_HRD_EMPLOYEE
                {
                    EMPID = d.EMP.EMPID,
                    FIRST_NAME = $"{d.EMP.EMPNO} - {d.EMP.FIRST_NAME} {d.EMP.LAST_NAME}"
                }).ToList();

            rndFabTestSampleViewModel.RndSampleInfoFinishings = await DenimDbContext.RND_SAMPLEINFO_FINISHING
                .Include(c => c.LTG.PROG)
                //.Where(c => !_denimDbContext.RND_FABTEST_SAMPLE.Any(g => g.SFINID.Equals(c.SFINID)))
                .Select(e => new RND_SAMPLEINFO_FINISHING
                {
                    SFINID = e.SFINID,
                    STYLE_NAME = e.STYLE_NAME
                        //- { e.LTG.PROG.PROG_NO }
                    }).OrderBy(e => e.STYLE_NAME)
                .ToListAsync();
            rndFabTestSampleViewModel.Trollies = await DenimDbContext.F_PR_FIN_TROLLY
                .Select(d => new F_PR_FIN_TROLLY
                {
                    FIN_TORLLY_ID = d.FIN_TORLLY_ID,
                    NAME = d.NAME
                }).ToListAsync();
            rndFabTestSampleViewModel.FHrShiftInfoList = await DenimDbContext.F_HR_SHIFT_INFO
                .Where(d => shift.ContainsKey(d.ID) || shift.ContainsValue(d.SHIFT))
                .Select(d => new F_HR_SHIFT_INFO
                {
                    ID = d.ID,
                    SHIFT = d.SHIFT
                }).ToListAsync();

            rndFabTestSampleViewModel.FBasTestmethodList = await DenimDbContext.F_BAS_TESTMETHOD
                .Select(d => new F_BAS_TESTMETHOD
                {
                    TMID = d.TMID,
                    TMNAME = d.TMNAME
                }).ToListAsync();
            rndFabTestSampleViewModel.FLoomMachineNoList = await DenimDbContext.F_LOOM_MACHINE_NO
                .Select(d => new F_LOOM_MACHINE_NO
                {
                    ID = d.ID,
                    LOOM_NO = d.LOOM_NO
                }).ToListAsync();
            rndFabTestSampleViewModel.PlProductionSetdistributionsList = await DenimDbContext.PL_PRODUCTION_SETDISTRIBUTION
                .Include(d => d.PROG_)
                .Select(d => new PL_PRODUCTION_SETDISTRIBUTION
                {
                    SETID = d.SETID,
                    PROG_ = new PL_BULK_PROG_SETUP_D
                    {
                        PROG_NO = d.PROG_.PROG_NO
                    }
                }).ToListAsync();

            return rndFabTestSampleViewModel;
        }

        public async Task<bool> FindByLabNo(string labNo)
        {
            return !await DenimDbContext.RND_FABTEST_SAMPLE.AnyAsync(e => e.LABNO.Equals(labNo));
        }

        public async Task<IEnumerable<RND_FABTEST_SAMPLE>> GetAllRndFabTestSampleAsync()
        {
            return await DenimDbContext.RND_FABTEST_SAMPLE
                .Include(d => d.WASHEDBYNavigation)
                .Include(d => d.UNWASHEDBYNavigation)
                .Include(d => d.SFIN)
                .Include(d => d.PROGNONavigation.PROG_)
                .Select(d => new RND_FABTEST_SAMPLE
                {
                    LTSID = d.LTSID,
                    EncryptedId = _protector.Protect(d.LTSID.ToString()),
                    LABNO = d.LABNO,
                    LTSDATE = d.LTSDATE,
                    COMMENTS = d.COMMENTS,
                    WASHEDBYNavigation = new F_HRD_EMPLOYEE
                    {
                        EMPNO = $"{d.WASHEDBYNavigation.EMPNO} - {d.WASHEDBYNavigation.FIRST_NAME} {d.WASHEDBYNavigation.LAST_NAME}"
                    },
                    UNWASHEDBYNavigation = new F_HRD_EMPLOYEE
                    {
                        EMPNO = $"{d.WASHEDBYNavigation.EMPNO} - {d.WASHEDBYNavigation.FIRST_NAME} {d.WASHEDBYNavigation.LAST_NAME}"
                    },
                    SFIN = new RND_SAMPLEINFO_FINISHING
                    {
                        STYLE_NAME = d.SFIN.STYLE_NAME
                    },
                    PROGNONavigation = new PL_PRODUCTION_SETDISTRIBUTION
                    {
                        PROG_ = new PL_BULK_PROG_SETUP_D
                        {
                            PROG_NO = d.PROGNONavigation.PROG_.PROG_NO
                        }
                    }
                }).ToListAsync();
        }
    }
}
