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
    public class SQLF_GS_GATEPASS_RETURN_RCV_MASTER_Repository : BaseRepository<F_GS_GATEPASS_RETURN_RCV_MASTER>, IF_GS_GATEPASS_RETURN_RCV_MASTER
    {
        private readonly IDataProtector _protector;

        public SQLF_GS_GATEPASS_RETURN_RCV_MASTER_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<IEnumerable<F_GS_GATEPASS_RETURN_RCV_MASTER>> GetAllFGenSRequirementAsync()
        {
            return await DenimDbContext.F_GS_GATEPASS_RETURN_RCV_MASTER
                .Include(d => d.GP)
                .Include(d => d.RCVD_BYNavigation)
                .Select(d => new F_GS_GATEPASS_RETURN_RCV_MASTER
                {
                    RCVID = d.RCVID,
                    EncryptedId = _protector.Protect(d.RCVID.ToString()),
                    RCVDATE = d.RCVDATE,
                    REMARKS = d.REMARKS,
                    GP = new F_GS_GATEPASS_INFORMATION_M
                    {
                        GPNO = d.GP.GPNO
                    },
                    RCVD_BYNavigation = new F_HRD_EMPLOYEE
                    {
                        FIRST_NAME = $"{d.RCVD_BYNavigation.EMPNO}, {d.RCVD_BYNavigation.FIRST_NAME} {d.RCVD_BYNavigation.LAST_NAME}"
                    }
                })
                .ToListAsync();
        }

        public async Task<FGsGatepassReturnRcvViewModel> GetInitObjByAsync(FGsGatepassReturnRcvViewModel fGsGatepassReturnRcvViewModel)
        {
            fGsGatepassReturnRcvViewModel.FGsGatepassInformationMList = await DenimDbContext.F_GS_GATEPASS_INFORMATION_M
                .Select(d => new F_GS_GATEPASS_INFORMATION_M
                {
                    GPID = d.GPID,
                    GPNO = d.GPNO
                }).OrderBy(d => d.GPID).ToListAsync();

            fGsGatepassReturnRcvViewModel.FHrEmployeesList = await DenimDbContext.F_HRD_EMPLOYEE
                .Select(d => new F_HRD_EMPLOYEE
                {
                    EMPID = d.EMPID,
                    FIRST_NAME = $"{d.EMPNO}, {d.FIRST_NAME} {d.LAST_NAME}"
                }).OrderBy(d => d.EMPNO).ToListAsync();

            return fGsGatepassReturnRcvViewModel;
        }

        public async Task<FGsGatepassReturnRcvViewModel> GetInitDetailsObjByAsync(FGsGatepassReturnRcvViewModel fGsGatepassReturnRcvViewModel)
        {
            foreach (var item in fGsGatepassReturnRcvViewModel.FGsGatepassReturnRcvDetailsList)
            {
                item.PROD = await DenimDbContext.F_GS_PRODUCT_INFORMATION
                    .Include(d => d.UNITNavigation)
                    .Where(d => d.PRODID.Equals(item.PRODID))
                    .Select(d => new F_GS_PRODUCT_INFORMATION
                    {
                        PRODID = d.PRODID,
                        PRODNAME = $"{d.PRODID} - {d.PRODNAME} - {d.PARTNO}",
                        UNITNavigation = new F_BAS_UNITS
                        {
                            UID = d.UNITNavigation != null ? d.UNITNavigation.UID : 0,
                            UNAME = d.UNITNavigation != null ? d.UNITNavigation.UNAME : ""
                        }
                    }).FirstOrDefaultAsync();
            }

            return fGsGatepassReturnRcvViewModel;
        }

        public async Task<FGsGatepassReturnRcvViewModel> FindByIdIncludeAllAsync(int rcvId)
        {
            return await DenimDbContext.F_GS_GATEPASS_RETURN_RCV_MASTER
                .Include(d=>d.GP)
                .Include(d => d.RCVD_BYNavigation)
                .Include(d => d.F_GS_GATEPASS_RETURN_RCV_DETAILS)
                .ThenInclude(d => d.PROD)
                .Where(d => d.RCVID.Equals(rcvId))
                .Select(d => new FGsGatepassReturnRcvViewModel
                {
                    FGsGatepassReturnRcvMaster = new F_GS_GATEPASS_RETURN_RCV_MASTER
                    {
                        RCVID = d.RCVID,
                        EncryptedId = _protector.Protect(d.RCVID.ToString()),
                        RCVDATE = d.RCVDATE,
                        GPID = d.GPID,
                        RCVD_BY = d.RCVD_BY,
                        REMARKS = d.REMARKS,
                        GP = new F_GS_GATEPASS_INFORMATION_M()
                        {
                            GPID = d.GP.GPID,
                            GPNO = d.GP.GPNO
                        },
                        RCVD_BYNavigation = new F_HRD_EMPLOYEE
                        {
                            EMPID = d.RCVD_BYNavigation.EMPID,
                            FIRST_NAME = $"{d.RCVD_BYNavigation.EMPNO}, {d.RCVD_BYNavigation.FIRST_NAME} {d.RCVD_BYNavigation.LAST_NAME}"
                        }
                    },
                    FGsGatepassReturnRcvDetailsList = d.F_GS_GATEPASS_RETURN_RCV_DETAILS
                        .Select(f => new F_GS_GATEPASS_RETURN_RCV_DETAILS
                        {
                            TRNSID = f.TRNSID,
                            RCVID = f.RCVID,
                            PRODID = f.PRODID,
                            RCV_QTY = f.RCV_QTY,
                            RCV_BAG = f.RCV_BAG,
                            GATE_ENTRYNO = f.GATE_ENTRYNO,
                            REMARKS = f.REMARKS,
                            PROD = new F_GS_PRODUCT_INFORMATION()
                            {
                                PRODID = f.PROD.PRODID,
                                PRODNAME = $"{f.PROD.PRODID} - {f.PROD.PRODNAME} - {f.PROD.PARTNO}"
                            }
                        }).ToList()
                }).FirstOrDefaultAsync();
        }
    }
}
