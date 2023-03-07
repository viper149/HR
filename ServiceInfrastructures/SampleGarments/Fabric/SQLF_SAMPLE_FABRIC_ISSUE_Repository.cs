using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces.SampleGarments.Fabric;
using DenimERP.ViewModels.SampleGarments.Fabric;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures.SampleGarments.Fabric
{
    public class SQLF_SAMPLE_FABRIC_ISSUE_Repository : BaseRepository<F_SAMPLE_FABRIC_ISSUE>, IF_SAMPLE_FABRIC_ISSUE
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDataProtector _protector;

        public SQLF_SAMPLE_FABRIC_ISSUE_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            IAuthorizationService authorizationService,
            IHttpContextAccessor httpContextAccessor) : base(denimDbContext)
        {
            _authorizationService = authorizationService;
            _httpContextAccessor = httpContextAccessor;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<CreateFSampleFabricIssueViewModel> GetInitObjByAsync(CreateFSampleFabricIssueViewModel createFSampleFabricIssueViewModel)
        {
            var filterTeams = new Dictionary<int, string> { { 215, "Team-A" }, { 216, "Team-C" }, { 217, "Team-B" } };

            createFSampleFabricIssueViewModel.BasBrandinfos = await DenimDbContext.BAS_BRANDINFO.Select(e => new BAS_BRANDINFO
            {
                BRANDID = e.BRANDID,
                BRANDNAME = e.BRANDNAME
            }).OrderBy(e => e.BRANDNAME).ToListAsync();

            createFSampleFabricIssueViewModel.MktTeams = await DenimDbContext.MKT_TEAM
                .Include(e => e.BasTeamInfo)
                .Where(e => filterTeams.Any(f => f.Key.Equals(e.TEAMID)))
                .Select(e => new MKT_TEAM
                {
                    MKT_TEAMID = e.MKT_TEAMID,
                    PERSON_NAME = $"{e.PERSON_NAME} ({e.BasTeamInfo.TEAM_NAME})",
                    BasTeamInfo = e.BasTeamInfo
                }).OrderBy(e => e.BasTeamInfo.TEAM_NAME).ToListAsync();

            createFSampleFabricIssueViewModel.RndFabricinfos = await DenimDbContext.RND_FABRICINFO
                //.Where(e => e.APPROVED.Equals(true))
                .Select(e => new RND_FABRICINFO
                {
                    FABCODE = e.FABCODE,
                    STYLE_NAME = e.STYLE_NAME
                }).OrderBy(e => e.STYLE_NAME).ToListAsync();

            createFSampleFabricIssueViewModel.BasBuyerinfos = await DenimDbContext.BAS_BUYERINFO.Select(e => new BAS_BUYERINFO
            {
                BUYERID = e.BUYERID,
                BUYER_NAME = e.BUYER_NAME
            }).OrderBy(e => e.BUYER_NAME).ToListAsync();

            return createFSampleFabricIssueViewModel;
        }

        public async Task<IEnumerable<ExtendFSampleFabricIssueViewModel>> GetAllForDataTableByAsync()
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(_httpContextAccessor.HttpContext.User, "SampleFabricIssueDelete");

            return await DenimDbContext.F_SAMPLE_FABRIC_ISSUE
                .Include(e => e.MKT_TEAM.BasTeamInfo)
                .Include(e => e.BUYER)
                .Include(e => e.BRAND)
                .Select(e => new ExtendFSampleFabricIssueViewModel
                {
                    SFIID = e.SFIID,
                    EncryptedId = _protector.Protect(e.SFIID.ToString()),
                    SRNO = e.SRNO,
                    REQ_DATE = e.REQ_DATE,
                    ISSUE_DATE = e.ISSUE_DATE,
                    REMARKS = e.REMARKS,
                    BUYER = new BAS_BUYERINFO
                    {
                        BUYER_NAME = $"{e.BUYER.BUYER_NAME}"
                    },
                    BRAND = new BAS_BRANDINFO
                    {
                        BRANDNAME = $"{e.BRAND.BRANDNAME}"
                    },
                    MARCHANDISER_NAME = e.MARCHANDISER_NAME,
                    MKT_TEAM = new MKT_TEAM
                    {
                        PERSON_NAME = $"{e.MKT_TEAM.PERSON_NAME}"
                    },
                    IsDelete = authorizationResult.Succeeded
                }).ToListAsync();
        }

        public async Task<bool> IsSrNoInUseByAsync(CreateFSampleFabricIssueViewModel createFSampleFabricIssueViewModel)
        {
            var filterTeams = new Dictionary<int, string> { { 215, "A-" }, { 216, "C-" }, { 217, "B-" } };
            var x = await DenimDbContext.F_SAMPLE_FABRIC_ISSUE
                .Include(e => e.MKT_TEAM.BasTeamInfo)
                .AnyAsync(e => e.SRNO.Equals($"{filterTeams.FirstOrDefault(f => f.Key.Equals(e.MKT_TEAM.BasTeamInfo.TEAMID)).Value + createFSampleFabricIssueViewModel.FSampleFabric.SRNO}") &&
                             e.MKT_TEAM.BasTeamInfo.TEAMID.Equals(DenimDbContext.MKT_TEAM.Include(f => f.BasTeamInfo)
                                 .FirstOrDefault(f =>
                                     f.MKT_TEAMID.Equals(createFSampleFabricIssueViewModel.FSampleFabric.MKT_TEAMID)).TEAMID));
            return x;
        }

        public async Task<CreateFSampleFabricIssueViewModel> FindByIdIncludeAllAsync(int sfiId)
        {
            return await DenimDbContext.F_SAMPLE_FABRIC_ISSUE
                .Include(e => e.MKT_TEAM.BasTeamInfo)
                .Include(e => e.BRAND)
                .Include(e => e.BUYER)
                .Include(e => e.F_SAMPLE_FABRIC_ISSUE_DETAILS)
                .ThenInclude(e => e.Fabricinfo)
                .Where(e => e.SFIID.Equals(sfiId))
                .Select(e => new CreateFSampleFabricIssueViewModel
                {
                    FSampleFabric = new F_SAMPLE_FABRIC_ISSUE
                    {
                        SFIID = e.SFIID,
                        EncryptedId = _protector.Protect(e.SFIID.ToString()),
                        REQ_DATE = e.REQ_DATE,
                        ISSUE_DATE = e.ISSUE_DATE,
                        SRNO = e.SRNO,
                        BRANDID = e.BRANDID,
                        MARCHANDISER_NAME = e.MARCHANDISER_NAME,
                        BUYERID = e.BUYERID,
                        MKT_TEAMID = e.MKT_TEAMID,
                        REMARKS = e.REMARKS,
                        HasRemoved = e.HasRemoved,
                        BRAND = new BAS_BRANDINFO
                        {
                            BRANDNAME = $"{e.BRAND.BRANDNAME}"
                        },
                        BUYER = new BAS_BUYERINFO
                        {
                            BUYER_NAME = $"{e.BUYER.BUYER_NAME}"
                        },
                        MKT_TEAM = new MKT_TEAM
                        {
                            PERSON_NAME = $"{e.MKT_TEAM.PERSON_NAME} ({e.MKT_TEAM.BasTeamInfo.TEAM_NAME})"
                        }
                    },
                    FSampleFabricIssueDetailses = e.F_SAMPLE_FABRIC_ISSUE_DETAILS.Select(f => new F_SAMPLE_FABRIC_ISSUE_DETAILS
                    {
                        SFIDID = f.SFIDID,
                        SFIID = f.SFIID,
                        FABCODE = f.FABCODE,
                        SR_QTY = f.SR_QTY,
                        SR_ISSUE_QTY = f.SR_ISSUE_QTY,
                        REMARKS = f.REMARKS,
                        HasRemoved = f.HasRemoved,
                        Fabricinfo = new RND_FABRICINFO
                        {
                            STYLE_NAME = $"{f.Fabricinfo.STYLE_NAME}"
                        }
                    }).ToList()
                }).FirstOrDefaultAsync();
        }

        public async Task<string> GetSrNoPrefixByAsync(CreateFSampleFabricIssueViewModel createFSampleFabricIssueViewModel)
        {
            var filterTeams = new Dictionary<int, string> { { 215, "A-" }, { 216, "C-" }, { 217, "B-" } };
            var firstOrDefaultAsync = await DenimDbContext.MKT_TEAM.FirstOrDefaultAsync(e => e.MKT_TEAMID.Equals(createFSampleFabricIssueViewModel.FSampleFabric.MKT_TEAMID));
            return filterTeams.FirstOrDefault(e => e.Key.Equals(firstOrDefaultAsync.TEAMID)).Value;
        }

        public async Task<bool> FindBySrNoAsync(string srNo)
        {
            return await DenimDbContext.F_SAMPLE_FABRIC_ISSUE.AnyAsync(e => e.SRNO.Equals(srNo));
        }

        public async Task<CreateFSampleFabricIssueViewModel> GetInitObjForDetailsTableByAsync(CreateFSampleFabricIssueViewModel createFSampleFabricIssueViewModel)
        {
            foreach (var item in createFSampleFabricIssueViewModel.FSampleFabricIssueDetailses)
            {
                item.Fabricinfo = await DenimDbContext.RND_FABRICINFO.FirstOrDefaultAsync(e => e.FABCODE.Equals(item.FABCODE));
            }

            return createFSampleFabricIssueViewModel;
        }

        public async Task<F_SAMPLE_FABRIC_ISSUE> FindByIdIncludeAllForDeleteAsync(int sfiId)
        {
            return await DenimDbContext.F_SAMPLE_FABRIC_ISSUE
                .Include(e => e.F_SAMPLE_FABRIC_ISSUE_DETAILS)
                .FirstOrDefaultAsync(e => e.SFIID.Equals(sfiId));
        }
    }
}
