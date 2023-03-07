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
    public class SQLF_YARN_REQ_MASTER_Repository : BaseRepository<F_YARN_REQ_MASTER>, IF_YARN_REQ_MASTER
    {
        private readonly IDataProtector _protector;

        public SQLF_YARN_REQ_MASTER_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<IEnumerable<F_YARN_REQ_MASTER>> GetAllYarnRequirementAsync()
        {
            var x = await DenimDbContext.F_YARN_REQ_MASTER
                .Include(d => d.Section)
                .Include(d => d.SubSection)
                .Include(d => d.F_YARN_REQ_DETAILS)
                .Select(d => new F_YARN_REQ_MASTER
                {
                    YSRNO = d.YSRNO,
                    YSRID = d.YSRID,
                    EncryptedId = _protector.Protect(d.YSRID.ToString()),
                    YSRDATE = d.YSRDATE,
                    REMARKS = d.REMARKS,
                    Section = new F_BAS_SECTION
                    {
                        SECNAME = d.Section.SECNAME
                    },
                    SSECNAME = d.SubSection == null ? "" : d.SubSection.SSECNAME,
                    ORD_TYPE = d.F_YARN_REQ_DETAILS.Any() ? d.F_YARN_REQ_DETAILS.FirstOrDefault().ORDER_TYPE.Equals("OrderNo") ? "Bulk" : "Sample" : null
                }).ToListAsync();

            return x;
        }

        public async Task<IEnumerable<F_YARN_REQ_MASTER>> GetYsrIdList()
        {
            try
            {
                var result = await DenimDbContext.F_YARN_REQ_MASTER
                    .Where(c => !DenimDbContext.F_YS_YARN_ISSUE_MASTER.Any(e => e.YSRID.Equals(c.YSRID)))
                    .Select(e => new F_YARN_REQ_MASTER
                    {
                        YSRID = e.YSRID,
                        YSRNO = $"Req. No-{e.YSRNO} - SR-{e.REMARKS}"
                    }).ToListAsync();

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<YarnRequirementViewModel> GetInitObjects(YarnRequirementViewModel yarnRequirementViewModel)
        {
            yarnRequirementViewModel.DepartmentList = await DenimDbContext.F_BAS_DEPARTMENT.Select(e => new F_BAS_DEPARTMENT
            {
                DEPTID = e.DEPTID,
                DEPTNAME = e.DEPTNAME
            }).OrderBy(e => e.DEPTNAME).ToListAsync();

            yarnRequirementViewModel.Lot = await DenimDbContext.BAS_YARN_LOTINFO.Select(e => new BAS_YARN_LOTINFO
            {
                LOTID = e.LOTID,
                LOTNO = e.LOTNO
            }).OrderBy(e => e.LOTNO).ToListAsync();

            yarnRequirementViewModel.FBasSectionList = await DenimDbContext.F_BAS_SECTION.Select(e => new F_BAS_SECTION
            {
                SECID = e.SECID,
                SECNAME = e.SECNAME
            }).OrderBy(e => e.SECNAME).ToListAsync();

            yarnRequirementViewModel.FBasSubSectionList = await DenimDbContext.F_BAS_SUBSECTION.Select(e => new F_BAS_SUBSECTION
            {
                SSECID = e.SSECID,
                SSECNAME = e.SSECNAME
            }).OrderBy(e => e.SSECNAME).ToListAsync();

            yarnRequirementViewModel.PlProductionSetDistributions = await DenimDbContext.PL_PRODUCTION_SETDISTRIBUTION.Include(c => c.PROG_).Select(e => new TypeTableViewModel
            {
                ID = e.SETID,
                Name = e.PROG_.PROG_NO
            }).OrderByDescending(e => e.Name).ToListAsync();

            yarnRequirementViewModel.PosoViewModels = await DenimDbContext.RND_PRODUCTION_ORDER
                .Include(e => e.SO)
                .Select(e => new POSOViewModel
                {
                    Poid = e.POID,
                    SO_NO = e.SO.SO_NO
                })
                .Where(e => !string.IsNullOrEmpty(e.SO_NO))
                .OrderBy(e => e.SO_NO).ToListAsync();

            yarnRequirementViewModel.RndSampleInfoDyeingList = await DenimDbContext.RND_SAMPLE_INFO_DYEING
                .Where(d=> !d.RSOrder.Equals(null))
                .Select(d => new RND_SAMPLE_INFO_DYEING
                {
                    SDID = d.SDID,
                    RSOrder = d.RSOrder
                }).ToListAsync();

            //yarnRequirementViewModel.RSNoViewModel = await _denimDbContext.RND_SAMPLE_INFO_DYEING
            //    .Select(e => new RSNoViewModel
            //    { 
            //        RSID = e.SDID,
            //        RS_NO = e.RSOrder
            //    }).ToListAsync();

            //yarnRequirementViewModel.PosoViewModels = await _denimDbContext.RND_SAMPLE_INFO_DYEING
            //    .Select(e => new RSNoViewModel
            //    {

            //        RSID = e.SDID,
            //        RS_NO = e.RSOrder
            //    })
            //    .ToListAsync();

            //yarnRequirementViewModel.PosoViewModels = await _denimDbContext.RND_PRODUCTION_ORDER
            //    .Include(e => e.RS)
            //    .Select(e => new POSOViewModel
            //    {
            //        RSID = e.RS.SDID,
            //        RS_NO = e.RS.RSOrder
            //    })
            //    .Where(e => !string.IsNullOrEmpty(e.RS_NO))
            //    .OrderBy(e => e.RS_NO).ToListAsync();


            yarnRequirementViewModel.BasUnits = await DenimDbContext.F_BAS_UNITS.Select(e => new F_BAS_UNITS
            {
                UID = e.UID,
                UNAME = e.UNAME
            }).OrderByDescending(e => e.UNAME.ToLower().Contains("kg")).ThenBy(e => e.UNAME).ToListAsync();

            return yarnRequirementViewModel;
        }

        public async Task<YarnRequirementViewModel> FindByIdIncludeAllAsync(int ysrId)
        {
            try
            {
                var x=  await DenimDbContext.F_YARN_REQ_MASTER
                    .Include(e => e.F_YARN_REQ_DETAILS)
                    .ThenInclude(e => e.FBasUnits)
                    .Include(e => e.F_YARN_REQ_DETAILS)
                    .ThenInclude(e => e.COUNT.COUNT)
                    .Include(e => e.F_YARN_REQ_DETAILS)
                    .ThenInclude(e => e.PO.SO)
                    .Include(e => e.F_YARN_REQ_DETAILS)
                    .ThenInclude(e => e.RS)
                    .Include(e => e.F_YARN_REQ_DETAILS)
                    .ThenInclude(e => e.RSCOUNT)
                    .Select(e => new YarnRequirementViewModel
                    {
                        FYarnRequirementMaster = new F_YARN_REQ_MASTER
                        {
                            YSRID = e.YSRID,
                            EncryptedId = _protector.Protect(e.YSRID.ToString()),
                            YSRNO = e.YSRNO,
                            YSRDATE = e.YSRDATE,
                            REMARKS = e.REMARKS,
                            SECID = e.SECID,
                            SUBSECID = e.SUBSECID,
                        },

                        FYarnRequirementDetailsList = new List<F_YARN_REQ_DETAILS>(e.F_YARN_REQ_DETAILS.Select(f => new F_YARN_REQ_DETAILS
                        {
                            TRNSID = f.TRNSID,
                            TRNSDATE = f.TRNSDATE,
                            YSRID = f.YSRID,
                            COUNTID = f.COUNTID,
                            ORDERNO = f.ORDERNO,
                            ORDER_TYPE = f.ORDER_TYPE,
                            RSID = f.RSID,
                            COUNTID_RS = f.COUNTID_RS,
                            LOTID = f.LOTID,
                            LOT = f.LOT,
                            UNIT = f.UNIT,
                            REQ_QTY = f.REQ_QTY,
                            REMARKS = f.REMARKS,
                            COUNT = new RND_FABRIC_COUNTINFO
                            {
                                COUNT = f.COUNT.COUNT
                            },
                            FBasUnits = new F_BAS_UNITS
                            {
                                UID = f.FBasUnits.UID,
                                UNAME = f.FBasUnits.UNAME
                            },
                            PO = new RND_PRODUCTION_ORDER
                            {
                                SO = new COM_EX_PI_DETAILS
                                {
                                    SO_NO = f.PO.SO.SO_NO
                                }
                            },
                            RS = new RND_SAMPLE_INFO_DYEING
                            {
                                RSOrder = f.RS.RSOrder
                            },
                            RSCOUNT = new BAS_YARN_COUNTINFO
                            {
                                RND_COUNTNAME = f.RSCOUNT.RND_COUNTNAME
                            }
                        }))
                    }).FirstOrDefaultAsync(e => e.FYarnRequirementMaster.YSRID.Equals(ysrId));

                return x;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
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
