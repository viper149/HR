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
    public class SQLF_YS_GP_MASTER_Repository : BaseRepository<F_YS_GP_MASTER>, IF_YS_GP_MASTER

    {
        private readonly IDataProtector _protector;

        public SQLF_YS_GP_MASTER_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }


        public async Task<IEnumerable<F_YS_GP_MASTER>> GetAllAsync()
        {
            return await DenimDbContext.F_YS_GP_MASTER

                .Select(d => new F_YS_GP_MASTER()
                {
                    GPID = d.GPID,
                    EncryptedId = _protector.Protect(d.GPID.ToString()),
                    GPNO = d.GPNO,
                    GPDATE = d.GPDATE,
                    GPTYPE = d.GPTYPE
                }).ToListAsync();

        }

        public async Task<FYsGpViewModel> GetInitDetailsObjByAsync(FYsGpViewModel fysgpViewModel)
        {
            try
            {

                foreach (var item in fysgpViewModel.fysgpdetailsList)
                {
                    item.LEDGER_ = await DenimDbContext.F_YS_LEDGER
                        .Where(c => c.LEDID.Equals(item.LEDGER_ID)).FirstOrDefaultAsync();

                    item.STOCK = await DenimDbContext.F_YARN_TRANSACTION_TYPE
                        .Where(c => c.STOCKID.Equals(item.STOCKID)).FirstOrDefaultAsync();

                    item.COUNT = await DenimDbContext.BAS_YARN_COUNTINFO.Where(c => c.COUNTID.Equals(item.COUNTID))
                        .FirstOrDefaultAsync();

                    item.LOT = await DenimDbContext.BAS_YARN_LOTINFO.Where(c => c.LOTID.Equals(item.LOTID))
                        .FirstOrDefaultAsync();

                    item.LOCATION_ = await DenimDbContext.F_YS_LOCATION.Where(c => c.LOCID.Equals(item.LOCATION_ID))
                        .FirstOrDefaultAsync();

                    item.RCV = await DenimDbContext.F_YS_YARN_RECEIVE_DETAILS
                        .Include(d => d.YRCV.IND.INDSL)
                        .FirstOrDefaultAsync(d => d.TRNSID.Equals(item.INDSLID));
                }
                return fysgpViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<FYsGpViewModel> GetInitObjByAsync(FYsGpViewModel fysgpViewModel)
        {
            fysgpViewModel.LcInformationList = await DenimDbContext.COM_IMP_LCINFORMATION
                .Select(d => new COM_IMP_LCINFORMATION
                {
                    LC_ID = d.LC_ID,
                    LCNO = d.LCNO
                }).OrderBy(d => d.LCNO).ToListAsync();

            fysgpViewModel.FYsPartyInfoList = await DenimDbContext.F_YS_PARTY_INFO
                .Select(d => new F_YS_PARTY_INFO
                {
                    PARTY_ID = d.PARTY_ID,
                    PARTY_NAME = d.PARTY_NAME
                }).OrderBy(d => d.PARTY_NAME).ToListAsync();


            //fysgpViewModel.FYsGpMasterList = await _denimDbContext.F_YS_GP_MASTER
            //    .Select(d => new F_YS_GP_MASTER
            //    {
            //        GPID = d.GPID,
            //        GPTYPE = d.GPTYPE
            //    }).OrderBy(d => d.GPTYPE).ToListAsync();

            fysgpViewModel.BasYarnCountInfoList = await DenimDbContext.BAS_YARN_COUNTINFO
                .Select(d => new BAS_YARN_COUNTINFO
                {
                    COUNTID = d.COUNTID,
                    COUNTNAME = d.COUNTNAME
                }).OrderBy(d => d.COUNTNAME).ToListAsync();

            //fysgpViewModel.BasYarnLotInfoList = await _denimDbContext.BAS_YARN_LOTINFO
            //    .Select(d => new BAS_YARN_LOTINFO
            //    {
            //        LOTID = d.LOTID,
            //        LOTNO = d.LOTNO
            //    }).OrderBy(d => d.LOTNO).ToListAsync();


            fysgpViewModel.FYsLocationList = await DenimDbContext.F_YS_LOCATION
               .Select(d => new F_YS_LOCATION
               {
                   LOCID = d.LOCID,
                   LOCNAME = d.LOCNAME
               }).OrderBy(d => d.LOCNAME).ToListAsync();

            fysgpViewModel.FYsLedgerList = await DenimDbContext.F_YS_LEDGER
              .Select(d => new F_YS_LEDGER
              {
                  LEDID = d.LEDID,
                  LEDNAME = d.LEDNAME
              }).OrderBy(d => d.LEDNAME).ToListAsync();

            fysgpViewModel.StockList = await DenimDbContext.F_YARN_TRANSACTION_TYPE
              .Select(d => new F_YARN_TRANSACTION_TYPE
              {
                  STOCKID = d.STOCKID,
                  NAME = d.NAME
              }).OrderBy(d => d.NAME).ToListAsync();


            return fysgpViewModel;
        }



        public async Task<FYsGpViewModel> FindByIdIncludeAllAsync(int id)
        {
            return await DenimDbContext.F_YS_GP_MASTER
                .Include(d => d.F_YS_GP_DETAILS)
                .Where(d => d.GPID.Equals(id))
                .Select(d => new FYsGpViewModel
                {
                    f_YS_GP_MASTER = new F_YS_GP_MASTER
                    {
                        GPID = d.GPID,
                        EncryptedId = _protector.Protect(d.GPID.ToString()),
                        GPNO = d.GPNO,
                        GPTYPE = d.GPTYPE,
                        PARTY_ID = d.PARTY_ID,
                        LC_ID = d.LC_ID,
                        GPDATE=d.GPDATE,
                        OPT1=d.OPT1,

                        LC_ = new COM_IMP_LCINFORMATION
                        {
                            LCNO = d.LC_.LCNO

                        },

                        PARTY_ = new F_YS_PARTY_INFO
                        {
                            PARTY_NAME = d.PARTY_.PARTY_NAME

                        },

                    },
                    fysgpdetailsList = d.F_YS_GP_DETAILS

                        .Select(f => new F_YS_GP_DETAILS
                        {
                            TRNSID = f.TRNSID,
                            GPID = f.GPID,
                            COUNTID = f.COUNTID,
                            LOTID = f.LOTID,
                            QTY_BAGS = f.QTY_BAGS,
                            QTY_KGS = f.QTY_KGS,
                            LOCATION_ID = f.LOCATION_ID,
                            PAGENO = f.PAGENO,
                            TRNSDATE=f.TRNSDATE,
                            STOCKID = f.STOCKID,

                             COUNT = new BAS_YARN_COUNTINFO
                             {
                                 COUNTNAME = f.COUNT.COUNTNAME

                             },

                            LEDGER_ = new F_YS_LEDGER
                            {
                                 LEDNAME = f.LEDGER_.LEDNAME

                            },

                            LOCATION_ = new F_YS_LOCATION
                            {
                                 LOCNAME = f.LOCATION_.LOCNAME

                            },

                            LOT = new BAS_YARN_LOTINFO
                            {
                                 LOTNO = f.LOT.LOTNO

                            },
                            STOCK= new F_YARN_TRANSACTION_TYPE
                            {
                                NAME=f.STOCK.NAME
                            },
                            RCV= new F_YS_YARN_RECEIVE_DETAILS
                            {
                                YRCV= new F_YS_YARN_RECEIVE_MASTER
                                {
                                    IND= new F_YS_INDENT_MASTER
                                    {
                                        INDSL= new RND_PURCHASE_REQUISITION_MASTER
                                        {
                                            INDSLNO=f.RCV.YRCV.IND.INDSL.INDSLNO
                                        }
                                    }
                                }
                            }

                        }).ToList()
                }).FirstOrDefaultAsync();
        }


        public async Task<F_YS_GP_MASTER> FindByIdForDeleteAsync(int dsId)
        {
            return await DenimDbContext.F_YS_GP_MASTER
                .Include(e => e.F_YS_GP_DETAILS)
                .FirstOrDefaultAsync(e => e.GPID.Equals(dsId));
        }


        public async Task<COM_IMP_LCINFORMATION> GetLcInfoAsync(int id)
        {
            try
            {
                var result = await DenimDbContext.COM_IMP_LCINFORMATION
                               .Include(d => d.SUPP)
                               .Where(d => d.LC_ID.Equals(id))
                               .FirstOrDefaultAsync();

                return result;

            }
            catch(Exception e)
            {
                return null;
            }
        }

        public async Task<IEnumerable<F_YS_YARN_RECEIVE_DETAILS>> GetYarnIndentDetails(int countId)
        {
            try
            {
                var result = await DenimDbContext.F_YS_YARN_RECEIVE_DETAILS
                    .Include(d => d.F_YS_YARN_ISSUE_DETAILS)
                    .ThenInclude(c => c.YISSUE)
                    .Include(d => d.YRCV.IND.INDSL.ORDER_NONavigation.SO)
                    .Include(d => d.YRCV.IND.INDSL.ORDER_NONavigation.RS)
                    .Include(d => d.YRCV.IND.INDSL.ORDERNO_SNavigation)
                    .Include(c => c.BasYarnLotinfo)
                    .Include(c => c.FYsRawPer)
                    .Include(c => c.FYarnFor)
                    .Include(c => c.LOCATION)
                    .Include(c => c.LEDGER)
                    .Where(d => d.PRODID.Equals(countId))
                    .Select(d => new F_YS_YARN_RECEIVE_DETAILS
                    {
                        BasYarnLotinfo = new BAS_YARN_LOTINFO
                        {
                            LOTID = d.BasYarnLotinfo.LOTID,
                            LOTNO= d.BasYarnLotinfo.LOTNO
                        },

                        TRNSID = d.TRNSID,
                        OPT1 = $"{d.YRCV.IND.INDSL.INDSLNO} - " +
                               $"{(d.YRCV.IND.INDSL != null ? d.YRCV.IND.INDSL.ORDER_NONavigation != null && d.YRCV.IND.INDSL.ORDER_NONavigation.SO != null ? d.YRCV.IND.INDSL.ORDER_NONavigation.SO.SO_NO : d.YRCV.IND.INDSL.ORDERNO_SNavigation != null ? d.YRCV.IND.INDSL.ORDERNO_SNavigation.SDRF_NO : "" : "")} - {d.BasYarnLotinfo.LOTNO}- {d.BAG - (DenimDbContext.F_YS_YARN_ISSUE_DETAILS.Where(c => c.RCVDID.Equals(d.TRNSID)).Sum(f => f.BAG) ?? 0)} Bag - {d.RCV_QTY - (DenimDbContext.F_YS_YARN_ISSUE_DETAILS.Include(c => c.YISSUE).Where(c => c.RCVDID.Equals(d.TRNSID)).Sum(f => f.ISSUE_QTY) ?? 0)} Kg - {d.FYsRawPer.RAWPER} - {d.FYarnFor.YARNNAME} - {d.LOCATION.LOCNAME} - {d.LEDGER.LEDNAME} - {d.PAGENO} - {d.YRCV.YRCVDATE}",

                        //OPT2 = d.RCV_QTY.ToString(),
                        //OPT3 = _denimDbContext.F_YS_YARN_ISSUE_DETAILS.Include(c => c.YISSUE).Where(c => c.RCVDID.Equals(d.TRNSID)).Sum(f => f.ISSUE_QTY).ToString()
                    }).ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return null;
            }
        }



        public async Task<IEnumerable<F_YS_YARN_RECEIVE_DETAILS>> GetLotId(int lotid)
        {
            try
            {
                var result = await DenimDbContext.F_YS_YARN_RECEIVE_DETAILS
                    .Include(c => c.BasYarnLotinfo)
                    .Where(d => d.TRNSID.Equals(lotid))
                    .ToListAsync();
                   

                return result;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return null;
            }
        }
    }

}
