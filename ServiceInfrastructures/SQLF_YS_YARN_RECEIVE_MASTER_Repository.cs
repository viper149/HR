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
using DenimERP.ViewModels.StaticData;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_YS_YARN_RECEIVE_MASTER_Repository : BaseRepository<F_YS_YARN_RECEIVE_MASTER>, IF_YS_YARN_RECEIVE_MASTER
    {
        private readonly IDataProtector _protector;

        public SQLF_YS_YARN_RECEIVE_MASTER_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<IEnumerable<F_YS_YARN_RECEIVE_MASTER>> GetAllYarnReceiveAsync()
        {
            try
            {
                return await DenimDbContext.F_YS_YARN_RECEIVE_MASTER
                    .Include(e => e.INV)
                    .Include(e => e.RCVT)
                    .Include(e => e.IND.INDSL.ORDER_NONavigation.SO.STYLE.FABCODENavigation)
                    .Include(e => e.IND.INDSL.ORDER_NONavigation.RS)
                    //.Include(e => e.ORDER_NONavigation)
                    //.ThenInclude(e => e.SO)
                    .OrderByDescending(c => c.YRCVID)
                    .Select(e => new F_YS_YARN_RECEIVE_MASTER
                    {
                        YRCVID = e.YRCVID,
                        EncryptedId = _protector.Protect(e.YRCVID.ToString()),
                        YRCVDATE = e.YRCVDATE,
                        RCVT = new F_BAS_RECEIVE_TYPE
                        {
                            RCVTYPE = e.RCVT.RCVTYPE
                        },
                        INV = new COM_IMP_INVOICEINFO
                        {
                            INVNO = e.INV != null ? e.INV.INVNO : ""
                        },
                        //ORDER_NONavigation = new RND_PRODUCTION_ORDER
                        //{
                        //    SO = new COM_EX_PI_DETAILS
                        //    {
                        //        SO_NO = e.ORDER_NONavigation.SO.SO_NO
                        //    }
                        //},
                        RCVFROM = e.RCVFROM,
                        G_ENTRY_NO = e.G_ENTRY_NO,
                        G_ENTRY_DATE = e.G_ENTRY_DATE,
                        //ORDER_NO = e.ORDER_NO,
                        REMARKS = e.REMARKS,
                        CHALLANNO = e.CHALLANNO,
                        OPT4 = e.INDENT_TYPE==0?"Bulk":"Sample",
                        OPT5 = $"{e.IND.INDNO} - {e.IND.INDSL.INDSLNO}",
                        OPT6 = $"{(e.IND.INDSL.ORDER_NONavigation.SO!=null?e.IND.INDSL.ORDER_NONavigation.SO.SO_NO: e.IND.INDSL.ORDER_NONavigation.RS!=null? e.IND.INDSL.ORDER_NONavigation.RS.RSOrder:"")}",
                        OPT7 = $"{(e.IND.INDSL.ORDER_NONavigation.SO!=null?e.IND.INDSL.ORDER_NONavigation.SO.STYLE.FABCODENavigation.STYLE_NAME: e.IND.INDSL.ORDER_NONavigation.RS!=null?e.IND.INDSL.ORDER_NONavigation.RS.SDRF.SDRF_NO:"")}"
                    }).ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<YarnReceiveViewModel> GetInitObjectsByAsync(YarnReceiveViewModel yarnReceiveViewModel)
        {
            yarnReceiveViewModel.FBasSections = await DenimDbContext.F_BAS_SECTION.Select(e => new F_BAS_SECTION
            {
                SECID = e.SECID,
                SECNAME = e.SECNAME
            }).OrderBy(e => e.SECNAME).ToListAsync();

            yarnReceiveViewModel.FBasReceiveTypeList = await DenimDbContext.F_BAS_RECEIVE_TYPE
                .Where(d => d.RCVTYPE.Equals("Receive"))
                .Select(e => new F_BAS_RECEIVE_TYPE
            {
                RCVTID = e.RCVTID,
                RCVTYPE = e.RCVTYPE
            }).OrderBy(e => e.RCVTYPE).ToListAsync();

            yarnReceiveViewModel.ComImpInvoiceInfoList = await DenimDbContext.COM_IMP_INVOICEINFO.Select(e => new COM_IMP_INVOICEINFO
            {
                INVID = e.INVID,
                INVNO = e.INVNO
            }).OrderBy(e => e.INVNO).ToListAsync();

            yarnReceiveViewModel.FYsLocationList = await DenimDbContext.F_YS_LOCATION.Select(e => new F_YS_LOCATION
            {
                LOCID = e.LOCID,
                LOCNAME = e.LOCNAME
            }).OrderBy(e => e.LOCNAME).ToListAsync();

            yarnReceiveViewModel.FYsLadgerList = await DenimDbContext.F_YS_LEDGER.OrderBy(e => e.LEDNAME).ToListAsync();
            yarnReceiveViewModel.YarnFor = await DenimDbContext.YARNFOR.OrderBy(e => e.YARNNAME).ToListAsync();

            yarnReceiveViewModel.FYarnLotinfoList = await DenimDbContext.BAS_YARN_LOTINFO.Select(e => new BAS_YARN_LOTINFO
            {
                LOTID = e.LOTID,
                LOTNO = $"Lot: {e.LOTNO}({e.BRAND})"
            }).ToListAsync();

            yarnReceiveViewModel.FYsRawPers = await DenimDbContext.F_YS_RAW_PER.Select(e => new F_YS_RAW_PER
            {
                RAWID = e.RAWID,
                RAWPER = $"Raw: {e.RAWPER}"
            }).ToListAsync();

            yarnReceiveViewModel.FYsIndentMastersList = await DenimDbContext.F_YS_INDENT_MASTER
                .Include(d => d.INDSL.ORDER_NONavigation.SO)
                .Include(d => d.INDSL.ORDER_NONavigation.RS)
                .Include(d => d.INDSL.ORDERNO_SNavigation)

                .Select(e => new F_YS_INDENT_MASTER
                {
                    INDID = e.INDID,
                    INDNO = $"{e.INDNO} - {e.INDSL.INDSLNO ?? e.OPT3} - { (e.INDSL != null ? e.INDSL.ORDER_NONavigation != null && e.INDSL.ORDER_NONavigation.SO != null ? e.INDSL.ORDER_NONavigation.SO.SO_NO : e.INDSL.ORDERNO_SNavigation != null ? e.INDSL.ORDERNO_SNavigation.SDRF_NO : "" : "") }"
                }).OrderBy(e => e.INDNO).ToListAsync();

            yarnReceiveViewModel.BasYarnCountInfoList = await DenimDbContext.BAS_YARN_COUNTINFO.Select(e => new BAS_YARN_COUNTINFO
            {
                COUNTID = e.COUNTID,
                RND_COUNTNAME = e.RND_COUNTNAME
            }).OrderBy(e => e.RND_COUNTNAME).ToListAsync();

            yarnReceiveViewModel.RndProductionOrders = await DenimDbContext.RND_PRODUCTION_ORDER
                .Include(e => e.SO)
                .Select(e => new RND_PRODUCTION_ORDER
                {
                    POID = e.POID,
                    SO = new COM_EX_PI_DETAILS
                    {
                        SO_NO = e.SO.SO_NO
                    }
                }).OrderBy(e => e.ORDERNO).ToListAsync();

            yarnReceiveViewModel.BasSupplierinfos = await DenimDbContext.BAS_SUPPLIERINFO.Select(e => new BAS_SUPPLIERINFO
            {
                SUPPID = e.SUPPID,
                SUPPNAME = e.SUPPNAME
            }).OrderBy(e => e.SUPPNAME).ToListAsync();

            yarnReceiveViewModel.SecList = await DenimDbContext.F_BAS_SECTION.Select(e => new F_BAS_SECTION
            {
                SECID = e.SECID,
                SECNAME = e.SECNAME
            }).OrderBy(e => e.SECNAME).ToListAsync();

            yarnReceiveViewModel.SubSecList = await DenimDbContext.F_BAS_SUBSECTION.Select(e => new F_BAS_SUBSECTION
            {
                SSECID = e.SSECID,
                SSECNAME = e.SSECNAME
            }).OrderBy(e => e.SSECNAME).ToListAsync();

            yarnReceiveViewModel.IsReturnable = StaticData.GetStatus();

            return yarnReceiveViewModel;
        }

        public async Task<YarnReceiveViewModel> FindByIdIncludeAllAsync(int yrcvId)
        {
            var fYsYarnReceiveMaster = await DenimDbContext.F_YS_YARN_RECEIVE_MASTER
                .Include(e => e.INV.LC)
                .Include(e => e.F_YS_YARN_RECEIVE_DETAILS)
                .ThenInclude(e => e.F_YARN_QC_APPROVE)
                .Include(e => e.F_YS_YARN_RECEIVE_DETAILS)
                .ThenInclude(e => e.F_YS_YARN_RECEIVE_REPORT)
                .Include(e => e.F_YS_YARN_RECEIVE_DETAILS)
                .ThenInclude(e => e.PROD)
                .Include(e => e.F_YS_YARN_RECEIVE_DETAILS)
                .ThenInclude(e => e.BasYarnLotinfo)
                .Include(e => e.F_YS_YARN_RECEIVE_DETAILS)
                .ThenInclude(e => e.LOCATION)
                .Include(e => e.F_YS_YARN_RECEIVE_DETAILS)
                .ThenInclude(e => e.LEDGER)
                .Include(d => d.F_YS_YARN_RECEIVE_DETAILS)
                .ThenInclude(d => d.FYarnFor)
                .FirstOrDefaultAsync(e => e.YRCVID.Equals(yrcvId));

            fYsYarnReceiveMaster.EncryptedId = _protector.Protect(fYsYarnReceiveMaster.YRCVID.ToString());

            return new YarnReceiveViewModel
            {
                FYsYarnReceiveMaster = fYsYarnReceiveMaster,
                FYsYarnReceiveDetailList = fYsYarnReceiveMaster.F_YS_YARN_RECEIVE_DETAILS.ToList()
            };
        }
    }
}
