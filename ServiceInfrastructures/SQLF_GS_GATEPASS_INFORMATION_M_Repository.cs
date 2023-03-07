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
    public class SQLF_GS_GATEPASS_INFORMATION_M_Repository : BaseRepository<F_GS_GATEPASS_INFORMATION_M>,
        IF_GS_GATEPASS_INFORMATION_M
    {
        private readonly IDataProtector _protector;

        public SQLF_GS_GATEPASS_INFORMATION_M_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<int> InsertAndGetId(F_GS_GATEPASS_INFORMATION_M fGsGatepassInformationM)
        {
            try
            {
                await DenimDbContext.F_GS_GATEPASS_INFORMATION_M.AddAsync(fGsGatepassInformationM);
                await SaveChanges();
                return fGsGatepassInformationM.GPID;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<F_GS_GATEPASS_INFORMATION_M>> GetAllGsGatePassAsync()
        {
            return await DenimDbContext.F_GS_GATEPASS_INFORMATION_M
                .Include(d => d.EMP)
                .Include(d => d.EMP_REQBYNavigation)
                .Include(d => d.V)
                .Include(d => d.SEC)
                .Include(d => d.DEPT)
                .Include(d => d.GPT)
                .Select(d => new F_GS_GATEPASS_INFORMATION_M
                {
                    GPID = d.GPID,
                    EncryptedId = _protector.Protect(d.GPID.ToString()),
                    GPDATE = d.GPDATE,
                    GPNO = d.GPNO,
                    SECID = d.SECID,
                    DEPTID = d.DEPTID,
                    EMPID = d.EMPID,
                    SENDTO = d.SENDTO,
                    ADDRESS = d.ADDRESS,
                    REQ_BY = d.REQ_BY,
                    VID = d.VID,
                    IS_RETURNABLE = d.IS_RETURNABLE,
                    REMARKS = d.REMARKS,
                    SEC = new F_BAS_SECTION
                    {
                        SECNAME = d.SEC.SECNAME
                    },
                    DEPT = new F_BAS_DEPARTMENT
                    {
                        DEPTNAME = d.DEPT.DEPTNAME
                    },
                    EMP = new F_HRD_EMPLOYEE
                    {
                        FIRST_NAME = $"{d.EMP.FIRST_NAME} {d.EMP.LAST_NAME}"
                    },
                    EMP_REQBYNavigation = new F_HRD_EMPLOYEE
                    {
                        FIRST_NAME = $"{d.EMP_REQBYNavigation.FIRST_NAME} {d.EMP_REQBYNavigation.LAST_NAME}"
                    },
                    GPT = new F_GATEPASS_TYPE
                    {
                        GPTID = d.GPT.GPTID,
                        NAME = d.GPT.NAME
                    },
                    V = new F_BAS_VEHICLE_INFO
                    {
                        VNUMBER = d.V.VNUMBER
                    }
                }).ToListAsync();
        }

        public async Task<FGsGatePassViewModel> GetInitObjByAsync(FGsGatePassViewModel fGsGatePassViewModel)
        {
            fGsGatePassViewModel.FBasDepartmentList = await DenimDbContext.F_BAS_DEPARTMENT
                .Select(d => new F_BAS_DEPARTMENT
                {
                    DEPTID = d.DEPTID,
                    DEPTNAME = d.DEPTNAME
                })
                .ToListAsync();

            fGsGatePassViewModel.FBasSectionList = await DenimDbContext.F_BAS_SECTION
                .Select(d => new F_BAS_SECTION
                {
                    SECID = d.SECID,
                    SECNAME = d.SECNAME
                }).ToListAsync();

            fGsGatePassViewModel.FHrEmployeeList = await DenimDbContext.F_HRD_EMPLOYEE
                .Select(d => new F_HRD_EMPLOYEE
                {
                    EMPID = d.EMPID,
                    FIRST_NAME = $"{d.FIRST_NAME} {d.LAST_NAME}"
                })
                .ToListAsync();

            fGsGatePassViewModel.FGatepassTypeList = await DenimDbContext.F_GATEPASS_TYPE
                .Select(d => new F_GATEPASS_TYPE
                {
                    GPTID = d.GPTID,
                    NAME = d.NAME
                }).ToListAsync();

            fGsGatePassViewModel.FBasVehicleInfoList = await DenimDbContext.F_BAS_VEHICLE_INFO
                .Select(d => new F_BAS_VEHICLE_INFO
                {
                    VID = d.VID,
                    VNUMBER = d.VNUMBER
                }).ToListAsync();

            fGsGatePassViewModel.FGsProductInformationList = await DenimDbContext.F_GS_PRODUCT_INFORMATION
                .Select(d => new F_GS_PRODUCT_INFORMATION
                {
                    PRODID = d.PRODID,
                    PRODNAME = d.PRODNAME
                })
                .ToListAsync();

            return fGsGatePassViewModel;
        }

        public async Task<FGsGatePassViewModel> GetInitDetailsObjByAsync(FGsGatePassViewModel fGsGatePassViewModel)
        {
            foreach (var item in fGsGatePassViewModel.FGsGatepassInformationDList)
            {
                item.PROD = await DenimDbContext.F_GS_PRODUCT_INFORMATION
                    .Where(d => d.PRODID.Equals(item.PRODID))
                    .Select(d => new F_GS_PRODUCT_INFORMATION
                    {
                        PRODID = d.PRODID,
                        PRODNAME = d.PRODNAME
                    }).FirstOrDefaultAsync();
            }

            return fGsGatePassViewModel;
        }

        public async Task<FGsGatePassViewModel> FindByIdIncludeAllAsync(int gpId)
        {
            return await DenimDbContext.F_GS_GATEPASS_INFORMATION_M
                .Include(d => d.SEC)
                .Include(d => d.DEPT)
                .Include(d => d.EMP)
                .Include(d => d.EMP_REQBYNavigation)
                .Include(d => d.GPT)
                .Include(d => d.V)
                .Include(d => d.F_GS_GATEPASS_INFORMATION_D)
                .ThenInclude(d => d.PROD.UNITNavigation)
                .Where(d => d.GPID.Equals(gpId))
                .Select(d => new FGsGatePassViewModel
                {
                    FGsGatepassInformationM = new F_GS_GATEPASS_INFORMATION_M
                    {
                        GPID = d.GPID,
                        GPNO = d.GPNO,
                        GPDATE = d.GPDATE,
                        DEPTID = d.DEPTID,
                        SECID = d.SECID,
                        EMPID = d.EMPID,
                        SENDTO = d.SENDTO,
                        ADDRESS = d.ADDRESS,
                        REQ_BY = d.REQ_BY,
                        GPTID = d.GPTID,
                        VID = d.VID,
                        IS_RETURNABLE = d.IS_RETURNABLE,
                        REMARKS = d.REMARKS,

                        DEPT = new F_BAS_DEPARTMENT
                        {
                            DEPTNAME = d.DEPT.DEPTNAME
                        },
                        SEC = new F_BAS_SECTION
                        {
                            SECNAME = d.SEC.SECNAME
                        },
                        EMP = new F_HRD_EMPLOYEE
                        {
                            FIRST_NAME = $"{d.EMP.FIRST_NAME} {d.EMP.LAST_NAME}"
                        },
                        EMP_REQBYNavigation = new F_HRD_EMPLOYEE
                        {
                            FIRST_NAME = $"{d.EMP_REQBYNavigation.FIRST_NAME} {d.EMP_REQBYNavigation.LAST_NAME}"
                        },
                        GPT = new F_GATEPASS_TYPE
                        {
                            NAME = d.GPT.NAME
                        },
                        V = new F_BAS_VEHICLE_INFO
                        {
                            VNUMBER = d.V.VNUMBER
                        }
                    },
                    FGsGatepassInformationDList = d.F_GS_GATEPASS_INFORMATION_D.Select(f =>
                        new F_GS_GATEPASS_INFORMATION_D
                        {
                            TRNSID = f.TRNSID,
                            GPID = f.GPID,
                            PRODID = f.PRODID,
                            ISSUE_QTY = f.ISSUE_QTY,
                            ISSUE_BAG = f.ISSUE_BAG,
                            ETR = f.ETR,
                            REMARKS = f.REMARKS,
                            PROD = new F_GS_PRODUCT_INFORMATION
                            {
                                PRODNAME = f.PROD.PRODNAME,
                                UNITNavigation = new F_BAS_UNITS
                                {
                                    UNAME = f.PROD.UNITNavigation.UNAME
                                }
                            }
                        }).ToList()
                }).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<FGsGatePassViewModel>> GetProductListByGpIdAsync(int gpId)
        {
            return await DenimDbContext.F_GS_GATEPASS_INFORMATION_M
                .Include(d => d.F_GS_GATEPASS_INFORMATION_D)
                .ThenInclude(d => d.PROD)
                .Where(d => d.GPID.Equals(gpId))
                .Select(d => new FGsGatePassViewModel
                {
                    FGsGatepassInformationDList = d.F_GS_GATEPASS_INFORMATION_D.Select(f =>
                        new F_GS_GATEPASS_INFORMATION_D
                        {
                            PROD = new F_GS_PRODUCT_INFORMATION
                            {
                                PRODID = f.PROD.PRODID,
                                PRODNAME = $"{f.PROD.PRODID} - {f.PROD.PRODNAME} - {f.PROD.PARTNO}"
                            }
                        }).ToList()
                }).ToListAsync();
        }
    }
}