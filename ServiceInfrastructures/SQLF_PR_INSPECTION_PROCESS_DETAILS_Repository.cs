using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels;
using DenimERP.ViewModels.Factory.Production;
using DenimERP.ViewModels.Home;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_PR_INSPECTION_PROCESS_DETAILS_Repository : BaseRepository<F_PR_INSPECTION_PROCESS_DETAILS>, IF_PR_INSPECTION_PROCESS_DETAILS
    {
        public SQLF_PR_INSPECTION_PROCESS_DETAILS_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
        public async Task<bool> IsRollNoExists(string rollNo)
        {
            try
            {
                return await DenimDbContext.F_PR_INSPECTION_PROCESS_DETAILS.AnyAsync(c => c.ROLLNO.Equals(rollNo));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<F_PR_INSPECTION_PROCESS_MASTER> IsSetNoExists(int setNo)
        {
            try
            {
                return await DenimDbContext.F_PR_INSPECTION_PROCESS_MASTER.Where(c => c.SETID.Equals(setNo)).FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<FPrInspectionProcessViewModel> GetInitData(FPrInspectionProcessViewModel prInspectionProcessViewModel)
        {
            try
            {
                //foreach (var item in prInspectionProcessViewModel.FPrInspectionProcessDetailsList)
                //{
                if (prInspectionProcessViewModel.FPrInspectionProcessDetails.FPrInspectionDefectPointsList != null)
                {
                    foreach (var i in prInspectionProcessViewModel.FPrInspectionProcessDetails.FPrInspectionDefectPointsList)
                    {
                        i.DEF_TYPE = await DenimDbContext.F_PR_INSPECTION_DEFECTINFO
                            .FirstOrDefaultAsync(c => c.DEF_TYPEID.Equals(i.DEF_TYPEID));
                    }
                }

                prInspectionProcessViewModel.FPrInspectionProcessDetails.Operator = await DenimDbContext.F_HRD_EMPLOYEE.Select(c => new F_HRD_EMPLOYEE
                {
                    FIRST_NAME = c.FIRST_NAME,
                    EMPID = c.EMPID
                }).FirstOrDefaultAsync(c => c.EMPID.Equals(prInspectionProcessViewModel.FPrInspectionProcessDetails.OPERATOR_ID));
                prInspectionProcessViewModel.FPrInspectionProcessDetails.MACHINE_ = await DenimDbContext.F_PR_INSPECTION_MACHINE.FirstOrDefaultAsync(c =>
                        c.ID.Equals(prInspectionProcessViewModel.FPrInspectionProcessDetails.MACHINE_ID));
                //item.BATCHNavigation =await _denimDbContext.F_PR_INSPECTION_BATCH.FirstOrDefaultAsync(c => c.ID.Equals(item.BATCH));
                prInspectionProcessViewModel.FPrInspectionProcessDetails.CUT_PCS_SECTIONNavigation = await DenimDbContext.F_BAS_SECTION.FirstOrDefaultAsync(
                            c => c.SECID.Equals(prInspectionProcessViewModel.FPrInspectionProcessDetails.CUT_PCS_SECTION));
                prInspectionProcessViewModel.FPrInspectionProcessDetails.TROLLEYNONavigation = await DenimDbContext.F_PR_FINISHING_FNPROCESS
                        .Include(c => c.TROLLNONavigation)
                        .Where(c => c.FIN_PROCESSID.Equals(prInspectionProcessViewModel.FPrInspectionProcessDetails.TROLLEYNO)).FirstOrDefaultAsync();
                //}
                return prInspectionProcessViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<int> InsertAndGetIdAsync(F_PR_INSPECTION_PROCESS_DETAILS fPrInspectionProcessDetails)
        {
            try
            {
                await DenimDbContext.F_PR_INSPECTION_PROCESS_DETAILS.AddAsync(fPrInspectionProcessDetails);
                await SaveChanges();
                return fPrInspectionProcessDetails.ROLL_ID;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<F_PR_FINISHING_FNPROCESS>> GetTrollyListBySetId(int setId)
        {
            try
            {
                var result = await DenimDbContext.F_PR_FINISHING_FNPROCESS
                    .Include(c => c.TROLLNONavigation)
                    .Include(c => c.FN_PROCESS.DOFF.WV_BEAM.WV_PROCESS.SET)
                    .Include(c => c.FIN_PRO_TYPE)
                    .Where(c => c.FIN_PRO_TYPE.NAME.Equals("FINISH") && c.FN_PROCESS.DOFF.WV_BEAM.WV_PROCESS.SET.SETID.Equals(setId)
                                                                     && !DenimDbContext.F_PR_INSPECTION_PROCESS_DETAILS.Any(e => e.TROLLEYNO.Equals(c.FIN_PROCESSID) && e.TROLLY_STATUS)
                    )
                    .ToListAsync();

                //NAME = $"{c.TROLLNONavigation.NAME}({c.LENGTH_OUT} Mtr,{Math.Round((double)(c.LENGTH_OUT * 1.094), 2)} Yds)"
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        public async Task<IEnumerable<F_PR_FINISHING_FNPROCESS>> GetTrollyListBySetIdEdit(int setId)
        {
            try
            {
                var result = await DenimDbContext.F_PR_FINISHING_FNPROCESS
                    .Include(c => c.TROLLNONavigation)
                    .Include(c => c.FN_PROCESS.DOFF.WV_BEAM.WV_PROCESS.SET)
                    .Include(c => c.FIN_PRO_TYPE)

                    .Where(c => c.FIN_PRO_TYPE.NAME.Equals("FINISH") && c.FN_PROCESS.DOFF.WV_BEAM.WV_PROCESS.SET.SETID.Equals(setId))
                    .Select(c => new F_PR_FINISHING_FNPROCESS
                    {
                        FIN_PROCESSID = c.FIN_PROCESSID,
                        TROLLNONavigation = new F_PR_FIN_TROLLY
                        {
                            NAME = $"{c.TROLLNONavigation.NAME}({c.LENGTH_OUT} Mtr,{Math.Round((double)(c.LENGTH_OUT * 1.094), 2)} Yds)"
                        }
                    })
                    .ToListAsync();
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public async Task<bool> IsRollNoInUseAsync(string rollNo)
        {
            try
            {
                var flag = await DenimDbContext.F_PR_INSPECTION_PROCESS_DETAILS.AnyAsync(c => c.ROLLNO.Equals(rollNo));
                return flag;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public async Task<List<F_PR_INSPECTION_PROCESS_DETAILS>> GetRollListByInsIdAsync(int insId)
        {
            try
            {
                var rollList = await DenimDbContext.F_PR_INSPECTION_PROCESS_DETAILS.Where(c => c.INSPID.Equals(insId))
                    .ToListAsync();

                foreach (var item in rollList)
                {
                    item.FPrInspectionDefectPointsList = await DenimDbContext.F_PR_INSPECTION_DEFECT_POINT
                        .Where(c => c.ROLL_ID.Equals(item.ROLL_ID)).ToListAsync();
                }

                return rollList;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        public async Task<F_PR_INSPECTION_PROCESS_DETAILS> GetRollDetailsByInsIdAsync(int rollId)
        {
            try
            {
                var roll = await DenimDbContext.F_PR_INSPECTION_PROCESS_DETAILS
                    .Include(c=>c.INSP.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation)
                    .Where(c => c.ROLL_ID.Equals(rollId))
                    .FirstOrDefaultAsync();

                roll.FPrInspectionDefectPointsList = await DenimDbContext.F_PR_INSPECTION_DEFECT_POINT
                        .Where(c => c.ROLL_ID.Equals(roll.ROLL_ID)).ToListAsync();
                
                return roll;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        public async Task<F_PR_INSPECTION_PROCESS_DETAILS> GetDefectDetailsByInsIdAsync(int insId)
        {
            try
            {
                var rollList = await DenimDbContext.F_PR_INSPECTION_PROCESS_DETAILS.Where(c => c.INSPID.Equals(insId))
                    .FirstOrDefaultAsync();
                return rollList;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public async Task<F_PR_INSPECTION_PROCESS_DETAILS> FindByRollNoAsync(string rollNO)
        {
            try
            {
                return await DenimDbContext.F_PR_INSPECTION_PROCESS_DETAILS
                    .Include(c => c.F_FS_FABRIC_RCV_DETAILS)
                    .Include(c=>c.F_FS_FABRIC_CLEARANCE_DETAILS)
                    //.Include(c => c.F_FS_CLEARANCE_MASTER_SAMPLE_ROLL)
                    .Include(c=>c.F_PR_INSPECTION_DEFECT_POINT)
                    .Where(c => c.ROLLNO.Equals(rollNO)).AsNoTracking().FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<List<F_PR_INSPECTION_DEFECT_POINT>> GetDefectListByInsIdAsync(string rollNO)
        {
            try
            {
                var roll = await DenimDbContext.F_PR_INSPECTION_PROCESS_DETAILS.Where(c => c.ROLLNO.Equals(rollNO))
                    .FirstOrDefaultAsync();
                if (roll != null)
                {
                    var result = await DenimDbContext.F_PR_INSPECTION_DEFECT_POINT
                        .Where(c => c.ROLL_ID.Equals(roll.ROLL_ID)).ToListAsync();
                    return result;
                }

                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<F_PR_FINISHING_FNPROCESS> GetTrollyDetails(int trollyId, int setId)
        {
            try
            {
                var result = await DenimDbContext.F_PR_FINISHING_FNPROCESS
                    .Include(c => c.FN_PROCESS)
                    .Include(c => c.FN_PROCESS.DOFF.WV_BEAM.WV_PROCESS.SET)
                    .Include(c => c.FN_PROCESS.DOFF.DOFFER_NAMENavigation)
                    .Include(c => c.FN_PROCESS.DOFF.WV_BEAM.F_PR_SLASHER_DYEING_DETAILS.W_BEAM)
                    .Include(c => c.FN_PROCESS.DOFF.WV_BEAM.F_PR_SIZING_PROCESS_ROPE_DETAILS.W_BEAM)
                    .Include(c => c.FN_PROCESS.DOFF.LOOM_NONavigation)
                    .Include(c => c.FN_PROCESS.DOFF.OTHER_DOFF)
                    .Include(c => c.FN_PROCESS.DOFF.WV)
                    .Include(c => c.FN_PROCESS.FABRICINFO)
                    .Include(c => c.FIN_PRO_TYPE)
                    .Include(c => c.FN_MACHINE)
                    .Include(c => c.PROCESS_BYNavigation)
                    .Include(c => c.F_PR_INSPECTION_PROCESS_DETAILS)
                    .FirstOrDefaultAsync(c => c.FIN_PROCESSID.Equals(trollyId));

                double? setLength = 0;
                double? totalProcessLength = 0;
                double? inspectedLength = 0;
                double? restTrollyLength = 0;

                foreach (var item in result.FN_PROCESS.DOFF.F_PR_FINISHING_PROCESS_MASTER)
                {
                    if (item.DOFF.WV_BEAM.WV_PROCESS.SET.SETID.Equals(setId))
                    {
                        setLength += item.DOFF.LENGTH_BULK;
                    }
                    totalProcessLength += item.DOFF.LENGTH_BULK;
                }

                foreach (var item in result.F_PR_INSPECTION_PROCESS_DETAILS)
                {
                    inspectedLength += item.LENGTH_MTR;
                }

                restTrollyLength = result.LENGTH_OUT - inspectedLength;

                setLength = (setLength * result.LENGTH_OUT) / totalProcessLength;

                result.OPT1 = setLength.ToString();
                result.OPT2 = restTrollyLength.ToString();
                result.OPT3 = result.FN_PROCESS.DOFF.WV_BEAM.F_PR_SIZING_PROCESS_ROPE_DETAILS != null
                    ? result.FN_PROCESS.DOFF.WV_BEAM.F_PR_SIZING_PROCESS_ROPE_DETAILS.W_BEAM.BEAM_NO
                    : result.FN_PROCESS.DOFF.WV_BEAM.F_PR_SLASHER_DYEING_DETAILS.W_BEAM.BEAM_NO;

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        public async Task<List<F_PR_INSPECTION_PROCESS_DETAILS>> GetRollListByStyle(int fabcode)
        {
            try
            {
                List<F_PR_INSPECTION_PROCESS_DETAILS> result;
                if (fabcode != 0)
                {
                    result = await DenimDbContext.F_PR_INSPECTION_PROCESS_DETAILS.Include(c =>
                            c.INSP.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation)
                        .Where(c =>
                            c.INSP.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.FABCODE
                                .Equals(fabcode))
                        .Select(c => new F_PR_INSPECTION_PROCESS_DETAILS
                        {
                            ROLL_ID = c.ROLL_ID,
                            ROLLNO = c.ROLLNO
                        })
                        .OrderByDescending(c=>c.ROLL_ID)
                        .ToListAsync();
                }
                else
                {
                    result = await DenimDbContext.F_PR_INSPECTION_PROCESS_DETAILS
                        .Select(c => new F_PR_INSPECTION_PROCESS_DETAILS
                        {
                            ROLL_ID = c.ROLL_ID,
                            ROLLNO = c.ROLLNO
                        })
                        .OrderByDescending(c => c.ROLL_ID)
                        .ToListAsync();
                }
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }



        public async Task<List<F_PR_INSPECTION_PROCESS_DETAILS>> GetRollListByStyleDynamic(string search, int page, int fabcode)
        {
            try
            {
                List<F_PR_INSPECTION_PROCESS_DETAILS> result = new List<F_PR_INSPECTION_PROCESS_DETAILS>();

                if (!string.IsNullOrEmpty(search))
                {
                    result = await DenimDbContext.F_PR_INSPECTION_PROCESS_DETAILS.Include(c =>
                            c.INSP.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation)
                        .Where(c =>
                            c.INSP.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.FABCODE
                                .Equals(fabcode) && c.ROLLNO.Contains(search.ToLower()))
                        .Select(c => new F_PR_INSPECTION_PROCESS_DETAILS
                        {
                            ROLL_ID = c.ROLL_ID,
                            ROLLNO = c.ROLLNO
                        })
                        .OrderByDescending(c => c.ROLL_ID)
                        .ToListAsync();

                    
                }
                else
                {

                    result = await DenimDbContext.F_PR_INSPECTION_PROCESS_DETAILS
                        .Where(c =>
                            c.ROLLNO.Contains(search.ToLower()))
                        .Select(c => new F_PR_INSPECTION_PROCESS_DETAILS
                        {
                            ROLL_ID = c.ROLL_ID,
                            ROLLNO = c.ROLLNO
                        })
                        .OrderByDescending(c => c.ROLL_ID)
                        .ToListAsync();
                }
                
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }



        public async Task<List<F_PR_INSPECTION_PROCESS_DETAILS>> GetRollListByDate(DateTime? date)
        {
            try
            {
                List<F_PR_INSPECTION_PROCESS_DETAILS> result;
                if (date != null)
                {
                    result = await DenimDbContext.F_PR_INSPECTION_PROCESS_DETAILS
                        .Where(c => c.ROLL_INSPDATE.Equals(date))
                        .Select(c => new F_PR_INSPECTION_PROCESS_DETAILS
                        {
                            ROLL_ID = c.ROLL_ID,
                            ROLLNO = c.ROLLNO
                        })
                        .OrderByDescending(c=>c.ROLL_ID)
                        .ToListAsync();
                }
                else
                {
                    result = await DenimDbContext.F_PR_INSPECTION_PROCESS_DETAILS
                        .Select(c => new F_PR_INSPECTION_PROCESS_DETAILS
                        {
                            ROLL_ID = c.ROLL_ID,
                            ROLLNO = c.ROLLNO
                        })
                        .OrderByDescending(c => c.ROLL_ID)
                        .ToListAsync();
                }
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<List<ChartViewModel>> GetInspectionDateWiseLengthGraph()
        {
            try
            {
                var inspectionChartDataViewModel = new InspectionChartDataViewModel();

                inspectionChartDataViewModel.InspectionLengthMtr = await DenimDbContext.F_PR_INSPECTION_PROCESS_DETAILS
                    .Where(c => c.ROLL_INSPDATE.Equals(Convert.ToDateTime("2022-01-24")) && c.PROCESS_TYPE.Equals(1))
                    .Select(d => new F_PR_INSPECTION_PROCESS_DETAILS()
                    {
                        LENGTH_MTR = d.LENGTH_MTR
                        
                    }).SumAsync(c => Convert.ToDouble(c.LENGTH_MTR ?? 0));

                inspectionChartDataViewModel.InspectionLengthYds = await DenimDbContext.F_PR_INSPECTION_PROCESS_DETAILS
                    .Where(c => c.ROLL_INSPDATE.Equals(Convert.ToDateTime("2022-01-24")) && c.PROCESS_TYPE.Equals(1))
                    .Select(d => new F_PR_INSPECTION_PROCESS_DETAILS()
                    {
                        LENGTH_YDS = d.LENGTH_YDS

                    }).SumAsync(c => Convert.ToDouble(c.LENGTH_YDS ?? 0));


                var data = new List<ChartViewModel>();

                var date = Convert.ToDateTime("2022-05-09").AddDays(-15);

                for (var i = 0; i < 15; i++)
                {
                    data.Add(new ChartViewModel
                    {
                        date = date.AddDays(i).ToString("yyyy-MM-dd"),
                        sales2 = await DenimDbContext.F_PR_INSPECTION_PROCESS_DETAILS
                            .Where(c => c.ROLL_INSPDATE.Equals(date.AddDays(i).Date) && c.FAB_GRADE.Equals("A") && c.PROCESS_TYPE.Equals(1))
                            .Select(d => new F_PR_INSPECTION_PROCESS_DETAILS()
                            {
                                LENGTH_MTR = d.LENGTH_MTR

                            }).SumAsync(c => Convert.ToDouble(c.LENGTH_MTR ?? 0)), /*Elite Production*/
                        sales1 = await DenimDbContext.F_PR_INSPECTION_PROCESS_DETAILS
                            .Where(c => c.ROLL_INSPDATE.Equals(date.AddDays(i).Date) && c.FAB_GRADE.Equals("A2") && c.PROCESS_TYPE.Equals(1))
                            .Select(d => new F_PR_INSPECTION_PROCESS_DETAILS()
                            {
                                LENGTH_MTR = d.LENGTH_MTR

                            }).SumAsync(c => Convert.ToDouble(c.LENGTH_MTR ?? 0)), /*A2 Production*/
                        market1 = await DenimDbContext.F_PR_INSPECTION_PROCESS_DETAILS
                            .Where(c => c.ROLL_INSPDATE.Equals(date.AddDays(i).Date) && c.PROCESS_TYPE.Equals(1))
                            .Select(d => new F_PR_INSPECTION_PROCESS_DETAILS()
                            {
                                LENGTH_MTR = d.LENGTH_MTR

                            }).SumAsync(c => Convert.ToDouble(c.LENGTH_MTR ?? 0)), /*Production*/
                        market2 = await DenimDbContext.F_PR_INSPECTION_PROCESS_DETAILS
                            .Where(c => c.ROLL_INSPDATE.Equals(date.AddDays(i).Date) && !c.PROCESS_TYPE.Equals(1))
                            .Select(d => new F_PR_INSPECTION_PROCESS_DETAILS()
                            {
                                LENGTH_MTR = d.LENGTH_MTR

                            }).SumAsync(c => Convert.ToDouble(c.LENGTH_MTR ?? 0)), /*Non Production*/
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

        public async Task<DashboardViewModel> GetInspectionTotalLength()
        {

            try
            {
                var date = Convert.ToDateTime("2022-05-09");
                var dashboardViewModel = new DashboardViewModel
                {
                    InspectionChartDataViewModel = new InspectionChartDataViewModel()
                    {
                        TotalInspectionYds = await DenimDbContext.F_PR_INSPECTION_PROCESS_DETAILS
                            .Where(c => c.ROLL_INSPDATE.Equals(date))
                            .Select(d => new F_PR_INSPECTION_PROCESS_DETAILS()
                            {
                                LENGTH_YDS = d.LENGTH_YDS

                            }).SumAsync(c => Convert.ToDouble(c.LENGTH_YDS ?? 0)),
                        
                    }
                };

                return dashboardViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<F_PR_INSPECTION_PROCESS_MASTER>> GetTopStyleProductionData()
        {
            return await DenimDbContext.F_PR_INSPECTION_PROCESS_MASTER
                .Include(d => d.F_PR_INSPECTION_PROCESS_DETAILS)
                .Include(d => d.FabricInfo)
                .Where(d => d.F_PR_INSPECTION_PROCESS_DETAILS.Any(e => e.ROLL_INSPDATE.Equals(Convert.ToDateTime("2022-07-16"))))
                .Select(d => new F_PR_INSPECTION_PROCESS_MASTER
                {
                    TotalProduction = d.F_PR_INSPECTION_PROCESS_DETAILS.Where(e => e.ROLL_INSPDATE.Equals(Convert.ToDateTime("2022-07-16"))).Sum(e => e.LENGTH_1 + e.LENGTH_2),
                    FabricInfo = new RND_FABRICINFO
                    {
                        STYLE_NAME = d.FabricInfo.STYLE_NAME
                    }
                }).OrderByDescending(d => d.TotalProduction).Take(5).ToListAsync();
        }
    }
}
