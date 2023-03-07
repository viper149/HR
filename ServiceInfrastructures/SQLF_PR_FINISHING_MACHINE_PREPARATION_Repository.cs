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
    public class SQLF_PR_FINISHING_MACHINE_PREPARATION_Repository : BaseRepository<F_PR_FINISHING_MACHINE_PREPARATION>, IF_PR_FINISHING_MACHINE_PREPARATION
    {
        public SQLF_PR_FINISHING_MACHINE_PREPARATION_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<IEnumerable<F_PR_FINISHING_MACHINE_PREPARATION>> GetAllAsync()
        {
            try
            {
                var result = await DenimDbContext.F_PR_FINISHING_MACHINE_PREPARATION
                    .Include(c => c.FABCODENavigation)
                    .Include(c => c.MACHINE_NONavigation)
                    .Select(c=>new F_PR_FINISHING_MACHINE_PREPARATION
                    {
                        FABCODENavigation =new RND_FABRICINFO
                        {
                            STYLE_NAME = c.FABCODENavigation.STYLE_NAME
                        },
                        MACHINE_NONavigation = new F_PR_FN_MACHINE_INFO
                        {
                            NAME = c.MACHINE_NONavigation.NAME
                        } ,
                        FINISH_ROUTE = c.FINISH_ROUTE,
                        REMARKS = c.REMARKS,
                        TRNSDATE = c.TRNSDATE,
                        FPMID = c.FPMID
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

        public async Task<FPrFinishingMachineCreatePreparationViewModel> GetInitObjects(
            FPrFinishingMachineCreatePreparationViewModel fPrFinishingMachineCreatePreparationViewModel)
        {
            try
            {
                fPrFinishingMachineCreatePreparationViewModel.RndFabricInfos = await DenimDbContext.RND_FABRICINFO
                    .Select(c => new RND_FABRICINFO
                    {
                        FABCODE = c.FABCODE,
                        STYLE_NAME = c.STYLE_NAME
                    })
                    .ToListAsync();

                fPrFinishingMachineCreatePreparationViewModel.FPrFnMachineInfos = await DenimDbContext.F_PR_FN_MACHINE_INFO.ToListAsync();
                fPrFinishingMachineCreatePreparationViewModel.FPrFnProcessTypeInfos = await DenimDbContext.F_PR_FN_PROCESS_TYPEINFO.ToListAsync();

                fPrFinishingMachineCreatePreparationViewModel.FPrWeavingProcessDetailsBs = await DenimDbContext
                    .F_PR_WEAVING_PROCESS_DETAILS_B
                    .Include(c => c.WV_BEAM.F_PR_FINISHING_BEAM_RECEIVE)
                    .ThenInclude(c => c.SET.PROG_)
                    .Include(c => c.LOOM_NONavigation)
                    .Select(c => new TypeTableViewModel
                    {
                        ID = c.TRNSID,
                        Name = c.WV_BEAM.F_PR_FINISHING_BEAM_RECEIVE.Select(e => e.SET.PROG_.PROG_NO).FirstOrDefault() + "-" + c.LOOM_NONavigation.LOOM_NO + "(Length-" + c.LENGTH_BULK + ")"
                    })
                    .ToListAsync();

                fPrFinishingMachineCreatePreparationViewModel.FChemStoreProductInfos = await DenimDbContext.F_CHEM_STORE_PRODUCTINFO.ToListAsync();
                fPrFinishingMachineCreatePreparationViewModel.FPrFnChemicalConsumption = new F_PR_FN_CHEMICAL_CONSUMPTION
                {
                    TRNSDATE = DateTime.Now
                };
                return fPrFinishingMachineCreatePreparationViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        public async Task<FPrFinishingMachineCreatePreparationViewModel> GetEditData(int machineId)
        {
            try
            {
                var result =
                    await DenimDbContext.F_PR_FINISHING_MACHINE_PREPARATION
                        .Include(c => c.F_PR_FINIGHING_DOFF_FOR_MACHINE)
                        .Include(c => c.F_PR_FN_CHEMICAL_CONSUMPTION)
                        .FirstOrDefaultAsync(c => c.FPMID.Equals(machineId));

                var fPrFinishingMachineCreatePreparationViewModel = new FPrFinishingMachineCreatePreparationViewModel
                {
                    FPrFinishingMachinePreparation = result,
                    FPrFinighingDoffForMachines = result.F_PR_FINIGHING_DOFF_FOR_MACHINE.ToList(),
                    FPrFnChemicalConsumptions = result.F_PR_FN_CHEMICAL_CONSUMPTION.ToList()
                };

                foreach (var item in fPrFinishingMachineCreatePreparationViewModel.FPrFinighingDoffForMachines)
                {
                        item.DOFF = await DenimDbContext.F_PR_FINISHING_PROCESS_MASTER
                            .Include(c => c.F_PR_FINISHING_FNPROCESS)
                            .Include(c => c.DOFF.LOOM_NONavigation)
                            .Include(c => c.DOFF.LOOM_TYPENavigation)
                            .Include(c => c.DOFF.OTHER_DOFF)
                            .Include(c => c.DOFF.WV_BEAM.F_PR_FINISHING_BEAM_RECEIVE)
                            .ThenInclude(c => c.SET.PROG_)
                            .Include(c => c.DOFF.WV_BEAM.F_PR_SIZING_PROCESS_ROPE_DETAILS.W_BEAM)
                            .Include(c => c.DOFF.WV_BEAM.F_PR_SLASHER_DYEING_DETAILS.W_BEAM)
                            .Where(c => c.FN_PROCESSID.Equals(item.FN_PROCESSID))
                            .Select(c => new F_PR_FINISHING_PROCESS_MASTER
                            {
                                OPT1 = c.DOFF.WV_BEAM.F_PR_SIZING_PROCESS_ROPE_DETAILS == null ? $"{c.DOFF.WV_BEAM.WV_PROCESS.SET.PROG_.PROG_NO}- {c.DOFF.WV_BEAM.F_PR_SLASHER_DYEING_DETAILS.W_BEAM.BEAM_NO}-{c.DOFF.LOOM_NONavigation.LOOM_NO}(Length-{c.DOFF.LENGTH_BULK})" : $"{c.DOFF.WV_BEAM.WV_PROCESS.SET.PROG_.PROG_NO}-{c.DOFF.WV_BEAM.F_PR_SIZING_PROCESS_ROPE_DETAILS.W_BEAM.BEAM_NO}-{c.DOFF.LOOM_NONavigation.LOOM_NO}(Length-{c.DOFF.LENGTH_BULK})",
                                OPT2 = c.F_PR_FINISHING_FNPROCESS.Where(e => e.FIN_PRO_TYPEID.Equals(fPrFinishingMachineCreatePreparationViewModel.FPrFinishingMachinePreparation.FIN_PRO_TYPEID) && e.FN_PROCESSID.Equals(c.FN_PROCESSID)).Select(d => d.LENGTH_OUT).FirstOrDefault().ToString()
                            }).FirstOrDefaultAsync();
                }

                return fPrFinishingMachineCreatePreparationViewModel;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        public async Task<IEnumerable<dynamic>> GetStyleDetailsAsync(int fabcode)
        {
            try
            {
                //List<dynamic> doffList = null;
                var doffList = await DenimDbContext.F_PR_FINISHING_PROCESS_MASTER
                    .Include(c => c.DOFF.LOOM_NONavigation)
                    .Include(c => c.DOFF.LOOM_TYPENavigation)
                    .Include(c => c.DOFF.OTHER_DOFF)
                    .Include(c => c.DOFF.WV_BEAM.WV_PROCESS.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.COLORCODENavigation)
                    .Include(c => c.DOFF.WV_BEAM.F_PR_SIZING_PROCESS_ROPE_DETAILS.W_BEAM)
                    .Include(c => c.DOFF.WV_BEAM.F_PR_SLASHER_DYEING_DETAILS.W_BEAM)
                    .Where(c => c.FABCODE.Equals(fabcode))
                    .Select(c=>new
                    {
                        Color = c.DOFF.WV_BEAM.WV_PROCESS.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.COLORCODENavigation.COLOR,
                        TotalEnds = c.DOFF.WV_BEAM.WV_PROCESS.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.TOTALENDS,
                        FinishRoute = c.DOFF.WV_BEAM.WV_PROCESS.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.FINISH_ROUTE,
                        LoomType = c.DOFF.WV_BEAM.WV_PROCESS.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.LOOM.LOOM_TYPE_NAME,
                        DoffList = new
                            {
                                FN_PROCESSID = c.FN_PROCESSID,
                                Doff = c.DOFF.WV_BEAM.F_PR_SIZING_PROCESS_ROPE_DETAILS == null ? $"{c.DOFF.WV_BEAM.WV_PROCESS.SET.PROG_.PROG_NO}- {c.DOFF.WV_BEAM.F_PR_SLASHER_DYEING_DETAILS.W_BEAM.BEAM_NO}-{c.DOFF.LOOM_NONavigation.LOOM_NO}(Length-{c.DOFF.LENGTH_BULK})" : $"{c.DOFF.WV_BEAM.WV_PROCESS.SET.PROG_.PROG_NO}-{c.DOFF.WV_BEAM.F_PR_SIZING_PROCESS_ROPE_DETAILS.W_BEAM.BEAM_NO}-{c.DOFF.LOOM_NONavigation.LOOM_NO}(Length-{c.DOFF.LENGTH_BULK})",
                            }

                    //    $("#FPrFinighingDoffForMachine_FN_PROCESSID").html('');
                    //    $("#FPrFinighingDoffForMachine_FN_PROCESSID").append('<option value="" selected>Select Doff</option>');
                    //    $.each(data,
                    //    function(id, option) {
                    //    $("#FPrFinighingDoffForMachine_FN_PROCESSID").append($('<option></option>').val(option.fN_PROCESSID).html(option.doff.wV_BEAM.wV_PROCESS.set.proG_.proG_NO + '-' + option.doff.looM_NONavigation.looM_NO + '(Length-' + option.lengtH_BEAM + ')'));
                    //});
            })
                    .ToListAsync();

                return doffList;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<dynamic> GetDoffDetails(FPrFinishingMachineCreatePreparationViewModel fPrFinishingMachineCreatePreparationViewModel)
        {
            try
            {
                var result = await DenimDbContext.F_PR_FINISHING_FNPROCESS
                    .Include(c=>c.TROLLNONavigation)
                    .Where(c => c.FIN_PRO_TYPEID.Equals(fPrFinishingMachineCreatePreparationViewModel.FPrFinishingMachinePreparation.FIN_PRO_TYPEID) && c.FN_PROCESSID.Equals(fPrFinishingMachineCreatePreparationViewModel.FPrFinighingDoffForMachine.FN_PROCESSID) && c.FIN_PROCESSDATE.Equals(fPrFinishingMachineCreatePreparationViewModel.FPrFinishingMachinePreparation.TRNSDATE) && !fPrFinishingMachineCreatePreparationViewModel.FPrFinighingDoffForMachines.Any(d=> d.OPT3.Equals(c.FIN_PROCESSID.ToString())))
                    .Select(c=>new
                    {
                        LengthOut = c.LENGTH_OUT,
                        LengthIn = c.LENGTH_IN,
                        TROLLNO = c.TROLLNONavigation.NAME,
                        FIN_PROCESSID = c.FIN_PROCESSID
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
