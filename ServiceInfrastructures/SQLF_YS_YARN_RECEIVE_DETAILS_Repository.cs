using System;
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
    public class SQLF_YS_YARN_RECEIVE_DETAILS_Repository : BaseRepository<F_YS_YARN_RECEIVE_DETAILS>, IF_YS_YARN_RECEIVE_DETAILS
    {
        public SQLF_YS_YARN_RECEIVE_DETAILS_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {

        }

        public async Task<YarnReceiveViewModel> GetDetailsData(YarnReceiveViewModel yarnReceiveViewModel)
        {
            try
            {
                foreach (var item in yarnReceiveViewModel.FYsYarnReceiveDetailList)
                {
                    item.BasYarnLotinfo = await DenimDbContext.BAS_YARN_LOTINFO.Where(c => c.LOTID.Equals(item.LOT)).FirstOrDefaultAsync();
                    item.FYsRawPer = await DenimDbContext.F_YS_RAW_PER.Where(c => c.RAWID.Equals(item.RAW))
                        .FirstOrDefaultAsync();
                }

                return yarnReceiveViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<F_YS_YARN_RECEIVE_DETAILS>> GetYarnIndentDetails(int countId, int indentType, DateTime? issueDate)
        {
            try
            {
                var result = await DenimDbContext.F_YS_YARN_RECEIVE_DETAILS
                    .Include(d => d.F_YS_YARN_ISSUE_DETAILS)
                    .ThenInclude(c=>c.YISSUE)
                    .Include(d => d.YRCV.IND.INDSL.ORDER_NONavigation.SO)
                    .Include(d => d.YRCV.IND.INDSL.ORDER_NONavigation.RS)
                    .Include(d => d.YRCV.IND.INDSL.ORDERNO_SNavigation)
                    .Include(c=>c.BasYarnLotinfo)
                    .Include(c=>c.FYsRawPer)
                    .Include(c=>c.FYarnFor)
                    .Include(c=>c.LOCATION)
                    .Include(c=>c.LEDGER)
                    .Where(d => d.PRODID.Equals(countId) && d.INDENT_TYPE.Equals(indentType) && d.YRCV.YRCVDATE <= issueDate 
                                && d.RCV_QTY - (d.F_YS_YARN_ISSUE_DETAILS.Where(c=>c.YISSUE.YISSUEDATE <= issueDate).Sum(f => f.ISSUE_QTY) ?? 0) > 0
                    )
                    .Select(d => new F_YS_YARN_RECEIVE_DETAILS
                    {
                        TRNSID = d.TRNSID,
                        OPT1 = $"{d.YRCV.IND.INDSL.INDSLNO} - " +
                               $"{(d.YRCV.IND.INDSL != null ? d.YRCV.IND.INDSL.ORDER_NONavigation != null && d.YRCV.IND.INDSL.ORDER_NONavigation.SO != null ? d.YRCV.IND.INDSL.ORDER_NONavigation.SO.SO_NO : d.YRCV.IND.INDSL.ORDERNO_SNavigation != null ? d.YRCV.IND.INDSL.ORDERNO_SNavigation.SDRF_NO : "" : "")} - {d.BasYarnLotinfo.LOTNO}- {d.BAG - (DenimDbContext.F_YS_YARN_ISSUE_DETAILS.Where(c => c.RCVDID.Equals(d.TRNSID)).Sum(f => f.BAG) ?? 0)} Bag - {d.RCV_QTY - (DenimDbContext.F_YS_YARN_ISSUE_DETAILS.Include(c=>c.YISSUE).Where(c=>c.RCVDID.Equals(d.TRNSID) && c.YISSUE.YISSUEDATE <= issueDate).Sum(f => f.ISSUE_QTY) ?? 0)} Kg - {d.FYsRawPer.RAWPER} - {d.FYarnFor.YARNNAME} - {d.LOCATION.LOCNAME} - {d.LEDGER.LEDNAME} - {d.PAGENO} - {d.YRCV.YRCVDATE}",
                        OPT2 = d.RCV_QTY.ToString(),
                        OPT3 = DenimDbContext.F_YS_YARN_ISSUE_DETAILS.Include(c=>c.YISSUE).Where(c => c.RCVDID.Equals(d.TRNSID) && c.YISSUE.YISSUEDATE <= issueDate).Sum(f => f.ISSUE_QTY).ToString()
                    }).ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return null;
            }
        }

        public async Task<F_YS_YARN_RECEIVE_DETAILS> GetAllByRcvdId(int id)
        {
            return await DenimDbContext.F_YS_YARN_RECEIVE_DETAILS
                .Include(d => d.YRCV.IND.INDSL)
                .Where(d => d.TRNSID.Equals(id))
                .Select(d => new F_YS_YARN_RECEIVE_DETAILS
                {
                    LOT = d.LOT,
                    YRCV = new F_YS_YARN_RECEIVE_MASTER
                    {
                        IND = new F_YS_INDENT_MASTER
                        {
                            INDSL = new RND_PURCHASE_REQUISITION_MASTER
                            {
                                ORDER_NO = d.YRCV.IND.INDSL.ORDER_NO ?? d.YRCV.IND.INDSL.ORDERNO_S
                            }
                        }
                    }
                }).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<F_YS_YARN_RECEIVE_DETAILS>> GetCountAsync(DateTime? issueDate)
        {
            return await DenimDbContext.F_YS_YARN_RECEIVE_DETAILS
                .Include(d => d.F_YS_YARN_ISSUE_DETAILS)
                .ThenInclude(c => c.YISSUE)
                .Include(d => d.YRCV)
                .Include(d => d.PROD)
                .Include(d=>d.BasYarnLotinfo)
                .Where(d => d.YRCV.YRCVDATE <= issueDate
                            && d.RCV_QTY - (d.F_YS_YARN_ISSUE_DETAILS.Where(c => c.YISSUE.YISSUEDATE <= issueDate).Sum(f => f.ISSUE_QTY) ?? 0) > 0)
                .Select(d => new F_YS_YARN_RECEIVE_DETAILS
                {
                    PROD = new BAS_YARN_COUNTINFO
                    {
                        COUNTID = d.PROD.COUNTID,
                        RND_COUNTNAME = $"{d.PROD.RND_COUNTNAME} - Lot: {d.BasYarnLotinfo.LOTNO} - Qty:{d.RCV_QTY - (d.F_YS_YARN_ISSUE_DETAILS.Sum(f => f.ISSUE_QTY) ?? 0)} {d.PROD.UNIT}",
                    }
                }).OrderBy(e => e.PROD.COUNTID).ToListAsync();
        }
    }
}
