using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces.ProcWorkOrder;
using DenimERP.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures.ProcWorkOrder
{
    public class SQLPROC_WORKORDER_MASTER_Repository : BaseRepository<PROC_WORKORDER_MASTER>, IPROC_WORKORDER_MASTER
    {
        public SQLPROC_WORKORDER_MASTER_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<IEnumerable<PROC_WORKORDER_MASTER>> GetAllProcWorkOrder()
        {
            try
            {
                return await DenimDbContext.PROC_WORKORDER_MASTER
                    .Select(d => new PROC_WORKORDER_MASTER
                    {
                        WODATE = d.WODATE,
                        PAYMODE = d.PAYMODE,
                        CURRENCY = d.CURRENCY,
                        UNIT = d.UNIT,
                        CARRING_AMT = d.CARRING_AMT,
                        DISC_AMT = d.DISC_AMT,
                        PAY_AMT = d.PAY_AMT
                    }).ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<ProcWorkOrderViewModel> GetInitObjByAsync(ProcWorkOrderViewModel procWorkOrderViewModel)
        {
            try
            {
                procWorkOrderViewModel.SupplierList = await DenimDbContext.BAS_SUPPLIERINFO
                    .Select(d => new BAS_SUPPLIERINFO
                    {
                        SUPPID = d.SUPPID,
                        SUPPNAME = d.SUPPNAME
                    }).ToListAsync();

                procWorkOrderViewModel.IndentList = await DenimDbContext.F_YS_INDENT_MASTER
                    .Include(d => d.INDSL)
                    .Where(d => !d.INDSL.INDSLNO.Equals(null))
                    .Select(d => new F_YS_INDENT_MASTER
                    {
                        INDID = d.INDID,
                        INDSL = new RND_PURCHASE_REQUISITION_MASTER
                        {
                            INDSLNO = d.INDSL.INDSLNO
                        }
                    }).OrderBy(d => d.INDID).ToListAsync();

                return procWorkOrderViewModel;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<ProcWorkOrderViewModel> GetIndentProductInfoByAsync(ProcWorkOrderViewModel procWorkOrderViewModel)
        {
            return await DenimDbContext.F_YS_INDENT_MASTER
                .Include(e => e.F_YS_INDENT_DETAILS)
                .ThenInclude(e => e.EncryptedId)
                .Where(e => e.INDID.Equals(procWorkOrderViewModel.ProcWorkOrderDetails.INDENTNO))
                .GroupJoin(DenimDbContext.F_GS_PRODUCT_INFORMATION,
                    f1 => f1.F_YS_INDENT_DETAILS.FirstOrDefault().PRODID,
                    f2 => f2.PRODID,
                    (f1, f2) => new ProcWorkOrderViewModel
                    {
                        F_GS_PRODUCT_INFORMATION = f2.ToList()
                    }).FirstOrDefaultAsync();

        }
    }
}
