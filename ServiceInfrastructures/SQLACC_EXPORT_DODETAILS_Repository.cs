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
    public class SQLACC_EXPORT_DODETAILS_Repository : BaseRepository<ACC_EXPORT_DODETAILS>, IACC_EXPORT_DODETAILS
    {
        public SQLACC_EXPORT_DODETAILS_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {

        }

        public async Task<IEnumerable<ACC_EXPORT_DODETAILS>> FindDoDetailsListByDoNoAsync(int doNo)
        {
            try
            {
                var result = await DenimDbContext.ACC_EXPORT_DODETAILS
                    .Include(c => c.PI)
                    .Where(pi => pi.DONO == doNo && pi.PI.PIID == pi.PIID).ToListAsync();

                foreach (var item in result)
                {
                    item.STYLENAME = await DenimDbContext.COM_EX_FABSTYLE.Where(c => c.STYLEID == item.STYLEID).Select(c => c.STYLENAME).FirstOrDefaultAsync();
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<IEnumerable<ACC_EXPORT_DODETAILS>> FindDODetailsBYDONoAndStyleIdAndPIIDAsync(int doNo, int? piID, int styleId)
        {
            try
            {
                if (piID != 0)
                {
                    var result = await DenimDbContext.ACC_EXPORT_DODETAILS
                        .Include(c => c.PI)
                        .Where(pi => pi.DONO == doNo && pi.STYLEID == styleId && pi.PIID == piID && pi.PI.PIID == pi.PIID).ToListAsync();

                    return result;
                }
                else
                {
                    var result = await DenimDbContext.ACC_EXPORT_DODETAILS
                        .Include(c => c.PI)
                        .Where(pi => pi.DONO == doNo && pi.STYLEID == styleId && pi.PI.PIID == pi.PIID).ToListAsync();

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<COM_EX_PI_DETAILS> GetPiDetailsByStyleIdAsync(AccExportDoMasterViewModel accExportDoMasterViewModel)
        {
            return await DenimDbContext.COM_EX_PI_DETAILS
                .Where(d => d.TRNSID.Equals(accExportDoMasterViewModel.ACC_EXPORT_DODETAILS.STYLEID))
                .Select(d => new COM_EX_PI_DETAILS
                {
                    TRNSID = d.TRNSID,
                    QTY = d.QTY,
                    UNITPRICE = d.UNITPRICE,
                    TOTAL = d.TOTAL,
                    REMARKS = d.REMARKS,
                    PIMASTER = new COM_EX_PIMASTER
                    {
                        PIID = d.PIMASTER.PIID,
                        PINO = d.PIMASTER.PINO
                    }
                }).FirstOrDefaultAsync();
        }

        public async Task<AccExportDoMasterViewModel> GetInitObjForDetailsByAsync(AccExportDoMasterViewModel accExportDoMasterViewModel)
        {
            try
            {
                foreach (var item in accExportDoMasterViewModel.aCC_EXPORT_DODETAILSList)
                {
                    item.PI = await DenimDbContext.COM_EX_PIMASTER.FirstOrDefaultAsync(e => e.PIID.Equals(item.PIID));
                    item.STYLE = await DenimDbContext.COM_EX_PI_DETAILS
                        .Include(e => e.STYLE.FABCODENavigation)
                        .Where(e => e.TRNSID.Equals(item.STYLEID))
                        .Select(e => new COM_EX_FABSTYLE
                        {
                            STYLEID = e.STYLE.STYLEID,
                            STYLENAME = e.STYLE.STYLENAME,
                            FABCODENavigation = e.STYLE.FABCODENavigation
                        }).FirstOrDefaultAsync();
                }

                return accExportDoMasterViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
