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
    public class SQLF_YS_YARN_RECEIVE_MASTER2_Repository : BaseRepository<F_YS_YARN_RECEIVE_MASTER2>, IF_YS_YARN_RECEIVE_MASTER2

    {
        private readonly IDataProtector _protector;

        public SQLF_YS_YARN_RECEIVE_MASTER2_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<IEnumerable<F_YS_YARN_RECEIVE_MASTER2>> GetAllYarnReceiveAsync()
        {
            
                return await DenimDbContext.F_YS_YARN_RECEIVE_MASTER2

                    .Select(d => new F_YS_YARN_RECEIVE_MASTER2()
                    {
                        YRCVID = d.YRCVID,
                        EncryptedId = _protector.Protect(d.YRCVID.ToString()),
                        YRCVDATE = d.YRCVDATE,
                        CHALLANNO = d.CHALLANNO,
                        CHALLANDATE = d.CHALLANDATE
                    }).ToListAsync();

            
        }

        public async Task<YarnReceiveForOthersViewModel> GetInitObjectsByAsync(YarnReceiveForOthersViewModel yarnReceiveForOthersViewModel)
        {
            yarnReceiveForOthersViewModel.FBasReceiveTypeList = await DenimDbContext.F_BAS_RECEIVE_TYPE
                .Where(d=>!d.RCVTYPE.Equals("Receive"))
                .Select(e => new F_BAS_RECEIVE_TYPE
            {
                RCVTID = e.RCVTID,
                RCVTYPE = e.RCVTYPE
            }).OrderBy(e => e.RCVTYPE).ToListAsync();

            yarnReceiveForOthersViewModel.FYsLocationList = await DenimDbContext.F_YS_LOCATION.Select(e => new F_YS_LOCATION
            {
                LOCID = e.LOCID,
                LOCNAME = e.LOCNAME
            }).OrderBy(e => e.LOCNAME).ToListAsync();



            yarnReceiveForOthersViewModel.FYarnLotinfoList = await DenimDbContext.BAS_YARN_LOTINFO.Select(e => new BAS_YARN_LOTINFO
            {
                LOTID = e.LOTID,
                LOTNO = $"Lot: {e.LOTNO}({e.BRAND})"
            }).ToListAsync();

            yarnReceiveForOthersViewModel.FYsRawPers = await DenimDbContext.F_YS_RAW_PER.Select(e => new F_YS_RAW_PER
            {
                RAWID = e.RAWID,
                RAWPER = $"Raw: {e.RAWPER}"
            }).ToListAsync();

            yarnReceiveForOthersViewModel.BasYarnCountInfoList = await DenimDbContext.BAS_YARN_COUNTINFO.Select(e => new BAS_YARN_COUNTINFO
            {
                COUNTID = e.COUNTID,
                RND_COUNTNAME = e.RND_COUNTNAME
            }).OrderBy(e => e.RND_COUNTNAME).ToListAsync();

            yarnReceiveForOthersViewModel.RndProductionOrders = await DenimDbContext.RND_PRODUCTION_ORDER
                .Include(e => e.SO)
                .Where(d=>d.ORDERNO.HasValue)
                .Select(e => new RND_PRODUCTION_ORDER
                {
                    POID = e.POID,
                    SO = new COM_EX_PI_DETAILS
                    {
                        SO_NO = e.SO.SO_NO
                    }
                }).OrderBy(e => e.ORDERNO).ToListAsync();

            yarnReceiveForOthersViewModel.BasSupplierinfos = await DenimDbContext.BAS_SUPPLIERINFO.Select(e => new BAS_SUPPLIERINFO
            {
                SUPPID = e.SUPPID,
                SUPPNAME = e.SUPPNAME
            }).OrderBy(e => e.SUPPNAME).ToListAsync();

            yarnReceiveForOthersViewModel.SecList = await DenimDbContext.F_BAS_SECTION.Select(e => new F_BAS_SECTION
            {
                SECID = e.SECID,
                SECNAME = e.SECNAME
            }).OrderBy(e => e.SECNAME).ToListAsync();

            yarnReceiveForOthersViewModel.SubSecList = await DenimDbContext.F_BAS_SUBSECTION.Select(e => new F_BAS_SUBSECTION
            {
                SSECID = e.SSECID,
                SSECNAME = e.SSECNAME
            }).OrderBy(e => e.SSECNAME).ToListAsync();

            yarnReceiveForOthersViewModel.FYsLadgerList = await DenimDbContext.F_YS_LEDGER
              .Select(d => new F_YS_LEDGER
              {
                  LEDID = d.LEDID,
                  LEDNAME = d.LEDNAME
              }).OrderBy(d => d.LEDNAME).ToListAsync();

            return yarnReceiveForOthersViewModel;
        }


        public async Task<YarnReceiveForOthersViewModel> GetInitDetailsObjByAsync(YarnReceiveForOthersViewModel YarnReceiveForOthersViewModel)
        {
            try
            {

                foreach (var item in YarnReceiveForOthersViewModel.FYsYarnReceiveDetailList)
                {
                    item.LEDGER = await DenimDbContext.F_YS_LEDGER
                        .Where(c => c.LEDID.Equals(item.LEDGERID)).FirstOrDefaultAsync();

                    item.COUNT = await DenimDbContext.BAS_YARN_COUNTINFO.Where(c => c.COUNTID.Equals(item.COUNTID))
                        .FirstOrDefaultAsync();

                    item.LOTNavigation = await DenimDbContext.BAS_YARN_LOTINFO.Where(c => c.LOTID.Equals(item.LOT))
                        .FirstOrDefaultAsync();

                    item.LOCATION = await DenimDbContext.F_YS_LOCATION.Where(c => c.LOCID.Equals(item.LOCATIONID))
                        .FirstOrDefaultAsync();

                    item.RAWNavigation = await DenimDbContext.F_YS_RAW_PER.Where(c => c.RAWID.Equals(item.RAW))
                        .FirstOrDefaultAsync();
                }
                return YarnReceiveForOthersViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        public async Task<YarnReceiveForOthersViewModel> FindByIdIncludeAllAsync(int id)
        {
            return await DenimDbContext.F_YS_YARN_RECEIVE_MASTER2
                .Include(d => d.F_YS_YARN_RECEIVE_DETAILS2)
                .Where(d => d.YRCVID.Equals(id))
                .Select(d => new YarnReceiveForOthersViewModel
                {
                    FYsYarnReceiveMaster = new F_YS_YARN_RECEIVE_MASTER2
                    {
                        YRCVID = d.YRCVID,
                        EncryptedId = _protector.Protect(d.YRCVID.ToString()),
                        YRCVDATE = d.YRCVDATE,
                        RCVTID = d.RCVTID,
                        SECID = d.SECID,
                        SUBSECID = d.SUBSECID,
                        CHALLANNO = d.CHALLANNO,
                        INDENT_TYPE = d.INDENT_TYPE,
                        CHALLANDATE = d.CHALLANDATE,
                        RCVFROM = d.RCVFROM,
                        G_ENTRY_NO = d.G_ENTRY_NO,
                        G_ENTRY_DATE = d.G_ENTRY_DATE,
                        SO_NO = d.SO_NO,
                        TRUCKNO=d.TRUCKNO,

                        RCVFROMNavigation = new BAS_SUPPLIERINFO
                        {
                            SUPPNAME = d.RCVFROMNavigation.SUPPNAME

                        },
                        SEC = new F_BAS_SECTION
                        {
                            SECNAME = d.SEC.SECNAME

                        },

                        SUBSEC = new  F_BAS_SUBSECTION
                        {
                            SSECNAME = d.SUBSEC.SSECNAME

                        },



                    },
                    FYsYarnReceiveDetailList = d.F_YS_YARN_RECEIVE_DETAILS2

                        .Select(f => new F_YS_YARN_RECEIVE_DETAILS2
                        {
                            TRNSID = f.TRNSID,
                            YRCVID = f.YRCVID,
                            COUNTID = f.COUNTID,
                            LOT = f.LOT,
                            BAG_QTY = f.BAG_QTY,
                            RCV_QTY = f.RCV_QTY,
                            LOCATIONID = f.LOCATIONID,
                            PAGENO = f.PAGENO,
                            TRNSDATE = f.TRNSDATE,
                            LEDGERID = f.LEDGERID,
                            RAW=f.RAW,

                            COUNT = new BAS_YARN_COUNTINFO
                            {
                                COUNTNAME = f.COUNT.COUNTNAME

                            },

                            LEDGER = new F_YS_LEDGER
                            {
                                LEDNAME = f.LEDGER.LEDNAME

                            },

                            LOCATION = new F_YS_LOCATION
                            {
                                LOCNAME = f.LOCATION.LOCNAME

                            },

                            LOTNavigation = new BAS_YARN_LOTINFO
                            {
                                LOTNO = f.LOTNavigation.LOTNO

                            },
                            RAWNavigation = new  F_YS_RAW_PER
                            {
                                RAWPER = f.RAWNavigation.RAWPER
                            },
                            

                        }).ToList()
                }).FirstOrDefaultAsync();
        }
    }
}
