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
    public class SQLF_CHEM_STORE_RECEIVE_DETAILS_Repository : BaseRepository<F_CHEM_STORE_RECEIVE_DETAILS>, IF_CHEM_STORE_RECEIVE_DETAILS
    {
        public SQLF_CHEM_STORE_RECEIVE_DETAILS_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public IEnumerable<F_CHEM_STORE_RECEIVE_DETAILS> FindAllChemByReceiveIdAsync(int id)
        {
            try
            {
                var result = DenimDbContext.F_CHEM_STORE_RECEIVE_DETAILS
                    .GroupJoin(DenimDbContext.F_CS_CHEM_RECEIVE_REPORT,
                        f1 => f1.TRNSID,
                        f2 => f2.CRDID,
                        (f1, f2) => new
                        {
                            F1 = f1,
                            F2 = f2.ToList()
                        })
                    .GroupJoin(DenimDbContext.F_CHEM_QC_APPROVE,
                        f3 => f3.F1.TRNSID,
                        f4 => f4.CRDID,
                        (f3, f4) => new F_CHEM_STORE_RECEIVE_DETAILS
                        {
                            CHEMRCVID = f3.F1.CHEMRCVID,
                            TRNSID = f3.F1.TRNSID,
                            TRNSDATE = f3.F1.TRNSDATE,
                            PRODUCTID = f3.F1.PRODUCTID,
                            UNIT = f3.F1.UNIT,
                            CINDID = f3.F1.CINDID,
                            CINDDATE = f3.F1.CINDDATE,
                            FRESH_QTY = f3.F1.FRESH_QTY,
                            REJ_QTY = f3.F1.REJ_QTY,
                            INVQTY = f3.F1.INVQTY,
                            RATE = f3.F1.RATE,
                            CURRENCY = f3.F1.CURRENCY,
                            AMOUNT = f3.F1.AMOUNT,
                            BATCHNO = f3.F1.BATCHNO,
                            MNGDATE = f3.F1.MNGDATE,
                            EXDATE = f3.F1.EXDATE,
                            REMARKS = f3.F1.REMARKS,
                            QC_APPROVE = IsApprovedByQc(f4.FirstOrDefault().APPROVED_BY),
                            MRR_CREATE = IsMRRCreated(f3.F2.FirstOrDefault().CRDID.ToString())
                        })
                    .Where(e => e.CHEMRCVID.Equals(id)).ToList();

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<double?> GetRemainingBalanceByBatchId(int? productId, string batchNo)
        {
            return await DenimDbContext.F_CHEM_STORE_RECEIVE_DETAILS
                .Where(d => d.PRODUCTID.Equals(productId) && d.BATCHNO.Equals(batchNo))
                .Select(d => d.FRESH_QTY).FirstOrDefaultAsync() - await DenimDbContext.F_CHEM_ISSUE_DETAILS
                .Include(d => d.CRCVIDDNavigation)
                .Where(d => d.PRODUCTID.Equals(productId) && d.CRCVIDDNavigation.BATCHNO.Equals(batchNo))
                .Select(d => d.ISSUE_QTY).FirstOrDefaultAsync();

            #region Obsolete

            //try
            //{
            //var freshQty = await _denimDbContext.F_CHEM_STORE_RECEIVE_DETAILS
            //    .Include(c => c.F_CHEM_TRANSECTION)
            //    .Where(e => e.BATCHNO.Equals(batchNo) && e.PRODUCTID.Equals(productId))
            //    .Select(e => e.F_CHEM_TRANSECTION.OrderByDescending(f => f.CTRID).Select(g => new F_CHEM_TRANSECTION
            //    {
            //        BALANCE = g.BALANCE
            //    }).FirstOrDefault()).FirstOrDefaultAsync();

            //return freshQty.BALANCE;



            //var result = await _denimDbContext.F_CHEM_STORE_RECEIVE_DETAILS
            //    .Include(c => c.F_CHEM_TRANSECTION)
            //    .FirstOrDefaultAsync(e => e.TRNSID.Equals(batchId) && e.BATCHNO.Equals(batchNo));

            //return result.F_CHEM_TRANSECTION.OrderByDescending(e => e.CTRID).FirstOrDefault()?.BALANCE;
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e);
            //    throw;
            //}

            #endregion
        }

        public async Task<FChemStoreReceiveViewModel> GetInitObjForDetails(FChemStoreReceiveViewModel fChemStoreReceiveDetailses)
        {

            foreach (var item in fChemStoreReceiveDetailses.FChemStoreReceiveDetailsList)
            {
                item.FChemStoreProductinfo = await DenimDbContext.F_CHEM_STORE_PRODUCTINFO.FirstOrDefaultAsync(e => e.PRODUCTID.Equals(item.PRODUCTID));
            }

            return fChemStoreReceiveDetailses;
        }

        public string IsApprovedByQc(string status)
        {
            return status != null ? "Approved" : "Not Approved";
        }

        public string IsMRRCreated(string mrrid)
        {
            return mrrid != "" ? "MRR Created" : "Not Created";
        }
    }
}
