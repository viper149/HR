using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_PR_INSPECTION_CUTPCS_TRANSFER_Repository : BaseRepository<F_PR_INSPECTION_CUTPCS_TRANSFER>,
        IF_PR_INSPECTION_CUTPCS_TRANSFER
    {
        private readonly IDataProtector _protector;

        public SQLF_PR_INSPECTION_CUTPCS_TRANSFER_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<IEnumerable<F_PR_INSPECTION_CUTPCS_TRANSFER>> GetAllFprInspectionCutPcsTransferAsync()
        {
            return await DenimDbContext.F_PR_INSPECTION_CUTPCS_TRANSFER
                .Include(e => e.SHIFTNavigation)
                .Select(d => new F_PR_INSPECTION_CUTPCS_TRANSFER()
                {
                    CPID = d.CPID,
                    EncryptedId = _protector.Protect(d.CPID.ToString()),
                    TRNS_DATE = d.TRNS_DATE,
                    ROLL_NO = d.ROLL_NO,
                    QTY_YDS = d.QTY_YDS,
                    QTY_KG = d.QTY_KG,
                    REMARKS = d.REMARKS,
                    SHIFTNavigation = new F_HR_SHIFT_INFO()
                    {
                        SHIFT = d.SHIFTNavigation.SHIFT
                    },

                }).ToListAsync();
        }

        public async Task<FprInspectionCutPcsTransferViewModel> GetInitObjByAsync(
            FprInspectionCutPcsTransferViewModel fprInspectionCutPcsTransferViewModel)
        {
            fprInspectionCutPcsTransferViewModel.ShiftList = await DenimDbContext.F_HR_SHIFT_INFO
                .Select(d => new F_HR_SHIFT_INFO
                {
                    ID = d.ID,
                    SHIFT = d.SHIFT
                }).ToListAsync();
            //fprInspectionCutPcsTransferViewModel.RollList = await _denimDbContext.F_PR_INSPECTION_PROCESS_DETAILS
            //    .Select(d => new F_PR_INSPECTION_PROCESS_DETAILS
            //    {
            //        ROLL_ID = d.ROLL_ID,
            //        ROLLNO = d.ROLLNO
            //    }).ToListAsync();

            return fprInspectionCutPcsTransferViewModel;
        }

        public async Task<FprInspectionCutPcsTransferViewModel> FindByIdIncludeAllAsync(int cpid)
        {
            var a = await DenimDbContext.F_PR_INSPECTION_CUTPCS_TRANSFER
                .Include(d => d.SHIFTNavigation)
                .Select(d => new FprInspectionCutPcsTransferViewModel()
                {
                    FPrInspectionCutpcsTransfer = new F_PR_INSPECTION_CUTPCS_TRANSFER()
                    {
                        CPID = d.CPID,
                        EncryptedId = _protector.Protect(d.CPID.ToString()),
                        TRNS_DATE = d.TRNS_DATE,
                        ROLL_NO = d.ROLL_NO,
                        QTY_YDS = d.QTY_YDS,
                        QTY_KG = d.QTY_KG,
                        REMARKS = d.REMARKS,

                        SHIFTNavigation = new F_HR_SHIFT_INFO()
                        {
                            SHIFT = d.SHIFTNavigation.SHIFT
                        }
                    }
                }).FirstOrDefaultAsync();
            return a;
        }
    }
}
