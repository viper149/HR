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
    public class SQLF_YS_YARN_RECEIVE_MASTER_S_Repository : BaseRepository<F_YS_YARN_RECEIVE_MASTER_S>, IF_YS_YARN_RECEIVE_MASTER_S
    {
        private readonly IDataProtector _protector;

        public SQLF_YS_YARN_RECEIVE_MASTER_S_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<IEnumerable<F_YS_YARN_RECEIVE_MASTER_S>> GetAllYarnReceiveAsync()
        {
            return await DenimDbContext.F_YS_YARN_RECEIVE_MASTER_S
                .Include(e => e.INV)
                .Include(e => e.RCVT)
                //.Include(e => e.ORDER_NONavigation)
                //.ThenInclude(e => e.SO)
                .Select(e => new F_YS_YARN_RECEIVE_MASTER_S
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
                    CHALLANNO = e.CHALLANNO
                }).ToListAsync();
        }

        public async Task<FYsYarnReceiveSViewModel> GetInitObjectsByAsync(FYsYarnReceiveSViewModel fYsYarnReceiveSViewModel)
        {
            fYsYarnReceiveSViewModel.FBasSections = await DenimDbContext.F_BAS_SECTION.Select(e => new F_BAS_SECTION
            {
                SECID = e.SECID,
                SECNAME = e.SECNAME
            }).OrderBy(e => e.SECNAME).ToListAsync();

            fYsYarnReceiveSViewModel.FBasReceiveTypeList = await DenimDbContext.F_BAS_RECEIVE_TYPE.Select(e => new F_BAS_RECEIVE_TYPE
            {
                RCVTID = e.RCVTID,
                RCVTYPE = e.RCVTYPE
            }).OrderBy(e => e.RCVTYPE).ToListAsync();

            fYsYarnReceiveSViewModel.ComImpInvoiceInfoList = await DenimDbContext.COM_IMP_INVOICEINFO.Select(e => new COM_IMP_INVOICEINFO
            {
                INVID = e.INVID,
                INVNO = e.INVNO
            }).OrderBy(e => e.INVNO).ToListAsync();

            fYsYarnReceiveSViewModel.FYsLocationList = await DenimDbContext.F_YS_LOCATION.Select(e => new F_YS_LOCATION
            {
                LOCID = e.LOCID,
                LOCNAME = e.LOCNAME
            }).OrderBy(e => e.LOCNAME).ToListAsync();

            fYsYarnReceiveSViewModel.FYsLadgerList = await DenimDbContext.F_YS_LEDGER.OrderBy(e => e.LEDNAME).ToListAsync();

            fYsYarnReceiveSViewModel.FYarnLotinfoList = await DenimDbContext.BAS_YARN_LOTINFO.Select(e => new BAS_YARN_LOTINFO
            {
                LOTID = e.LOTID,
                LOTNO = $"Lot: {e.LOTNO}({e.BRAND})"
            }).ToListAsync();

            fYsYarnReceiveSViewModel.FYsRawPers = await DenimDbContext.F_YS_RAW_PER.Select(e => new F_YS_RAW_PER
            {
                RAWID = e.RAWID,
                RAWPER = $"Raw: {e.RAWPER}"
            }).ToListAsync();

            fYsYarnReceiveSViewModel.FYsIndentMastersList = await DenimDbContext.F_YS_INDENT_MASTER
                .Include(d=>d.INDSL)
                .Where(e=>e.INDSL.INDSLNO.Contains("SYW"))
                .Select(e => new F_YS_INDENT_MASTER
            {
                INDID = e.INDID,
                INDNO = $"{e.INDNO} - {e.INDSL.INDSLNO}"
            }).OrderBy(e => e.INDNO).ToListAsync();

            fYsYarnReceiveSViewModel.BasYarnCountInfoList = await DenimDbContext.BAS_YARN_COUNTINFO.Select(e => new BAS_YARN_COUNTINFO
            {
                COUNTID = e.COUNTID,
                COUNTNAME = e.COUNTNAME
            }).OrderBy(e => e.COUNTNAME).ToListAsync();

            fYsYarnReceiveSViewModel.RndProductionOrders = await DenimDbContext.RND_PRODUCTION_ORDER
                .Include(e => e.SO)
                .Select(e => new RND_PRODUCTION_ORDER
                {
                    POID = e.POID,
                    SO = new COM_EX_PI_DETAILS
                    {
                        SO_NO = e.SO.SO_NO
                    }
                }).OrderBy(e => e.ORDERNO).ToListAsync();

            fYsYarnReceiveSViewModel.BasSupplierinfos = await DenimDbContext.BAS_SUPPLIERINFO.Select(e => new BAS_SUPPLIERINFO
            {
                SUPPID = e.SUPPID,
                SUPPNAME = e.SUPPNAME
            }).OrderBy(e => e.SUPPNAME).ToListAsync();

            fYsYarnReceiveSViewModel.IsReturnable = StaticData.GetStatus();

            return fYsYarnReceiveSViewModel;
        }

        public async Task<FYsYarnReceiveSViewModel> FindByIdIncludeAllAsync(int yrcvId)
        {
            var fYsYarnReceiveMaster = await DenimDbContext.F_YS_YARN_RECEIVE_MASTER_S
                .Include(e => e.INV.LC)
                .Include(e => e.F_YS_YARN_RECEIVE_DETAILS_S)
                .ThenInclude(e => e.F_YARN_QC_APPROVE_S)
                .Include(e => e.F_YS_YARN_RECEIVE_DETAILS_S)
                .ThenInclude(e => e.F_YS_YARN_RECEIVE_REPORT_S)
                .Include(e => e.F_YS_YARN_RECEIVE_DETAILS_S)
                .ThenInclude(e => e.PROD)
                .Include(e => e.F_YS_YARN_RECEIVE_DETAILS_S)
                .ThenInclude(e => e.LOTNavigation)
                .Include(e => e.F_YS_YARN_RECEIVE_DETAILS_S)
                .ThenInclude(e => e.LOCATION)
                .Include(e => e.F_YS_YARN_RECEIVE_DETAILS_S)
                .ThenInclude(e => e.LEDGER)
                //.Include(e => e.ORDER_NONavigation.SO)
                .FirstOrDefaultAsync(e => e.YRCVID.Equals(yrcvId));

            fYsYarnReceiveMaster.EncryptedId = _protector.Protect(fYsYarnReceiveMaster.YRCVID.ToString());

            return new FYsYarnReceiveSViewModel
            {
                FYsYarnReceiveMaster = fYsYarnReceiveMaster,
                FYsYarnReceiveDetailList = fYsYarnReceiveMaster.F_YS_YARN_RECEIVE_DETAILS_S.ToList()
            };
        }
    }
}
