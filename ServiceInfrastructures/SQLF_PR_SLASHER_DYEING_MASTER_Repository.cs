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
    public class SQLF_PR_SLASHER_DYEING_MASTER_Repository:BaseRepository<F_PR_SLASHER_DYEING_MASTER>, IF_PR_SLASHER_DYEING_MASTER
    {
        public SQLF_PR_SLASHER_DYEING_MASTER_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<IEnumerable<F_PR_SLASHER_DYEING_MASTER>> GetAllAsync()
        {
            try
            {
                var slasherList = await DenimDbContext.F_PR_SLASHER_DYEING_MASTER
                    .Include(c=>c.F_PR_SLASHER_CHEM_CONSM)
                    .Include(c=>c.SET.PROG_.BLK_PROG_)
                    .ToListAsync();
                return slasherList;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        public async Task<FDyeingProcessSlasherViewModel> GetInitObjects(FDyeingProcessSlasherViewModel fDyeingProcessSlasherViewModel)
        {

            try
            {
                fDyeingProcessSlasherViewModel.FPrSlasherMachineInfos = await DenimDbContext.F_PR_SLASHER_MACHINE_INFO
                    .Select(c => new F_PR_SLASHER_MACHINE_INFO
                    {
                        SL_MID = c.SL_MID,
                        SL_MNO = c.SL_MNO
                    }).ToListAsync();

                fDyeingProcessSlasherViewModel.FChemStoreProductInfos =
                    await DenimDbContext.F_CHEM_STORE_PRODUCTINFO.Select(c => new F_CHEM_STORE_PRODUCTINFO
                    {
                        PRODUCTID = c.PRODUCTID,
                        PRODUCTNAME = c.PRODUCTNAME
                    }).ToListAsync();

                fDyeingProcessSlasherViewModel.Units =
                    await DenimDbContext.F_BAS_UNITS.Select(c => new F_BAS_UNITS
                    {
                        UID = c.UID,
                        UNAME = c.UNAME
                    }).ToListAsync();

                fDyeingProcessSlasherViewModel.FWeavingBeams = await DenimDbContext.F_WEAVING_BEAM
                    .Select(c => new F_WEAVING_BEAM
                    {
                        ID = c.ID,
                        BEAM_NO = c.BEAM_NO
                    }).ToListAsync();


                var FHrEmployees = await DenimDbContext.F_HR_EMP_OFFICIALINFO
                    .Include(c => c.EMP)
                    .Where(c => c.SECID.Equals(158) && !c.OPN2.Equals("Y"))
                    .ToListAsync();

                fDyeingProcessSlasherViewModel.FHrEmployees = FHrEmployees.Select(c => new F_HRD_EMPLOYEE
                {
                    EMPID = c.EMP.EMPID,
                    FIRST_NAME = c.EMP.FIRST_NAME + " " + c.EMP.LAST_NAME + '-' + c.EMP.EMPNO
                }).ToList();

                if (fDyeingProcessSlasherViewModel.FPrSlasherDyeingMaster == null)
                {
                    fDyeingProcessSlasherViewModel.PlProductionSetDistributions = await DenimDbContext
                        .PL_PRODUCTION_SETDISTRIBUTION
                        .Include(c => c.PROG_)
                        .Include(c => c.SUBGROUP.GROUP)
                        .Where(c => DenimDbContext.F_PR_WARPING_PROCESS_DW_MASTER.Any(e => e.SETID.Equals(c.SETID)) && !DenimDbContext.F_PR_SLASHER_DYEING_MASTER.Any(e => e.SETID.Equals(c.SETID)) && c.SUBGROUP.GROUP.DYEING_TYPE.Equals(2005))
                        .Select(c => new PL_PRODUCTION_SETDISTRIBUTION
                        {
                            SETID = c.SETID,
                            PROG_ = c.PROG_
                        }).ToListAsync();
                }
                else
                {
                    fDyeingProcessSlasherViewModel.PlProductionSetDistributions = await DenimDbContext
                        .PL_PRODUCTION_SETDISTRIBUTION
                        .Include(c => c.PROG_)
                        .Include(c => c.SUBGROUP.GROUP)
                        .Where(c => DenimDbContext.F_PR_WARPING_PROCESS_DW_MASTER.Any(e => e.SETID.Equals(c.SETID)) && DenimDbContext.F_PR_SLASHER_DYEING_MASTER.Any(e => e.SETID.Equals(c.SETID)) && c.SUBGROUP.GROUP.DYEING_TYPE.Equals(2005))
                        .Select(c => new PL_PRODUCTION_SETDISTRIBUTION
                        {
                            SETID = c.SETID,
                            PROG_ = c.PROG_
                        }).ToListAsync();
                }

                return fDyeingProcessSlasherViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        public async Task<dynamic> GetSetDetails(int setId)
        {
            try
            {
                var result =await DenimDbContext.PL_PRODUCTION_SETDISTRIBUTION
                    .Include(c => c.SUBGROUP)
                    .Include(c => c.PROG_.BLK_PROG_.RndProductionOrder)

                    .Include(c => c.F_PR_SLASHER_DYEING_MASTER)
                    .ThenInclude(c => c.F_PR_SLASHER_DYEING_DETAILS)
                    .ThenInclude(c => c.W_BEAM)

                    .Include(c => c.F_PR_WARPING_PROCESS_DW_MASTER)
                    .ThenInclude(c => c.F_PR_WARPING_PROCESS_DW_DETAILS)
                    .ThenInclude(c => c.BALL_NONavigation)

                    .Include(c => c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.WV)
                    .Include(c => c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.COLORCODENavigation)
                    .Include(c => c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.LOOM)
                    .Include(c => c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.WV)
                    .Include(c => c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.WV)

                    .Include(c => c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.RND_FABRIC_COUNTINFO)
                    .ThenInclude(c=>c.LOT)

                    .Include(c => c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.RND_FABRIC_COUNTINFO)
                    .ThenInclude(c=>c.SUPP)

                    .Include(c => c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.RND_FABRIC_COUNTINFO)
                    .ThenInclude(c=>c.COUNT)
                    
                    .Include(c => c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.RND_YARNCONSUMPTION)

                    .Include(c => c.PROG_.BLK_PROG_.RndProductionOrder.SO.PIMASTER.BUYER)
                    .Where( c => c.SETID.Equals(setId))
                    .Select(c => new
                    {

                        StyleName = c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.STYLE_NAME,
                        Buyer = c.PROG_.BLK_PROG_.RndProductionOrder.SO.PIMASTER.BUYER.BUYER_NAME,
                        Color = c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.COLORCODENavigation.COLOR,
                        Loomtype = c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.LOOM.LOOM_TYPE_NAME,

                        OrderNo = c.PROG_.BLK_PROG_.RndProductionOrder.SO.SO_NO,
                        Ratio = c.SUBGROUP.RATIO,
                        TotalEnds = c.PROG_.BLK_PROG_.RndProductionOrder.TOTAL_ENOS,
                        PiNo = c.PROG_.BLK_PROG_.RndProductionOrder.SO.PIMASTER.PINO,
                        SetLength = c.F_PR_WARPING_PROCESS_DW_MASTER.Select(e=>e.WARPLENGTH).FirstOrDefault()
                        
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
        
        public async Task<string> GetSetWarpLength(int setId)
        {
            try
            {
                var result =await DenimDbContext.PL_PRODUCTION_SETDISTRIBUTION
                    .Include(c => c.SUBGROUP)
                    .Include(c => c.PROG_.BLK_PROG_.RndProductionOrder)
                    
                    .Include(c => c.F_PR_WARPING_PROCESS_DW_MASTER)
                    .ThenInclude(c => c.F_PR_WARPING_PROCESS_DW_DETAILS)
                    .ThenInclude(c => c.BALL_NONavigation)
                    .FirstOrDefaultAsync(c => c.SETID.Equals(setId));

                
                return result.F_PR_WARPING_PROCESS_DW_MASTER.FirstOrDefault().WARPLENGTH;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        public async Task<FDyeingProcessSlasherViewModel> FindAllByIdAsync(int sId)
        {
            try
            {
                var result = await DenimDbContext.F_PR_SLASHER_DYEING_MASTER
                    .Include(c => c.SET.PROG_)
                    .Include(c => c.F_PR_SLASHER_CHEM_CONSM)
                    .ThenInclude(c => c.CHEM_PROD)
                    .Include(c => c.F_PR_SLASHER_DYEING_DETAILS)
                    .ThenInclude(c => c.EMP)
                    .Include(c => c.F_PR_SLASHER_DYEING_DETAILS)
                    .ThenInclude(c => c.SL_M)
                    .Include(c => c.F_PR_SLASHER_DYEING_DETAILS)
                    .ThenInclude(c => c.W_BEAM)
                    .Where(c => c.SLID.Equals(sId))
                    .FirstOrDefaultAsync();

                var fDyeingProcessSlasherViewModel = new FDyeingProcessSlasherViewModel
                {
                    FPrSlasherDyeingMaster = result,
                    FPrSlasherChemConsmList = result.F_PR_SLASHER_CHEM_CONSM.ToList(),
                    FPrSlasherDyeingDetailsList = result.F_PR_SLASHER_DYEING_DETAILS.ToList()
                };

                return fDyeingProcessSlasherViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
    }
}
