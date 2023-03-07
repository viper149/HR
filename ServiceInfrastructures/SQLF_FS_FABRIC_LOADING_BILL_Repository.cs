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
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_FS_FABRIC_LOADING_BILL_Repository : BaseRepository<F_FS_FABRIC_LOADING_BILL>, IF_FS_FABRIC_LOADING_BILL
    {
        private readonly IDataProtector _protector;

        public SQLF_FS_FABRIC_LOADING_BILL_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<IEnumerable<F_FS_FABRIC_LOADING_BILL>> GetAllFFsFabricLoadingBillAsync()
        {
            return await DenimDbContext.F_FS_FABRIC_LOADING_BILL
                .Include(d=> d.VEHICLE_)
                .Select(d => new F_FS_FABRIC_LOADING_BILL()
                {
                    TRANSID = d.TRANSID,
                    EncryptedId = _protector.Protect(d.TRANSID.ToString()),
                    TRANSDATE = d.TRANSDATE,
                    START_TIME = d.START_TIME,
                    END_TIME = d.END_TIME,
                    ROLL_QTY = d.ROLL_QTY,
                    RATE = d.RATE,
                    BILL_NO = d.BILL_NO,
                    REMARKS = d.REMARKS,
                    VEHICLE_ = new F_BAS_VEHICLE_INFO()
                    {
                        VNUMBER = d.VEHICLE_.VNUMBER,
                    }
                })
                .OrderByDescending(c=>c.TRANSID)
                .ToListAsync();
        }

        public async Task<FFsFabricLoadingBillViewModel> GetInitObjByAsync(FFsFabricLoadingBillViewModel fFsFabricLoadingBillViewModel)
        {
            fFsFabricLoadingBillViewModel.VehicleList = await DenimDbContext.F_BAS_VEHICLE_INFO
                .Select(d => new F_BAS_VEHICLE_INFO
                {
                    VID = d.VID,
                    VNUMBER = d.VNUMBER
                })
                .ToListAsync();
            return fFsFabricLoadingBillViewModel;
        }

        public async Task<F_FS_FABRIC_LOADING_BILL> UpdateByAsync(F_FS_FABRIC_LOADING_BILL fFsFabricLoadingBill)
        {
            throw new NotImplementedException();
        }
    }
}
