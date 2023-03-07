using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels.Factory.Production;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_PR_WEAVING_BEAM_RECEIVING_Repository:BaseRepository<F_PR_WEAVING_BEAM_RECEIVING>, IF_PR_WEAVING_BEAM_RECEIVING
    {
        public SQLF_PR_WEAVING_BEAM_RECEIVING_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
        public async Task<IEnumerable<F_PR_WEAVING_BEAM_RECEIVING>> GetAllAsync()
        {
            try
            {
                var result = await DenimDbContext.F_PR_WEAVING_BEAM_RECEIVING
                    .Include(c=>c.RCVDBYNavigation)
                    .Include(c=>c.SET.PROG_)
                    .ToListAsync();

                foreach (var item in result)
                {
                    if (item.RCVDBYNavigation != null)
                    {
                        item.RCVDBYNavigation = new F_HRD_EMPLOYEE
                        {
                            FIRST_NAME = item.RCVDBYNavigation.FIRST_NAME + " " + item.RCVDBYNavigation.LAST_NAME
                        };
                    }
                    else
                    {
                        item.RCVDBYNavigation=new F_HRD_EMPLOYEE();
                    }
                }

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<PrWeavingProcessViewModel> GetInitObjects(PrWeavingProcessViewModel prWeavingProcessViewModel)
        {
            try
            {

                var FHrEmployees = await DenimDbContext.F_HR_EMP_OFFICIALINFO
                    .Include(c => c.EMP)
                    .Where(c => c.SECID.Equals(165) && !c.OPN2.Equals("Y"))
                    .ToListAsync();

                prWeavingProcessViewModel.FHrEmployees = FHrEmployees.Select(c => new F_HRD_EMPLOYEE
                {
                    EMPID = c.EMP.EMPID,
                    FIRST_NAME = c.EMP.FIRST_NAME + " " + c.EMP.LAST_NAME + '-' + c.EMP.EMPNO
                }).ToList();

                prWeavingProcessViewModel.PlProductionSetDistributions = await DenimDbContext
                    .PL_PRODUCTION_SETDISTRIBUTION
                    .Include(c => c.PROG_)
                    .Where(c => (DenimDbContext.F_PR_SIZING_PROCESS_ROPE_MASTER.Any(e => e.SETID.Equals(c.SETID)) || DenimDbContext.F_PR_WARPING_PROCESS_DW_MASTER.Any(e => e.SETID.Equals(c.SETID)) ) && !DenimDbContext.F_PR_WEAVING_BEAM_RECEIVING.Any(e => e.SETID.Equals(c.SETID)))
                    .Select(c => new PL_PRODUCTION_SETDISTRIBUTION
                    {
                        SETID = c.SETID,
                        PROG_ = c.PROG_
                    }).ToListAsync();

                return prWeavingProcessViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

    }
}
