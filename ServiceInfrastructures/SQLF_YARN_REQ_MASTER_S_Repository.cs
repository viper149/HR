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
    public class SQLF_YARN_REQ_MASTER_S_Repository : BaseRepository<F_YARN_REQ_MASTER_S>, IF_YARN_REQ_MASTER_S
    {
        private readonly IDataProtector _protector;

        public SQLF_YARN_REQ_MASTER_S_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<IEnumerable<F_YARN_REQ_MASTER_S>> GetAllYarnRequirementAsync()
        {
            return await DenimDbContext.F_YARN_REQ_MASTER_S
                .Select(e => new F_YARN_REQ_MASTER_S
                {
                    YSRNO = e.YSRNO,
                    EncryptedId = _protector.Protect(e.YSRID.ToString()),
                    YSRDATE = e.YSRDATE,
                   
                    REMARKS = e.REMARKS,
                }).ToListAsync();
        }

        public async Task<IEnumerable<F_YARN_REQ_MASTER_S>> GetYsrIdList()
        {
            return await DenimDbContext.F_YARN_REQ_MASTER_S
                //.Where(c=>!_denimDbContext.F_YS_YARN_ISSUE_MASTER_S.Any(e=>e.YSRID.Equals(c.YSRID)))
                .Select(e => new F_YARN_REQ_MASTER_S
                {
                    YSRID = e.YSRID,
                    YSRNO = $"Req. No-{e.YSRNO} - SR-{e.REMARKS}"
                }).ToListAsync();
        }

        public async Task<FYarnReqSViewModel> GetInitObjects(FYarnReqSViewModel fYarnReqSViewModel)
        {
            fYarnReqSViewModel.DepartmentList = await DenimDbContext.F_BAS_DEPARTMENT.Select(e => new F_BAS_DEPARTMENT
            {
                DEPTID = e.DEPTID,
                DEPTNAME = e.DEPTNAME
            }).OrderBy(e => e.DEPTNAME).ToListAsync();

            fYarnReqSViewModel.Lot = await DenimDbContext.BAS_YARN_LOTINFO.Select(e => new BAS_YARN_LOTINFO
            {
                LOTID = e.LOTID,
                LOTNO = e.LOTNO
            }).OrderBy(e => e.LOTNO).ToListAsync();

            fYarnReqSViewModel.FBasSectionList = await DenimDbContext.F_BAS_SECTION.Select(e => new F_BAS_SECTION
            {
                SECID = e.SECID,
                SECNAME = e.SECNAME
            }).OrderBy(e => e.SECNAME).ToListAsync();

            fYarnReqSViewModel.PlProductionSetDistributions = await DenimDbContext.PL_PRODUCTION_SETDISTRIBUTION.Include(c => c.PROG_).Select(e => new TypeTableViewModel
            {
                ID = e.SETID,
                Name = e.PROG_.PROG_NO
            }).OrderByDescending(e => e.Name).ToListAsync();

            fYarnReqSViewModel.PosoViewModels = await DenimDbContext.RND_PRODUCTION_ORDER
                .Include(e => e.SO)
                .Select(e => new POSOViewModel
                {
                    Poid = e.POID,
                    SO_NO = e.SO.SO_NO
                })
                .Where(e => !string.IsNullOrEmpty(e.SO_NO))
                .OrderBy(e => e.SO_NO).ToListAsync();

            fYarnReqSViewModel.BasUnits = await DenimDbContext.F_BAS_UNITS.Select(e => new F_BAS_UNITS
            {
                UID = e.UID,
                UNAME = e.UNAME
            }).OrderByDescending(e => e.UNAME.ToLower().Contains("kg")).ThenBy(e => e.UNAME).ToListAsync();

            return fYarnReqSViewModel;
        }

        public async Task<FYarnReqSViewModel> FindByIdIncludeAllAsync(int ysrId)
        {
            return await DenimDbContext.F_YARN_REQ_MASTER_S
                    .Include(e => e.F_YARN_REQ_DETAILS_S)
                    .ThenInclude(e => e.UNITNavigation)
                    .Include(e => e.F_YARN_REQ_DETAILS_S)
                    .ThenInclude(e => e.COUNT.COUNT)
                    .Include(e => e.F_YARN_REQ_DETAILS_S)
                    .ThenInclude(e => e.ORDERNONavigation.SO)
                    .Select(e => new FYarnReqSViewModel
                    {
                        FYarnRequirementMasterS = new F_YARN_REQ_MASTER_S
                        {
                            YSRID = e.YSRID,
                            EncryptedId = _protector.Protect(e.YSRID.ToString()),
                            YSRNO = e.YSRNO,
                            YSRDATE = e.YSRDATE,
                            REMARKS = e.REMARKS,
                        },

                        FYarnRequirementDetailsSList = new List<F_YARN_REQ_DETAILS_S>(e.F_YARN_REQ_DETAILS_S.Select(f => new F_YARN_REQ_DETAILS_S
                        {
                            TRNSID = f.TRNSID,
                            TRNSDATE = f.TRNSDATE,
                            YSRID = f.YSRID,
                            COUNTID = f.COUNTID,
                            ORDERNO = f.ORDERNO,
                            UNIT = f.UNIT,
                            REQ_QTY = f.REQ_QTY,
                            REMARKS = f.REMARKS,
                            COUNT = new RND_FABRIC_COUNTINFO
                            {
                                COUNT = f.COUNT.COUNT
                            },
                            UNITNavigation = new F_BAS_UNITS
                            {
                                UID = f.UNITNavigation.UID,
                                UNAME = f.UNITNavigation.UNAME
                            },
                            ORDERNONavigation = new RND_PRODUCTION_ORDER
                            {
                                SO = new COM_EX_PI_DETAILS
                                {
                                    SO_NO = f.ORDERNONavigation.SO.SO_NO
                                }
                            }
                        }))
                    }).FirstOrDefaultAsync(e => e.FYarnRequirementMasterS.YSRID.Equals(ysrId));
        }

        public async Task<dynamic> GetRequiredKgsWithLotdAsync(int poId, int countId)
        {
            return await DenimDbContext.RND_PRODUCTION_ORDER
                .Include(e => e.SO.STYLE.FABCODENavigation.RND_FABRIC_COUNTINFO)
                .ThenInclude(e => e.LOT)
                .Include(e => e.SO.STYLE.FABCODENavigation.RND_YARNCONSUMPTION)
                .Where(e => e.POID.Equals(poId))
                .Select(e => new
                {
                    LOTNO = e.SO.STYLE.FABCODENavigation.RND_FABRIC_COUNTINFO.FirstOrDefault(f => f.TRNSID.Equals(countId)).LOT.LOTNO,
                    REQUIRED_KGS = e.ORDER_QTY_YDS * e.SO.STYLE.FABCODENavigation.RND_YARNCONSUMPTION.FirstOrDefault(f => f.COUNTID.Equals(e.SO.STYLE.FABCODENavigation.RND_FABRIC_COUNTINFO.FirstOrDefault(g => g.TRNSID.Equals(countId)).COUNTID)).AMOUNT
                }).FirstOrDefaultAsync();
        }
    }
}
