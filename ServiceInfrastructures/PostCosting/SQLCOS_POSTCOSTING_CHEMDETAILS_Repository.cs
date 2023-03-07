using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces.PostCosting;
using DenimERP.ViewModels.PostCostingMaster;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures.PostCosting
{
    public class SQLCOS_POSTCOSTING_CHEMDETAILS_Repository:BaseRepository<COS_POSTCOSTING_CHEMDETAILS>, ICOS_POSTCOSTING_CHEMDETAILS
    {
        public SQLCOS_POSTCOSTING_CHEMDETAILS_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<PostCostingViewModel> GetChemicalDetailsBySo(PostCostingViewModel postCostingViewModel)
        {
            try
            {
                var dyeingRope = await DenimDbContext.F_DYEING_PROCESS_ROPE_CHEM
                    .Include(c => c.CHEM_PROD_)
                    .Include(c => c.ROPE_D.F_DYEING_PROCESS_ROPE_DETAILS)
                    .Include(c => c.ROPE_D.GROUP.PL_PRODUCTION_PLAN_DETAILS)
                    .ThenInclude(c => c.PL_PRODUCTION_SETDISTRIBUTION)
                    .ThenInclude(c => c.PROG_.BLK_PROG_.RndProductionOrder.SO)
                    .Include(c => c.ROPE_D.GROUP.PL_PRODUCTION_PLAN_DETAILS)
                    .ThenInclude(c => c.F_PR_WARPING_PROCESS_ROPE_MASTER)
                    .ThenInclude(c=>c.SUBGROUP.PL_PRODUCTION_SETDISTRIBUTION)
                    .ThenInclude(c=>c.PROG_.BLK_PROG_.RndProductionOrder)
                    .Where(c => c.ROPE_D.GROUP.PL_PRODUCTION_PLAN_DETAILS.Any(e =>
                        e.PL_PRODUCTION_SETDISTRIBUTION.Any(f =>
                            f.PROG_.BLK_PROG_.RndProductionOrder.POID.Equals(postCostingViewModel.CosPostcostingMaster.SO_NO))))
                    .Select(c=>new
                    {
                        c.CHEM_PROD_ID,
                        c.CHEM_PROD_.PRODUCTNAME,
                        c.UNIT,
                        SoQty = c.ROPE_D.GROUP.PL_PRODUCTION_PLAN_DETAILS.Sum(e=>e.PL_PRODUCTION_SETDISTRIBUTION.Sum(f=>f.PROG_.SET_QTY)),
                        groupChemQty = c.QTY,
                        chemQty = (c.QTY* c.ROPE_D.GROUP.PL_PRODUCTION_PLAN_DETAILS.Sum(e => e.F_PR_WARPING_PROCESS_ROPE_MASTER.Where(q=>q.SUBGROUP.PL_PRODUCTION_SETDISTRIBUTION.Any(f=>f.PROG_.BLK_PROG_.RndProductionOrder.POID.Equals(postCostingViewModel.CosPostcostingMaster.SO_NO))).Sum(f => f.WARP_LENGTH))) /c.ROPE_D.GROUP_LENGTH,
                        Type = "Dyeing"
                    })
                    .OrderBy(c=>c.PRODUCTNAME)
                    .ToListAsync();

                var dyeingSlasher = await DenimDbContext.F_PR_SLASHER_CHEM_CONSM
                    .Include(c => c.CHEM_PROD)
                    .Include(c => c.SL.F_PR_SLASHER_DYEING_DETAILS)
                    .ThenInclude(c => c.SL.SET.PROG_.BLK_PROG_.RndProductionOrder)
                    .Where(c => c.SL.SET.PROG_.BLK_PROG_.RndProductionOrder.POID.Equals(postCostingViewModel.CosPostcostingMaster.SO_NO))
                    .Select(c=>new
                    {
                        c.CHEM_PRODID,
                        c.CHEM_PROD.PRODUCTNAME,
                        c.UNIT,
                        SoQty = c.SL.SET.PROG_.SET_QTY,
                        groupChemQty = c.QTY,
                        chemQty = c.QTY,
                        Type = "Dyeing"
                    })
                    .OrderBy(c=>c.PRODUCTNAME)
                    .ToListAsync();


                var sizingChem = await DenimDbContext.F_PR_SIZING_PROCESS_ROPE_CHEM
                    .Include(c => c.CHEM_PROD)
                    .Include(c => c.S.SET.PROG_.BLK_PROG_.RndProductionOrder)
                    .Where(c => c.S.SET.PROG_.BLK_PROG_.RndProductionOrder.POID.Equals(postCostingViewModel.CosPostcostingMaster.SO_NO))
                    .Select(c=>new
                    {
                        c.CHEM_PRODID,
                        c.CHEM_PROD.PRODUCTNAME,
                        c.UNIT,
                        SoQty = c.S.SET.PROG_.SET_QTY,
                        groupChemQty = c.QTY,
                        chemQty = c.QTY,
                        Type = "Sizing"
                    })
                    .OrderBy(c=>c.PRODUCTNAME)
                    .ToListAsync();


                var finishingChem = await DenimDbContext.F_PR_FN_CHEMICAL_CONSUMPTION
                    .Include(c => c.CHEM_PROD_)
                    .Include(c => c.FPM.F_PR_FINIGHING_DOFF_FOR_MACHINE)
                    .ThenInclude(c=>c.DOFF.SET.PROG_.BLK_PROG_.RndProductionOrder)
                    .Where(c => c.FPM.F_PR_FINIGHING_DOFF_FOR_MACHINE.Any(e=>e.DOFF.SET.PROG_.BLK_PROG_.RndProductionOrder.POID.Equals(postCostingViewModel.CosPostcostingMaster.SO_NO)))
                    .Select(c=>new
                    {
                        c.CHEM_PROD_ID,
                        c.CHEM_PROD_.PRODUCTNAME,
                        UNIT = "KG",
                        SoQty = c.FPM.F_PR_FINIGHING_DOFF_FOR_MACHINE.Sum(e=>e.DOFF.SET.PROG_.SET_QTY),
                        groupChemQty = c.FPM.F_PR_FINIGHING_DOFF_FOR_MACHINE.Sum(e=>e.DOFF.LENGTH_ACT),
                        chemQty = c.QTY,
                        Type = "Finishing"
                    })
                    .OrderBy(c=>c.PRODUCTNAME)
                    .ToListAsync();


                var chemList = new List<COS_POSTCOSTING_CHEMDETAILS>();

                foreach (var item in dyeingRope)
                {
                    var unit = await DenimDbContext.F_BAS_UNITS.Where(c => c.UNAME.Equals(item.UNIT)).Select(c=>c.UID).FirstOrDefaultAsync();

                    chemList.Add(new COS_POSTCOSTING_CHEMDETAILS
                    {
                        CHEM_PRODID = item.CHEM_PROD_ID,
                        UNIT = unit,
                        CONSUMPTION = dyeingRope.Where(c => c.CHEM_PROD_ID.Equals(item.CHEM_PROD_ID)).Sum(c => c.chemQty),
                        SECTION = item.Type
                    });
                }

                foreach (var item in dyeingSlasher)
                {
                    var unit = await DenimDbContext.F_BAS_UNITS.Where(c => c.UNAME.Equals(item.UNIT)).Select(c=>c.UID).FirstOrDefaultAsync();

                    chemList.Add(new COS_POSTCOSTING_CHEMDETAILS
                    {
                        CHEM_PRODID = item.CHEM_PRODID,
                        UNIT = unit,
                        CONSUMPTION = dyeingSlasher.Where(c => c.CHEM_PRODID.Equals(item.CHEM_PRODID)).Sum(c => double.Parse(c.chemQty)),
                        SECTION = item.Type
                    });
                }

                foreach (var item in sizingChem)
                {
                    var unit = await DenimDbContext.F_BAS_UNITS.Where(c => c.UNAME.Equals(item.UNIT)).Select(c=>c.UID).FirstOrDefaultAsync();

                    chemList.Add(new COS_POSTCOSTING_CHEMDETAILS
                    {
                        CHEM_PRODID = item.CHEM_PRODID,
                        UNIT = unit,
                        CONSUMPTION = sizingChem.Where(c => c.CHEM_PRODID.Equals(item.CHEM_PRODID)).Sum(c => c.chemQty),
                        SECTION = item.Type
                    });
                }


                foreach (var item in finishingChem)
                {
                    var unit = await DenimDbContext.F_BAS_UNITS.Where(c => c.UNAME.Equals(item.UNIT)).Select(c=>c.UID).FirstOrDefaultAsync();

                    chemList.Add(new COS_POSTCOSTING_CHEMDETAILS
                    {
                        CHEM_PRODID = item.CHEM_PROD_ID,
                        UNIT = unit,
                        CONSUMPTION = finishingChem.Where(c => c.CHEM_PROD_ID.Equals(item.CHEM_PROD_ID)).Sum(c => c.chemQty),
                        SECTION = item.Type
                    });
                }

                postCostingViewModel.CosPostCostingChemDetailsList = new List<COS_POSTCOSTING_CHEMDETAILS>();
                for (var i = 0; i < chemList.Count; i++)
                {
                    if (i == chemList.TakeWhile(t => !(t.CHEM_PRODID.Equals(chemList[i].CHEM_PRODID) && t.CONSUMPTION == chemList[i].CONSUMPTION && t.SECTION == chemList[i].SECTION)).Count())
                    {
                        if (chemList[i].UNIT == 0)
                        {
                            chemList[i].UNIT = 1;
                        }

                        postCostingViewModel.CosPostCostingChemDetailsList.Add(chemList[i]);
                    }
                }

                return postCostingViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
