using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_PR_SLASHER_DYEING_DETAILS_Repository:BaseRepository<F_PR_SLASHER_DYEING_DETAILS>, IF_PR_SLASHER_DYEING_DETAILS
    {
        public SQLF_PR_SLASHER_DYEING_DETAILS_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<IEnumerable<F_PR_SLASHER_DYEING_DETAILS>> GetInitBeamData(List<F_PR_SLASHER_DYEING_DETAILS> fPrSlasherDyeingDetailses)
        {
            try
            {
                foreach (var item in fPrSlasherDyeingDetailses)
                {
                    item.EMP = await DenimDbContext.F_HRD_EMPLOYEE
                        .Where(c => c.EMPID.Equals(item.EMPID))
                        .Select(c => new F_HRD_EMPLOYEE
                        {
                            EMPID = c.EMPID,
                            FIRST_NAME = $"{c.FIRST_NAME} - {c.EMPNO}"
                        })
                        .FirstOrDefaultAsync();

                    item.OFFICER = await DenimDbContext.F_HRD_EMPLOYEE
                        .Where(c => c.EMPID.Equals(item.OFFICERID))
                        .Select(c => new F_HRD_EMPLOYEE
                        {
                            EMPID = c.EMPID,
                            FIRST_NAME = $"{c.FIRST_NAME} - {c.EMPNO}"
                        })
                        .FirstOrDefaultAsync();

                    item.SL_M = await DenimDbContext.F_PR_SLASHER_MACHINE_INFO
                        .Where(c => c.SL_MID.Equals(item.SL_MID))
                        .FirstOrDefaultAsync();

                    item.W_BEAM = await DenimDbContext.F_WEAVING_BEAM
                        .Where(c => c.ID.Equals(item.W_BEAMID)).FirstOrDefaultAsync();
                }
                return fPrSlasherDyeingDetailses;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

    }
}
