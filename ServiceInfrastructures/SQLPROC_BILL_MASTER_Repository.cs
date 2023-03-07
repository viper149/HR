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
    public class SQLPROC_BILL_MASTER_Repository : BaseRepository<PROC_BILL_MASTER>, IPROC_BILL_MASTER
    {
        public SQLPROC_BILL_MASTER_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<IEnumerable<PROC_BILL_MASTER>> GetAllProcBillMaster()
        {
            try
            {
                return await DenimDbContext.PROC_BILL_MASTER
                    .Select(d => new PROC_BILL_MASTER
                    {
                        BILLDATE = d.BILLDATE,
                        BILLAMOUNT = d.BILLAMOUNT,
                        PAYMODE = d.PAYMODE,
                        CHALLANID = d.CHALLANID,
                        SOURCE = d.SOURCE,
                        ACTBILL = d.ACTBILL,
                        REMARKS = d.REMARKS

                    }).ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<ProcBillMasterViewModel> GetInitObjByAsync(ProcBillMasterViewModel procBillMasterViewModel)
        {
            try
            {
                procBillMasterViewModel.ChallanList = await DenimDbContext.F_GEN_S_RECEIVE_MASTER
                    .Select(d => new F_GEN_S_RECEIVE_MASTER
                    {
                        GRCVID = d.GRCVID,
                        CHALLAN_NO = d.CHALLAN_NO
                    }).ToListAsync();

                return procBillMasterViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}

