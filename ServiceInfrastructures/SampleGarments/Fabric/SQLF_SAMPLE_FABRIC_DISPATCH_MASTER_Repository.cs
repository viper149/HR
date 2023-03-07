using System;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces.SampleGarments.Fabric;
using DenimERP.ViewModels;
using DenimERP.ViewModels.SampleGarments.Fabric;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures.SampleGarments.Fabric
{
    public class SQLF_SAMPLE_FABRIC_DISPATCH_MASTER_Repository : BaseRepository<F_SAMPLE_FABRIC_DISPATCH_MASTER>, IF_SAMPLE_FABRIC_DISPATCH_MASTER
    {
        private readonly IDataProtector _protector;

        public SQLF_SAMPLE_FABRIC_DISPATCH_MASTER_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<int> GetGetPassNo()
        {
            return await DenimDbContext.F_SAMPLE_FABRIC_DISPATCH_MASTER
                .OrderByDescending(e => e.GPNO)
                .Select(e => e.GPNO)
                .FirstOrDefaultAsync() + 1 ?? 1;
        }

        public async Task<CreateFSampleFabricDispatchMasterViewModel> GetInitObjectsByAsync(CreateFSampleFabricDispatchMasterViewModel createFSampleFabricDispatchMasterViewModel)
        {
            if (createFSampleFabricDispatchMasterViewModel.FSampleFabricDispatchMaster is { DRID: { } })
            {
                createFSampleFabricDispatchMasterViewModel.FBasDriverinfos = await DenimDbContext.F_BAS_DRIVERINFO
                    .Select(e => new F_BAS_DRIVERINFO
                    {
                        DRID = e.DRID,
                        DRIVER_NAME = e.DRIVER_NAME
                    }).OrderBy(e => e.DRIVER_NAME).ToListAsync();
            }

            if (createFSampleFabricDispatchMasterViewModel.FSampleFabricDispatchMaster is { VID: { } })
            {
                createFSampleFabricDispatchMasterViewModel.FBasVehicleInfos = await DenimDbContext.F_BAS_VEHICLE_INFO
                    .Select(e => new F_BAS_VEHICLE_INFO
                    {
                        VID = e.VID,
                        VNUMBER = e.VNUMBER
                    }).OrderBy(e => e.VNUMBER).ToListAsync();
            }

            if (createFSampleFabricDispatchMasterViewModel.FSampleFabricDispatchMaster is { GPTYPEID: { } })
            {
                createFSampleFabricDispatchMasterViewModel.GatepassTypes = await DenimDbContext.GATEPASS_TYPE
                    .Select(e => new GATEPASS_TYPE
                    {
                        GPTYPEID = e.GPTYPEID,
                        GPTYPENAME = e.GPTYPENAME
                    }).OrderBy(e => e.GPTYPENAME).ToListAsync();
            }

            createFSampleFabricDispatchMasterViewModel.GatepassTypes = await DenimDbContext.GATEPASS_TYPE
                .OrderBy(e => e.GPTYPENAME)
                .Where(e => e.GPTYPENAME.ToLower().Contains("non returnable"))
                .ToListAsync();

            createFSampleFabricDispatchMasterViewModel.FBasUnitses = await DenimDbContext.F_BAS_UNITS
                .Select(e => new F_BAS_UNITS
                {
                    UID = e.UID,
                    UNAME = e.UNAME
                }).OrderByDescending(e => e.UNAME.ToLower().Contains("yds")).ThenBy(e => e.UNAME).ToListAsync();

            createFSampleFabricDispatchMasterViewModel.MktTeams = await DenimDbContext.MKT_TEAM
                .Include(e => e.BasTeamInfo)
                .Select(e => new MKT_TEAM
                {
                    MKT_TEAMID = e.MKT_TEAMID,
                    PERSON_NAME = $"{e.PERSON_NAME} ({e.BasTeamInfo.TEAM_NAME})"
                }).OrderBy(e => e.PERSON_NAME).ToListAsync();

            createFSampleFabricDispatchMasterViewModel.BasBuyerinfos = await DenimDbContext.BAS_BUYERINFO.Select(e => new BAS_BUYERINFO
            {
                BUYERID = e.BUYERID,
                BUYER_NAME = e.BUYER_NAME
            }).OrderBy(e => e.BUYER_NAME).ToListAsync();

            createFSampleFabricDispatchMasterViewModel.FSampleFabricDispatchSampleTypes = await DenimDbContext.F_SAMPLE_FABRIC_DISPATCH_SAMPLE_TYPE.Select(e => new F_SAMPLE_FABRIC_DISPATCH_SAMPLE_TYPE
            {
                STYPEID = e.STYPEID,
                STYPE = e.STYPE
            }).OrderBy(e => e.STYPE).ToListAsync();

            createFSampleFabricDispatchMasterViewModel.BasBrandinfos = await DenimDbContext.BAS_BRANDINFO.Select(e => new BAS_BRANDINFO
            {
                BRANDID = e.BRANDID,
                BRANDNAME = e.BRANDNAME
            }).OrderBy(e => e.BRANDNAME).ToListAsync();

            createFSampleFabricDispatchMasterViewModel.FSampleFabricRcvDs = DenimDbContext.F_SAMPLE_FABRIC_RCV_D
                .Include(e => e.SITEM)
                .Include(e => e.FABCODENavigation.WV)
                .Include(e => e.FABCODENavigation.COLORCODENavigation)
                .Include(e => e.SET.PROG_)
                .Include(e => e.FSampleFabricDispatchDetailses)
                .ThenInclude(e => e.F_SAMPLE_FABRIC_DISPATCH_TRANSACTION)
                .OrderBy(e => e.SITEM.NAME)
                //.Where(e => (e.QTY - _denimDbContext.F_SAMPLE_FABRIC_DISPATCH_DETAILS.Include(f => f.FSampleFabricRcvD).Where(f => f.TRNSID.Equals(e.TRNSID)).Sum(g => g.DEL_QTY) > 0))
                .Where(e => (e.QTY - (DenimDbContext.F_SAMPLE_FABRIC_DISPATCH_DETAILS.Include(f => f.FSampleFabricRcvD).Where(f => f.TRNSID.Equals(e.TRNSID)).Sum(g => g.DEL_QTY) ?? 0) > 0))
                .Select(e => new F_SAMPLE_FABRIC_RCV_D
                {
                    TRNSID = e.TRNSID,
                    SITEM = new F_SAMPLE_ITEM_DETAILS
                    {
                        NAME = $"{e.TRNSID} {e.FABCODENavigation.WV.FABCODE}, " +
                               $"{e.ROLLNO}, " +
                               $"{e.FAB_GRADE}, " +
                               $"{e.SET.PROG_.PROG_NO}, " +
                               $"{e.FABCODENavigation.COLORCODENavigation.COLOR}, " +
                               $"{e.SITEM.NAME}, " +
                               $"Balance Qty: {e.QTY - (DenimDbContext.F_SAMPLE_FABRIC_DISPATCH_DETAILS.Include(f => f.FSampleFabricRcvD).Where(f => f.TRNSID.Equals(e.TRNSID)).Sum(g => g.DEL_QTY) ?? 0):N}"
                    }
                }).ToList();

            return createFSampleFabricDispatchMasterViewModel;
        }

        public async Task<CreateFSampleFabricDispatchMasterViewModel> GetInitObjectsForDetailsTableByAsync(CreateFSampleFabricDispatchMasterViewModel createFSampleFabricDispatchMasterViewModel)
        {
            foreach (var item in createFSampleFabricDispatchMasterViewModel.FSampleFabricDispatchDetailses)
            {
                item.FSampleFabricRcvD = await DenimDbContext.F_SAMPLE_FABRIC_RCV_D
                    .Include(e => e.SITEM)
                    .FirstOrDefaultAsync(e => e.TRNSID.Equals(item.TRNSID));

                item.TEAM = await DenimDbContext.MKT_TEAM.FirstOrDefaultAsync(e => e.MKT_TEAMID.Equals(item.MKT_TEAMID));
                item.BUYER = await DenimDbContext.BAS_BUYERINFO.FirstOrDefaultAsync(e => e.BUYERID.Equals(item.BYERID));
                item.STYPE = await DenimDbContext.F_SAMPLE_FABRIC_DISPATCH_SAMPLE_TYPE.FirstOrDefaultAsync(e => e.STYPEID.Equals(item.STYPEID));
                item.BRAND = await DenimDbContext.BAS_BRANDINFO.FirstOrDefaultAsync(e => e.BRANDID.Equals(item.BRANDID));
                item.UNIT = await DenimDbContext.F_BAS_UNITS.FirstOrDefaultAsync(e => e.UID.Equals(item.UID));
            }

            return createFSampleFabricDispatchMasterViewModel;
        }

        public async Task<CreateFSampleFabricDispatchMasterViewModel> FindByIdIncludeAllAsync(int dpId)
        {
            return await DenimDbContext.F_SAMPLE_FABRIC_DISPATCH_MASTER
                .Include(e => e.F_SAMPLE_FABRIC_DISPATCH_DETAILS)
                .ThenInclude(e => e.FSampleFabricRcvD.SITEM)
                .Include(e => e.F_SAMPLE_FABRIC_DISPATCH_DETAILS)
                .ThenInclude(e => e.TEAM.BasTeamInfo)
                .Include(e => e.F_SAMPLE_FABRIC_DISPATCH_DETAILS)
                .ThenInclude(e => e.BUYER)
                .Include(e => e.F_SAMPLE_FABRIC_DISPATCH_DETAILS)
                .ThenInclude(e => e.UNIT)
                .Include(e => e.F_SAMPLE_FABRIC_DISPATCH_DETAILS)
                .ThenInclude(e => e.STYPE)
                .Include(e => e.GPTYPE)
                .Include(e => e.DR)
                .Include(e => e.V)
                .Where(e => e.DPID.Equals(dpId))
                .Select(e => new CreateFSampleFabricDispatchMasterViewModel
                {
                    FSampleFabricDispatchMaster = new F_SAMPLE_FABRIC_DISPATCH_MASTER
                    {
                        DPID = e.DPID,
                        EncryptedId = _protector.Protect(e.DPID.ToString()),
                        GPNO = e.GPNO,
                        GPDATE = e.GPDATE,
                        GPTYPEID = e.GPTYPEID,
                        DRID = e.DRID,
                        VID = e.VID,
                        REMARKS = e.REMARKS,
                        IS_CANCELLED = e.IS_CANCELLED,
                        CAUSE_OF_CANCEL = e.CAUSE_OF_CANCEL,
                        GPTYPE = new GATEPASS_TYPE
                        {
                            GPTYPENAME = $"{e.GPTYPE.GPTYPENAME}"
                        },
                        DR = new F_BAS_DRIVERINFO
                        {
                            DRIVER_NAME = $"{e.DR.DRIVER_NAME}"
                        },
                        V = new F_BAS_VEHICLE_INFO
                        {
                            VNUMBER = $"{e.V.VNUMBER}"
                        }
                    },
                    FSampleFabricDispatchDetailses = e.F_SAMPLE_FABRIC_DISPATCH_DETAILS.Select(f => new F_SAMPLE_FABRIC_DISPATCH_DETAILS
                    {
                        DPDID = f.DPDID,
                        DPID = f.DPID,
                        TRNSID = f.TRNSID,
                        BYERID = f.BYERID,
                        BUYER_REF = f.BUYER_REF,
                        REQ_QTY = f.REQ_QTY,
                        DEL_QTY = f.DEL_QTY,
                        REMARKS = f.REMARKS,
                        ATT_PERSON = f.ATT_PERSON,
                        FSampleFabricRcvD = new F_SAMPLE_FABRIC_RCV_D
                        {
                            SITEM = new F_SAMPLE_ITEM_DETAILS
                            {
                                NAME = $"{f.FSampleFabricRcvD.SITEM.NAME}"
                            }
                        },
                        TEAM = new MKT_TEAM
                        {
                            PERSON_NAME = $"{f.TEAM.PERSON_NAME} ({f.TEAM.BasTeamInfo.TEAM_NAME})"
                        },
                        BUYER = new BAS_BUYERINFO
                        {
                            BUYER_NAME = $"{f.BUYER.BUYER_NAME}"
                        },
                        STYPE = new F_SAMPLE_FABRIC_DISPATCH_SAMPLE_TYPE
                        {
                            STYPE = $"{f.STYPE.STYPE}"
                        },
                        UNIT = new F_BAS_UNITS
                        {
                            UNAME = $"{f.UNIT.UNAME}"
                        }
                    }).ToList()
                }).FirstOrDefaultAsync();
        }

        public async Task<dynamic> GetQtyByAsync(CreateFSampleFabricDispatchMasterViewModel createFSampleFabricDispatchMasterView)
        {
            return await DenimDbContext.F_SAMPLE_FABRIC_RCV_D
                .Where(e => e.TRNSID.Equals(createFSampleFabricDispatchMasterView.FSampleFabricDispatchDetails.TRNSID))
                .Select(e => new
                {
                    QTY = e.QTY
                }).FirstOrDefaultAsync();
        }

        public async Task<DataTableObject<F_SAMPLE_FABRIC_DISPATCH_MASTER>> GetForDataTableByAsync(string sortColumn, string sortColumnDirection, string searchValue, string draw, int skip,
            int pageSize)
        {
            try
            {
                var navigationPropertyStrings = new[] { "" };

                var fSampleFabricDispatchMasters = DenimDbContext.F_SAMPLE_FABRIC_DISPATCH_MASTER
                    .Select(e => new F_SAMPLE_FABRIC_DISPATCH_MASTER
                    {
                        GPNO = e.GPNO,
                        GPDATE = e.GPDATE,
                        EncryptedId = _protector.Protect(e.DPID.ToString()),
                        IS_CANCELLED = e.IS_CANCELLED,
                        REMARKS = e.REMARKS
                    });

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    fSampleFabricDispatchMasters = OrderedResult<F_SAMPLE_FABRIC_DISPATCH_MASTER>.GetOrderedResult(sortColumnDirection, sortColumn, navigationPropertyStrings, fSampleFabricDispatchMasters);
                }

                if (!string.IsNullOrEmpty(searchValue))
                {
                    fSampleFabricDispatchMasters = fSampleFabricDispatchMasters
                        .Where(m => m.GPNO != null && m.GPNO.ToString().Contains(searchValue)
                                    || m.GPDATE != null && m.GPDATE.ToString().ToUpper().Contains(searchValue)
                                    || m.IS_CANCELLED.ToString().ToUpper().Contains(searchValue)
                                    || m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue));

                    fSampleFabricDispatchMasters = OrderedResult<F_SAMPLE_FABRIC_DISPATCH_MASTER>.GetOrderedResult(sortColumnDirection, sortColumn, navigationPropertyStrings, fSampleFabricDispatchMasters);
                }

                var recordsTotal = await fSampleFabricDispatchMasters.CountAsync();

                return new DataTableObject<F_SAMPLE_FABRIC_DISPATCH_MASTER>(draw, recordsTotal, recordsTotal, await fSampleFabricDispatchMasters.Skip(skip).Take(pageSize).ToListAsync());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<CreateFSampleFabricDispatchMasterViewModel> GetGatePassFor(string search, int page = 1)
        {
            var createFSampleFabricDispatchMasterViewModel = new CreateFSampleFabricDispatchMasterViewModel();

            if (!string.IsNullOrEmpty(search))
            {
                createFSampleFabricDispatchMasterViewModel.FSampleDespatchMasterTypes = await DenimDbContext.F_SAMPLE_DESPATCH_MASTER_TYPE
                    .OrderBy(e => e.TYPENAME)
                    .Select(e => new F_SAMPLE_DESPATCH_MASTER_TYPE
                    {
                        TYPEID = e.TYPEID,
                        TYPENAME = e.TYPENAME
                    }).Where(e => e.TYPENAME.ToLower().Contains(search.ToLower())).ToListAsync();
            }
            else
            {
                createFSampleFabricDispatchMasterViewModel.FSampleDespatchMasterTypes = await DenimDbContext.F_SAMPLE_DESPATCH_MASTER_TYPE
                    .OrderBy(e => e.TYPENAME)
                    .Select(e => new F_SAMPLE_DESPATCH_MASTER_TYPE
                    {
                        TYPEID = e.TYPEID,
                        TYPENAME = e.TYPENAME
                    }).ToListAsync();
            }

            return createFSampleFabricDispatchMasterViewModel;
        }

        public async Task<CreateFSampleFabricDispatchMasterViewModel> GetGatePassType(string search, int page = 1)
        {
            var createFSampleFabricDispatchMasterViewModel = new CreateFSampleFabricDispatchMasterViewModel();

            if (!string.IsNullOrEmpty(search))
            {
                createFSampleFabricDispatchMasterViewModel.GatepassTypes = await DenimDbContext.GATEPASS_TYPE
                    .OrderBy(e => e.GPTYPENAME)
                    .Select(e => new GATEPASS_TYPE
                    {
                        GPTYPEID = e.GPTYPEID,
                        GPTYPENAME = e.GPTYPENAME
                    }).Where(e => e.GPTYPENAME.ToLower().Contains(search.ToLower())).ToListAsync();
            }
            else
            {
                createFSampleFabricDispatchMasterViewModel.GatepassTypes = await DenimDbContext.GATEPASS_TYPE
                    .OrderBy(e => e.GPTYPENAME)
                    .Select(e => new GATEPASS_TYPE
                    {
                        GPTYPEID = e.GPTYPEID,
                        GPTYPENAME = e.GPTYPENAME
                    }).ToListAsync();
            }

            return createFSampleFabricDispatchMasterViewModel;
        }

        public async Task<CreateFSampleFabricDispatchMasterViewModel> GetDriverInfo(string search, int page = 1)
        {
            var createFSampleFabricDispatchMasterViewModel = new CreateFSampleFabricDispatchMasterViewModel();

            if (!string.IsNullOrEmpty(search))
            {
                createFSampleFabricDispatchMasterViewModel.FBasDriverinfos = await DenimDbContext.F_BAS_DRIVERINFO
                    .OrderBy(e => e.DRIVER_NAME)
                    .Select(e => new F_BAS_DRIVERINFO
                    {
                        DRID = e.DRID,
                        DRIVER_NAME = e.DRIVER_NAME
                    }).Where(e => e.DRIVER_NAME.ToLower().Contains(search.ToLower())).ToListAsync();
            }
            else
            {
                createFSampleFabricDispatchMasterViewModel.FBasDriverinfos = await DenimDbContext.F_BAS_DRIVERINFO
                    .OrderBy(e => e.DRIVER_NAME)
                    .Select(e => new F_BAS_DRIVERINFO
                    {
                        DRID = e.DRID,
                        DRIVER_NAME = e.DRIVER_NAME
                    }).ToListAsync();
            }

            return createFSampleFabricDispatchMasterViewModel;
        }

        public async Task<CreateFSampleFabricDispatchMasterViewModel> GetVehicleInfo(string search, int page = 1)
        {
            var createFSampleFabricDispatchMasterViewModel = new CreateFSampleFabricDispatchMasterViewModel();

            if (!string.IsNullOrEmpty(search))
            {
                createFSampleFabricDispatchMasterViewModel.FBasVehicleInfos = await DenimDbContext.F_BAS_VEHICLE_INFO
                    .OrderBy(e => e.VNUMBER)
                    .Select(e => new F_BAS_VEHICLE_INFO
                    {
                        VID = e.VID,
                        VNUMBER = e.VNUMBER
                    }).Where(e => e.VNUMBER.ToLower().Contains(search.ToLower())).ToListAsync();
            }
            else
            {
                createFSampleFabricDispatchMasterViewModel.FBasVehicleInfos = await DenimDbContext.F_BAS_VEHICLE_INFO
                    .OrderBy(e => e.VNUMBER)
                    .Select(e => new F_BAS_VEHICLE_INFO
                    {
                        VID = e.VID,
                        VNUMBER = e.VNUMBER
                    }).ToListAsync();
            }

            return createFSampleFabricDispatchMasterViewModel;
        }

        public async Task<F_SAMPLE_FABRIC_DISPATCH_MASTER> FindByIdForDeleteAsync(int dpId)
        {
            try
            {
                var result =  await DenimDbContext.F_SAMPLE_FABRIC_DISPATCH_MASTER
                    .Include(e => e.F_SAMPLE_FABRIC_DISPATCH_DETAILS)
                    .ThenInclude(e => e.F_SAMPLE_FABRIC_DISPATCH_TRANSACTION)
                    .Where(e => e.DPID.Equals(dpId))
                    .FirstOrDefaultAsync();
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
           
        }
    }
}
