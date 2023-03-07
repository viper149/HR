using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels.Factory.Production;
using DenimERP.ViewModels.Rnd;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_PR_INSPECTION_PROCESS_MASTER_Repository : BaseRepository<F_PR_INSPECTION_PROCESS_MASTER>, IF_PR_INSPECTION_PROCESS_MASTER
    {
        public SQLF_PR_INSPECTION_PROCESS_MASTER_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<IEnumerable<F_PR_INSPECTION_PROCESS_MASTER>> GetAllAsync()
        {
            try
            {
                var result = await DenimDbContext.F_PR_INSPECTION_PROCESS_MASTER
                    .Include(c => c.SET)
                    .ThenInclude(c => c.PROG_)
                    .ToListAsync();
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<F_PR_INSPECTION_PROCESS_DETAILS>> GetAllDAsync()
        {
            try
            {
                var result = await DenimDbContext.F_PR_INSPECTION_PROCESS_DETAILS
                    .Include(c => c.INSP.SET.PROG_)
                    .Select(c=>new F_PR_INSPECTION_PROCESS_DETAILS
                    {
                        ROLLNO = c.ROLLNO,
                        ROLL_ID = c.ROLL_ID,
                        ROLL_INSPDATE = c.ROLL_INSPDATE,
                        OPT1 = c.INSP.SET.PROG_.PROG_NO
                    })
                    .OrderByDescending(c=>c.ROLL_ID)
                    .ToListAsync();
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<FPrInspectionProcessViewModel> GetInitObjects(FPrInspectionProcessViewModel prInspectionProcessViewModel)
        {
            try
            {
                prInspectionProcessViewModel.FPrInspectionMachines = await DenimDbContext.F_PR_INSPECTION_MACHINE.ToListAsync();
                prInspectionProcessViewModel.FPrInspectionBatches = await DenimDbContext.F_PR_INSPECTION_BATCH.ToListAsync();
                prInspectionProcessViewModel.FPrInspectionDefectInfos = await DenimDbContext.F_PR_INSPECTION_DEFECTINFO
                    .Select(c => new F_PR_INSPECTION_DEFECTINFO
                    {
                        DEF_TYPEID = c.DEF_TYPEID,
                        NAME = $"{c.CODE} - {c.NAME}"
                    })
                    .OrderBy(c => c.CODE)
                    .ToListAsync();

                prInspectionProcessViewModel.FPrInspectionProcess = await DenimDbContext.F_PR_INSPECTION_PROCESS
                    .Select(c => new F_PR_INSPECTION_PROCESS
                    {
                        ID = c.ID,
                        NAME = $"{c.REMARKS} - {c.NAME}"
                    })
                    .OrderBy(c => c.REMARKS)
                    .ToListAsync();

                prInspectionProcessViewModel.FBasSections = await DenimDbContext.F_BAS_SECTION.ToListAsync();
                //prInspectionProcessViewModel.RollList = await _denimDbContext.F_PR_INSPECTION_PROCESS_DETAILS.Select(c=>new F_PR_INSPECTION_PROCESS_DETAILS
                //{
                //    ROLLNO = c.ROLLNO,
                //    ROLL_ID = c.ROLL_ID
                //})
                //    .OrderByDescending(c=>c.ROLL_ID)
                //    .ToListAsync();

                prInspectionProcessViewModel.StyleList = await DenimDbContext.RND_FABRICINFO.Select(c=>new RND_FABRICINFO
                {
                    FABCODE = c.FABCODE,
                    STYLE_NAME = c.STYLE_NAME
                }).OrderByDescending(c=>c.FABCODE).ToListAsync();

                prInspectionProcessViewModel.FPrFinTrollies = await DenimDbContext.F_PR_FINISHING_FNPROCESS
                    .Include(c => c.TROLLNONavigation)
                    .Include(c => c.FN_PROCESS.DOFF.WV_BEAM.WV_PROCESS.SET)
                    .Include(c => c.FIN_PRO_TYPE)
                    .Where(c => c.LENGTH_OUT > 0 && !DenimDbContext.F_PR_INSPECTION_PROCESS_DETAILS.Any(e => e.TROLLEYNO.Equals(c.FIN_PROCESSID) && e.TROLLY_STATUS))
                    .Select(c => new F_PR_FINISHING_FNPROCESS
                    {
                        FIN_PROCESSID = c.FIN_PROCESSID,
                        TROLLNONavigation = new F_PR_FIN_TROLLY
                        {
                            NAME = $"{c.TROLLNONavigation.NAME}({c.LENGTH_OUT} Mtr,{Math.Round((double)(c.LENGTH_OUT * 1.094), 2)} Yds)"
                        }
                    }).ToListAsync();

                if (prInspectionProcessViewModel.FPrInspectionProcessMaster != null)
                {
                    prInspectionProcessViewModel.FPrFinTrolliesEdit = await DenimDbContext.F_PR_FINISHING_FNPROCESS
                        .Include(c => c.TROLLNONavigation)
                        .Include(c => c.FN_PROCESS.DOFF.WV_BEAM.WV_PROCESS.SET)
                        .Include(c => c.FIN_PRO_TYPE)
                        .Where(c => c.LENGTH_OUT > 0 && c.FIN_PRO_TYPE.NAME.Equals("FINISH") && c.FN_PROCESS.DOFF.WV_BEAM.WV_PROCESS.SET.SETID.Equals(prInspectionProcessViewModel.FPrInspectionProcessMaster.SETID) && !DenimDbContext.F_PR_INSPECTION_PROCESS_DETAILS.Any(e => e.TROLLEYNO.Equals(c.FIN_PROCESSID) && e.TROLLY_STATUS))
                        .Select(c => new F_PR_FINISHING_FNPROCESS
                        {
                            FIN_PROCESSID = c.FIN_PROCESSID,
                            TROLLNONavigation = new F_PR_FIN_TROLLY
                            {
                                NAME = $"{c.TROLLNONavigation.NAME}({c.LENGTH_OUT} Mtr,{Math.Round((double)(c.LENGTH_OUT * 1.094), 2)} Yds)"
                            }
                        })
                        .ToListAsync();
                }


                if (prInspectionProcessViewModel.FPrInspectionProcessMaster == null)
                {
                    prInspectionProcessViewModel.PlProductionSetDistributions = await DenimDbContext.PL_PRODUCTION_SETDISTRIBUTION
                        //.Include(c => c.F_PR_WEAVING_PROCESS_MASTER_B)
                        //.ThenInclude(c => c.F_PR_WEAVING_PROCESS_BEAM_DETAILS_B)
                        //.ThenInclude(c => c.F_PR_WEAVING_PROCESS_DETAILS_B)
                        //.ThenInclude(c => c.F_PR_FINISHING_PROCESS_MASTER)
                        //.ThenInclude(c => c.F_PR_FINISHING_FNPROCESS)
                        .Include(c => c.PROG_)
                        .Where(c=>c.PROG_.BLK_PROG_.RndProductionOrder.SO.SO_NO.Contains("SO-22-")||c.PROG_.BLK_PROG_.RndProductionOrder.SO.SO_NO.Contains("LO-22-"))
                        //.Where(c => !_denimDbContext.F_PR_INSPECTION_PROCESS_MASTER.Any(e => e.SETID.Equals(c.SETID)) && c.F_PR_WEAVING_PROCESS_MASTER_B.Any(e => e.F_PR_WEAVING_PROCESS_BEAM_DETAILS_B.Any(f => f.F_PR_WEAVING_PROCESS_DETAILS_B.Any((h => h.F_PR_FINISHING_PROCESS_MASTER.Any(i => i.F_PR_FINISHING_FNPROCESS.Any(d => d.FIN_PRO_TYPEID.Equals(17))))))))
                        .Select(c => new PL_PRODUCTION_SETDISTRIBUTION
                        {
                            SETID = c.SETID,
                            PROG_ = new PL_BULK_PROG_SETUP_D
                            {
                                PROG_NO = c.PROG_.PROG_NO
                            }
                        }).ToListAsync();
                    //prInspectionProcessViewModel.PlProductionSetDistributions = await _denimDbContext
                    //    .PL_PRODUCTION_SETDISTRIBUTION
                    //    .Include(c => c.PROG_)
                    //    .Where(c => !_denimDbContext.F_PR_INSPECTION_PROCESS_MASTER.Any(e => e.SETID.Equals(c.SETID)))
                    //    .Select(c => new PL_PRODUCTION_SETDISTRIBUTION
                    //    {
                    //        SETID = c.SETID,
                    //        PROG_ = c.PROG_
                    //    }).ToListAsync();
                }
                else
                {
                    prInspectionProcessViewModel.PlProductionSetDistributions = await DenimDbContext
                        .PL_PRODUCTION_SETDISTRIBUTION
                        .Include(c => c.PROG_)
                        .Where(c => c.PROG_.BLK_PROG_.RndProductionOrder.SO.SO_NO.Contains("SO-22-")|| c.PROG_.BLK_PROG_.RndProductionOrder.SO.SO_NO.Contains("LO-22-") &&
                            DenimDbContext.F_PR_INSPECTION_PROCESS_MASTER.Any(e => e.SETID.Equals(c.SETID)))
                        .Select(c => new PL_PRODUCTION_SETDISTRIBUTION
                        {
                            SETID = c.SETID,
                            PROG_ = new PL_BULK_PROG_SETUP_D
                            {
                                PROG_NO = c.PROG_.PROG_NO
                            }
                        }).ToListAsync();


                    //prInspectionProcessViewModel.PlProductionSetDistributions = await _denimDbContext
                    //    .PL_PRODUCTION_SETDISTRIBUTION

                    //    .Include(c => c.PROG_)
                    //    .Select(c => new PL_PRODUCTION_SETDISTRIBUTION
                    //    {
                    //        SETID = c.SETID,
                    //        PROG_ = c.PROG_
                    //    }).ToListAsync();
                }


                var FHrEmployees = await DenimDbContext.F_HR_EMP_OFFICIALINFO
                    .Include(c => c.EMP)
                    .Where(c => c.SECID.Equals(167) && !c.OPN2.Equals("Y"))
                    .ToListAsync();

                prInspectionProcessViewModel.FHrEmployees = FHrEmployees.Select(c => new F_HRD_EMPLOYEE
                {
                    EMPID = c.EMP.EMPID,
                    FIRST_NAME = c.EMP.FIRST_NAME + " " + c.EMP.LAST_NAME + '-' + c.EMP.EMPNO
                }).ToList();

                return prInspectionProcessViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<int> InsertAndGetIdAsync(F_PR_INSPECTION_PROCESS_MASTER fPrInspectionProcessMaster)
        {
            try
            {
                await DenimDbContext.F_PR_INSPECTION_PROCESS_MASTER.AddAsync(fPrInspectionProcessMaster);
                await SaveChanges();
                return fPrInspectionProcessMaster.INSPID;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<string> GetRollNoBySetId(int? setId)
        {
            try
            {
                var set = await DenimDbContext.PL_PRODUCTION_SETDISTRIBUTION
                    .Include(c => c.PROG_)
                    .Where(c => c.SETID.Equals(setId))
                    .Select(c => new PL_PRODUCTION_SETDISTRIBUTION
                    {
                        PROG_ = new PL_BULK_PROG_SETUP_D
                        {
                            PROG_NO = c.PROG_.PROG_NO
                        }
                    })
                    .FirstOrDefaultAsync();
                var rollNo = "";
                var setNo = "";
                if (set != null && set.PROG_ != null)
                {
                    setNo = set.PROG_.PROG_NO;
                }
                if (setNo != "")
                {
                    setNo = setNo.Replace("/", "");
                    var inspDetails = await DenimDbContext.F_PR_INSPECTION_PROCESS_DETAILS
                        .Where(c => c.ROLLNO.Contains(setNo)).OrderByDescending(c => c.ROLLNO).FirstOrDefaultAsync();
                    if (inspDetails != null)
                    {
                        inspDetails.ROLLNO = inspDetails.ROLLNO.Substring(0, 8);
                        rollNo = (long.Parse(inspDetails.ROLLNO)).ToString();
                    }
                    else
                    {
                        rollNo = setNo + "001";
                    }
                }

                return rollNo;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<dynamic> GetRemarks()
        {
            try
            {
                var result = await DenimDbContext.F_PR_INSPECTION_REMARKS
                    .Select(c => new[] { c.REMARKS })
                    .ToListAsync();
                return result.ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        public async Task<dynamic> GetConstruction()
        {
            try
            {
                var result = await DenimDbContext.F_PR_INSPECTION_CONSTRUCTION
                    .Select(c => new[] { c.ACT_CONSTRUCTION })
                    .ToListAsync();
                return result.ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<RndProductionOrderDetailViewModel> GetSetDetails(int setId)
        {
            try
            {
                var result = DenimDbContext.PL_PRODUCTION_SETDISTRIBUTION
                    .Include(c => c.SUBGROUP)
                    .Include(c => c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.COLORCODENavigation)
                    .Include(c => c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.LOOM)

                    .Include(c => c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.RND_FABRIC_COUNTINFO)
                    .ThenInclude(c => c.LOT)

                    .Include(c => c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.RND_FABRIC_COUNTINFO)
                    .ThenInclude(c => c.SUPP)

                    .Include(c => c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.RND_FABRIC_COUNTINFO)
                    .ThenInclude(c => c.COUNT)

                    .Include(c => c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.RND_YARNCONSUMPTION)

                    .Include(c => c.PROG_.BLK_PROG_.RndProductionOrder.SO.PIMASTER.BUYER)
                    .Include(c => c.PROG_.BLK_PROG_.RndProductionOrder.SO.PIMASTER.BRAND)

                    .Select(c => new RndProductionOrderDetailViewModel
                    {
                        ComExPiDetails = c.PROG_.BLK_PROG_.RndProductionOrder.SO,
                        PlProductionSetDistribution = c,
                        RndFabricCountInfoViewModels = c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.RND_FABRIC_COUNTINFO
                            .Select(e => new RndFabricCountInfoViewModel
                            {
                                RndFabricCountinfo = e,
                                AMOUNT = c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.RND_YARNCONSUMPTION.FirstOrDefault(d => d.FABCODE.Equals(e.FABCODE) && d.COUNTID.Equals(e.COUNTID)).AMOUNT
                            }).ToList()
                    })
                    .FirstOrDefault(c => c.PlProductionSetDistribution.SETID.Equals(setId));


                var setNo = result.PlProductionSetDistribution.PROG_.PROG_NO.Replace("/", "");
                var inspDetails = await DenimDbContext.F_PR_INSPECTION_PROCESS_DETAILS.Where(c => c.ROLLNO.Contains(setNo)).OrderByDescending(c => c.ROLLNO).FirstOrDefaultAsync();
                if (inspDetails != null)
                {
                    inspDetails.ROLLNO = inspDetails.ROLLNO.Substring(0, 8);
                    result.PlProductionSetDistribution.OPT3 = (long.Parse(inspDetails.ROLLNO)).ToString();
                }
                else
                {
                    result.PlProductionSetDistribution.OPT3 = setNo; //+ "001";
                }


                if (result.ComExPiDetails.SO_NO == "LO-22-0000")
                {
                    result.ComExPiDetails.STYLE.FABCODENavigation = await DenimDbContext.RND_FABRICINFO
                        .Include(c=>c.RND_FABRIC_COUNTINFO)
                        .ThenInclude(c=>c.COUNT)
                        .Include(c=>c.RND_YARNCONSUMPTION)
                        .FirstOrDefaultAsync(c => c.FABCODE.Equals(result.PlProductionSetDistribution.PROG_.BLK_PROG_.FABCODE));
                }



                result.PlProductionSetDistribution.OPT1 = string.Join(" + ",
                    result.ComExPiDetails.STYLE.FABCODENavigation.RND_FABRIC_COUNTINFO.Where(c => c.YARNFOR.Equals(1))
                        .Select(p => p.COUNT.RND_COUNTNAME));
                result.PlProductionSetDistribution.OPT2 = string.Join(" + ",
                    result.ComExPiDetails.STYLE.FABCODENavigation.RND_FABRIC_COUNTINFO.Where(c => c.YARNFOR.Equals(2))
                        .Select(p => p.COUNT.RND_COUNTNAME));
                result.PlProductionSetDistribution.OPT1 =
                    $"{result.PlProductionSetDistribution.OPT1} X {result.PlProductionSetDistribution.OPT2} / {result.ComExPiDetails.STYLE.FABCODENavigation.FNEPI}X{result.ComExPiDetails.STYLE.FABCODENavigation.FNPPI}";

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        public async Task<bool> GetRollConfirm(string roll)
        {
            try
            {
                var result = await DenimDbContext.F_PR_INSPECTION_PROCESS_DETAILS.AnyAsync(c => c.ROLLNO.Equals(roll));
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<F_PR_INSPECTION_PROCESS_MASTER> FindByIdAllAsync(int insId)
        {
            try
            {
                var result = await DenimDbContext
                    .F_PR_INSPECTION_PROCESS_MASTER
                    .Include(c=>c.F_PR_INSPECTION_PROCESS_DETAILS)
                    .Where(c=>c.F_PR_INSPECTION_PROCESS_DETAILS.Any(d=>d.ROLL_ID.Equals(insId)))
                    .FirstOrDefaultAsync();


                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

    }
}
