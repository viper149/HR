using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces.Marketing;
using DenimERP.ViewModels.Marketing;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures.Marketing
{
    public class SQLMKT_SDRF_INFO_Repository : BaseRepository<MKT_SDRF_INFO>, IMKT_SDRF_INFO
    {
        private readonly IDataProtector _protector;

        public SQLMKT_SDRF_INFO_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<IEnumerable<MKT_SDRF_INFO>> GetMktSdrfAllAsync()
        {
            try
            {
                var result = await DenimDbContext.MKT_SDRF_INFO
                    .Include(e => e.TEAM_M)
                    .Include(e => e.TEAM_R)
                    .Include(e => e.MKT_PERSON)
                    .Include(e => e.RND_ANALYSIS_SHEET)
                    .Select(e => new MKT_SDRF_INFO
                    {
                        EncryptedId = _protector.Protect(e.SDRFID.ToString()),
                        SDRF_NO = e.SDRF_NO,
                        TRANSDATE = e.TRANSDATE,
                        AID = e.AID,
                        CONSTRUCTION = e.CONSTRUCTION,
                        TEAM_M = new BAS_TEAMINFO
                        {
                            TEAM_NAME = e.TEAM_M.TEAM_NAME
                        },
                        TEAM_R = new BAS_TEAMINFO
                        {
                            TEAM_NAME = e.TEAM_R.TEAM_NAME
                        },
                        RND_ANALYSIS_SHEET = new RND_ANALYSIS_SHEET
                        {
                            MKT_QUERY_NO = e.RND_ANALYSIS_SHEET.MKT_QUERY_NO
                        },
                        MKT_DGM_APPROVE = e.MKT_DGM_APPROVE,
                        RND_APPROVE = e.RND_APPROVE,
                        PLN_APPROVE = e.PLN_APPROVE,
                        PLANT_HEAD_APPROVE = e.PLANT_HEAD_APPROVE,
                        REMARKS = e.REMARKS,
                        SEASON = e.SEASON,
                        MKT_PERSON = new MKT_TEAM
                        {
                            PERSON_NAME = e.MKT_PERSON.PERSON_NAME
                        }
                    })
                    .ToListAsync();

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<int> GetSdrfNumberAsync()
        {
            try
            {
                var result = await DenimDbContext.MKT_SDRF_INFO
                    .OrderByDescending(c => c.SDRF_NO)
                    .Select(c => c.SDRF_NO)
                    .FirstOrDefaultAsync();

                var res = result == null ? 0 : int.Parse(result);
                return res != 0 ? res : 1900;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public async Task<bool> FindBySdrfNoInUseAsync(string sdrfNo)
        {
            var isExistSdrfNo = await DenimDbContext.MKT_SDRF_INFO.AnyAsync(c => c.SDRF_NO.Equals(sdrfNo));
            return isExistSdrfNo;
        }
        public async Task<bool> GetAvailableDate(DateTime date)
        {
            var isDateAvailable = await DenimDbContext.MKT_SDRF_INFO.Where(c => c.ACTUAL_DATE.Equals(date)).ToListAsync();
            if (isDateAvailable.Count > 2)
            {
                return false;
            }
            return true;
        }

        public async Task<MktSdrfInfoViewModel> GetInitObjects(MktSdrfInfoViewModel mktSdrfInfoViewModel)
        {
            try
            {
                mktSdrfInfoViewModel.TeamInfos = await DenimDbContext.BAS_TEAMINFO.OrderBy(e => e.TEAM_NAME).ToListAsync();
                mktSdrfInfoViewModel.DevTypes = await DenimDbContext.MKT_DEV_TYPE.OrderBy(e => e.DEV_TYPE).ToListAsync();
                mktSdrfInfoViewModel.MktTeams = await DenimDbContext.MKT_TEAM.OrderBy(e => e.PERSON_NAME).ToListAsync();
                mktSdrfInfoViewModel.BuyerInfos = await DenimDbContext.BAS_BUYERINFO.OrderBy(e => e.BUYER_NAME).ToListAsync();
                mktSdrfInfoViewModel.MktFactories = await DenimDbContext.MKT_FACTORY.OrderBy(e => e.FACTORY_NAME).ToListAsync();
                mktSdrfInfoViewModel.Countries = await DenimDbContext.COUNTRIES.OrderBy(e => e.CONTINENT_NAME).ToListAsync();
                mktSdrfInfoViewModel.RndFinishType = await DenimDbContext.RND_FINISHTYPE.OrderBy(e => e.TYPENAME).ToListAsync();
                mktSdrfInfoViewModel.RndAnalysisSheets = await DenimDbContext.RND_ANALYSIS_SHEET.OrderBy(e => e.SWATCH_ID).ToListAsync();
                mktSdrfInfoViewModel.BasBrandinfos = await DenimDbContext.BAS_BRANDINFO.OrderBy(e => e.BRANDNAME).ToListAsync();

                return mktSdrfInfoViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
