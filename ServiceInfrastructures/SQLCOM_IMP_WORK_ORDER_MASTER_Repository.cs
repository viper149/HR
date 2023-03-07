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
    public class SQLCOM_IMP_WORK_ORDER_MASTER_Repository : BaseRepository<COM_IMP_WORK_ORDER_MASTER>, ICOM_IMP_WORK_ORDER_MASTER
    {
        private readonly IDataProtector _protector;

        public SQLCOM_IMP_WORK_ORDER_MASTER_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<IEnumerable<COM_IMP_WORK_ORDER_MASTER>> GetAllComImpWorkOrderAsync()
        {
            return await DenimDbContext.COM_IMP_WORK_ORDER_MASTER
                .Include(d => d.SUPP)
                .Include(d => d.IND.INDSL)
                .Where(d=> !d.IS_REVISED)
                .Select(d => new COM_IMP_WORK_ORDER_MASTER
                {
                    WOID = d.WOID,
                    EncryptedId = _protector.Protect(d.WOID.ToString()),
                    WODATE = d.WODATE,
                    CONTNO = d.CONTNO,
                    REMARKS = d.REMARKS,
                    VALDATE = d.VALDATE,
                    BTL_APPROVE = d.BTL_APPROVE,
                    FIN_APPROVE = d.FIN_APPROVE,
                    INDID = d.INDID,
                    SUPPID = d.SUPPID,
                    SUPP = new BAS_SUPPLIERINFO
                    {
                        SUPPNAME = d.SUPP.SUPPNAME
                    },
                    IND = new F_YS_INDENT_MASTER
                    {
                        INDSL = new RND_PURCHASE_REQUISITION_MASTER
                        {
                            INDSLNO = d.IND.INDSL.INDSLNO
                        }
                    }
                }).ToListAsync();
        }

        public async Task<ComImpWorkOrderViewModel> GetInitObjByAsync(ComImpWorkOrderViewModel comImpWorkOrderViewModel)
        {
            var supplier = new Dictionary<int, string>
            {
                {591, "KAMAL YARN LIMITED"},
                {800, "BADSHA TEXTILES LTD."},
                {2658, "BADSHA TEXTILE LTD.(EXT.)"},
                {2659, "BADSHA TEXTILES LTD.(OE)"}
            };

            comImpWorkOrderViewModel.BasSupplierinfoList = await DenimDbContext.BAS_SUPPLIERINFO
                .Where(d => supplier.ContainsKey(d.SUPPID) || supplier.ContainsValue(d.SUPPNAME))
                .Select(d => new BAS_SUPPLIERINFO
                {
                    SUPPID = d.SUPPID,
                    SUPPNAME = d.SUPPNAME
                }).OrderBy(d => d.SUPPNAME).ToListAsync();


            comImpWorkOrderViewModel.FYsIndentMasterList = await DenimDbContext.F_YS_INDENT_MASTER
                .Include(d => d.INDSL)
                .Where(d => d.INDSL.INDSLNO.Contains("-22-") && (!DenimDbContext.COM_IMP_WORK_ORDER_MASTER.Any(f => f.INDID.Equals(d.INDID)) || d.INDSL.IS_REVISED))
                .Select(d => new F_YS_INDENT_MASTER
                {
                    INDID = d.INDID,
                    INDSL = new RND_PURCHASE_REQUISITION_MASTER
                    {
                        INDSLNO = d.INDSL.INDSLNO
                    }
                }).OrderBy(d => d.INDID).ToListAsync();

            return comImpWorkOrderViewModel;
        }

        public async Task<int> GetLastWOID()
        {
            var comImpWorkOrderMasters = DenimDbContext.COM_IMP_WORK_ORDER_MASTER.AsQueryable();
            return comImpWorkOrderMasters.Any() ? await comImpWorkOrderMasters.MaxAsync(d => d.WOID) : 10000;
        }

        public async Task<ComImpWorkOrderViewModel> GetInitDetailsObjByAsync(ComImpWorkOrderViewModel comImpWorkOrderViewModel)
        {
            foreach (var item in comImpWorkOrderViewModel.ComImpWorkOrderDetailsList)
            {
                item.INDD = await DenimDbContext.F_YS_INDENT_DETAILS
                    .Include(d => d.IND.INDSL.ORDER_NONavigation.SO.STYLE)
                    .Include(d => d.BASCOUNTINFO)
                    .Include(d => d.RAWNavigation)
                    .Where(d => d.TRNSID.Equals(item.COUNTID))
                    .Select(d => new F_YS_INDENT_DETAILS
                    {
                        BASCOUNTINFO = new BAS_YARN_COUNTINFO
                        {
                            COUNTID = d.TRNSID,
                            //RND_COUNTNAME = (d.BASCOUNTINFO.RND_COUNTNAME != null) ? (d.RAWNavigation.RAWPER != null) ? $"{d.BASCOUNTINFO.RND_COUNTNAME} {(_denimDbContext.RND_FABRIC_COUNTINFO.Where(g => g.FABCODE.Equals(d.IND.INDSL.ORDER_NONavigation.SO.STYLE.FABCODE) && g.COUNTID.Equals(d.BASCOUNTINFO.COUNTID) && g.YARNFOR.Equals(d.YARN_FOR)).Select(g => g.YARNTYPE).FirstOrDefault()) ?? ""} ({d.RAWNavigation.RAWPER})" : d.BASCOUNTINFO.RND_COUNTNAME : d.BASCOUNTINFO.COUNTNAME,
                            RND_COUNTNAME = (d.BASCOUNTINFO.RND_COUNTNAME != null) ? (d.RAWNavigation.RAWPER != null) ? $"{d.BASCOUNTINFO.RND_COUNTNAME} ({d.RAWNavigation.RAWPER})" : d.BASCOUNTINFO.RND_COUNTNAME : d.BASCOUNTINFO.COUNTNAME,
                            UNIT = d.BASCOUNTINFO.UNIT
                        }
                    }).FirstOrDefaultAsync();

                item.LOT = await DenimDbContext.BAS_YARN_LOTINFO
                    .Where(d => d.LOTID.Equals(item.LOTID))
                    .Select(d => new BAS_YARN_LOTINFO
                    {
                        LOTID = (int?)d.LOTID ?? 0,
                        LOTNO = d.LOTNO ?? ""
                    }).FirstOrDefaultAsync();
            }

            return comImpWorkOrderViewModel;
        }

        public async Task<ComImpWorkOrderViewModel> FindByIdIncludeAllAsync(int woId)
        {
            try
            {
                var result = await DenimDbContext.COM_IMP_WORK_ORDER_MASTER
                    .Include(d => d.IND.INDSL.ORDER_NONavigation.SO.STYLE.FABCODENavigation)
                    .Include(d => d.SUPP)
                    .Include(d => d.COM_IMP_WORK_ORDER_DETAILS)
                    .ThenInclude(d => d.INDD.BASCOUNTINFO)
                    .Include(d => d.COM_IMP_WORK_ORDER_DETAILS)
                    .ThenInclude(d => d.INDD.RAWNavigation)
                    .Include(d => d.COM_IMP_WORK_ORDER_DETAILS)
                    .ThenInclude(d => d.INDD.IND.INDSL.ORDER_NONavigation.SO.STYLE)
                    .Include(d => d.COM_IMP_WORK_ORDER_DETAILS)
                    .ThenInclude(d => d.LOT)
                    .Include(d => d.IND.INDSL.ORDERNO_SNavigation.RND_ANALYSIS_SHEET)
                    .Where(d => d.WOID.Equals(woId))
                    .Select(d => new ComImpWorkOrderViewModel
                    {
                        ComImpWorkOrderMaster = new COM_IMP_WORK_ORDER_MASTER
                        {
                            WOID = d.WOID,
                            EncryptedId = _protector.Protect(d.WOID.ToString()),
                            WODATE = d.WODATE,
                            CONTNO = d.CONTNO,
                            SUPPID = d.SUPPID,
                            INDID = d.INDID,
                            REMARKS = d.REMARKS,
                            VALDATE = d.VALDATE,
                            BTL_APPROVE = d.BTL_APPROVE,
                            FIN_APPROVE = d.FIN_APPROVE,
                            SUPP = new BAS_SUPPLIERINFO
                            {
                                SUPPID = d.SUPP.SUPPID,
                                SUPPNAME = d.SUPP.SUPPNAME
                            },
                            IND = new F_YS_INDENT_MASTER
                            {
                                INDID = d.IND.INDID,
                                INDSL = new RND_PURCHASE_REQUISITION_MASTER
                                {
                                    INDSLNO = d.IND.INDSL.INDSLNO,
                                    YARN_FOR = d.IND.INDSL.YARN_FOR,
                                    SAMPLE_L = d.IND.INDSL.SAMPLE_L,
                                    ORDER_NONavigation = new RND_PRODUCTION_ORDER
                                    {
                                        SO = new COM_EX_PI_DETAILS
                                        {
                                            SO_NO = d.IND.INDSL.ORDER_NONavigation.SO.SO_NO,
                                            QTY = d.IND.INDSL.ORDER_NONavigation.SO.QTY,
                                            STYLE = new COM_EX_FABSTYLE
                                            {
                                                FABCODENavigation = new RND_FABRICINFO
                                                {
                                                    STYLE_NAME = d.IND.INDSL.ORDER_NONavigation.SO.STYLE.FABCODENavigation.STYLE_NAME
                                                }
                                            }
                                        }
                                    },
                                    ORDERNO_SNavigation = new MKT_SDRF_INFO
                                    {
                                        RND_ANALYSIS_SHEET = new RND_ANALYSIS_SHEET
                                        {
                                            MKT_QUERY_NO = d.IND.INDSL.ORDERNO_SNavigation.RND_ANALYSIS_SHEET.MKT_QUERY_NO ?? d.IND.INDSL.ORDERNO_SNavigation.OPTION5
                                        }
                                    }
                                }
                            }
                        },
                        ComImpWorkOrderDetailsList = d.COM_IMP_WORK_ORDER_DETAILS
                            .Select(f => new COM_IMP_WORK_ORDER_DETAILS
                            {
                                TRANSID = f.TRANSID,
                                WOID = f.WOID,
                                COUNTID = f.COUNTID,
                                LOTID = f.LOTID,
                                QTY = f.QTY,
                                UPRICE = f.UPRICE,
                                TOTAL = f.TOTAL,
                                REMARKS = f.REMARKS,
                                NOTES = f.NOTES,
                                INDD = new F_YS_INDENT_DETAILS
                                {
                                    BASCOUNTINFO = new BAS_YARN_COUNTINFO
                                    {
                                        COUNTID = f.INDD.TRNSID,
                                        //RND_COUNTNAME = $"{f.INDD.BASCOUNTINFO.RND_COUNTNAME ?? f.INDD.BASCOUNTINFO.COUNTNAME} ({f.INDD.RAWNavigation.RAWPER})",

                                        RND_COUNTNAME = (f.INDD.BASCOUNTINFO.RND_COUNTNAME != null) ? (f.INDD.RAWNavigation.RAWPER != null) ? $"{f.INDD.BASCOUNTINFO.RND_COUNTNAME} ({f.INDD.RAWNavigation.RAWPER})" : f.INDD.BASCOUNTINFO.RND_COUNTNAME : f.INDD.BASCOUNTINFO.COUNTNAME,

                                        UNIT = f.INDD.BASCOUNTINFO.UNIT
                                    }
                                },
                                
                                LOT = new BAS_YARN_LOTINFO
                                {
                                    LOTID = (int?)f.LOT.LOTID ?? 0,
                                    LOTNO = f.LOT.LOTNO ?? ""
                                }
                            }).ToList()
                    }).FirstOrDefaultAsync();
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        public async Task<string> GetLastContNoAsync()
        {
            string contNo;
            var result = await DenimDbContext.COM_IMP_WORK_ORDER_MASTER
                .Where(d => d.CONTNO.Contains("PDL"))
                .OrderByDescending(d => d.CONTNO).
                Select(d => d.CONTNO).FirstOrDefaultAsync();
            var year = DateTime.Now.Year % 100;

            if (result != null)
            {
                var resultArray = result.Split("-");
                if (int.Parse(resultArray[1]) < year)
                {
                    contNo = $"PDL-{year}-{"1".PadLeft(4, '0')}";
                }
                else
                {
                    int.TryParse(new string(resultArray[2].SkipWhile(x => !char.IsDigit(x)).TakeWhile(char.IsDigit).ToArray()), out var currentNumber);

                    contNo = $"PDL-{year}-{(currentNumber + 1).ToString().PadLeft(4, '0')}";
                }
            }
            else
            {
                contNo = $"PDL-{year}-{"1".PadLeft(4, '0')}";
            }

            return contNo;
        }

        public async Task<int> GetIndslIdByInd(int indid)
        {
            return await DenimDbContext.F_YS_INDENT_MASTER
                .Include(d => d.INDSL)
                .Where(d=> d.INDID.Equals(indid))
                .Select(d => d.INDSL.INDSLID)
                .FirstOrDefaultAsync();
        }

        public async Task<COM_IMP_WORK_ORDER_MASTER> FindPreviousWorkOrder(int? indid)
        {
            return await DenimDbContext.COM_IMP_WORK_ORDER_MASTER
                .FirstOrDefaultAsync(d => d.INDID.Equals(indid));
        }
    }
}
