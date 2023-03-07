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
    public class SQLF_PR_FINISHING_FNPROCESS_Repository : BaseRepository<F_PR_FINISHING_FNPROCESS>, IF_PR_FINISHING_FNPROCESS
    {
        public SQLF_PR_FINISHING_FNPROCESS_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<IEnumerable<F_PR_FINISHING_FNPROCESS>> GetInitFinishData(List<F_PR_FINISHING_FNPROCESS> fPrFinishingFnProcesses)
        {
            try
            {
                foreach (var item in fPrFinishingFnProcesses)
                {
                    item.FN_MACHINE = await DenimDbContext.F_PR_FN_MACHINE_INFO.FirstOrDefaultAsync(c =>
                            c.FN_MACHINEID.Equals(item.FN_MACHINEID));
                    item.FIN_PRO_TYPE = await DenimDbContext.F_PR_FN_PROCESS_TYPEINFO.FirstOrDefaultAsync(c =>
                            c.FIN_PRO_TYPEID.Equals(item.FIN_PRO_TYPEID));
                    item.PROCESS_BYNavigation = await DenimDbContext.F_HRD_EMPLOYEE
                        .Select(c => new F_HRD_EMPLOYEE
                        {
                            FIRST_NAME = c.FIRST_NAME + " " + c.LAST_NAME,
                            EMPID = c.EMPID
                        })
                        .FirstOrDefaultAsync(c => c.EMPID.Equals(item.PROCESS_BY));
                    item.TROLLNONavigation = await DenimDbContext.F_PR_FIN_TROLLY.FirstOrDefaultAsync(c => c.FIN_TORLLY_ID.Equals(item.TROLLNO));
                    item.SEC = await DenimDbContext.F_BAS_SECTION.FirstOrDefaultAsync(c => c.SECID.Equals(item.SECID));
                }
                return fPrFinishingFnProcesses;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<F_PR_FINISHING_FNPROCESS>> GetFinishList(int finId)
        {
            try
            {
                var fPrFinishingFnProcesses = await DenimDbContext.F_PR_FINISHING_FNPROCESS
                    .Where(c => c.FN_PROCESSID.Equals(finId))
                    .ToListAsync();
                return fPrFinishingFnProcesses;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        public async Task<FinishingChartDataViewModel> GetFinishingProductionData()
        {
            try
            {
                var date = Common.Common.GetDate();
                var defaultDate = Common.Common.GetDefaultDate();
                var finishingChartDataViewModel = new FinishingChartDataViewModel();

                var todaysFinishing = await DenimDbContext.F_PR_FINISHING_FNPROCESS
                    .Include(c => c.FN_PROCESS)
                    .Where(c => (c.FN_PROCESS.FN_PROCESSDATE ?? defaultDate).Date.Equals(date.Date) && c.FIN_PRO_TYPEID == 17 )
                    .Select(c => new F_PR_FINISHING_FNPROCESS
                    {
                        LENGTH_OUT = c.LENGTH_OUT ?? 0
                    }).SumAsync(c => c.LENGTH_OUT);

                var todaysPending = await DenimDbContext.F_PR_WEAVING_PROCESS_DETAILS_B
                    .Where(c => !DenimDbContext.F_PR_FINISHING_PROCESS_MASTER.Any(d => d.DOFF_ID.Equals(c.TRNSID)) && (c.DOFF_TIME ?? defaultDate).Date<=date.Date && c.IS_DELIVERABLE)
                    .Select(c => new F_PR_WEAVING_PROCESS_DETAILS_B
                    {
                        LENGTH_BULK = c.LENGTH_BULK ?? 0
                    }).SumAsync(c => c.LENGTH_BULK);
                    

                finishingChartDataViewModel.TodaysFinishing = todaysFinishing;
                finishingChartDataViewModel.TodaysPending = todaysPending;
                return finishingChartDataViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<List<FinishingChartDataViewModel>> GetFinishingDateWiseLengthGraph()
        {
            //try
            //{
            //    var finishingChartDataViewModel = new FinishingChartDataViewModel();

            //    finishingChartDataViewModel.FinishingLength = await _denimDbContext.F_PR_FINISHING_FNPROCESS
            //        .Where(c => c.FIN_PROCESSDATE.Equals(Convert.ToDateTime("2022-01-24")) && c.FIN_PRO_TYPEID.Equals(17))
            //        .Select(d => new F_PR_FINISHING_FNPROCESS()
            //        {
            //            LENGTH_OUT = d.LENGTH_OUT
            //        }).SumAsync(c => Convert.ToDouble(c.LENGTH_OUT ?? 0));

            //    return finishingChartDataViewModel;
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e);
            //    throw;
            //}
            try

            {
                var data = new List<FinishingChartDataViewModel>();
                var date = Convert.ToDateTime("2022-05-09");

                for (var i = 0; i < 15; i++)
                {
                    data.Add(new FinishingChartDataViewModel
                    {
                        TotalFinishing = await DenimDbContext.F_PR_FINISHING_FNPROCESS
                            .Where(c => c.FIN_PROCESSDATE.Equals(date.AddDays(i).Date))
                            .Select(d => new F_PR_FINISHING_FNPROCESS
                            {
                                LENGTH_OUT = d.LENGTH_OUT
                            }).SumAsync(c => Convert.ToDouble(c.LENGTH_OUT ?? 0))
                    });
                }


                return data;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<FinishingChartDataViewModel> GetFinishingDataDayMonthAsync()
        {
            {
                var date = Common.Common.GetDate();
                var defaultDate = Common.Common.GetDefaultDate();

                var x = new FinishingChartDataViewModel();

                var TodaysFinishing = await DenimDbContext.F_PR_FINISHING_FNPROCESS
                    .CountAsync(c => (c.FIN_PROCESSDATE ?? defaultDate).Date.Equals(date.Date) && c.FIN_PRO_TYPEID.Equals(17));

                var MonthlyFinishing = await DenimDbContext.F_PR_FINISHING_FNPROCESS
                    .Where(c => (c.FIN_PROCESSDATE ?? defaultDate).ToString("yyyy-MM").Equals(date.ToString("yyyy-MM"))).SumAsync(c => c.LENGTH_OUT);

                return x;
            }
        }
    }
}
