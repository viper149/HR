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
    public class SQLF_PR_RECONE_MASTER_Repository : BaseRepository<F_PR_RECONE_MASTER>, IF_PR_RECONE_MASTER
    {
        public SQLF_PR_RECONE_MASTER_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<List<F_PR_RECONE_MASTER>> GetFPrReconeMasterInfoAsync()
        {
            return await DenimDbContext.F_PR_RECONE_MASTER
              
               .Select(d => new F_PR_RECONE_MASTER()
               {
                  TRANSID = d.TRANSID,
                  TRANSDATE = d.TRANSDATE,
                  SET_NO = d.SET_NO,
                  WARP_LENGTH = d.WARP_LENGTH,
                  WARP_RATIO = d. WARP_RATIO,
                  CONVERTED = d.CONVERTED,
                   REMARKS = d.REMARKS

               })
               .ToListAsync();
        }

        public async Task<FPrReconeMasterViewModel> GetInitObjByAsync(FPrReconeMasterViewModel fPrReconeMasterViewModel)
        {
            fPrReconeMasterViewModel.SetList = await DenimDbContext.PL_PRODUCTION_SETDISTRIBUTION
                .Include(d=>d.PROG_)
                .Where(c=>c.PROG_.YARN_TYPE.Equals(4))
                .Select(d => new PL_PRODUCTION_SETDISTRIBUTION
                {
                    SETID = d.SETID,
                    PROG_ = new PL_BULK_PROG_SETUP_D
                    {
                        PROG_NO = d.PROG_.PROG_NO
                    }
                }).ToListAsync();

            fPrReconeMasterViewModel.BallList = await DenimDbContext.F_LCB_PRODUCTION_ROPE_DETAILS
                .Include(d=>d.CAN.BALL.BALL_ID_FKNavigation)
                .Select(d => new F_LCB_PRODUCTION_ROPE_DETAILS
                {
                    LCB_D_ID = d.LCB_D_ID,
                    CAN = new F_DYEING_PROCESS_ROPE_DETAILS
                    {
                        BALL = new F_PR_WARPING_PROCESS_ROPE_BALL_DETAILS
                        {
                            BALL_ID_FKNavigation = new F_BAS_BALL_INFO
                            {
                                BALL_NO = d.CAN.BALL.BALL_ID_FKNavigation.BALL_NO
                            }
                        }
                    }
                }).ToListAsync();

            fPrReconeMasterViewModel.LinkBallList = await DenimDbContext.F_LCB_PRODUCTION_ROPE_DETAILS
                .Include(d => d.CAN.BALL.BALL_ID_FKNavigation)
                .Select(d => new F_LCB_PRODUCTION_ROPE_DETAILS
                {
                    LCB_D_ID = d.LCB_D_ID,
                    CAN = new F_DYEING_PROCESS_ROPE_DETAILS
                    {
                        BALL = new F_PR_WARPING_PROCESS_ROPE_BALL_DETAILS
                        {
                            BALL_ID_FKNavigation = new F_BAS_BALL_INFO
                            {
                                BALL_NO = d.CAN.BALL.BALL_ID_FKNavigation.BALL_NO
                            }
                        }
                    }
                }).ToListAsync();

            fPrReconeMasterViewModel.RndCountList = await DenimDbContext.RND_FABRIC_COUNTINFO
                .Include(d=> d.COUNT)
                .Select(d => new RND_FABRIC_COUNTINFO
                {
                    COUNTID = d.COUNTID,

                    COUNT = new BAS_YARN_COUNTINFO()
                    {
                        COUNTNAME = d.COUNT.COUNTNAME,
                    }
                }).ToListAsync();

            fPrReconeMasterViewModel.ShiftList = await DenimDbContext.F_HR_SHIFT_INFO
                .Select(d => new F_HR_SHIFT_INFO
                {
                    ID = d.ID,
                    SHIFT = d.SHIFT
                }).ToListAsync();

            fPrReconeMasterViewModel.MachineList = await DenimDbContext.F_LCB_MACHINE
                .Select(d => new F_LCB_MACHINE
                {
                    ID = d.ID,
                    MACHINE_NO = d.MACHINE_NO,
                }).ToListAsync();

            fPrReconeMasterViewModel.YarnCountList = await DenimDbContext.BAS_YARN_COUNTINFO
                .Select(d => new BAS_YARN_COUNTINFO
                {
                    COUNTID = d.COUNTID,
                    COUNTNAME = d.COUNTNAME
                }).ToListAsync();

            return fPrReconeMasterViewModel;

           
        }


    }
}
