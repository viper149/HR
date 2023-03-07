using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels;
using DenimERP.ViewModels.Rnd.Finish;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLRND_SAMPLEINFO_FINISHING_Repository : BaseRepository<RND_SAMPLEINFO_FINISHING>, IRND_SAMPLEINFO_FINISHING
    {
        private readonly IDataProtector _protector;

        public SQLRND_SAMPLEINFO_FINISHING_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<CreateRndSampleInfoFinishViewModel> GetInitObjects(CreateRndSampleInfoFinishViewModel createRndSampleInfoFinishViewModel)
        {
            createRndSampleInfoFinishViewModel.RndSampleinfoFinishing.FINISHDATE = DateTime.Now;
            createRndSampleInfoFinishViewModel.RndFabtestGreys = await DenimDbContext.RND_FABTEST_GREY
                .Include(c => c.PROG.RND_SAMPLE_INFO_WEAVING)
                .Select(e => new RND_FABTEST_GREY
                {
                    LTGID = e.LTGID,
                    OPTION1 = $"G-{e.LAB_NO} - {e.DEVELOPMENTNO??e.OPTION1}",
                    //OPTION2 =  e.PROG.RND_SAMPLE_INFO_WEAVING.FirstOrDefault().FABCODE
                    //PROG = new PL_SAMPLE_PROG_SETUP
                    //{
                    //    SDID = e.PROG.SDID
                    //}
                })
                .ToListAsync();

            createRndSampleInfoFinishViewModel.BasColors = await DenimDbContext.BAS_COLOR.ToListAsync();

            return createRndSampleInfoFinishViewModel;
        }

        public async Task<CreateRndSampleInfoFinishViewModel> GetPreviousData(int ltgId)
        {
            try
            {
                return await DenimDbContext.RND_FABTEST_GREY
                    .Include(c => c.PROG.RND_SAMPLE_INFO_WEAVING)
                    .Include(c => c.EMP_WASHEDBY)
                    .Include(c => c.EMP_UNWASHEDBY)
                    .Where(e => e.LTGID.Equals(ltgId))
                    .Select(c => new CreateRndSampleInfoFinishViewModel
                    {
                        RndFabtestGrey = c
                    })
                    .FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<DataTableObject<RND_SAMPLEINFO_FINISHING>> GetForDataTableByAsync(string sortColumn, string sortColumnDirection, string searchValue, string draw, int skip, int pageSize)
        {
            var rndSampleinfoFinishings = DenimDbContext.RND_SAMPLEINFO_FINISHING
                .Select(e => new RND_SAMPLEINFO_FINISHING
                {
                    EncryptedId = _protector.Protect(e.SFINID.ToString()),
                    FINISHDATE = e.FINISHDATE,
                    STYLE_NAME = e.STYLE_NAME,
                    FINISH_ROUTE = e.FINISH_ROUTE,
                    PROCESSED_LENGTH = e.PROCESSED_LENGTH,
                    WASHPICK = e.WASHPICK,
                    GRCONST = e.GRCONST,
                    BWGBW = e.BWGBW,
                    BWGAW = e.BWGAW,
                    REMARKS = e.REMARKS
                });

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                switch (sortColumnDirection)
                {
                    case "asc":
                        rndSampleinfoFinishings = rndSampleinfoFinishings.OrderBy(c => c.GetType().GetProperty(sortColumn ?? string.Empty).GetValue(c));
                        break;
                    default:
                        rndSampleinfoFinishings = rndSampleinfoFinishings.OrderByDescending(c => c.GetType().GetProperty(sortColumn ?? string.Empty).GetValue(c));
                        break;
                }
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                rndSampleinfoFinishings = rndSampleinfoFinishings
                    .Where(m => m.FINISHDATE.ToString(CultureInfo.InvariantCulture).ToUpper().Contains(searchValue)
                                || m.STYLE_NAME != null && m.STYLE_NAME.ToUpper().Contains(searchValue)
                                || m.FINISH_ROUTE != null && m.FINISH_ROUTE.ToUpper().Contains(searchValue)
                                || m.PROCESSED_LENGTH != null && m.PROCESSED_LENGTH.ToString().Contains(searchValue)
                                || m.WASHPICK != null && m.WASHPICK.ToUpper().Contains(searchValue)
                                || m.GRCONST != null && m.GRCONST.ToUpper().Contains(searchValue)
                                || m.BWGBW != null && m.BWGBW.ToUpper().Contains(searchValue)
                                || m.BWGAW != null && m.BWGAW.ToUpper().Contains(searchValue)
                                || m.REMARKS != null && m.REMARKS.ToString().ToUpper().Contains(searchValue)
                    );
            }

            var recordsTotal = await rndSampleinfoFinishings.CountAsync();
            return new DataTableObject<RND_SAMPLEINFO_FINISHING>(draw, recordsTotal, recordsTotal, await rndSampleinfoFinishings.Skip(skip).Take(pageSize).ToListAsync());
        }

        public async Task<CreateRndSampleInfoFinishViewModel> FindByFnIdAsync(int fnId)
        {
            try
            {
                var result = await DenimDbContext.RND_SAMPLEINFO_FINISHING
                    .Include(c => c.LTG.EMP_WASHEDBY)
                    .Include(c => c.LTG.EMP_UNWASHEDBY)
                    .Include(c => c.LTG.PROG.RND_SAMPLE_INFO_WEAVING)
                    .Where(c => c.SFINID.Equals(fnId))
                    .Select(c => new CreateRndSampleInfoFinishViewModel
                    {
                        RndSampleinfoFinishing = c
                    })
                    .FirstOrDefaultAsync();

                result.RndSampleinfoFinishing.EncryptedId = _protector.Protect(result.RndSampleinfoFinishing.SFINID.ToString());

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public async Task<RND_SAMPLEINFO_FINISHING> FindByLtgIdAsync(int fnId)
        {
            var rndSampleinfoFinishing = await DenimDbContext.RND_SAMPLEINFO_FINISHING
                .Include(e => e.LTG)
                .ThenInclude(e => e.EMP_WASHEDBY)
                .Include(e => e.LTG)
                .ThenInclude(e => e.EMP_UNWASHEDBY)
                .Where(e => e.SFINID.Equals(fnId))
                .FirstOrDefaultAsync();

            rndSampleinfoFinishing.EncryptedId = _protector.Protect(rndSampleinfoFinishing.SFINID.ToString());

            return rndSampleinfoFinishing;
        }

        public async Task<RND_SAMPLEINFO_FINISHING> GetProgNoBySfnIdAsync(int sfnId)
        {
            return await DenimDbContext.RND_SAMPLEINFO_FINISHING
                .Include(d => d.LTG.PROG)
                .Where(d => d.SFINID.Equals(sfnId))
                .Select(d => new RND_SAMPLEINFO_FINISHING
                {
                    LTG = new RND_FABTEST_GREY
                    {
                        PROG = new PL_SAMPLE_PROG_SETUP
                        {
                            PROG_NO = d.LTG.PROG.PROG_NO
                        }
                    }
                }).FirstOrDefaultAsync();
        }
    }
}