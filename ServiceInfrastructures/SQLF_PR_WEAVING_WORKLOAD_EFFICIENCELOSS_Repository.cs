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
    public class SQLF_PR_WEAVING_WORKLOAD_EFFICIENCELOSS_Repository : BaseRepository<F_PR_WEAVING_WORKLOAD_EFFICIENCELOSS>, IF_PR_WEAVING_WORKLOAD_EFFICIENCELOSS
    {
        public SQLF_PR_WEAVING_WORKLOAD_EFFICIENCELOSS_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<IEnumerable<F_PR_WEAVING_WORKLOAD_EFFICIENCELOSS>> GetAllFPrWeavingWorkLoadEfficiencyLossAsync()
        {
            return await DenimDbContext.F_PR_WEAVING_WORKLOAD_EFFICIENCELOSS
                .Include(e => e.SIEMP)
                .Include(e => e.LOOM)
                .Include(e => e.LOOM)
                .Include(e => e.SHIFT)
                .Select(d => new F_PR_WEAVING_WORKLOAD_EFFICIENCELOSS()
                {
                    WWEID = d.WWEID,
                    SHIFT_DATE = d.SHIFT_DATE,
                   KNOTTING = d.KNOTTING,
                   NEWRUN = d.NEWRUN,
                   PENDING = d.PENDING,
                   RE_KNOTTING = d.RE_KNOTTING,
                   REED_CHANGE = d.RE_KNOTTING,
                   CAM_CHANGE = d.CAM_CHANGE,
                   ARTICLE_CHANGE = d.ARTICLE_CHANGE,
                   PATTERN_CHANGE = d.PATTERN_CHANGE,
                   EXTRA_WARP_INSERTION = d.EXTRA_WARP_INSERTION,
                   EXTRA_WARP_OUT = d.EXTRA_WARP_OUT,
                   BEAM_SHORT = d.BEAM_SHORT,
                   YARN_SHORT = d.YARN_SHORT,
                   MECHANICAL_WORK = d.MECHANICAL_WORK,
                   ELECTRICAL_WORK = d.ELECTRICAL_WORK,
                   COMPRESSOR_WORK = d.COMPRESSOR_WORK,
                   QA_HOLD = d.QA_HOLD,
                   QA_STOP = d.QA_STOP,
                   RND_STOP = d.RND_STOP,
                   OTHER_STOP = d.OTHER_STOP,
                   KNOTTING_GAITING = d.KNOTTING_GAITING,
                   BREAKAGES = d.BREAKAGES,
                   OP1 = d.OP1,
                   REMARKS = d.REMARKS

                })
                .ToListAsync();
        }

        public async Task<FPrWeavingWorkLoadEfficiencyLossViewModel> GetInitObjByAsync(FPrWeavingWorkLoadEfficiencyLossViewModel fPrWeavingWorkLoadEfficiencyLossViewModel)
        {
            fPrWeavingWorkLoadEfficiencyLossViewModel.EmployeeList = await DenimDbContext.F_HRD_EMPLOYEE
                .Select(d => new F_HRD_EMPLOYEE
                {
                    EMPID = d.EMPID,
                    FIRST_NAME = d.FIRST_NAME
                })
                .ToListAsync();

            fPrWeavingWorkLoadEfficiencyLossViewModel.LoomTypeList = await DenimDbContext.LOOM_TYPE
                .Select(d => new LOOM_TYPE
                {
                    LOOMID = d.LOOMID,
                    LOOM_TYPE_NAME = d.LOOM_TYPE_NAME
                })
                .ToListAsync();
           
            //fPrWeavingWorkLoadEfficiencyLossViewModel.ShiftList = await _denimDbContext.F_HR_SHIFT_INFO
            //    .Select(d => new F_HR_SHIFT_INFO()
            //    {
            //        ID = d.ID,
            //        SHIFT = d.SHIFT
            //    })
            //    .ToListAsync();

            return fPrWeavingWorkLoadEfficiencyLossViewModel;
        }


    }
}
