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
    public class SQLF_FS_CLEARANCE_WASTAGE_TRANSFER_Repository : BaseRepository<F_FS_CLEARANCE_WASTAGE_TRANSFER>, IF_FS_CLEARANCE_WASTAGE_TRANSFER
    {
        private readonly IDataProtector _protector;

        public SQLF_FS_CLEARANCE_WASTAGE_TRANSFER_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<FFsClearanceWastageTransferViewModel> GetInitObjByAsync(FFsClearanceWastageTransferViewModel fFsClearanceWastageTransferViewModel)
        {
            fFsClearanceWastageTransferViewModel.FFsClearanceWastageItemsList = await DenimDbContext
                .F_FS_CLEARANCE_WASTAGE_ITEM
                .Select(d => new F_FS_CLEARANCE_WASTAGE_ITEM
                {
                    IID = d.IID,
                    INAME = d.INAME
                }).ToListAsync();
            fFsClearanceWastageTransferViewModel.FHrEmployeesList = await DenimDbContext
                .F_HRD_EMPLOYEE
                .Select(d => new F_HRD_EMPLOYEE
                {
                    EMPID = d.EMPID,
                    FIRST_NAME = $"{d.EMPNO}, {d.FIRST_NAME} {d.LAST_NAME}"
                }).ToListAsync();

            return fFsClearanceWastageTransferViewModel;
        }

        public async Task<IEnumerable<F_FS_CLEARANCE_WASTAGE_TRANSFER>> GetAllFFsClearanceWastageTransferAsync()
        {
            return await DenimDbContext.F_FS_CLEARANCE_WASTAGE_TRANSFER
                .Include(d => d.ITEMNavigation)
                .Include(d => d.CLRBYNavigation)
                .Select(d => new F_FS_CLEARANCE_WASTAGE_TRANSFER
                {
                    TRANSID = d.TRANSID,
                    EncryptedId = _protector.Protect(d.TRANSID.ToString()),
                    TRANSDATE = d.TRANSDATE,
                    QTY_YDS = d.QTY_YDS,
                    QTY_KG = d.QTY_KG,
                    WTRNO = d.WTRNO,
                    REMARKS = d.REMARKS,
                    ITEMNavigation = new F_FS_CLEARANCE_WASTAGE_ITEM
                    {
                        INAME = d.ITEMNavigation.INAME
                    },
                    CLRBYNavigation = new F_HRD_EMPLOYEE
                    {
                        FIRST_NAME = $"{d.CLRBYNavigation.EMPNO}, {d.CLRBYNavigation.FIRST_NAME} {d.CLRBYNavigation.LAST_NAME}"
                    }
                }).ToListAsync();
        }
    }
}
