using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels;
using DenimERP.ViewModels.AccountFinance;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLACC_EXPORT_DOMASTER_Repository : BaseRepository<ACC_EXPORT_DOMASTER>, IACC_EXPORT_DOMASTER
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataProtector _protector;

        public SQLACC_EXPORT_DOMASTER_Repository(DenimDbContext denimDbContext,
            UserManager<ApplicationUser> userManager,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _userManager = userManager;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<COM_EX_LCINFO> GetLCDetailsByLCNoAsync(int? lcId)
        {
            return await DenimDbContext.COM_EX_LCINFO
                .Include(d => d.BUYER)
                .Where(d => d.LCID.Equals(lcId))
                .Select(d => new COM_EX_LCINFO
                {
                    LCID = d.LCID,
                    LCDATE = d.LCDATE,
                    BUYERID = d.BUYERID,
                    FILENO = d.FILENO,

                    BUYER = new BAS_BUYERINFO
                    {
                        BUYERID = d.BUYER.BUYERID,
                        BUYER_NAME = d.BUYER.BUYER_NAME,
                        ADDRESS = d.BUYER.ADDRESS
                    }
                }).FirstOrDefaultAsync();
        }

        public async Task<string> GetLastDoNoAsync()
        {
            var result = await DenimDbContext.ACC_EXPORT_DOMASTER
                .OrderByDescending(c => c.DONO)
                .Select(c => c.DONO)
                .ToListAsync();
            if (result == null) return null;
            var doNo = int.Parse(result.FirstOrDefault() ?? string.Empty) + 1;
            return doNo.ToString();
        }

        public async Task<bool> FindByDoNoInUseAsync(string doNo)
        {
            return await DenimDbContext.ACC_EXPORT_DOMASTER.Where(c => c.DONO == doNo).AnyAsync();
        }

        public async Task<DetailsAccExportDoMasterViewModel> FindByDoTrnsIdAsync(int doTrnsId)
        {
            return await DenimDbContext.ACC_EXPORT_DOMASTER
                .GroupJoin(DenimDbContext.COM_EX_LCINFO
                        .Include(e => e.BUYER),
                    f1 => f1.LCID,
                    f2 => f2.LCID,
                    (f1, f2) => new
                    {
                        F1 = f1,
                        F2 = f2.FirstOrDefault()
                    })
                .GroupJoin(DenimDbContext.Users,
                    f3 => f3.F1.USRID,
                    f4 => f4.Id,
                    (f3, f4) => new
                    {
                        F3 = f3,
                        F4 = f4
                    })
                .GroupJoin(DenimDbContext.ACC_EXPORT_DODETAILS
                        .Include(e => e.PI)
                        .GroupJoin(DenimDbContext.COM_EX_FABSTYLE,
                            g1 => g1.STYLEID,
                            g2 => g2.STYLEID,
                            (g1, g2) => new ACC_EXPORT_DODETAILS
                            {
                                TRNSID = g1.TRNSID,
                                TRNSDATE = g1.TRNSDATE,
                                DONO = g1.DONO,
                                PIID = g1.PIID,
                                PINO = g1.PINO,
                                STYLEID = g1.STYLEID,
                                STYLENAME = g2.FirstOrDefault().STYLENAME,
                                QTY = g1.QTY,
                                RATE = g1.RATE,
                                AMOUNT = g1.AMOUNT,
                                ORACLE_DO_DELIVERY = g1.ORACLE_DO_DELIVERY,
                                REMARKS = g1.REMARKS,
                                USRID = g1.USRID,
                                PI = g1.PI
                            })
                    ,
                    f5 => f5.F3.F1.TRNSID,
                    f6 => f6.DONO,
                    (f5, f6) => new DetailsAccExportDoMasterViewModel
                    {
                        ExtendAccExportDoMasterViewModel = new ExtendAccExportDoMasterViewModel
                        {
                            AccExportDomaster = new ACC_EXPORT_DOMASTER
                            {
                                TRNSID = f5.F3.F1.TRNSID,
                                EncryptedId = _protector.Protect(f5.F3.F1.TRNSID.ToString()),
                                AUDITBY = f5.F4.FirstOrDefault().UserName,
                                AUDITON = f5.F3.F1.AUDITON,
                                COMMENTS = f5.F3.F1.COMMENTS,
                                REMARKS = f5.F3.F1.REMARKS,
                                DONO = f5.F3.F1.DONO,
                                DODATE = f5.F3.F1.DODATE
                            },
                            ComExLcinfo = new COM_EX_LCINFO
                            {
                                LCNO = f5.F3.F2.LCNO,
                                LCDATE = f5.F3.F2.LCDATE,
                                FILENO = f5.F3.F2.FILENO,
                                BUYER = f5.F3.F2.BUYER
                            }
                        },
                        AccExportDodetailses = f6.ToList()
                    })
                .Where(e => e.ExtendAccExportDoMasterViewModel.AccExportDomaster.TRNSID.Equals(doTrnsId))
                .FirstOrDefaultAsync();
        }

        public async Task<List<ACC_EXPORT_DOMASTER>> GetForDataTableByAsync()
        {
            return await DenimDbContext.ACC_EXPORT_DOMASTER
                .Include(e => e.ComExLcInfo)
                .GroupJoin(DenimDbContext.Users,
                    f1 => f1.USRID,
                    f2 => f2.Id,
                    (f1, f2) => new ACC_EXPORT_DOMASTER
                    {
                        TRNSID = f1.TRNSID,
                        EncryptedId = _protector.Protect(f1.TRNSID.ToString()),
                        DONO = f1.DONO,
                        DOEX = f1.DOEX,
                        DODATE = f1.DODATE,
                        LCNO = $"{f1.ComExLcInfo.LCNO}",
                        ComExLcInfo = new COM_EX_LCINFO
                        {
                            LCNO = $"{f1.ComExLcInfo.LCNO}"
                        },
                        AUDITBY = $"{f2.FirstOrDefault().UserName}",
                        AUDITON = f1.AUDITON,
                        COMMENTS = f1.COMMENTS,
                        IS_CANCELLED = f1.IS_CANCELLED,
                        REMARKS = f1.REMARKS
                    }).ToListAsync();
        }

        public async Task<ACC_EXPORT_DOMASTER> GetDoDetails(int doId)
        {
            return await DenimDbContext.ACC_EXPORT_DOMASTER
                .Include(c => c.ACC_EXPORT_DODETAILS)
                .Include(c => c.ComExLcInfo)
                .ThenInclude(c => c.COM_EX_LCDETAILS)
                .ThenInclude(c => c.PI)
                .Include(c => c.ComExLcInfo)
                .ThenInclude(c => c.BUYER)
                .Where(c => c.TRNSID.Equals(doId))
                .FirstOrDefaultAsync();
        }

        public async Task<COM_EX_LCINFO> GetFabStyleByLcIdAsync(int lcId)
        {
            return await DenimDbContext.COM_EX_LCINFO
                .Include(d => d.COM_EX_LCDETAILS)
                .ThenInclude(d => d.PI)
                .ThenInclude(d => d.COM_EX_PI_DETAILS)
                .ThenInclude(d => d.STYLE.FABCODENavigation)
                .Where(d => d.LCID.Equals(lcId) && d.COM_EX_LCDETAILS.All(c => c.PI.COM_EX_PI_DETAILS.All(m => m.SO_STATUS)))
                .FirstOrDefaultAsync();
        }

        public async Task<AccExportDoMasterViewModel> FindByIdIncludeAllAsync(int trnsId)
        {
            return await DenimDbContext.ACC_EXPORT_DOMASTER
                    .Include(e => e.ComExLcInfo.BUYER)
                    .Include(e => e.ACC_EXPORT_DODETAILS)
                    .ThenInclude(e => e.PI)
                    .Include(e => e.ACC_EXPORT_DODETAILS)
                    .ThenInclude(d => d.STYLE.FABCODENavigation)
                    .Where(e => e.TRNSID.Equals(trnsId))
                    .GroupJoin(DenimDbContext.Users,
                        f1 => f1.AUDITBY ?? "",
                        f2 => f2.Id,
                        (f1, f2) => new AccExportDoMasterViewModel
                        {
                            ACC_EXPORT_DOMASTER = new ACC_EXPORT_DOMASTER
                            {
                                TRNSID = f1.TRNSID,
                                EncryptedId = _protector.Protect(f1.TRNSID.ToString()),
                                DONO = f1.DONO,
                                DODATE = f1.DODATE,
                                LCID = f1.LCID,
                                REMARKS = f1.REMARKS,
                                AUDITBY = $"{f2.FirstOrDefault().UserName}",
                                AUDITON = f1.AUDITON,
                                COMMENTS = f1.COMMENTS,
                                IS_CANCELLED = f1.IS_CANCELLED,
                                ComExLcInfo = new COM_EX_LCINFO
                                {
                                    LCNO = $"{f1.ComExLcInfo.LCNO}",
                                    LCDATE = f1.ComExLcInfo != null ? f1.ComExLcInfo.LCDATE : null,
                                    FILENO = $"{f1.ComExLcInfo.FILENO}",
                                    BUYER = new BAS_BUYERINFO
                                    {
                                        BUYER_NAME = $"{f1.ComExLcInfo.BUYER.BUYER_NAME}",
                                        ADDRESS = $"{f1.ComExLcInfo.BUYER.ADDRESS}"
                                    }
                                }
                            },
                            aCC_EXPORT_DODETAILSList = f1.ACC_EXPORT_DODETAILS.Select(f => new ACC_EXPORT_DODETAILS
                            {
                                TRNSID = f.TRNSID,
                                DONO = f.DONO,
                                PIID = f.PIID,
                                STYLEID = f.STYLEID,
                                QTY = f.QTY,
                                RATE = f.RATE,
                                AMOUNT = f.AMOUNT,
                                ORACLE_DO_DELIVERY = f.ORACLE_DO_DELIVERY,
                                REMARKS = f.REMARKS,
                                PI = new COM_EX_PIMASTER
                                {
                                    PIID = f.PI != null ? f.PI.PIID : 0,
                                    PINO = $"{f.PI.PINO}"
                                },
                                STYLE = new COM_EX_FABSTYLE
                                {
                                    STYLEID = f.STYLE != null ? f.STYLE.STYLEID : 0,
                                    STYLENAME = $"{f.STYLE.STYLENAME}",
                                    FABCODENavigation = f.STYLE.FABCODENavigation
                                }
                            }).ToList()
                        }).FirstOrDefaultAsync();
        }
    }
}