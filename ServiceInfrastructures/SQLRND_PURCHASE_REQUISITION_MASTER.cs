using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels.Rnd;
using DenimERP.ViewModels.StaticData;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLRND_PURCHASE_REQUISITION_MASTER : BaseRepository<RND_PURCHASE_REQUISITION_MASTER>, IRND_PURCHASE_REQUISITION_MASTER
    {
        private readonly IDataProtector _protector;

        public SQLRND_PURCHASE_REQUISITION_MASTER(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<IEnumerable<RND_PURCHASE_REQUISITION_MASTER>> GetAllPurchaseRequisationAsync()
        {
            try
            {
                return await DenimDbContext.RND_PURCHASE_REQUISITION_MASTER
                    .Include(e => e.EMP)
                    .Include(e => e.RESEMP)
                    .Include(e => e.ORDER_NONavigation.SO)
                    .Include(e => e.ORDERNO_SNavigation)
                    .Where(c=>!c.OPT1.Equals("MENUALLY INSERTED BY MD. HASAN UZZAMAN NOYON"))
                    .Select(e => new RND_PURCHASE_REQUISITION_MASTER
                    {
                        INDSLID = e.INDSLID,
                        EncryptedId = _protector.Protect(e.INDSLID.ToString()),
                        RESEMPID = e.RESEMPID,
                        INDENT_SL_NO = e.INDENT_SL_NO,
                        INDSLNO = e.INDSLNO,
                        INDSLDATE = e.INDSLDATE,
                        YARN_FOR = e.YARN_FOR,
                        ORDER_NO = e.ORDER_NO,
                        STATUS = e.STATUS,
                        REMARKS = e.REMARKS,
                        OPT2 = e.ORDER_NONavigation.SO.SO_NO ?? e.ORDERNO_SNavigation.SDRF_NO,
                        EMP = new F_HRD_EMPLOYEE
                        {
                            FIRST_NAME = e.EMP.FIRST_NAME,
                            LAST_NAME = e.EMP.LAST_NAME
                        },
                        ORDER_NONavigation = new RND_PRODUCTION_ORDER
                        {
                            SO = new COM_EX_PI_DETAILS
                            {
                                SO_NO = e.ORDER_NONavigation.SO.SO_NO ?? e.ORDERNO_SNavigation.SDRF_NO
                            }
                        },
                        RESEMP = new F_HRD_EMPLOYEE
                        {
                            FIRST_NAME = e.RESEMP.FIRST_NAME
                        }
                    }).ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<RND_PURCHASE_REQUISITION_MASTER> GetSinglePurchaseRequisitionByIdAsync(int id)
        {
            try
            {
                return await DenimDbContext.RND_PURCHASE_REQUISITION_MASTER
                    .Include(e => e.EMP)
                    .Where(e => e.INDSLID == id)
                    .FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<RND_PURCHASE_REQUISITION_MASTER>> GetIndslidListWithStatusZero()
        {
            try
            {
                return await DenimDbContext.RND_PURCHASE_REQUISITION_MASTER
                    .Include(e => e.EMP)
                    .Select(e => new RND_PURCHASE_REQUISITION_MASTER
                    {
                        INDSLID = e.INDSLID,
                        RESEMPID = e.RESEMPID,
                        INDSLDATE = e.INDSLDATE,
                        YARN_FOR = e.YARN_FOR,
                        ORDER_NO = e.ORDER_NO,
                        STATUS = e.STATUS,
                        REMARKS = e.REMARKS,
                        EMP = e.EMP,
                        RESEMP = e.RESEMP
                    }).Where(e => e.STATUS.Equals("0"))
                    .ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<RndYarnRequisitionViewModel> GetPurchaseRequisitionById(int id)
        {
            try
            {
                var result = await DenimDbContext.RND_PURCHASE_REQUISITION_MASTER.ToListAsync();
                /*
                    .Include(e => e.F_YS_INDENT_DETAILS)
                    .Include(e => e.F_YS_INDENT_MASTER)

                    .GroupJoin(_denimDbContext.COM_EX_PI_DETAILS
                            .Include(e => e.STYLE),
                        f1 => f1.ORDER_NO,
                        f2 => f2.PIID,
                        (f1, f2) => new RndYarnRequisitionViewModel
                        {
                            RndPurchaseRequisitionMaster = f1,
                            ComExPiDetails = f2.FirstOrDefault()
                        })

                    .GroupJoin(_denimDbContext.COM_EX_PIMASTER,
                        f3 => f3.ComExPiDetails.PIID,
                        f4 => f4.PIID,
                        (f3, f4) => new RndYarnRequisitionViewModel
                        {
                            RndPurchaseRequisitionMaster = f3.RndPurchaseRequisitionMaster,
                            ComExPiDetails = f3.ComExPiDetails,
                            ComExPimasters = f4.ToList()
                        })
                    .Select(e => new RndYarnRequisitionViewModel
                    {
                        RndPurchaseRequisitionMaster = new RND_PURCHASE_REQUISITION_MASTER
                        {
                            INDSLID = e.RndPurchaseRequisitionMaster.INDSLID
                        },
                        ComExPiDetails = e.ComExPiDetails,
                        ComExPimaster = e.ComExPimasters.FirstOrDefault()
                    }).FirstOrDefaultAsync();*/


                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<RndYarnRequisitionViewModel> GetInitObjectsByAsync(RndYarnRequisitionViewModel rndYarnRequisitionViewModel)
        {
            var definedSectionIds = new[]
                {
                    157, // Planning & Co-ordination
                    171  // Research & Development (R&D)
                };

            rndYarnRequisitionViewModel.RndFabricinfos = await DenimDbContext.RND_FABRICINFO.Select(e => new RND_FABRICINFO
            {
                FABCODE = e.FABCODE,
                STYLE_NAME = e.STYLE_NAME
            }).OrderBy(e => e.STYLE_NAME).ToListAsync();

            rndYarnRequisitionViewModel.Yarnfroms = await DenimDbContext.YARNFROM.Select(e => new YARNFROM
            {
                YFID = e.YFID,
                TYPENAME = e.TYPENAME
            }).OrderBy(e => e.TYPENAME).ToListAsync();

            rndYarnRequisitionViewModel.FBasSectionList = await DenimDbContext.F_BAS_SECTION
                .Where(e => e.SECNAME.ToLower().Contains("weaving") || e.SECNAME.ToLower().Contains("warping"))
                .OrderBy(e => e.SECNAME)
                .ToListAsync();

            rndYarnRequisitionViewModel.BasYarnCountInfos = await DenimDbContext.BAS_YARN_COUNTINFO.OrderBy(e => e.RND_COUNTNAME).ToListAsync();

            rndYarnRequisitionViewModel.FHrEmployeeListRef = await DenimDbContext.F_HRD_EMPLOYEE
                .Where(c => c.EMPID.Equals(4128))
                .Select(c => new F_HRD_EMPLOYEE
                {
                    EMPID = c.EMPID,
                    FIRST_NAME = $"{c.EMPNO}, {c.FIRST_NAME} {c.LAST_NAME}"
                }).OrderBy(e => e.EMPID).ToListAsync();

            rndYarnRequisitionViewModel.PoList = await DenimDbContext.RND_PRODUCTION_ORDER
                .Select(c => new RND_PRODUCTION_ORDER
                {
                    POID = c.POID,
                    OPT1 = $"{c.SO.SO_NO}"
                }).OrderByDescending(e => e.POID).ToListAsync();

            rndYarnRequisitionViewModel.FHrEmployeeList = await DenimDbContext.F_HR_EMP_OFFICIALINFO
                .Include(e => e.SEC)
                .Include(c => c.EMP)
                .Where(c => definedSectionIds.Any(f => f.Equals(c.SECID)))
                .Select(c => new F_HRD_EMPLOYEE
                {
                    EMPID = c.EMP.EMPID,
                    FIRST_NAME = $"{c.EMP.EMPNO}, {c.EMP.FIRST_NAME} {c.EMP.LAST_NAME}"
                }).OrderBy(e => e.FIRST_NAME).ToListAsync();

            rndYarnRequisitionViewModel.FBasDepartments = await DenimDbContext.F_BAS_DEPARTMENT.OrderBy(e => e.DEPTNAME).ToListAsync();
            rndYarnRequisitionViewModel.BasUnits = await DenimDbContext.F_BAS_UNITS.OrderByDescending(e => e.UNAME.Contains("kg")).ThenBy(e => e.UNAME).ToListAsync();
            rndYarnRequisitionViewModel.YarnFor = StaticData.GetYarnFor();
            rndYarnRequisitionViewModel.YarnforList = await DenimDbContext.YARNFOR.OrderBy(e => e.YARNNAME).ToListAsync();

            rndYarnRequisitionViewModel.LotList = await DenimDbContext.BAS_YARN_LOTINFO
                .Select(e => new BAS_YARN_LOTINFO
                {
                    LOTID = e.LOTID,
                    LOTNO = $"Lot no: {e.LOTNO}, Brand name: {e.BRAND}"
                }).OrderBy(e => e.LOTNO).ToListAsync();

            rndYarnRequisitionViewModel.SlubList = await DenimDbContext.F_YS_SLUB_CODE.OrderBy(e => e.NAME).ToListAsync();
            rndYarnRequisitionViewModel.RawList = await DenimDbContext.F_YS_RAW_PER.OrderBy(e => e.RAWPER).ToListAsync();
            rndYarnRequisitionViewModel.CosPrecostingMasters = await DenimDbContext.COS_PRECOSTING_MASTER
                .Include(e => e.FABCODENavigation.STYLE_NAME)
                .Select(e => new
                {
                    CSID = e.CSID,
                    STYLENAME = $"{e.CSID}, {e.FABCODENavigation.STYLE_NAME}"
                }).OrderByDescending(e => e.CSID).ToListAsync();

            rndYarnRequisitionViewModel.BasBuyerinfos = await DenimDbContext.BAS_BUYERINFO.Select(e => new BAS_BUYERINFO
            {
                BUYERID = e.BUYERID,
                BUYER_NAME = e.BUYER_NAME
            }).OrderBy(e => e.BUYER_NAME).ToListAsync();

            return rndYarnRequisitionViewModel;
        }

        public async Task<RndYarnRequisitionViewModel> FindByIdIncludeAllAsync(int indslId)
        {
            var rndYarnRequisitionViewModel = await DenimDbContext.RND_PURCHASE_REQUISITION_MASTER
                .Include(e => e.ORDER_NONavigation.SO)
                .Include(e => e.F_YS_INDENT_DETAILS)
                .ThenInclude(e => e.BASCOUNTINFO)
                .Include(e => e.F_YS_INDENT_DETAILS)
                .ThenInclude(e => e.SEC)
                .Include(e => e.F_YS_INDENT_DETAILS)
                .ThenInclude(e => e.YARN_FORNavigation)
                .Include(e => e.F_YS_INDENT_DETAILS)
                .ThenInclude(e => e.FBasUnits)
                .Include(e => e.F_YS_INDENT_DETAILS)
                .ThenInclude(e => e.YARN_FROMNavigation)
                .Include(e => e.F_YS_INDENT_DETAILS)
                .ThenInclude(e => e.LOT)
                .Include(e => e.F_YS_INDENT_DETAILS)
                .ThenInclude(e => e.RAWNavigation)
                .Include(e => e.F_YS_INDENT_DETAILS)
                .ThenInclude(e => e.SLUB_CODENavigation)
                .Include(e => e.BasBuyerinfo)
                .Include(c=>c.F_YS_INDENT_MASTER)
                .ThenInclude(c=>c.COM_IMP_WORK_ORDER_MASTER)
                .Select(e => new RndYarnRequisitionViewModel
                {
                    RndPurchaseRequisitionMaster = new RND_PURCHASE_REQUISITION_MASTER
                    {
                        INDSLID = e.INDSLID,
                        EncryptedId = _protector.Protect(e.INDSLID.ToString()),
                        INDSLNO = e.INDSLNO,
                        INDENT_SL_NO = e.INDENT_SL_NO,
                        INDSLDATE = e.INDSLDATE,
                        RESEMPID = e.RESEMPID,
                        EMPID = e.EMPID,
                        YARN_FOR = e.YARN_FOR,
                        ORDER_NO = e.ORDER_NO,
                        ORDERNO_S = e.ORDERNO_S,
                        BUYERID = e.BUYERID,
                        OPT2 = e.OPT2,
                        OPT3 = e.OPT3,
                        OPT4 = e.OPT4,
                        REVISE_NO = e.REVISE_NO,
                        REVISE_DATE = e.REVISE_DATE,
                        REMARKS = e.REMARKS,
                        STATUS = e.STATUS,
                        COSTREFID = e.COSTREFID,
                        EMP = e.EMP,
                        RESEMP = e.RESEMP,
                        SAMPLE_L = e.SAMPLE_L,
                        ORDER_NONavigation = new RND_PRODUCTION_ORDER
                        {
                            SO = new COM_EX_PI_DETAILS
                            {
                                SO_NO = e.ORDER_NONavigation.SO.SO_NO
                            }
                        },
                        ORDERNO_SNavigation = e.ORDERNO_SNavigation,
                        COSTREF = e.COSTREF,
                        BasBuyerinfo = e.BasBuyerinfo,
                        FLAG = e.F_YS_INDENT_MASTER.Any(c=>c.COM_IMP_WORK_ORDER_MASTER.Any())
                    },
                    FysIndentDetailList = new List<F_YS_INDENT_DETAILS>(e.F_YS_INDENT_DETAILS.Select(f => new F_YS_INDENT_DETAILS
                    {
                        TRNSID = f.TRNSID,
                        INDSLID = f.INDSLID,
                        TRNSDATE = f.TRNSDATE,
                        INDID = f.INDID,
                        SECID = f.SECID,
                        PRODID = f.PRODID,
                        SLUB_CODE = f.SLUB_CODE,
                        RAW = f.RAW,
                        PREV_LOTID = f.PREV_LOTID,
                        STOCK_AMOUNT = f.STOCK_AMOUNT,
                        ORDER_QTY = f.ORDER_QTY,
                        YARN_FOR = f.YARN_FOR,
                        REMARKS = f.REMARKS,
                        ETR = f.ETR,
                        NO_OF_CONE = f.NO_OF_CONE,
                        UNIT = f.UNIT,
                        LAST_INDENT_NO = f.LAST_INDENT_NO,
                        LAST_INDENT_DATE = f.LAST_INDENT_DATE,
                        SEC = f.SEC,
                        BASCOUNTINFO = f.BASCOUNTINFO,
                        YARN_FORNavigation = f.YARN_FORNavigation,
                        YARN_FROMNavigation = f.YARN_FROMNavigation,
                        LOT = f.LOT,
                        RAWNavigation = f.RAWNavigation,
                        SLUB_CODENavigation = f.SLUB_CODENavigation,
                        FBasUnits = f.FBasUnits
                    }))
                }).FirstOrDefaultAsync(e => e.RndPurchaseRequisitionMaster.INDSLID.Equals(indslId));

            return rndYarnRequisitionViewModel;
        }

        public async Task<object> GetCountNameByOrderNoAsync(RndYarnRequisitionViewModel rndYarnRequisition)
        {
            try
            {
                if (rndYarnRequisition.RndPurchaseRequisitionMaster.YARN_FOR.ToLower().Contains("sample") &&
                    rndYarnRequisition.RndPurchaseRequisitionMaster.ORDERNO_S != null)
                {
                    var rndProductionOrders = await DenimDbContext.RND_PRODUCTION_ORDER
                        .Include(e => e.RS.SDRF.RND_SAMPLE_INFO_DYEING)
                        .ThenInclude(e => e.RND_SAMPLE_INFO_DETAILS)
                        .ThenInclude(e => e.COUNT) // WARP
                        .Include(e => e.RS.PL_SAMPLE_PROG_SETUP)
                        .ThenInclude(e => e.RND_SAMPLE_INFO_WEAVING)
                        .ThenInclude(e => e.RND_SAMPLE_INFO_WEAVING_DETAILS)
                        .ThenInclude(e => e.COUNT) // WEFT
                        .Where(e => e.RSNO.Equals(rndYarnRequisition.RndPurchaseRequisitionMaster.ORDERNO_S)).ToListAsync();

                    return rndProductionOrders;
                }

                if (rndYarnRequisition.RndPurchaseRequisitionMaster.YARN_FOR.ToLower().Contains("export") &&
                    rndYarnRequisition.RndPurchaseRequisitionMaster.ORDER_NO != null)
                {
                    var rs = await DenimDbContext.RND_PRODUCTION_ORDER
                        .Include(e => e.SO.STYLE.FABCODENavigation.RND_FABRIC_COUNTINFO)
                        .ThenInclude(e => e.COUNT)
                        .Where(e => e.ORDERNO.Equals(rndYarnRequisition.RndPurchaseRequisitionMaster.ORDER_NO))
                        .ToListAsync();

                    return rs;
                }

                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        public async Task<string> GetLastIndentNoAsync(string yarnFor)
        {
            string piNo;

            var result = await DenimDbContext.RND_PURCHASE_REQUISITION_MASTER
                .Where(d => d.INDSLNO.Contains(yarnFor))
                .OrderByDescending(c => c.INDSLNO)
                .Select(c => c.INDSLNO).FirstOrDefaultAsync();
            var year = DateTime.Now.Year % 100;

            if (result != null)
            {
                var resultArray = result.Split("-");
                if (int.Parse(resultArray[1]) < year)
                {
                    piNo = $"{yarnFor}-{year}-{"1".PadLeft(4, '0')}";
                }
                else
                {
                    int.TryParse(new string(resultArray[2].SkipWhile(x => !char.IsDigit(x)).TakeWhile(char.IsDigit).ToArray()), out var currentNumber);

                    piNo = $"{yarnFor}-{year}-{(currentNumber + 1).ToString().PadLeft(4, '0')}";
                }
            }
            else
            {
                piNo = $"{yarnFor}-{year}-{"1".PadLeft(4, '0')}";
            }

            return piNo;
        }
    }
}
