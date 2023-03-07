using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_QA_YARN_TEST_INFORMATION_COTTON_Repository : BaseRepository<F_QA_YARN_TEST_INFORMATION_COTTON>, IF_QA_YARN_TEST_INFORMATION_COTTON
    {
        public SQLF_QA_YARN_TEST_INFORMATION_COTTON_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {

        }
        public async Task<FQaYarnTestInformationCottonViewModel> GetOtherDetailsOfYMaster(int yRcvId)
        {
            var result = await DenimDbContext.F_YS_YARN_RECEIVE_MASTER
                .Include(d => d.IND)
                .Include(d => d.F_YS_YARN_RECEIVE_DETAILS)
                .ThenInclude(d => d.BasYarnLotinfo)
                .Include(d => d.F_YS_YARN_RECEIVE_DETAILS)
                .ThenInclude(d => d.PROD)
                .Where(d => d.YRCVID.Equals(yRcvId))
                .Select(d => new FQaYarnTestInformationCottonViewModel
                {
                    FYsYarnReceiveMaster = new F_YS_YARN_RECEIVE_MASTER
                    {
                        INDID = d.INDID,
                        CHALLANNO = d.CHALLANNO
                    },

                    ReceiveDetailsList = d.F_YS_YARN_RECEIVE_DETAILS.Select(f => new F_YS_YARN_RECEIVE_DETAILS
                    {
                        TRNSID = f.TRNSID,
                        PROD = new BAS_YARN_COUNTINFO
                        {
                            COUNTID = f.PROD.COUNTID,
                            RND_COUNTNAME = $"{f.PROD.RND_COUNTNAME} - {f.BasYarnLotinfo.LOTNO} - {f.BasYarnLotinfo.BRAND} - {f.RCV_QTY} Kg"
                        }
                    }).ToList()
                }).FirstOrDefaultAsync();
            return result;
        }
        public async Task<IEnumerable<F_QA_YARN_TEST_INFORMATION_COTTON>> GetAllAsync()
        {
            var result = await DenimDbContext.F_QA_YARN_TEST_INFORMATION_COTTON
                .Include(d => d.YRCV)
                .Select(d => new F_QA_YARN_TEST_INFORMATION_COTTON
                {
                    TESTID = d.TESTID,
                    TESTDATE = d.TESTDATE,
                    COUNT_ACT = d.COUNT_ACT,
                    TENACITY = d.TENACITY,
                    TM = d.TM,
                    TM_TPI = d.TM_TPI,
                    REMARKS = d.REMARKS,

                    YRCV = new F_YS_YARN_RECEIVE_MASTER
                    {
                        CHALLANNO = d.YRCV.CHALLANNO
                    }
                }).ToListAsync();
            return result;
        }
        public async Task<FQaYarnTestInformationCottonViewModel> GetInitObjByAsync(
            FQaYarnTestInformationCottonViewModel fQaYarnTestInformationCottonViewModel)
        {
            fQaYarnTestInformationCottonViewModel.ReceiveMasterList = await DenimDbContext.F_YS_YARN_RECEIVE_MASTER
                .Include(d => d.IND)
                .Select(d => new F_YS_YARN_RECEIVE_MASTER
                {
                    YRCVID = d.YRCVID,
                    CHALLANNO = $"{d.CHALLANNO} - {d.IND.INDNO}"
                })
                .ToListAsync();
            fQaYarnTestInformationCottonViewModel.ReceiveDetailsList = await DenimDbContext.F_YS_YARN_RECEIVE_DETAILS
                .Include(d => d.PROD)
                .Select(d => new F_YS_YARN_RECEIVE_DETAILS
                {
                    TRNSID = d.TRNSID,
                    PROD = new BAS_YARN_COUNTINFO
                    {
                        RND_COUNTNAME = d.PROD.RND_COUNTNAME
                    }
                }).ToListAsync();

            return fQaYarnTestInformationCottonViewModel;
        }
    }
}
