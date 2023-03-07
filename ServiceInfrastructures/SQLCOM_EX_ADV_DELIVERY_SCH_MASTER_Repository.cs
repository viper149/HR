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
    public class SQLCOM_EX_ADV_DELIVERY_SCH_MASTER_Repository : BaseRepository<COM_EX_ADV_DELIVERY_SCH_MASTER>, ICOM_EX_ADV_DELIVERY_SCH_MASTER
    {
        private readonly IDataProtector _protector;

        public SQLCOM_EX_ADV_DELIVERY_SCH_MASTER_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<IEnumerable<COM_EX_ADV_DELIVERY_SCH_MASTER>> GetAllAsync()
        {
            return await DenimDbContext.COM_EX_ADV_DELIVERY_SCH_MASTER
                .Include(d => d.BUYER)
                .Select(d => new COM_EX_ADV_DELIVERY_SCH_MASTER()
                {
                    DSID = d.DSID,
                    EncryptedId = _protector.Protect(d.DSID.ToString()),
                    DSNO = d.DSNO,
                    DSTYPE = d.DSTYPE,
                    DSDATE = d.DSDATE,
                    BUYER_ID = d.BUYER_ID,
                    BUYER = new BAS_BUYERINFO
                    {
                        BUYER_NAME = d.BUYER.BUYER_NAME
                    },
                    REMARKS = d.REMARKS
                }).ToListAsync();
        }

        public async Task<string> GetLastDSNoAsync()
        {
            string dsNo;
            var result = await DenimDbContext.COM_EX_ADV_DELIVERY_SCH_MASTER
                .OrderByDescending(c => c.DSNO)
                .Select(c => c.DSNO).FirstOrDefaultAsync();
            var year = DateTime.Now.Year % 100;

            if (result != null)
            {
                var resultArray = result.Split("-");
                if (int.Parse(resultArray[1]) < year)
                {
                    dsNo = $"AD-{year}-{"1".PadLeft(4, '0')}";
                }
                else
                {
                    int.TryParse(new string(resultArray[2].SkipWhile(x => !char.IsDigit(x)).TakeWhile(char.IsDigit).ToArray()), out var currentNumber);

                    dsNo = $"AD-{year}-{(currentNumber + 1).ToString().PadLeft(4, '0')}";
                }
            }
            else
            {
                dsNo = $"AD-{year}-{"1".PadLeft(4, '0')}";
            }

            return dsNo;
        }

        public async Task<ComExAdvDeliverySchViewModel> GetInitObjByAsync(ComExAdvDeliverySchViewModel comExAdvDeliverySchViewModel)
        {
            comExAdvDeliverySchViewModel.BasBuyerList = await DenimDbContext.BAS_BUYERINFO
                .Select(d => new BAS_BUYERINFO
                {
                    BUYERID = d.BUYERID,
                    BUYER_NAME = d.BUYER_NAME
                }).OrderBy(d => d.BUYER_NAME).ToListAsync();

            //comExAdvDeliverySchViewModel.ComExPimasterList = await _denimDbContext.COM_EX_PIMASTER
            //    .Select(d => new COM_EX_PIMASTER
            //    {
            //        PIID = d.PIID,
            //        PINO = d.PINO
            //    }).OrderByDescending(d => d.PIID).ToListAsync();

            return comExAdvDeliverySchViewModel;
        }

        public async Task<ComExAdvDeliverySchViewModel> GetInitDetailsObjByAsync(ComExAdvDeliverySchViewModel fComExAdvDeliverySchViewModel)
        {
            foreach (var item in fComExAdvDeliverySchViewModel.ComExAdvDeliverySchDetailsList)
            {
                item.PI = await DenimDbContext.COM_EX_PIMASTER
                    .Where(d=>d.PIID.Equals(item.PIID))
                    .Select(d => new COM_EX_PIMASTER
                    {
                        PINO = d.PINO
                    }).FirstOrDefaultAsync();

                item.STYLE = await DenimDbContext.COM_EX_PI_DETAILS
                    .Include(d=>d.STYLE)
                    .Include(d=>d.F_BAS_UNITS)
                    .Where(d => d.TRNSID.Equals(item.STYLE_ID))
                    .Select(d => new COM_EX_PI_DETAILS
                    {
                        STYLE = new COM_EX_FABSTYLE
                        {
                            STYLENAME = d.STYLE.STYLENAME
                        },
                        F_BAS_UNITS = new F_BAS_UNITS
                        {
                            UNAME = d.F_BAS_UNITS.UNAME
                        }
                    }).FirstOrDefaultAsync();
            }

            return fComExAdvDeliverySchViewModel;
        }

        public async Task<ComExAdvDeliverySchViewModel> FindByIdIncludeAllAsync(int id)
        {
            return await DenimDbContext.COM_EX_ADV_DELIVERY_SCH_MASTER
                .Include(d => d.COM_EX_ADV_DELIVERY_SCH_DETAILS)
                .ThenInclude(d => d.PI)
                .Include(d => d.COM_EX_ADV_DELIVERY_SCH_DETAILS)
                .ThenInclude(d => d.STYLE.STYLE)
                .Include(d => d.COM_EX_ADV_DELIVERY_SCH_DETAILS)
                .ThenInclude(d => d.STYLE.F_BAS_UNITS)
                .Where(d => d.DSID.Equals(id))
                .Select(d => new ComExAdvDeliverySchViewModel
                {
                    ComExAdvDeliverySchMaster = new COM_EX_ADV_DELIVERY_SCH_MASTER
                    {
                        DSID = d.DSID,
                        EncryptedId = _protector.Protect(d.DSID.ToString()),
                        DSNO = d.DSNO,
                        DSDATE = d.DSDATE,
                        DSTYPE = d.DSTYPE,
                        REMARKS = d.REMARKS,
                        BUYER_ID = d.BUYER_ID
                    },
                    ComExAdvDeliverySchDetailsList = d.COM_EX_ADV_DELIVERY_SCH_DETAILS
                        .Select(f => new COM_EX_ADV_DELIVERY_SCH_DETAILS
                        {
                            TRNSID = f.TRNSID,
                            DSID = f.DSID,
                            PIID = f.PIID,
                            STYLE_ID = f.STYLE_ID,
                            PREV_QTY = f.PREV_QTY,
                            QTY = f.QTY,
                            REMARKS = f.REMARKS,
                            PI = new COM_EX_PIMASTER
                            {
                                PINO = f.PI.PINO
                            },
                            STYLE = new COM_EX_PI_DETAILS
                            {
                                STYLE = new COM_EX_FABSTYLE
                                {
                                    STYLENAME = f.STYLE.STYLE.STYLENAME
                                },
                                F_BAS_UNITS = new F_BAS_UNITS
                                {
                                    UNAME = f.STYLE.F_BAS_UNITS.UNAME
                                }
                            }
                        }).ToList()
                }).FirstOrDefaultAsync();
        }
    }
}
