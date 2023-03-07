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
    public class SQLCOM_EX_FABSTYLE_Repository : BaseRepository<COM_EX_FABSTYLE>, ICOM_EX_FABSTYLE
    {
        private readonly IDataProtector _protector;

        public SQLCOM_EX_FABSTYLE_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<ComExFabStyleViewModel> GetFabricInfoAsync(int fabricCode)
        {
            try
            {
                var fabricInfo = await DenimDbContext.RND_FABRICINFO
                    .AsNoTracking()
                    .Include(cn => cn.COLORCODENavigation)
                    .Include(bi => bi.BUYER)
                    .Include(c => c.RND_FINISHTYPE)
                    .Include(c => c.RND_WEAVE)
                    .Include(c => c.LOOM)
                    .Where(e => e.FABCODE.Equals(fabricCode))
                    .FirstOrDefaultAsync();

                var fabricCountInfo = await DenimDbContext.RND_FABRIC_COUNTINFO
                    .AsNoTracking()
                    .Include(yci => yci.COUNT)
                    .Include(c => c.Color)
                    .Include(c => c.YarnFor)
                    .Where(c => c.FABCODE == fabricCode && !c.COUNT.YARN_CAT_ID.Equals(8102699))
                    .ToListAsync();

                foreach (var x in fabricCountInfo)
                {
                    x.COUNT.RND_FABRIC_COUNTINFO = null;
                }

                var finalResult = new ComExFabStyleViewModel()
                {
                    rND_FABRICINFO = fabricInfo,
                    rND_FABRIC_COUNTINFOs = fabricCountInfo
                };


                finalResult.WarpCount = string.Join(" + ",
                    finalResult.rND_FABRIC_COUNTINFOs.Where(c => c.YARNFOR.Equals(1))
                        .Select(p => p.COUNT.RND_COUNTNAME));
                finalResult.WeftCount = string.Join(" + ",
                    finalResult.rND_FABRIC_COUNTINFOs.Where(c => c.YARNFOR.Equals(2))
                        .Select(p => p.COUNT.RND_COUNTNAME));
                finalResult.Construction =
                    $"{finalResult.WarpCount} X {finalResult.WeftCount} / {finalResult.rND_FABRICINFO.FNEPI}X{finalResult.rND_FABRICINFO.FNPPI}";

                return finalResult;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<ComExFabStyleViewModel> GetInitObjects(ComExFabStyleViewModel comExFabStyleViewModel)
        {
            comExFabStyleViewModel.bAS_BRANDINFOs = await DenimDbContext.BAS_BRANDINFO
                .OrderBy(e => e.BRANDNAME)
                .ToListAsync();

            comExFabStyleViewModel.rND_FABRICINFOs = await DenimDbContext.RND_FABRICINFO
                .Include(c => c.WV)
                .Select(c => new RND_FABRICINFO
                {
                    FABCODE = c.FABCODE,
                    WV = new RND_SAMPLE_INFO_WEAVING
                    {
                        FABCODE = c.WV != null ? c.WV.FABCODE : ""
                    },
                    STYLE_NAME = c.STYLE_NAME
                }).OrderBy(e => e.WV.FABCODE).ToListAsync();

            return comExFabStyleViewModel;
        }

        public async Task<double> GetGetInvBalanceAsync(int trnsId, int lcId)
        {
            var result = await DenimDbContext.COM_EX_INVDETAILS
                .Include(d => d.InvoiceMaster)
                .Where(d => !d.IS_OLD && d.InvoiceMaster.LCID.Equals(lcId) && d.PIIDD_TRNSID.Equals(trnsId))
                .Select(d => new COM_EX_INVDETAILS
                {
                    PREV_QTY = (double?)d.QTY ?? 0
                }).ToListAsync();

            double sum = 0;
            if (result != null)
            {
                foreach (var i in result)
                {
                    sum += i.PREV_QTY;
                }
            }
            return sum;
        }

        public async Task<COM_EX_FABSTYLE> GetComExFabricInfoAsync(int styleId)
        {
            try
            {
                var fabricStyleInfo = await DenimDbContext.COM_EX_FABSTYLE
                    .Include(fs => fs.BRAND)
                    .Include(c => c.FABCODENavigation)
                    .Where(fs => fs.STYLEID == styleId).FirstOrDefaultAsync();

                return fabricStyleInfo;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<COM_EX_PI_DETAILS> GetComExFabricInfoAsync(int trnsId, int lcId)
        {
            return await DenimDbContext.COM_EX_PI_DETAILS
                .Include(e => e.PIMASTER.COM_EX_LCDETAILS)
                .Include(e => e.COM_EX_INVDETAILS)
                .Where(e => e.TRNSID.Equals(trnsId) && e.PIMASTER.COM_EX_LCDETAILS.Any(f => f.LCID.Equals(lcId)))
                .Select(e => new COM_EX_PI_DETAILS
                {
                    QTY = e.QTY,
                    UNITPRICE = e.UNITPRICE
                }).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<COM_EX_PIMASTER>> FindPIListByStyleIdAsync(int styleId)
        {
            try
            {
                var pIInfo = await DenimDbContext.COM_EX_PI_DETAILS
                    .Where(fs => fs.STYLEID == styleId)
                    .ToListAsync();

                var comExPimasters = new List<COM_EX_PIMASTER>();

                foreach (var item in pIInfo)
                {
                    comExPimasters.Add(new COM_EX_PIMASTER
                    {
                        PIID = await DenimDbContext.COM_EX_PIMASTER.Where(c => c.PINO == item.PINO).Select(c => c.PIID).FirstOrDefaultAsync(),
                        PINO = item.PINO
                    });
                }
                return comExPimasters;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<COM_EX_FABSTYLE> FindByIdForStyleNameAsync(int styleId)
        {
            return await DenimDbContext.COM_EX_FABSTYLE
                .FirstOrDefaultAsync(fs => fs.STYLEID.Equals(styleId));
        }

        public async Task<DataTableObject<COM_EX_FABSTYLE>> GetForDataTableByAsync(string sortColumn, string sortColumnDirection, string searchValue, string draw, int skip, int pageSize)
        {
            try
            {
                var navigationPropertyStrings = new[] { "BRAND" };

                var comExFabstyles = DenimDbContext.COM_EX_FABSTYLE
                    .Include(fs => fs.BRAND)
                    .Include(c => c.FABCODENavigation)
                    .ThenInclude(c => c.WV)
                    .Select(e => new COM_EX_FABSTYLE
                    {
                        EncryptedId = _protector.Protect(e.STYLEID.ToString()),
                        STYLENAME = e.STYLENAME,
                        BRAND = e.BRAND,
                        FABCODE = e.FABCODE,
                        HSCODE = e.HSCODE,
                        STATUS = e.STATUS,
                        //Option 1 is for show Fabric Style
                        OPTION2 = e.FABCODENavigation.STYLE_NAME
                        //FABCODENavigation = new RND_FABRICINFO
                        //{
                        //    WV = new RND_SAMPLE_INFO_WEAVING
                        //    {
                        //        FABCODE = e.FABCODENavigation.WV.FABCODE
                        //    }, 
                        // STYLE_NAME = e.STYLE_NAME
                        //}
                        ,
                        CREATED_AT = e.CREATED_AT
                    });

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    comExFabstyles = OrderedResult<COM_EX_FABSTYLE>.GetOrderedResult(sortColumnDirection, sortColumn, navigationPropertyStrings, comExFabstyles);
                }

                if (!string.IsNullOrEmpty(searchValue))
                {
                    comExFabstyles = comExFabstyles
                        .Where(m => m.STYLENAME != null && m.STYLENAME.ToUpper().Contains(searchValue)
                                    || m.BRAND.BRANDNAME != null && m.BRAND.BRANDNAME.ToUpper().Contains(searchValue)
                                    || m.STATUS != null && m.STATUS.ToUpper().Contains(searchValue)
                                    || m.OPTION2 != null && m.OPTION2.ToUpper().Contains(searchValue)
                                    || m.HSCODE != null && m.HSCODE.ToUpper().Contains(searchValue)
                                    || m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue)
                    );

                    comExFabstyles = OrderedResult<COM_EX_FABSTYLE>.GetOrderedResult(sortColumnDirection, sortColumn, navigationPropertyStrings, comExFabstyles);
                }

                var recordsTotal = await comExFabstyles.CountAsync();

                return new DataTableObject<COM_EX_FABSTYLE>(draw, recordsTotal, recordsTotal, await comExFabstyles.Skip(skip).Take(pageSize).ToListAsync()); ;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public async Task<bool> DeleteFabStyle(int id)
        {
            try
            {
                var isFound = await DenimDbContext.COM_EX_FABSTYLE.FindAsync(id);
                if (isFound != null)
                {
                    var fabStyle = await DenimDbContext.COM_EX_FABSTYLE.Where(pi => pi.STYLEID == isFound.STYLEID).ToListAsync();
                    if (fabStyle.Any())
                    {
                        DenimDbContext.COM_EX_FABSTYLE.RemoveRange(fabStyle);
                    }
                    DenimDbContext.COM_EX_FABSTYLE.Remove(isFound);
                    await SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new System.InvalidOperationException(ex.Message);
            }
        }
    }
}