using System;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_YARN_TRANSACTION_Repository : BaseRepository<F_YARN_TRANSACTION>, IF_YARN_TRANSACTION
    {
        public SQLF_YARN_TRANSACTION_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {

        }

        public async Task<double?> GetLastBalanceByCountIdAsync(int? countId, int lotId, int INDENT_TYPE)
        {
            try
            {
                //return await _denimDbContext.F_YARN_TRANSACTION
                //    .Where(e => _denimDbContext.F_YARN_REQ_DETAILS
                //        .Include(h => h.COUNT.COUNT)
                //        .Where(f => f.TRNSID.Equals(id))
                //        .Any(g => g.COUNT.COUNT.COUNTID.Equals(e.COUNTID)))
                //    .SumAsync(e => e.BALANCE);
                var  result = await DenimDbContext.F_YARN_TRANSACTION
                    .Where(c => c.COUNTID.Equals(countId) && c.LOTID.Equals(lotId) && c.INDENT_TYPE.Equals(INDENT_TYPE))
                    .OrderBy(c => c.YTRNID)
                    .Select(c => c.BALANCE)
                    .LastOrDefaultAsync();
                return result ?? 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<double?> GetLastBalanceByIndentAsync(int? RCVDID, int INDENT_TYPE,DateTime? YIssueDate)
        {
            try
            {
                var  result = await DenimDbContext.F_YS_YARN_RECEIVE_DETAILS
                    .Include(c=>c.YRCV)
                    .Where(c => c.TRNSID.Equals(RCVDID) && c.INDENT_TYPE.Equals(INDENT_TYPE) && c.YRCV.YRCVDATE <= YIssueDate)
                    .OrderBy(c => c.TRNSID)
                    .Select(c => c.RCV_QTY-c.REJ_QTY)
                    .LastOrDefaultAsync();

                var issueQty = await DenimDbContext.F_YS_YARN_ISSUE_DETAILS.Where(c => c.RCVDID.Equals(RCVDID) && c.YISSUE.YISSUEDATE <= YIssueDate)
                    .SumAsync(c => c.ISSUE_QTY);

                result -= issueQty;
                
                return result ?? 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        public async Task<int?> GetLastBagBalanceByIndentAsync(int? RCVDID, int INDENT_TYPE, DateTime? YIssueDate)
        {
            try
            {
                var result = await DenimDbContext.F_YS_YARN_RECEIVE_DETAILS
                    .Include(c => c.YRCV)
                    .Where(c => c.TRNSID.Equals(RCVDID) && c.INDENT_TYPE.Equals(INDENT_TYPE) && c.YRCV.YRCVDATE <= YIssueDate)
                    .OrderBy(c => c.TRNSID)
                    .Select(c => c.BAG)
                    .LastOrDefaultAsync();

                var issueQty = await DenimDbContext.F_YS_YARN_ISSUE_DETAILS.Where(c => c.RCVDID.Equals(RCVDID))
                    .SumAsync(c => c.BAG);

                result -= issueQty;

                return result ?? 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        
        public async Task<int?> GetLastBagBalanceByCountIdAsync(int? countId, int lotId, int INDENT_TYPE)
        {
            try
            {
                var  result = await DenimDbContext.F_YARN_TRANSACTION
                    .Where(c => c.COUNTID.Equals(countId) && c.LOTID.Equals(lotId) && c.INDENT_TYPE.Equals(INDENT_TYPE))
                    .OrderBy(c => c.YTRNID)
                    .Select(c => c.BAG)
                    .LastOrDefaultAsync();
                return result ?? 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        public async Task<dynamic> GetPoidByRcvId(int id)
        {
            try
            {
                var poid = await DenimDbContext.F_YS_YARN_RECEIVE_DETAILS
                    .Include(c => c.YRCV.IND.INDSL.ORDER_NONavigation)
                    .Include(c => c.YRCV.IND.INDSL.ORDERNO_SNavigation)
                    .Where(c => c.YRCVID.Equals(id)).FirstOrDefaultAsync();

                return poid!=null?poid.YRCV.IND.INDSL.ORDER_NONavigation.POID != 0 ? poid.YRCV.IND.INDSL.ORDER_NONavigation.POID : poid.YRCV.IND.INDSL.ORDERNO_SNavigation.SDRFID:poid;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<int?> GetPoidByIndId(int id)
        {
            try
            {
                var poid = await DenimDbContext.F_YS_INDENT_MASTER
                    .Include(c => c.INDSL.ORDER_NONavigation)
                    .Include(c => c.INDSL.ORDERNO_SNavigation)
                    .Where(c => c.INDID.Equals(id)).FirstOrDefaultAsync();

                if (poid.INDSL.ORDERNO_SNavigation == null && poid.INDSL.ORDER_NONavigation == null)
                {
                    return null;
                }

                return poid.INDSL.ORDER_NONavigation?.POID ?? poid.INDSL.ORDERNO_SNavigation.SDRFID;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
