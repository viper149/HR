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
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLACC_LOCAL_DOMASTER_Repository : BaseRepository<ACC_LOCAL_DOMASTER>, IACC_LOCAL_DOMASTER
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataProtector _protector;

        public SQLACC_LOCAL_DOMASTER_Repository(DenimDbContext denimDbContext,
            UserManager<ApplicationUser> userManager,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _userManager = userManager;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<IEnumerable<ACC_LOCAL_DOMASTER>> GetAccDoMasterAllAsync()
        {
            var accLocalDomasters = await DenimDbContext.ACC_LOCAL_DOMASTER
                .Include(e => e.ComExScinfo)
                .GroupJoin(DenimDbContext.Users,
                    f1 => f1.AUDITBY,
                    f2 => f2.Id,
                    (f1, f2) => new ACC_LOCAL_DOMASTER
                    {
                        EncryptedId = _protector.Protect(f1.TRNSID.ToString()),
                        DONO = f1.DONO,
                        DODATE = f1.DODATE,
                        AUDITBY = f2.FirstOrDefault().UserName,
                        AUDITON = f1.AUDITON,
                        COMMENTS = f1.COMMENTS,
                        REMARKS = f1.REMARKS,
                        ComExScinfo = new COM_EX_SCINFO
                        {
                            SCNO = $"{f1.ComExScinfo.SCNO}"
                        }
                    }).ToListAsync();

            return accLocalDomasters;
        }

        public async Task<string> GetLastDoNoAsync()
        {
            var result = await DenimDbContext.ACC_LOCAL_DOMASTER
                .OrderByDescending(c => c.DONO)
                .Select(c => c.DONO)
                .ToListAsync();

            if (result == null) return null;

            var doNo = int.Parse(result.FirstOrDefault() ?? "0647") + 1;
            return $"{doNo}".PadLeft(4, '0');
        }

        public async Task<bool> FindByDoNoInUseAsync(string doNo)
        {
            return await DenimDbContext.ACC_EXPORT_DOMASTER.AnyAsync(e => e.DONO.ToLower().Equals(doNo.ToLower()));
        }

        public async Task<AccLocalDoMasterViewModel> GetInitObjectsByAsync(AccLocalDoMasterViewModel accLocalDoMasterViewModel)
        {
            if (accLocalDoMasterViewModel.ACC_LOCAL_DOMASTER.SCID != null)
            {
                accLocalDoMasterViewModel.COM_EX_SCINFOs = await DenimDbContext.COM_EX_SCINFO
                    .OrderBy(e => e.SCNO)
                    .Where(e => e.SCID.Equals(accLocalDoMasterViewModel.ACC_LOCAL_DOMASTER.SCID))
                    .ToListAsync();
            }

            if (accLocalDoMasterViewModel.ACC_LOCAL_DOMASTER.LCID != null)
            {
                accLocalDoMasterViewModel.ComExLcinfos = await DenimDbContext.COM_EX_LCINFO
                    .Where(e => e.LCID.Equals(accLocalDoMasterViewModel.ACC_LOCAL_DOMASTER.LCID))
                    .ToListAsync();
            }

            return accLocalDoMasterViewModel;
        }

        public async Task<AccLocalDoMasterViewModel> GetInitObjectsForDetailsTable(AccLocalDoMasterViewModel accLocalDoMasterViewModel)
        {
            if (accLocalDoMasterViewModel.ACC_LOCAL_DOMASTER.IS_COMPENSATION)
            {
                foreach (var item in accLocalDoMasterViewModel.ACC_LOCAL_DODETAILs)
                {
                    item.ComExPiDetails = await DenimDbContext.COM_EX_PI_DETAILS
                        .Include(e => e.STYLE.FABCODENavigation)
                        .FirstOrDefaultAsync(e => e.TRNSID.Equals(item.STYLEID));
                }
            }
            else
            {
                foreach (var item in accLocalDoMasterViewModel.ACC_LOCAL_DODETAILs)
                {
                    item.STYLE = await DenimDbContext.COM_EX_SCDETAILS
                        .Include(e => e.STYLE.FABCODENavigation)
                        .FirstOrDefaultAsync(e => e.TRNSID.Equals(item.STYLEID));
                }
            }

            return accLocalDoMasterViewModel;
        }

        public async Task<AccLocalDoMasterViewModel> GetLocalSaleOrdersByAsync(string search, int page)
        {
            var accLocalDoMasterViewModel = new AccLocalDoMasterViewModel();

            if (!string.IsNullOrEmpty(search))
            {
                accLocalDoMasterViewModel.COM_EX_SCINFOs = await DenimDbContext.COM_EX_SCINFO
                    .OrderByDescending(e => e.SCNO)
                    .Select(e => new COM_EX_SCINFO
                    {
                        SCID = e.SCID,
                        SCNO = e.SCNO
                    }).Where(e => e.SCNO.ToLower().Contains(search.ToLower())).ToListAsync();
            }
            else
            {
                accLocalDoMasterViewModel.COM_EX_SCINFOs = await DenimDbContext.COM_EX_SCINFO
                    .OrderByDescending(e => e.SCNO)
                    .Select(e => new COM_EX_SCINFO
                    {
                        SCID = e.SCID,
                        SCNO = e.SCNO
                    }).ToListAsync();
            }

            return accLocalDoMasterViewModel;
        }

        public async Task<AccLocalDoMasterViewModel> GetCommercialExportLCByAsync(AccLocalDoMasterViewModel accLocalDoMasterViewModel, string search, int page)
        {
            if (accLocalDoMasterViewModel.ACC_LOCAL_DOMASTER.IS_COMPENSATION)
            {
                if (!string.IsNullOrEmpty(search))
                {
                    accLocalDoMasterViewModel.ComExLcinfos = await DenimDbContext.COM_EX_LCINFO
                        .Include(d=>d.COM_EX_LCDETAILS)
                        .OrderBy(e => e.LCNO)
                        .Select(e => new COM_EX_LCINFO
                        {
                            LCID = e.LCID,
                            LCNO = $"{e.LCNO} ({e.FILENO})"
                        }).Where(e => e.LCNO.ToLower().Contains(search.ToLower())).ToListAsync();
                }
                else
                {
                    accLocalDoMasterViewModel.ComExLcinfos = await DenimDbContext.COM_EX_LCINFO
                        .OrderBy(e => e.LCNO)
                        .Select(e => new COM_EX_LCINFO
                        {
                            LCID = e.LCID,
                            LCNO = $"{e.LCNO} ({e.FILENO})"
                        }).ToListAsync();
                }
            }

            return accLocalDoMasterViewModel;
        }

        public async Task<IEnumerable<COM_EX_LCDETAILS>> GetGetStylesLcWiseByAsync(int lcId)
        {
            return await DenimDbContext.COM_EX_LCDETAILS
                .Include(d => d.LC)
                .Include(d => d.PI.COM_EX_PI_DETAILS)
                .ThenInclude(d => d.STYLE.FABCODENavigation)
                .Where(d => d.LC.LCID.Equals(lcId))
                .Select(d => new COM_EX_LCDETAILS
                {
                    PI = new COM_EX_PIMASTER
                    {
                        COM_EX_PI_DETAILS = d.PI.COM_EX_PI_DETAILS.Select(e=> new COM_EX_PI_DETAILS
                        {
                            TRNSID = e.TRNSID,
                            STYLE = new COM_EX_FABSTYLE
                            {
                                FABCODENavigation = new RND_FABRICINFO
                                {
                                    STYLE_NAME = $"PI: {e.PIMASTER.PINO}, Style: {e.STYLE.FABCODENavigation.STYLE_NAME}"
                                }
                            }
                        }).ToList()
                    }
                }).ToListAsync();


            //.Select(d => new COM_EX_LCDETAILS
            //{
            //    TRNSID = g.TRNSID,
            //    STYLE = new COM_EX_FABSTYLE
            //    {
            //        STYLENAME = $"PI: {g.PINO}, {g.STYLE.FABCODENavigation.STYLE_NAME}"
            //    }
            //}))).FirstOrDefaultAsync();
        }

        public async Task<dynamic> GetOtherInfoByAsync(AccLocalDoMasterViewModel accLocalDoMasterViewModel)
        {
            if (accLocalDoMasterViewModel.ACC_LOCAL_DOMASTER.IS_COMPENSATION)
            {
                return await DenimDbContext.COM_EX_PI_DETAILS
                    .Where(e => e.TRNSID.Equals(accLocalDoMasterViewModel.ACC_LOCAL_DODETAILS.STYLEID))
                    .Select(e => new
                    {
                        QTY = e.QTY,
                        UNITPRICE = e.UNITPRICE,
                        TOTAL = e.TOTAL,
                        REMARKS = e.REMARKS
                    }).FirstOrDefaultAsync();
            }
            else
            {
                return await DenimDbContext.COM_EX_SCDETAILS
                    .Where(e => e.TRNSID.Equals(accLocalDoMasterViewModel.ACC_LOCAL_DODETAILS.STYLEID))
                    .Select(e => new
                    {
                        QTY = e.QTY,
                        UNITPRICE = e.RATE,
                        TOTAL = e.AMOUNT,
                        REMARKS = e.REMARKS
                    }).FirstOrDefaultAsync();
            }
        }

        public async Task<AccLocalDoMasterViewModel> FindByIdIncludeAllByAsync(int trnsId)
        {
            return await DenimDbContext.ACC_LOCAL_DOMASTER
                .Include(e => e.ACC_LOCAL_DODETAILS)
                .ThenInclude(e => e.ComExPiDetails.STYLE.FABCODENavigation)
                .Include(e => e.ComExScinfo.BUYER)
                .Include(e => e.ACC_LOCAL_DODETAILS)
                .ThenInclude(e => e.STYLE.STYLE.FABCODENavigation)
                .Where(e => e.TRNSID.Equals(trnsId))
                .Select(e => new AccLocalDoMasterViewModel
                {
                    ACC_LOCAL_DOMASTER = new ACC_LOCAL_DOMASTER
                    {
                        TRNSID = e.TRNSID,
                        IS_COMPENSATION = e.IS_COMPENSATION,
                        EncryptedId = _protector.Protect(e.TRNSID.ToString()),
                        TRNSDATE = e.TRNSDATE,
                        DONO = e.DONO,
                        DODATE = e.DODATE,
                        SCID = e.SCID,
                        LCID = e.LCID,
                        SCNO = e.SCNO,
                        DOEX = e.DOEX,
                        REMARKS = e.REMARKS,
                        AUDITBY = e.AUDITBY,
                        AUDITON = e.AUDITON,
                        COMMENTS = e.COMMENTS,
                        USRID = e.USRID,
                        ComExScinfo = new COM_EX_SCINFO
                        {
                            SCNO = $"{e.ComExScinfo.SCNO}",
                            SCDATE = e.ComExScinfo.SCDATE,
                            SCPERSON = $"{e.ComExScinfo.SCPERSON}",
                            BUYER = new BAS_BUYERINFO
                            {
                                BUYER_NAME = $"{e.ComExScinfo.BUYER.BUYER_NAME}",
                                ADDRESS = $"{e.ComExScinfo.BUYER.ADDRESS}"
                            }
                        }
                    },
                    ACC_LOCAL_DODETAILs = e.ACC_LOCAL_DODETAILS.Select(f => new ACC_LOCAL_DODETAILS
                    {
                        TRNSID = f.TRNSID,
                        TRNSDATE = f.TRNSDATE,
                        DONO = f.DONO,
                        STYLEID = f.STYLEID,
                        PI_TRNSID = f.PI_TRNSID,
                        QTY = f.QTY,
                        RATE = f.RATE,
                        AMOUNT = f.AMOUNT,
                        REMARKS = f.REMARKS,
                        USRID = f.USRID,
                        ComExPiDetails = new COM_EX_PI_DETAILS
                        {
                            TRNSID = f.ComExPiDetails != null ? f.ComExPiDetails.TRNSID : 0,
                            STYLE = new COM_EX_FABSTYLE
                            {
                                FABCODENavigation = new RND_FABRICINFO
                                {
                                    STYLE_NAME = $"{f.ComExPiDetails.STYLE.FABCODENavigation.STYLE_NAME}"
                                }
                            }
                        },
                        STYLE = new COM_EX_SCDETAILS
                        {
                            TRNSID = f.STYLE != null ? f.STYLE.TRNSID : 0,
                            STYLE = new COM_EX_FABSTYLE
                            {
                                FABCODENavigation = new RND_FABRICINFO
                                {
                                    STYLE_NAME = $"{f.STYLE.STYLE.STYLENAME}"
                                }
                            }
                        }
                    }).ToList()
                }).FirstOrDefaultAsync();
        }

        public async Task<ACC_LOCAL_DOMASTER> FindByIdIncludeAllForDeleteAsync(int doId)
        {
            return await DenimDbContext.ACC_LOCAL_DOMASTER
                .Include(d => d.ACC_LOCAL_DODETAILS).FirstOrDefaultAsync(d => d.TRNSID.Equals(doId));
        }
    }
}
