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
    public class SQLF_CHEM_ISSUE_DETAILS_Repository : BaseRepository<F_CHEM_ISSUE_DETAILS>, IF_CHEM_ISSUE_DETAILS
    {
        public SQLF_CHEM_ISSUE_DETAILS_Repository(DenimDbContext denimDbContext) : base(denimDbContext) {}
        public async Task<int> InsertAndGetIdAsync(F_CHEM_ISSUE_DETAILS fChemIssueDetails)
        {
            try
            {
                await DenimDbContext.F_CHEM_ISSUE_DETAILS.AddAsync(fChemIssueDetails);
                await SaveChanges();
                return fChemIssueDetails.CISSDID;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<DashboardViewModel> GetIssuedChemicalData()
        {
            var dashBoardViewModel = new DashboardViewModel
            {
                FChemIssueViewModel = new FChemIssueViewModel
                {
                    TotalIssuedChemical = await DenimDbContext.F_CHEM_ISSUE_DETAILS
                        .Where(c => c.CISSDDATE.Equals(Convert.ToDateTime("2022-01-01")))
                        .Select(c => c.ISSUE_QTY).SumAsync()
                }
            };
            return dashBoardViewModel;
        }
    }
}
