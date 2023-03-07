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
    public class SQLF_PR_INSPECTION_FABRIC_D_MASTER_Repository : BaseRepository<F_PR_INSPECTION_FABRIC_D_MASTER>, IF_PR_INSPECTION_FABRIC_D_MASTER
    {
        private readonly IDataProtector _protector;

        public SQLF_PR_INSPECTION_FABRIC_D_MASTER_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<FPrInspectionFabricDispatchViewModel> FindByRollRcvIdAsync(int rollRcvId)
        {
            try
            {
                var result = await DenimDbContext.F_PR_INSPECTION_FABRIC_D_MASTER
                    .Include(c => c.F_PR_INSPECTION_FABRIC_D_DETAILS)
                    .ThenInclude(c => c.FABCODENavigation)
                    .Include(c => c.F_PR_INSPECTION_FABRIC_D_DETAILS)
                    .ThenInclude(c => c.SO_NONavigation)
                    .Include(c => c.F_PR_INSPECTION_FABRIC_D_DETAILS)
                    .ThenInclude(c => c.LOCATIONNavigation)
                    .Include(c => c.F_PR_INSPECTION_FABRIC_D_DETAILS)
                    .ThenInclude(c => c.PO_NONavigation)
                    .Include(c => c.SEC)
                    .Include(c => c.F_PR_INSPECTION_FABRIC_D_DETAILS)
                    .ThenInclude(c => c.ROLL_)
                    .Where(c => c.DID.Equals(rollRcvId)).FirstOrDefaultAsync();

                var fFsRollReceiveViewModel = new FPrInspectionFabricDispatchViewModel
                {
                    FPrInspectionFabricDMaster = result,
                    FPrInspectionFabricDDetailsList = result.F_PR_INSPECTION_FABRIC_D_DETAILS.ToList()
                };

                return fFsRollReceiveViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<F_PR_INSPECTION_FABRIC_D_MASTER>> GetAllAsync()
        {
            try
            {
                var result = await DenimDbContext.F_PR_INSPECTION_FABRIC_D_MASTER
                    .Include(c => c.F_PR_INSPECTION_FABRIC_D_DETAILS)
                    .Select(d=>new F_PR_INSPECTION_FABRIC_D_MASTER
                    {
                        DID = d.DID,
                        EncryptedId = _protector.Protect(d.DID.ToString()),
                        DDATE = d.DDATE,
                        NO_ROLL = d.F_PR_INSPECTION_FABRIC_D_DETAILS.Count(),
                        REMARKS = d.REMARKS
                    }).ToListAsync();
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<FPrInspectionFabricDispatchViewModel> GetInitObjects(FPrInspectionFabricDispatchViewModel fPrInspectionFabricDispatchViewModel)
        {
            try
            {
                fPrInspectionFabricDispatchViewModel.FBasSections = await DenimDbContext.F_BAS_SECTION.ToListAsync();
                fPrInspectionFabricDispatchViewModel.FFsLocations = await DenimDbContext.F_FS_LOCATION.OrderBy(c => c.LOC_NO).ToListAsync();
                return fPrInspectionFabricDispatchViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<F_PR_INSPECTION_FABRIC_D_MASTER> GetRollDetailsByDate(DateTime? dDate)
        {
            try
            {
                var fPrInspectionFabricDMaster = await DenimDbContext.F_PR_INSPECTION_FABRIC_D_MASTER
                    .FirstOrDefaultAsync(c => c.DDATE.Equals(dDate));
                return fPrInspectionFabricDMaster;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<int> InsertAndGetIdAsync(F_PR_INSPECTION_FABRIC_D_MASTER fPrInspectionFabricDMaster)
        {
            try
            {
                await DenimDbContext.F_PR_INSPECTION_FABRIC_D_MASTER.AddAsync(fPrInspectionFabricDMaster);
                await SaveChanges();
                return fPrInspectionFabricDMaster.DID;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
