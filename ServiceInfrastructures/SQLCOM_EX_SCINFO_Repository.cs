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
    public class SQLCOM_EX_SCINFO_Repository : BaseRepository<COM_EX_SCINFO>, ICOM_EX_SCINFO
    {
        private readonly IDataProtector _protector;

        public SQLCOM_EX_SCINFO_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<IEnumerable<COM_EX_SCINFO>> GetComExScInfoAllAsync()
        {
            try
            {
                var result = await DenimDbContext.COM_EX_SCINFO
                    .Select(e => new COM_EX_SCINFO
                    {
                        SCID = e.SCID,
                        EncryptedId = _protector.Protect(e.SCID.ToString()),
                        SCNO = e.SCNO,
                        SCDATE = e.SCDATE,
                        SCPERSON = e.SCPERSON,
                        BCPERSON = e.BCPERSON,
                        DELDATE = e.DELDATE,
                        PAYDATE = e.PAYDATE,
                        PAYMODE = e.PAYMODE
                    })
                    .ToListAsync();

                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> FindByScNoInUseAsync(string scNo)
        {
            return await DenimDbContext.COM_EX_SCINFO.Where(c => c.SCNO == scNo).AnyAsync();
        }

        public async Task<COM_EX_SCINFO> GetComExScInfoList(int scNo)
        {
            var comExScinfos = await DenimDbContext.COM_EX_SCINFO
                .Include(e => e.BUYER)
                .Include(e => e.BANK_)
                .Include(e => e.COM_EX_SCDETAILS)
                .ThenInclude(e => e.ACC_LOCAL_DODETAILS)
                .Include(e => e.COM_EX_SCDETAILS)
                .ThenInclude(e => e.STYLE.FABCODENavigation)
                .Where(e => e.SCID.Equals(scNo))
                .Select(e => new COM_EX_SCINFO
                {
                    SCDATE = e.SCDATE,
                    SCPERSON = e.SCPERSON,
                    BUYER = new BAS_BUYERINFO
                    {
                        BUYER_NAME = $"{e.BUYER.BUYER_NAME}",
                        ADDRESS = $"{e.BUYER.ADDRESS}"
                    },
                    COM_EX_SCDETAILS = e.COM_EX_SCDETAILS.Select(f => new COM_EX_SCDETAILS
                    {
                        TRNSID = f.TRNSID,
                        RATE = f.RATE,
                        AMOUNT = f.AMOUNT,
                        QTY = f.QTY /*- f.ACC_LOCAL_DODETAILS.Sum(g => g.QTY)*/,
                        STYLE = new COM_EX_FABSTYLE
                        {
                            STYLENAME = $"{f.STYLE.FABCODENavigation.STYLE_NAME}"
                        }
                    }).ToList()
                }).FirstOrDefaultAsync();

            //foreach (var item in result)
            //{
            //    var x = await _denimDbContext.ACC_LOCAL_DODETAILS
            //        .Where(c => c.STYLEID.Equals(item.COM_EX_SCDETAILS.FirstOrDefault().TRNSID)).ToListAsync();

            //    if (x == null) continue;
            //    var totalQty = x.Sum(c => c.QTY);
            //    var restQty = item.COM_EX_SCDETAILS.FirstOrDefault()?.QTY - totalQty;
            //    item.COM_EX_SCDETAILS.FirstOrDefault().QTY = restQty;
            //}

            return comExScinfos;
        }

        public async Task<COM_EX_SCINFO> GetComExScInfoByScNo(int scNo)
        {
            try
            {
                return await DenimDbContext.COM_EX_SCINFO.Where(c => c.SCID == scNo).FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<string> GetLastSCNoAsync()
        {
            try
            {
                var scNo = "";
                var result = await DenimDbContext.COM_EX_SCINFO.OrderByDescending(c => c.SCID).Select(c => c.SCNO).FirstOrDefaultAsync();
                var year = DateTime.Now.Year - 2000;

                if (result != null)
                {
                    var resultArray = result.Split("/");
                    if (int.Parse(resultArray[3]) < year)
                    {
                        scNo = "PDL/LSC/" + year + "/" + "1".PadLeft(4, '0');
                    }
                    else
                    {
                        scNo = "PDL/LSC/" + year + "/" + (int.Parse(resultArray[3]) + 1).ToString().PadLeft(4, '0');
                    }
                }
                else
                {
                    scNo = "PDL/LSC/" + year + "/" + "1".PadLeft(4, '0');
                }

                return scNo;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
