using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces.SampleGarments.GatePass;
using DenimERP.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures.SampleGarments.GatePass
{
    public class SQLF_BAS_VEHICLE_INFO_Repository : BaseRepository<F_BAS_VEHICLE_INFO>, IF_BAS_VEHICLE_INFO
    {
        public SQLF_BAS_VEHICLE_INFO_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<IEnumerable<F_BAS_VEHICLE_INFO>> GetAllFBasVehicleInfoAsync()
        {
            return await DenimDbContext.F_BAS_VEHICLE_INFO
                .Select(d => new F_BAS_VEHICLE_INFO()
                {
                    VID = d.VID,
                    VNUMBER = d.VNUMBER,
                    VEHICLE_TYPE = d.VEHICLE_TYPE,
                    REMARKS = d.REMARKS

                })
                .ToListAsync();

            
        }

        public async Task<FBasVehicleInfoViewModel> GetInitObjByAsync(FBasVehicleInfoViewModel fBasVehicleInfoViewModel)
        {
            fBasVehicleInfoViewModel.DriverList = await DenimDbContext.F_BAS_DRIVERINFO
                .Select(d => new F_BAS_DRIVERINFO
                {
                    DRID = d.DRID,
                    DRIVER_NAME = d.DRIVER_NAME
                })
                .ToListAsync();

            fBasVehicleInfoViewModel.VehicleTypeList = await DenimDbContext.VEHICLE_TYPE

                .Select(d => new VEHICLE_TYPE

                {
                    ID = d.ID,
                    TYPE_NAME = d.TYPE_NAME
                })
                .ToListAsync();

            return fBasVehicleInfoViewModel;
        }
    }
}
