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
    public class SQLRND_FABTEST_SAMPLE_BULK_Repository : BaseRepository<RND_FABTEST_SAMPLE_BULK>, IRND_FABTEST_SAMPLE_BULK
    {
        private readonly IDataProtector _protector;

        public SQLRND_FABTEST_SAMPLE_BULK_Repository(DenimDbContext denimDbContext, IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<IEnumerable<RND_FABTEST_SAMPLE_BULK>> GetAllRndFabTestSampleBulkAsync()
        {
            return await DenimDbContext.RND_FABTEST_SAMPLE_BULK
                .Include(d => d.EMPWASHBY)
                .Include(d => d.EMPUNWASHBY)
                .Include(d => d.SFIN)
                .Select(d => new RND_FABTEST_SAMPLE_BULK
                {
                    EncryptedId = _protector.Protect(d.LTSGID.ToString()),
                    LABNO = d.LABNO,
                    LTSGDATE = d.LTSGDATE,
                    PROGNO = d.PROGNO,
                    COMMENTS = d.COMMENTS,
                    EMPWASHBY = new F_HRD_EMPLOYEE
                    {
                        FIRST_NAME = $"{d.EMPWASHBY.FIRST_NAME} {d.EMPWASHBY.LAST_NAME}"
                    },
                    EMPUNWASHBY = new F_HRD_EMPLOYEE
                    {
                        FIRST_NAME = $"{d.EMPUNWASHBY.FIRST_NAME} {d.EMPUNWASHBY.LAST_NAME}"
                    },
                    SFIN = new RND_SAMPLEINFO_FINISHING
                    {
                        STYLE_NAME = d.SFIN.STYLE_NAME
                    }
                }).ToListAsync();
        }

        public async Task<RndFabTestSampleBulkViewModel> GetInitObjByAsync(RndFabTestSampleBulkViewModel rndFabTestSampleBulkViewModel)
        {
            //MAPPED WITH DB ROWS
            var shift = new Dictionary<int, string>
                {
                    {2, "P/A"},
                    {3, "P/B"},
                    {4, "P/B"}
                };
            rndFabTestSampleBulkViewModel.FHrShiftInfoList = await DenimDbContext.F_HR_SHIFT_INFO
                .Where(d => shift.ContainsKey(d.ID) || shift.ContainsValue(d.SHIFT))
                .Select(d => new F_HR_SHIFT_INFO
                {
                    ID = d.ID,
                    SHIFT = d.SHIFT
                }).ToListAsync();

            var FHrEmployees = await DenimDbContext.F_HR_EMP_OFFICIALINFO
                .Include(d => d.EMP)
                .Where(d => d.SECID.Equals(168) && !d.OPN2.Equals("Y"))  //PHYSICAL LAB
                .ToListAsync();

            rndFabTestSampleBulkViewModel.FHrEmployeeList = FHrEmployees
                .Select(d => new F_HRD_EMPLOYEE
                {
                    EMPID = d.EMP.EMPID,
                    FIRST_NAME = $"{d.EMP.EMPNO} - {d.EMP.FIRST_NAME} {d.EMP.LAST_NAME}"
                }).ToList();
            rndFabTestSampleBulkViewModel.FBasTestmethodList = await DenimDbContext.F_BAS_TESTMETHOD
                .Select(d => new F_BAS_TESTMETHOD
                {
                    TMID = d.TMID,
                    TMNAME = d.TMNAME
                })
                .ToListAsync();
            rndFabTestSampleBulkViewModel.FLoomMachineNoList = await DenimDbContext.F_LOOM_MACHINE_NO
                .Select(d => new F_LOOM_MACHINE_NO
                {
                    ID = d.ID,
                    LOOM_NO = d.LOOM_NO
                }).ToListAsync();
            rndFabTestSampleBulkViewModel.FPrFinTrollyList = await DenimDbContext.F_PR_FIN_TROLLY
                .Select(d => new F_PR_FIN_TROLLY
                {
                    FIN_TORLLY_ID = d.FIN_TORLLY_ID,
                    NAME = d.NAME
                }).ToListAsync();
            rndFabTestSampleBulkViewModel.RndSampleinfoFinishingList = await DenimDbContext.RND_SAMPLEINFO_FINISHING
                .Select(d => new RND_SAMPLEINFO_FINISHING
                {
                    SFINID = d.SFINID,
                    STYLE_NAME = d.STYLE_NAME
                }).ToListAsync();
            return rndFabTestSampleBulkViewModel;
        }

        public async Task<RND_SAMPLEINFO_FINISHING> FindObjectsByIdAsync(int sfinId)
        {
            return await DenimDbContext.RND_SAMPLEINFO_FINISHING
                .Include(d => d.LTG.PROG)
                .Where(d => d.SFINID.Equals(sfinId))
                .Select(d => new RND_SAMPLEINFO_FINISHING
                {
                    SFINID = d.SFINID,
                    STYLE_NAME = d.STYLE_NAME
                }).FirstOrDefaultAsync();
        }

        public async Task<bool> FindByLabNo(string labNo)
        {
            return !await DenimDbContext.RND_FABTEST_SAMPLE_BULK.AnyAsync(e => e.LABNO.Equals(labNo));
        }
    }
}
