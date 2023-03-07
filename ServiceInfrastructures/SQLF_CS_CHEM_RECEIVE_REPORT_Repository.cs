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
    public class SQLF_CS_CHEM_RECEIVE_REPORT_Repository : BaseRepository<F_CS_CHEM_RECEIVE_REPORT>, IF_CS_CHEM_RECEIVE_REPORT
    {
        public SQLF_CS_CHEM_RECEIVE_REPORT_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {

        }

        public async Task<int> GetLastMrrNo()
        {
            try
            {
                var result = await DenimDbContext.F_CS_CHEM_RECEIVE_REPORT.OrderByDescending(e => e.CMRRNO).ToListAsync();
                if (result.Any())
                {
                    return int.Parse(result.FirstOrDefault().CMRRNO.ToString()) + 1;
                }
                else
                {
                    return 400000;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
