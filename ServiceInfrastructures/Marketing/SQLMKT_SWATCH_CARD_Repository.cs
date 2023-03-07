using System;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces.Marketing;
using DenimERP.ViewModels;
using DenimERP.ViewModels.Marketing;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures.Marketing
{
    public class SQLMKT_SWATCH_CARD_Repository : BaseRepository<MKT_SWATCH_CARD>, IMKT_SWATCH_CARD
    {
        private readonly IDataProtector _protector;

        public SQLMKT_SWATCH_CARD_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<CreateMktSwatchCardViewModel> GetInitObjects(CreateMktSwatchCardViewModel createMktSwatchCardView)
        {
            createMktSwatchCardView.BasColors = await DenimDbContext.BAS_COLOR.Select(e => new BAS_COLOR
            {
                COLOR = e.COLOR,
                COLORCODE = e.COLORCODE
            }).OrderBy(e => e.COLOR).ToListAsync();

            createMktSwatchCardView.BasBuyerinfos = await DenimDbContext.BAS_BUYERINFO.Select(e => new BAS_BUYERINFO
            {
                BUYERID = e.BUYERID,
                BUYER_NAME = e.BUYER_NAME
            }).OrderBy(e => e.BUYER_NAME).ToListAsync();

            createMktSwatchCardView.RndFinishtypes = await DenimDbContext.RND_FINISHTYPE.Select(e => new RND_FINISHTYPE
            {
                FINID = e.FINID,
                TYPENAME = e.TYPENAME
            }).OrderBy(e => e.TYPENAME).ToListAsync();

            createMktSwatchCardView.MktTeams = await DenimDbContext.MKT_TEAM.Select(e => new MKT_TEAM
            {
                MKT_TEAMID = e.MKT_TEAMID,
                PERSON_NAME = e.PERSON_NAME
            }).OrderBy(e => e.PERSON_NAME).ToListAsync();

            return createMktSwatchCardView;
        }

        public async Task<DataTableObject<MKT_SWATCH_CARD>> GetForDataTableByAsync(string sortColumn, string sortColumnDirection, string searchValue, string draw, int skip,
            int pageSize)
        {
            try
            {
                var navigationPropertyStrings = new[] { "BUYER", "FN", "BasColor" };
                var listAsync = DenimDbContext.MKT_SWATCH_CARD
                    .Include(e => e.BUYER)
                    .Include(e => e.FN)
                    .Include(c => c.TEAM)
                    .Include(c => c.BasTeamInfo)
                    .Include(c => c.BasColor)
                    .Select(e => new MKT_SWATCH_CARD
                    {
                        SWCDID = e.SWCDID,
                        EncryptedId = _protector.Protect(e.SWCDID.ToString()),
                        MKTQUERY = e.MKTQUERY,
                        CONSTRUCTION = e.CONSTRUCTION,
                        WIDTH = e.WIDTH,
                        FNWEIGHT = e.FNWEIGHT,
                        BasColor = new BAS_COLOR
                        {
                            COLOR = e.BasColor.COLOR
                        },
                        REMARKS = e.REMARKS,
                        MKTTEAM = e.MKTTEAM,
                        MKTPERSON = e.MKTPERSON,
                        BUYER = e.BUYER,
                        FN = new RND_FINISHTYPE
                        {
                            TYPENAME = e.FN.TYPENAME
                        },
                        TEAM = e.TEAM,
                        BasTeamInfo = e.BasTeamInfo
                    });

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    listAsync = OrderedResult<MKT_SWATCH_CARD>.GetOrderedResult(sortColumnDirection, sortColumn, navigationPropertyStrings, listAsync);
                }

                if (!string.IsNullOrEmpty(searchValue))
                {
                    listAsync = listAsync
                        .Where(m => m.MKTQUERY.ToString().ToUpper().Contains(searchValue)
                                    || m.MKTQUERY != null && m.MKTQUERY.Contains(searchValue)
                                    || m.CONSTRUCTION != null && m.CONSTRUCTION.Contains(searchValue)
                                    || m.WIDTH != null && m.WIDTH.Contains(searchValue)
                                    || m.FNWEIGHT != null && m.FNWEIGHT.Contains(searchValue)
                                    || m.BasColor.COLOR != null && m.BasColor.COLOR.Contains(searchValue)
                                    || m.REMARKS != null && m.REMARKS.Contains(searchValue)
                                    || m.MKTPERSON != null && m.TEAM.PERSON_NAME.Contains(searchValue)
                                    || m.BUYER.BUYER_NAME != null && m.BUYER.BUYER_NAME.Contains(searchValue)
                                    || m.FN.TYPENAME != null && m.FN.TYPENAME.Contains(searchValue));

                    listAsync = OrderedResult<MKT_SWATCH_CARD>.GetOrderedResult(sortColumnDirection, sortColumn, navigationPropertyStrings, listAsync);
                }

                var recordsTotal = await listAsync.CountAsync();

                return new DataTableObject<MKT_SWATCH_CARD>(draw, recordsTotal, recordsTotal, await listAsync.Skip(skip).Take(pageSize).ToListAsync());

            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return null;
            }
        }

        public async Task<CreateMktSwatchCardViewModel> FindBySwCdIdAsync(int swCdId)
        {
            var createMktSwatchCardViewModel = await DenimDbContext.MKT_SWATCH_CARD
                .Include(e => e.BUYER)
                .Include(e => e.FN)
                .Select(e => new CreateMktSwatchCardViewModel
                {
                    MktSwatchCard = e
                }).FirstOrDefaultAsync(e => e.MktSwatchCard.SWCDID.Equals(swCdId));

            createMktSwatchCardViewModel.MktSwatchCard.EncryptedId =
                _protector.Protect(createMktSwatchCardViewModel.MktSwatchCard.SWCDID.ToString());

            return createMktSwatchCardViewModel;
        }
    }
}
