using System;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels;
using DenimERP.ViewModels.Home;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_PR_SIZING_PROCESS_ROPE_DETAILS_Repository : BaseRepository<F_PR_SIZING_PROCESS_ROPE_DETAILS>, IF_PR_SIZING_PROCESS_ROPE_DETAILS
    {
        public SQLF_PR_SIZING_PROCESS_ROPE_DETAILS_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<DashboardViewModel> GetSizingDateWiseLengthGraph()
        {
            try
            {
                var dashBoardViewModel = new DashboardViewModel
                {
                    SizingChartDataViewModel = new SizingChartDataViewModel
                    {
                        SizingData = await DenimDbContext.F_PR_SIZING_PROCESS_ROPE_MASTER
                            .Include(d => d.SET)
                            .Include(d => d.F_PR_SIZING_PROCESS_ROPE_DETAILS)
                            .Where(c => c.TRNSDATE.Equals(Convert.ToDateTime("2022-01-24")))
                            .Select(d => d.F_PR_SIZING_PROCESS_ROPE_DETAILS.Sum(e => e.LENGTH_PER_BEAM))
                            .SumAsync()
            }
                };
                return dashBoardViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
