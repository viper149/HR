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
    public class SQLF_QA_YARN_TEST_INFORMATION_POLYESTER_Repository : BaseRepository<F_QA_YARN_TEST_INFORMATION_POLYESTER>, IF_QA_YARN_TEST_INFORMATION_POLYESTER
    {
        public SQLF_QA_YARN_TEST_INFORMATION_POLYESTER_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {

        }
        public async Task<FQaYarnTestInformationPolyesterViewModel> GetOtherDetailsOfYMaster(int yRcvId)
        {
            return await DenimDbContext.F_YS_YARN_RECEIVE_MASTER
                .Include(e => e.IND)
                .Include(c => c.F_YS_YARN_RECEIVE_DETAILS)
                .ThenInclude(e => e.BasYarnLotinfo)
                .Include(c => c.F_YS_YARN_RECEIVE_DETAILS)
                .ThenInclude(c => c.PROD)
                .Where(e => e.YRCVID.Equals(yRcvId))
                .Select(e => new FQaYarnTestInformationPolyesterViewModel
                {
                    FYsYarnReceiveMaster = new F_YS_YARN_RECEIVE_MASTER
                    {
                        INDID = e.INDID,
                        CHALLANNO = e.CHALLANNO
                    },

                    ReceiveDetailsList = e.F_YS_YARN_RECEIVE_DETAILS.Select(f => new F_YS_YARN_RECEIVE_DETAILS
                    {
                        TRNSID = f.TRNSID,
                        PROD = new BAS_YARN_COUNTINFO
                        {
                            COUNTID = f.PROD.COUNTID,
                            COUNTNAME = $"{f.PROD.COUNTNAME} - {f.BasYarnLotinfo.LOTNO} - {f.BasYarnLotinfo.BRAND} - {f.RCV_QTY} Kg"
                        }
                    }).ToList()
                }).FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<F_QA_YARN_TEST_INFORMATION_POLYESTER>> GetAllAsync()
        {
            return await DenimDbContext.F_QA_YARN_TEST_INFORMATION_POLYESTER
                .Include(e => e.COLOR)
                .Include(c => c.YRCV)
                .ToListAsync();
        }
        public async Task<FQaYarnTestInformationPolyesterViewModel> GetInitObjByAsync(
            FQaYarnTestInformationPolyesterViewModel fQaYarnTestInformationPolyesterViewModel)
        {
            fQaYarnTestInformationPolyesterViewModel.ReceiveMasterList = await DenimDbContext.F_YS_YARN_RECEIVE_MASTER
                .Include(e => e.IND)
                .Select(c => new F_YS_YARN_RECEIVE_MASTER
                {
                    YRCVID = c.YRCVID,
                    CHALLANNO = $"{c.CHALLANNO} - {c.IND.INDNO}"
                }).OrderBy(e => e.CHALLANNO).ToListAsync();

            fQaYarnTestInformationPolyesterViewModel.Colors = await DenimDbContext.BAS_COLOR
                .Select(c => new BAS_COLOR
                {
                    COLORCODE = c.COLORCODE,
                    COLOR = c.COLOR
                }).ToListAsync();

            fQaYarnTestInformationPolyesterViewModel.ReceiveDetailsList = await DenimDbContext.F_YS_YARN_RECEIVE_DETAILS
                .Include(c => c.PROD)
                .Select(c => new F_YS_YARN_RECEIVE_DETAILS
                {
                    TRNSID = c.TRNSID,
                    PROD = new BAS_YARN_COUNTINFO
                    {
                        COUNTNAME = c.PROD.COUNTNAME
                    }
                }).ToListAsync();

            return fQaYarnTestInformationPolyesterViewModel;
        }
    }
}
