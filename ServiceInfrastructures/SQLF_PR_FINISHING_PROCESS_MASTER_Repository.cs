using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels;
using DenimERP.ViewModels.Factory.Production;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_PR_FINISHING_PROCESS_MASTER_Repository : BaseRepository<F_PR_FINISHING_PROCESS_MASTER>, IF_PR_FINISHING_PROCESS_MASTER
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDataProtector _protector;

        public SQLF_PR_FINISHING_PROCESS_MASTER_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            IHttpContextAccessor httpContextAccessor
        )
            : base(denimDbContext)
        {
            _httpContextAccessor = httpContextAccessor;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<IEnumerable<dynamic>> GetAllAsync()
        {
            try
            {
                var result = await DenimDbContext.F_PR_FINISHING_PROCESS_MASTER
                    .Include(c => c.FABRICINFO)
                    .Include(c => c.DOFF.WV_BEAM.WV_PROCESS.SET.PROG_)
                    .Include(c => c.DOFF.WV_BEAM.F_PR_SIZING_PROCESS_ROPE_DETAILS.W_BEAM)
                    .Include(c => c.DOFF.WV_BEAM.F_PR_SLASHER_DYEING_DETAILS.W_BEAM)
                    .Include(c => c.DOFF.LOOM_NONavigation)
                    .Select(c => new
                    {
                        STYLE_NAME = c.FABRICINFO.STYLE_NAME,
                        FN_PROCESSDATE = c.FN_PROCESSDATE,
                        BEAM_NO = c.DOFF.WV_BEAM.F_PR_SIZING_PROCESS_ROPE_DETAILS == null ? $"{c.DOFF.WV_BEAM.WV_PROCESS.SET.PROG_.PROG_NO}- {c.DOFF.WV_BEAM.F_PR_SLASHER_DYEING_DETAILS.W_BEAM.BEAM_NO}-{c.DOFF.LOOM_NONavigation.LOOM_NO}(Length-{c.DOFF.LENGTH_BULK})" : $"{c.DOFF.WV_BEAM.WV_PROCESS.SET.PROG_.PROG_NO}-{c.DOFF.WV_BEAM.F_PR_SIZING_PROCESS_ROPE_DETAILS.W_BEAM.BEAM_NO}-{c.DOFF.LOOM_NONavigation.LOOM_NO}(Length-{c.DOFF.LENGTH_BULK})",

                        LENGTH_BEAM = c.LENGTH_BEAM,
                        LENGTH_ACT = c.LENGTH_ACT,
                        REMARKS = c.REMARKS,
                        FN_PROCESSID = c.FN_PROCESSID,
                        EncryptedId = _protector.Protect(c.FN_PROCESSID.ToString())
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

        public async Task<PrFinishingProcessViewModel> GetInitObjects(PrFinishingProcessViewModel prFinishingProcessViewModel)
        {
            try
            {
                //prFinishingProcessViewModel.PlProductionSetDistributions = await _denimDbContext
                //    .PL_PRODUCTION_SETDISTRIBUTION
                //    .Include(c => c.PROG_)
                //    .Where(c => _denimDbContext.F_PR_WEAVING_PROCESS_MASTER_B.Any(e => e.SETID.Equals(c.SETID)))
                //    .Select(c => new PL_PRODUCTION_SETDISTRIBUTION
                //    {
                //        SETID = c.SETID,
                //        PROG_ = c.PROG_
                //    }).ToListAsync();


                var FHrEmployees = await DenimDbContext.F_HR_EMP_OFFICIALINFO
                    .Include(c => c.EMP)
                    .Where(c => c.SECID.Equals(163) && !c.OPN2.Equals("Y"))
                    .ToListAsync();

                prFinishingProcessViewModel.FHrEmployees = FHrEmployees.Select(c => new F_HRD_EMPLOYEE
                {
                    EMPID = c.EMP.EMPID,
                    FIRST_NAME = c.EMP.FIRST_NAME + " " + c.EMP.LAST_NAME + '-' + c.EMP.EMPNO
                }).ToList();


                prFinishingProcessViewModel.FabricInfos = await DenimDbContext.RND_FABRICINFO
                    .Select(c => new RND_FABRICINFO
                    {
                        FABCODE = c.FABCODE,
                        STYLE_NAME = c.STYLE_NAME
                    }).ToListAsync();

                prFinishingProcessViewModel.PlProductionSetDistributions = await DenimDbContext.PL_PRODUCTION_SETDISTRIBUTION
                    .Include(c => c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation)
                    .Select(c => new PL_PRODUCTION_SETDISTRIBUTION()
                    {
                        SETID = c.SETID,
                        OPT1 = $"{c.PROG_.BLK_PROG_.RndProductionOrder.SO.SO_NO} - {c.PROG_.PROG_NO} - {c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.STYLE_NAME} "
                    }).ToListAsync();

                prFinishingProcessViewModel.FPrWeavingProcessDetailsBs = await DenimDbContext.F_PR_WEAVING_PROCESS_DETAILS_B
                    .Include(c => c.WV_BEAM.F_PR_SIZING_PROCESS_ROPE_DETAILS.W_BEAM)
                    .Include(c => c.WV_BEAM.F_PR_SLASHER_DYEING_DETAILS.W_BEAM)
                    .Include(c => c.LOOM_NONavigation)
                    .Include(c => c.WV_BEAM.WV_PROCESS.SET.PROG_)
                    .Where(c => c.IS_DELIVERABLE)
                    .Select(c => new TypeTableViewModel
                    {
                        ID = c.TRNSID,
                        Name = c.WV_BEAM.WV_PROCESS.SET.PROG_.PROG_NO + "-" + c.LOOM_NONavigation.LOOM_NO + "-" + c.LENGTH_BULK
                    })
                    .ToListAsync();
                //+c.WV_BEAM.F_PR_SIZING_PROCESS_ROPE_DETAILS == null ? c.WV_BEAM.F_PR_SLASHER_DYEING_DETAILS.W_BEAM.BEAM_NO : c.WV_BEAM.F_PR_SIZING_PROCESS_ROPE_DETAILS.W_BEAM.BEAM_NO
                prFinishingProcessViewModel.FPrFabProcessMachineInfos = await DenimDbContext.F_PR_PROCESS_MACHINEINFO.ToListAsync();
                prFinishingProcessViewModel.FPrFabProcessTypeInfos = await DenimDbContext.F_PR_PROCESS_TYPE_INFO.ToListAsync();
                prFinishingProcessViewModel.FPrFnMachineInfos = await DenimDbContext.F_PR_FN_MACHINE_INFO.ToListAsync();
                prFinishingProcessViewModel.FPrFnProcessTypeInfos = await DenimDbContext.F_PR_FN_PROCESS_TYPEINFO.ToListAsync();
                prFinishingProcessViewModel.FPrWeavingProcessBeamDetailsBs = await DenimDbContext.F_PR_WEAVING_PROCESS_BEAM_DETAILS_B
                    .Include(c => c.F_PR_SIZING_PROCESS_ROPE_DETAILS)
                    .ThenInclude(c => c.W_BEAM)
                    .ToListAsync();
                prFinishingProcessViewModel.FPrFinTrollies = await DenimDbContext.F_PR_FIN_TROLLY.OrderBy(c => c.NAME).ToListAsync();
                prFinishingProcessViewModel.FBasSections = await DenimDbContext.F_BAS_SECTION.ToListAsync();
                prFinishingProcessViewModel.FChemStoreProductInfos = await DenimDbContext.F_CHEM_STORE_PRODUCTINFO.ToListAsync();

                //prFinishingProcessViewModel.FPrFinishingMachinePreparations = await _denimDbContext.F_PR_FINISHING_MACHINE_PREPARATION
                //    .Include(c => c.FABCODENavigation.WV)
                //    .Select(c => new F_PR_FINISHING_MACHINE_PREPARATION
                //    {
                //        FPMID = c.FPMID,
                //        FABCODENavigation = new RND_FABRICINFO
                //        {
                //            WV = new RND_SAMPLE_INFO_WEAVING
                //            {
                //                FABCODE = c.FABCODENavigation.WV.FABCODE
                //            },
                //            FABCODE = (int)c.FABCODE,
                //            STYLE_NAME = c.FABCODENavigation.STYLE_NAME
                //        }
                //    })
                //    .ToListAsync();


                return prFinishingProcessViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<F_PR_WEAVING_PROCESS_BEAM_DETAILS_B> GetBeamDetails(int beamId)
        {
            try
            {
                var result = await DenimDbContext.F_PR_WEAVING_PROCESS_BEAM_DETAILS_B
                    .Include(c => c.F_PR_SIZING_PROCESS_ROPE_DETAILS)
                    .ThenInclude(c => c.W_BEAM)
                    .Include(c => c.F_PR_WEAVING_PROCESS_DETAILS_B)
                    .ThenInclude(c => c.LOOM_NONavigation)
                    .FirstOrDefaultAsync(c => c.WV_BEAMID.Equals(beamId));
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<F_PR_WEAVING_PROCESS_DETAILS_B> GetLoomDetails(int loomId)
        {
            try
            {
                var result = await DenimDbContext.F_PR_WEAVING_PROCESS_DETAILS_B
                    .Include(c => c.LOOM_NONavigation)
                    .Include(c => c.LOOM_TYPENavigation)
                    .FirstOrDefaultAsync(c => c.TRNSID.Equals(loomId));
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<int> InsertAndGetIdAsync(F_PR_FINISHING_PROCESS_MASTER fPrFinishingProcessMaster)
        {
            try
            {
                await DenimDbContext.F_PR_FINISHING_PROCESS_MASTER.AddAsync(fPrFinishingProcessMaster);
                await SaveChanges();
                return fPrFinishingProcessMaster.FN_PROCESSID;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw;
            }
        }


        public async Task<IEnumerable<F_PR_WEAVING_PROCESS_DETAILS_B>> GetStyleDetails(int fabcode, int setId)
        {
            try
            {
                var result = await DenimDbContext.F_PR_WEAVING_PROCESS_DETAILS_B
                    .Include(c => c.LOOM_NONavigation)
                    .Include(c => c.WV_BEAM.F_PR_SIZING_PROCESS_ROPE_DETAILS.W_BEAM)
                    .Include(c => c.WV_BEAM.F_PR_SLASHER_DYEING_DETAILS.W_BEAM)
                    .Include(c => c.WV_BEAM.WV_PROCESS.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.COLORCODENavigation)
                    .Include(c => c.WV_BEAM.WV_PROCESS.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.LOOM)
                    //.Where(c => c.WV_BEAM.WV_PROCESS.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.FABCODE.Equals(fabcode) && c.IS_DELIVERABLE && !_denimDbContext.F_PR_FINISHING_PROCESS_MASTER.Any(e=>e.DOFF_ID.Equals(c.TRNSID)))
                    .Where(c => c.WV_BEAM.WV_PROCESS.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.FABCODE.Equals(fabcode) && c.WV_BEAM.WV_PROCESS.SET.SETID.Equals(setId) && c.IS_DELIVERABLE && (c.LENGTH_BULK - DenimDbContext.F_PR_FINISHING_PROCESS_MASTER.Where(m => m.DOFF_ID.Equals(c.TRNSID)).Sum(e => e.LENGTH_BEAM) > 0 || !DenimDbContext.F_PR_FINISHING_PROCESS_MASTER.Any(e => e.DOFF_ID.Equals(c.TRNSID))
                        ))
                    .Select(c => new F_PR_WEAVING_PROCESS_DETAILS_B
                    {
                        TRNSID = c.TRNSID,
                        OPT1 = c.WV_BEAM.F_PR_SIZING_PROCESS_ROPE_DETAILS == null ? c.WV_BEAM.F_PR_SLASHER_DYEING_DETAILS.W_BEAM.BEAM_NO + " - " + c.LOOM_NONavigation.LOOM_NO.Substring(9) + " (Length-" + c.LENGTH_BULK + ")" : c.WV_BEAM.F_PR_SIZING_PROCESS_ROPE_DETAILS.W_BEAM.BEAM_NO + " - " + c.LOOM_NONavigation.LOOM_NO.Substring(9) + " (Length-" + c.LENGTH_BULK + ")",
                        OPT2 = c.WV_BEAM.WV_PROCESS.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.COLORCODENavigation.COLOR,
                        OPT3 = c.WV_BEAM.WV_PROCESS.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.LOOM.LOOM_TYPE_NAME,
                        OPT4 = c.WV_BEAM.WV_PROCESS.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.TOTALENDS.ToString(),
                        OPT5 = c.WV_BEAM.WV_PROCESS.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.FINISH_ROUTE
                    })
                    .ToListAsync();


                foreach (var item in result)
                {
                    var finishProcessList = await DenimDbContext.F_PR_FINISHING_PROCESS_MASTER.Where(c => c.DOFF_ID.Equals(item.TRNSID))
                        .ToListAsync();

                    if (finishProcessList.Any())
                    {
                        var doffSum = finishProcessList.Sum(c => c.LENGTH_BEAM);

                        //var opt1 = item.OPT1.Split("(Length-")[0];
                        var length = double.Parse(item.OPT1.Split("(Length-")[1].Split(")")[0]);

                        length = length - doffSum ?? 0;

                        item.OPT1 = item.OPT1 + "(Remaining-" + length + ")";
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




        public async Task<IEnumerable<F_PR_WEAVING_PROCESS_DETAILS_B>> GetStyleDetailsEdit(int fabcode)
        {
            try
            {
                var result = await DenimDbContext.F_PR_WEAVING_PROCESS_DETAILS_B
                    .Include(c => c.LOOM_NONavigation)
                    .Include(c => c.WV_BEAM.F_PR_SIZING_PROCESS_ROPE_DETAILS.W_BEAM)
                    .Include(c => c.WV_BEAM.F_PR_SLASHER_DYEING_DETAILS.W_BEAM)
                    .Include(c => c.WV_BEAM.WV_PROCESS.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.COLORCODENavigation)
                    .Include(c => c.WV_BEAM.WV_PROCESS.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.LOOM)
                    //.Where(c => c.WV_BEAM.WV_PROCESS.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.FABCODE.Equals(fabcode) && c.IS_DELIVERABLE && !_denimDbContext.F_PR_FINISHING_PROCESS_MASTER.Any(e=>e.DOFF_ID.Equals(c.TRNSID)))
                    .Where(c => c.WV_BEAM.WV_PROCESS.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.FABCODE.Equals(fabcode) && c.IS_DELIVERABLE && (c.LENGTH_BULK - DenimDbContext.F_PR_FINISHING_PROCESS_MASTER.Where(m => m.DOFF_ID.Equals(c.TRNSID)).Sum(e => e.LENGTH_BEAM) > 0 || DenimDbContext.F_PR_FINISHING_PROCESS_MASTER.Any(e => e.DOFF_ID.Equals(c.TRNSID))
                        ))
                    .Select(c => new F_PR_WEAVING_PROCESS_DETAILS_B
                    {
                        TRNSID = c.TRNSID,
                        OPT1 = c.WV_BEAM.F_PR_SIZING_PROCESS_ROPE_DETAILS == null ? c.WV_BEAM.WV_PROCESS.SET.PROG_.PROG_NO + "-" + c.WV_BEAM.F_PR_SLASHER_DYEING_DETAILS.W_BEAM.BEAM_NO + " " + c.LOOM_NONavigation.LOOM_NO + " (Length-" + c.LENGTH_BULK + ")" : c.WV_BEAM.WV_PROCESS.SET.PROG_.PROG_NO + "-" + c.WV_BEAM.F_PR_SIZING_PROCESS_ROPE_DETAILS.W_BEAM.BEAM_NO + " " + c.LOOM_NONavigation.LOOM_NO + " (Length-" + c.LENGTH_BULK + ")",
                        OPT2 = c.WV_BEAM.WV_PROCESS.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.COLORCODENavigation.COLOR,
                        OPT3 = c.WV_BEAM.WV_PROCESS.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.LOOM.LOOM_TYPE_NAME,
                        OPT4 = c.WV_BEAM.WV_PROCESS.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.TOTALENDS.ToString(),
                        OPT5 = c.WV_BEAM.WV_PROCESS.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.FINISH_ROUTE,
                        OPT6 = c.WV_BEAM.WV_PROCESS.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.STYLE_NAME
                    })
                    .ToListAsync();


                foreach (var item in result)
                {
                    var finishProcessList = await DenimDbContext.F_PR_FINISHING_PROCESS_MASTER.Where(c => c.DOFF_ID.Equals(item.TRNSID))
                        .ToListAsync();

                    if (finishProcessList.Any())
                    {
                        var doffSum = finishProcessList.Sum(c => c.LENGTH_BEAM);

                        //var opt1 = item.OPT1.Split("(Length-")[0];
                        var length = double.Parse(item.OPT1.Split("(Length-")[1].Split(")")[0]);

                        length = length - doffSum ?? 0;

                        item.OPT1 = item.OPT1 + "(Remaining-" + length + ")";
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

        public async Task<PrFinishingProcessViewModel> GetFinishingDetails(int finId)
        {
            try
            {
                var result = await DenimDbContext.F_PR_FINISHING_PROCESS_MASTER
                    .Include(c => c.DOFF)
                    .Include(c => c.FABRICINFO)
                    .Include(c => c.F_PR_FINISHING_FNPROCESS)
                    .FirstOrDefaultAsync(c => c.FN_PROCESSID.Equals(finId));

                var finishingProcessViewModel = new PrFinishingProcessViewModel
                {
                    FPrFinishingProcessMaster = result,
                    FPrFinishingFnProcessList = result.F_PR_FINISHING_FNPROCESS.ToList()
                };

                return finishingProcessViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<dynamic> GetStyleDetailsBySetId(int setId)
        {
            try
            {
                var result = await DenimDbContext.PL_PRODUCTION_SETDISTRIBUTION
                    .Include(c => c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation)
                    .Where(c => c.SETID.Equals(setId))
                    .Select(c => new
                    {
                        Fabcode = c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.FABCODE,
                        Style_Name = c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.STYLE_NAME,
                    })
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
