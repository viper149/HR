using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLCOM_EX_GSPINFO_Repository : BaseRepository<COM_EX_GSPINFO>, ICOM_EX_GSPINFO
    {
        private readonly IDataProtector _protector;

        public SQLCOM_EX_GSPINFO_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<IEnumerable<COM_EX_GSPINFO>> GetGspInfoWithPaged(int pageNumber, int pageSize)
        {
            try
            {
                var excludeResult = (pageSize * pageNumber) - pageSize;

                return await DenimDbContext.COM_EX_GSPINFO
                    .OrderByDescending(d => d.GSPID)
                    .Skip(excludeResult)
                    .Take(pageSize)
                    .ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<COM_EX_GSPINFO>> GetAllForDataTableByAsync()
        {

            return await DenimDbContext.COM_EX_GSPINFO
                .Include(d=>d.INV.BUYER)
                .Select(e => new COM_EX_GSPINFO
                {
                    GSPID = e.GSPID,
                    EncryptedId = _protector.Protect(e.GSPID.ToString()),
                    GSPNO = e.GSPNO,
                    EXPLCNO = e.EXPLCNO,
                    EXPLCDATE = e.EXPLCDATE,
                    EXPITEMS = e.EXPITEMS,
                    SUBDATE = e.SUBDATE,
                    RCVDDATE = e.RCVDDATE,
                    REMARKS = e.REMARKS,

                    INV = new COM_EX_INVOICEMASTER()
                    {
                        INVNO = e.INV.INVNO,
                        INVID = e.INV.INVID,
                        EncryptedInvId = _protector.Protect(e.INV.INVID.ToString()),
                        BUYER = new BAS_BUYERINFO
                        {
                            BUYER_NAME = e.INV.BUYER.BUYER_NAME
                        },
                        LC = new COM_EX_LCINFO
                        {
                            FILENO = e.INV.LC.FILENO
                        }
                        
                    }

                }).OrderByDescending(x=>x.GSPID).ToListAsync();
        }

        public async Task<dynamic> GetForGSPInformationByAsync(int invId)
        {
            try
            {
                var result =  await DenimDbContext.COM_EX_INVOICEMASTER
                    .Include(e => e.LC.COM_EX_LCDETAILS)
                    .ThenInclude(d => d.PI)
                    .ThenInclude(d => d.COM_EX_PI_DETAILS)
                    .ThenInclude(d => d.STYLE.FABCODENavigation.WGDEC)
                    .Include(e => e.LC.COM_EX_LCDETAILS)
                    .ThenInclude(d => d.PI.COM_EX_PI_DETAILS)
                    .ThenInclude(d => d.STYLE.FABCODENavigation.COLORCODENavigation)
                    .Include(e => e.LC.COM_EX_LCDETAILS)
                    .ThenInclude(d => d.PI.COM_EX_PI_DETAILS)
                    .ThenInclude(d => d.F_BAS_UNITS)
                    .Where(e => e.INVID.Equals(invId))
                    .Select(e => new
                    {
                        Invdate = $"{e.INVDATE}",
                        EXPLCNO = $"{e.LC.MLCNO}",
                        EXPLCDATE = $"{e.LC.MLCDATE}",
                        LCDATE = e.LC.LCDATE,
                        AMTNO = e.LC.AMENTNO,
                        AMTDATE = e.LC.AMENTDATE,


                        //LCAMDDATE = $"{.}, AMD : {e.LC.AMENTNO}, DT. {e.LC.AMENTDATE}",
                        EXPITEMS = $"{e.LC.GARMENT_QTY +" PCS, "+ e.LC.ITEM}",
                        VCNO = $"{e.LC.VAT_REG}",
                        VCDATE = $"{e.LC.EX_DATE}",
                        FABDESCRIPTION = $"DENIM FABRICS\n" +
                                         $"WEIGHT : {string.Join(", ", e.ComExInvdetailses.Select(g => g.ComExFabstyle.FABCODENavigation.WGDEC).Distinct())} OZ/YD\n" +
                                         $"WIDTH : {string.Join(", ", e.ComExInvdetailses.Select(g => g.ComExFabstyle.FABCODENavigation.WIDEC).Distinct())}\n" +
                                         $"COLOR : {string.Join(", ", e.ComExInvdetailses.Select(g => g.ComExFabstyle.FABCODENavigation.COLORCODENavigation.COLOR).Distinct())}",

                        //FABDESCRIPTION = $"DENIM FABRICS\n" +
                        //                 $"WEIGHT : {string.Join(", ", e.LC.COM_EX_LCDETAILS.SelectMany(f => f.PI.COM_EX_PI_DETAILS.Select(g => g.STYLE.FABCODENavigation.WGDEC)).Distinct())} OZ/YD\n" +
                        //                 $"WIDTH : {string.Join(", ", e.LC.COM_EX_LCDETAILS.SelectMany(f => f.PI.COM_EX_PI_DETAILS.Select(g => g.STYLE.FABCODENavigation.WIDEC)).Distinct())}\n" +
                        //                 $"COLOR : {string.Join(", ", e.LC.COM_EX_LCDETAILS.SelectMany(f => f.PI.COM_EX_PI_DETAILS.Select(g => g.STYLE.FABCODENavigation.COLORCODENavigation.COLOR)).Distinct())}",
                        //ITEMQTY_YDS = e.LC.COM_EX_LCDETAILS.Sum(f => f.PI.COM_EX_PI_DETAILS.Sum(g => g.F_BAS_UNITS != null && g.UNIT == 7 ? g.QTY : (g.QTY * 1.09361))),
                        
                        //ITEMQTY_MTS = e.LC.COM_EX_LCDETAILS.Sum(f => f.PI.COM_EX_PI_DETAILS.Sum(g => g.F_BAS_UNITS != null && g.UNIT == 6 ? g.QTY : g.QTY * 0.9144)),
                        ITEMQTY_YDS = e.LC.COM_EX_LCDETAILS.Select(f => f.PI.COM_EX_PI_DETAILS.Select(g => g.F_BAS_UNITS != null && g.UNIT == 7 ? e.INV_QTY  : e.INV_QTY * 1.09361).FirstOrDefault()).FirstOrDefault(),

                        ITEMQTY_MTS = e.LC.COM_EX_LCDETAILS.Select(f => f.PI.COM_EX_PI_DETAILS.Select(g => g.F_BAS_UNITS != null && g.UNIT == 6 ? e.INV_QTY : e.INV_QTY * 0.9144).FirstOrDefault()).FirstOrDefault(),
                        //NetWeight = e.LC.COM_EX_LCDETAILS.Select(f => f.PI.NET_WEIGHT).ToList()

                    }).FirstOrDefaultAsync();
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<COM_EX_GSPINFO>> GetGSPNo(string GSPNO)
        {
            return DenimDbContext.COM_EX_GSPINFO.Where(x => x.GSPNO.Equals(GSPNO)).ToList();
        }
    }
}
