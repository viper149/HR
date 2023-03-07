using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces.SampleGarments.Fabric;
using DenimERP.ViewModels;
using DenimERP.ViewModels.SampleFabric;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures.SampleGarments.Fabric
{
    public class SQLH_SAMPLE_FABRIC_DISPATCH_MASTER_Repository : BaseRepository<H_SAMPLE_FABRIC_DISPATCH_MASTER>, IH_SAMPLE_FABRIC_DISPATCH_MASTER
    {
        private readonly IDataProtector _protector;

        public SQLH_SAMPLE_FABRIC_DISPATCH_MASTER_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<dynamic> GetMerchandisersByAsync(string search, int page)
        {
            if (!string.IsNullOrEmpty(search))
            {
                return await DenimDbContext.MERCHANDISER
                    .OrderBy(e => e.MERCHANDISER_NAME)
                    .Select(e => new MERCHANDISER
                    {
                        MERCID = e.MERCID,
                        MERCHANDISER_NAME = string.Join("; ", e.MERCHANDISER_NAME, e.DESIGNATION, e.PHONE_NUMBER, e.ADDRESS)
                    }).Where(e => e.MERCHANDISER_NAME.ToLower().Contains(search.ToLower())).ToListAsync();
            }
            else
            {
                return await DenimDbContext.MERCHANDISER
                    .OrderBy(e => e.MERCHANDISER_NAME)
                    .Select(e => new MERCHANDISER
                    {
                        MERCID = e.MERCID,
                        MERCHANDISER_NAME = string.Join("; ", e.MERCHANDISER_NAME, e.DESIGNATION, e.PHONE_NUMBER, e.ADDRESS)
                    }).ToListAsync();
            }
        }

        public async Task<dynamic> GetAvailableItemsByAsync(string search, int page)
        {
            if (!string.IsNullOrEmpty(search))
            {
                return await DenimDbContext.H_SAMPLE_FABRIC_RECEIVING_D
                    .Include(e => e.H_SAMPLE_FABRIC_DISPATCH_DETAILS)
                    .Where(e => e.QTY - (e.H_SAMPLE_FABRIC_DISPATCH_DETAILS.Sum(g => g.DEL_QTY) ?? 0) > 0)
                    .OrderBy(e => e.DPD.FSampleFabricRcvD.SITEM)
                    .Select(e => new H_SAMPLE_FABRIC_RECEIVING_D
                    {
                        RCVDID = e.RCVDID,
                        DPD = new F_SAMPLE_FABRIC_DISPATCH_DETAILS
                        {
                            FSampleFabricRcvD = new F_SAMPLE_FABRIC_RCV_D
                            {
                                SITEM = new F_SAMPLE_ITEM_DETAILS
                                {
                                    NAME = $"{e.DPD.FSampleFabricRcvD.SITEM.NAME}"
                                }
                            }
                        }
                    }).Where(e => e.DPD.FSampleFabricRcvD.SITEM.NAME.ToLower().Contains(search.ToLower())).ToListAsync();
            }
            else
            {
                return await DenimDbContext.H_SAMPLE_FABRIC_RECEIVING_D
                    .Include(e => e.H_SAMPLE_FABRIC_DISPATCH_DETAILS)
                    .Where(e => e.QTY - (e.H_SAMPLE_FABRIC_DISPATCH_DETAILS.Sum(g => g.DEL_QTY) ?? 0) > 0)
                    .OrderBy(e => e.DPD.FSampleFabricRcvD.SITEM)
                    .Select(e => new H_SAMPLE_FABRIC_RECEIVING_D
                    {
                        RCVDID = e.RCVDID,
                        DPD = new F_SAMPLE_FABRIC_DISPATCH_DETAILS
                        {
                            FSampleFabricRcvD = new F_SAMPLE_FABRIC_RCV_D
                            {
                                SITEM = new F_SAMPLE_ITEM_DETAILS
                                {
                                    NAME = $"{e.DPD.FSampleFabricRcvD.SITEM.NAME}; Remaining Qty: {e.QTY - (e.H_SAMPLE_FABRIC_DISPATCH_DETAILS.Sum(g => g.DEL_QTY) ?? 0)}"
                                }
                            }
                        }
                    }).ToListAsync();
            }
        }

        public async Task<string> GetGatePassNoByAsync()
        {
            return $"{await DenimDbContext.H_SAMPLE_FABRIC_DISPATCH_MASTER.CountAsync() + 1}".PadLeft(6, '0');
        }

        public async Task<dynamic> GetBuyersByAsync(string search, int page)
        {
            if (!string.IsNullOrEmpty(search))
            {
                return await DenimDbContext.BAS_BUYERINFO
                    .OrderBy(e => e.BUYER_NAME)
                    .Select(e => new BAS_BUYERINFO
                    {
                        BUYERID = e.BUYERID,
                        BUYER_NAME = e.BUYER_NAME
                    }).Where(e => e.BUYER_NAME.ToLower().Contains(search.ToLower())).ToListAsync();
            }
            else
            {
                return await DenimDbContext.BAS_BUYERINFO
                    .OrderBy(e => e.BUYER_NAME)
                    .Select(e => new BAS_BUYERINFO
                    {
                        BUYERID = e.BUYERID,
                        BUYER_NAME = e.BUYER_NAME
                    }).ToListAsync();
            }
        }

        public async Task<dynamic> GetBrandsByAsync(string search, int page)
        {
            if (!string.IsNullOrEmpty(search))
            {
                return await DenimDbContext.BAS_BRANDINFO
                    .OrderBy(e => e.BRANDNAME)
                    .Select(e => new BAS_BRANDINFO
                    {
                        BRANDID = e.BRANDID,
                        BRANDNAME = e.BRANDNAME
                    }).Where(e => e.BRANDNAME.ToLower().Contains(search.ToLower())).ToListAsync();
            }
            else
            {
                return await DenimDbContext.BAS_BRANDINFO
                    .OrderBy(e => e.BRANDNAME)
                    .Select(e => new BAS_BRANDINFO
                    {
                        BRANDID = e.BRANDID,
                        BRANDNAME = e.BRANDNAME
                    }).ToListAsync();
            }
        }

        public async Task<DataTableObject<H_SAMPLE_FABRIC_DISPATCH_MASTER>> GetForDataTableByAsync(string sortColumn, string sortColumnDirection, string searchValue, string draw, int skip,
            int pageSize)
        {
            var navigationPropertyStrings = new[] { "" };

            var hSampleFabricDispatchMasters = DenimDbContext.H_SAMPLE_FABRIC_DISPATCH_MASTER
                .Select(e => new H_SAMPLE_FABRIC_DISPATCH_MASTER
                {
                    SFDID = e.SFDID,
                    GPNO = e.GPNO,
                    EncryptedId = _protector.Protect(e.SFDID.ToString()),
                    ISSUE_DATE = e.ISSUE_DATE,
                    ISRETURNABLE = e.ISRETURNABLE
                }).AsQueryable();

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                hSampleFabricDispatchMasters = OrderedResult<H_SAMPLE_FABRIC_DISPATCH_MASTER>.GetOrderedResult(sortColumnDirection, sortColumn, navigationPropertyStrings, hSampleFabricDispatchMasters);
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                hSampleFabricDispatchMasters = hSampleFabricDispatchMasters
                    .Where(m => m.SFDID.ToString().ToUpper().Contains(searchValue)
                                || !string.IsNullOrEmpty(m.GPNO) && m.GPNO.ToUpper().Contains(searchValue)
                                || m.ISSUE_DATE != null && m.ISSUE_DATE.ToString().ToUpper().Contains(searchValue)
                                || m.ISRETURNABLE.ToString().ToUpper().Contains(searchValue));

                hSampleFabricDispatchMasters = OrderedResult<H_SAMPLE_FABRIC_DISPATCH_MASTER>.GetOrderedResult(sortColumnDirection, sortColumn, navigationPropertyStrings, hSampleFabricDispatchMasters);
            }

            var recordsTotal = await hSampleFabricDispatchMasters.CountAsync();

            return new DataTableObject<H_SAMPLE_FABRIC_DISPATCH_MASTER>(draw, recordsTotal, recordsTotal, await hSampleFabricDispatchMasters.Skip(skip).Take(pageSize).ToListAsync());
        }

        public async Task<dynamic> GetOtherInfoByAsync(CreateHSampleFabricDispatchMaster createHSampleFabricDispatchMaster)
        {
            return await DenimDbContext.H_SAMPLE_FABRIC_RECEIVING_D
                .Include(e => e.H_SAMPLE_FABRIC_DISPATCH_DETAILS)
                .Where(e => e.RCVDID.Equals(createHSampleFabricDispatchMaster.HSampleFabricDispatchDetails.RCVDID))
                .Select(e => new H_SAMPLE_FABRIC_RECEIVING_D
                {
                    QTY = e.QTY - (e.H_SAMPLE_FABRIC_DISPATCH_DETAILS.Sum(f => f.DEL_QTY) ?? 0),
                    REMARKS = e.REMARKS
                }).FirstOrDefaultAsync();
        }

        public async Task<H_SAMPLE_FABRIC_DISPATCH_MASTER> GetForSafeDeleteByAsync(int sfdId)
        {
            return await DenimDbContext.H_SAMPLE_FABRIC_DISPATCH_MASTER.Include(e => e.H_SAMPLE_FABRIC_DISPATCH_DETAILS)
                .FirstOrDefaultAsync(e => e.SFDID.Equals(sfdId));
        }

        public async Task<CreateHSampleFabricDispatchMaster> FindByIdIncludeAllAsync(int sfdId)
        {
            return await DenimDbContext.H_SAMPLE_FABRIC_DISPATCH_MASTER
                .Include(e => e.H_SAMPLE_FABRIC_DISPATCH_DETAILS)
                .Where(e => e.SFDID.Equals(sfdId))
                .Select(e => new CreateHSampleFabricDispatchMaster
                {
                    HSampleFabricDispatchMaster = new H_SAMPLE_FABRIC_DISPATCH_MASTER
                    {
                        SFDID = e.SFDID,
                        EncryptedId = _protector.Protect(e.SFDID.ToString()),
                        GPNO = e.GPNO,
                        ISSUE_DATE = e.ISSUE_DATE,
                        BUYERID = e.BUYERID,
                        BRANDID = e.BRANDID,
                        MERCID = e.MERCID,
                        PURPOSE = e.PURPOSE,
                        ISRETURNABLE = e.ISRETURNABLE,
                        RETURN_DATE = e.RETURN_DATE,
                        MKT_TEAMID = e.MKT_TEAMID,
                        THROUGH = e.THROUGH
                    },
                    HSampleFabricDispatchDetailses = e.H_SAMPLE_FABRIC_DISPATCH_DETAILS.Select(f => new H_SAMPLE_FABRIC_DISPATCH_DETAILS
                    {
                        SFDDID = f.SFDDID,
                        SFDID = f.SFDID,
                        BARCODE = f.BARCODE,
                        RCVDID = f.RCVDID,
                        DEL_QTY = f.DEL_QTY,
                        CSPRICE = f.CSPRICE,
                        NEGO_PRICE = f.NEGO_PRICE,
                        REMARKS = f.REMARKS
                    }).ToList()
                }).FirstOrDefaultAsync();
        }

        public async Task<CreateHSampleFabricDispatchMaster> GetInitObjByAsync(CreateHSampleFabricDispatchMaster createHSampleFabricDispatchMaster)
        {
            if (createHSampleFabricDispatchMaster.HSampleFabricDispatchMaster is { BUYERID: { } })
            {
                createHSampleFabricDispatchMaster.BasBuyerinfos = await DenimDbContext.BAS_BUYERINFO
                    .Where(e => e.BUYERID.Equals(createHSampleFabricDispatchMaster.HSampleFabricDispatchMaster.BUYERID))
                    .ToListAsync();
            }

            if (createHSampleFabricDispatchMaster.HSampleFabricDispatchMaster is { BRANDID: { } })
            {
                createHSampleFabricDispatchMaster.BasBrandinfos = await DenimDbContext.BAS_BRANDINFO
                    .Where(e => e.BRANDID.Equals(createHSampleFabricDispatchMaster.HSampleFabricDispatchMaster.BRANDID))
                    .ToListAsync();
            }

            if (createHSampleFabricDispatchMaster.HSampleFabricDispatchMaster is { MERCID: { } })
            {
                createHSampleFabricDispatchMaster.Merchandisers = await DenimDbContext.MERCHANDISER
                    .Where(e => e.MERCID.Equals(createHSampleFabricDispatchMaster.HSampleFabricDispatchMaster.MERCID))
                    .ToListAsync();
            }

            return createHSampleFabricDispatchMaster;
        }

        public async Task<CreateHSampleFabricDispatchMaster> GetInitObjForDetailsTableByAsync(CreateHSampleFabricDispatchMaster createHSampleFabricDispatchMaster)
        {
            foreach (var item in createHSampleFabricDispatchMaster.HSampleFabricDispatchDetailses)
            {
                item.RCVD = await DenimDbContext.H_SAMPLE_FABRIC_RECEIVING_D
                    .Include(e => e.DPD.FSampleFabricRcvD.SITEM)
                    .FirstOrDefaultAsync(e => e.RCVDID.Equals(item.RCVDID));
            }

            return createHSampleFabricDispatchMaster;
        }
    }
}
