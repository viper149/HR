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
    public class SQLF_QA_FIRST_MTR_ANALYSIS_M_Repository : BaseRepository<F_QA_FIRST_MTR_ANALYSIS_M>, IF_QA_FIRST_MTR_ANALYSIS_M
    {
        private readonly IDataProtector _protector;

        public SQLF_QA_FIRST_MTR_ANALYSIS_M_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<IEnumerable<F_QA_FIRST_MTR_ANALYSIS_M>> GetAllFirstMeterAnalysisInformation()
        {
            return await DenimDbContext.F_QA_FIRST_MTR_ANALYSIS_M
                .Include(d => d.EMP)
                .Include(d => d.BEAM.F_PR_SIZING_PROCESS_ROPE_DETAILS.W_BEAM)
                .Include(d => d.BEAM.F_PR_SLASHER_DYEING_DETAILS.W_BEAM)
                .Include(d => d.SET.PROG_)
                .Select(d => new F_QA_FIRST_MTR_ANALYSIS_M
                {
                    FMID = d.FMID,
                    EncryptedId = _protector.Protect(d.FMID.ToString()),
                    RPTNO = d.RPTNO,
                    TRANS_DATE = d.TRANS_DATE,
                    ACT_DENT = d.ACT_DENT,
                    ACT_RATIO = d.ACT_RATIO,
                    ACT_REED = d.ACT_REED,
                    ACT_RS = d.ACT_RS,
                    ACT_WIDTH = d.ACT_WIDTH,
                    REMARKS = d.REMARKS,
                    EMP = new F_HRD_EMPLOYEE
                    {
                        EMPNO = $"{d.EMP.EMPNO} - {d.EMP.FIRST_NAME} {d.EMP.LAST_NAME}"
                    },
                    BEAM = new F_PR_WEAVING_PROCESS_BEAM_DETAILS_B
                    {
                        F_PR_SIZING_PROCESS_ROPE_DETAILS = new F_PR_SIZING_PROCESS_ROPE_DETAILS
                        {
                            W_BEAM = new F_WEAVING_BEAM
                            {
                                BEAM_NO = d.BEAM.F_PR_SIZING_PROCESS_ROPE_DETAILS.W_BEAM.BEAM_NO ?? d.BEAM.F_PR_SLASHER_DYEING_DETAILS.W_BEAM.BEAM_NO
                            }
                        }
                    },
                    SET = new PL_PRODUCTION_SETDISTRIBUTION()
                    {
                        PROG_ = new PL_BULK_PROG_SETUP_D
                        {
                            PROG_NO = d.SET.PROG_.PROG_NO
                        }
                    }
                }).ToListAsync();


        }

        public async Task<FQAFirstMtrAnalysisMViewModel> GetInitObjByAsync(FQAFirstMtrAnalysisMViewModel fqaFirstMtrAnalysisMViewModel)
        {
            fqaFirstMtrAnalysisMViewModel.BeamList = await DenimDbContext.F_PR_WEAVING_PROCESS_BEAM_DETAILS_B
                .Include(d => d.F_PR_SIZING_PROCESS_ROPE_DETAILS.W_BEAM)
                .Include(d => d.F_PR_SLASHER_DYEING_DETAILS.W_BEAM)
                .Select(d => new F_PR_WEAVING_PROCESS_BEAM_DETAILS_B
                {
                    WV_BEAMID = d.WV_BEAMID,
                    F_PR_SIZING_PROCESS_ROPE_DETAILS = new F_PR_SIZING_PROCESS_ROPE_DETAILS
                    {
                        W_BEAM = new F_WEAVING_BEAM
                        {
                            BEAM_NO = $"{d.F_PR_SIZING_PROCESS_ROPE_DETAILS.W_BEAM.BEAM_NO ?? d.F_PR_SLASHER_DYEING_DETAILS.W_BEAM.BEAM_NO} - {d.BEAM_LENGTH} Mtr."
                        }
                    }
                }).OrderByDescending(d => d.F_PR_SIZING_PROCESS_ROPE_DETAILS.W_BEAM.BEAM_NO).ToListAsync();

            fqaFirstMtrAnalysisMViewModel.EmployeeList = await DenimDbContext.F_HRD_EMPLOYEE
                .Where(d=>d.EMPID.Equals(615) || d.EMPID.Equals(2254))
                .Select(d => new F_HRD_EMPLOYEE
                {
                    EMPID = d.EMPID,
                    EMPNO = $"{d.EMPNO} - {d.FIRST_NAME} {d.LAST_NAME}"
                }).OrderBy(d => d.EMPNO).ToListAsync();

            fqaFirstMtrAnalysisMViewModel.SetList = await DenimDbContext.PL_PRODUCTION_SETDISTRIBUTION
                .Include(d => d.PROG_)
                .Select(d => new PL_PRODUCTION_SETDISTRIBUTION
                {
                    SETID = d.SETID,
                    PROG_ = new PL_BULK_PROG_SETUP_D
                    {
                        PROG_NO = d.PROG_.PROG_NO
                    }
                }).OrderBy(d => d.PROG_.PROG_NO).ToListAsync();

            fqaFirstMtrAnalysisMViewModel.LotList = await DenimDbContext.BAS_YARN_LOTINFO
                .Select(d => new BAS_YARN_LOTINFO
                {
                    LOTID = d.LOTID,
                    LOTNO = d.LOTNO
                }).OrderBy(d => d.LOTNO).ToListAsync();

            fqaFirstMtrAnalysisMViewModel.SupplierList = await DenimDbContext.BAS_SUPPLIERINFO
                .Select(d => new BAS_SUPPLIERINFO
                {
                    SUPPID = d.SUPPID,
                    SUPPNAME = d.SUPPNAME
                }).OrderBy(d => d.SUPPNAME).ToListAsync();

            return fqaFirstMtrAnalysisMViewModel;
        }

        public async Task<string> GetLastReptNoAsync()
        {
            string rptNo;
            var result = await DenimDbContext.F_QA_FIRST_MTR_ANALYSIS_M
                .OrderByDescending(d => d.RPTNO).
                Select(d => d.RPTNO).FirstOrDefaultAsync();

            var year = DateTime.Now.Year % 100;

            if (result != null)
            {
                var resultArray = result.Split("-");
                if (int.Parse(resultArray[1]) < year)
                {
                    rptNo = $"RPT-{year}-{"1".PadLeft(4, '0')}";
                }
                else
                {
                    int.TryParse(new string(resultArray[2].SkipWhile(x => !char.IsDigit(x)).TakeWhile(char.IsDigit).ToArray()), out var currentNumber);

                    rptNo = $"RPT-{year}-{(currentNumber + 1).ToString().PadLeft(4, '0')}";
                }
            }
            else
            {
                rptNo = $"RPT-{year}-{"1".PadLeft(4, '0')}";
            }

            return rptNo;
        }

        public async Task<PL_PRODUCTION_SETDISTRIBUTION> GetBySetIdAsync(int setId)
        {
            return await DenimDbContext.PL_PRODUCTION_SETDISTRIBUTION
                .Include(d=>d.F_PR_WEAVING_PROCESS_MASTER_B)
                .ThenInclude(d=> d.F_PR_WEAVING_PROCESS_BEAM_DETAILS_B)
                .ThenInclude(d=> d.F_PR_SIZING_PROCESS_ROPE_DETAILS.W_BEAM)
                .Include(d => d.F_PR_WEAVING_PROCESS_MASTER_B)
                .ThenInclude(d => d.F_PR_WEAVING_PROCESS_BEAM_DETAILS_B)
                .ThenInclude(d => d.F_PR_SLASHER_DYEING_DETAILS.W_BEAM)
                .Include(d => d.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.RND_WEAVE)
                .Include(d => d.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.COLORCODENavigation)
                .Include(d => d.PROG_.BLK_PROG_.RndProductionOrder.SO.PIMASTER.BUYER)
                .Include(d => d.PROG_.BLK_PROG_.RndProductionOrder.SO.PIMASTER.BRAND)
                .Include(d => d.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.RND_FABRIC_COUNTINFO)
                .ThenInclude(d => d.COUNT)
                .Include(d => d.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.RND_FABRIC_COUNTINFO)
                .ThenInclude(d => d.LOT)
                .Include(d => d.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.RND_FABRIC_COUNTINFO)
                .ThenInclude(d => d.SUPP)
                .Where(d => d.SETID.Equals(setId))
                .Select(d => new PL_PRODUCTION_SETDISTRIBUTION
                {
                    F_PR_WEAVING_PROCESS_MASTER_B = d.F_PR_WEAVING_PROCESS_MASTER_B.Select(f=>new F_PR_WEAVING_PROCESS_MASTER_B
                    {
                        F_PR_WEAVING_PROCESS_BEAM_DETAILS_B = f.F_PR_WEAVING_PROCESS_BEAM_DETAILS_B.Select(g=>new F_PR_WEAVING_PROCESS_BEAM_DETAILS_B
                        {
                            WV_BEAMID = g.WV_BEAMID,
                            F_PR_SIZING_PROCESS_ROPE_DETAILS = new F_PR_SIZING_PROCESS_ROPE_DETAILS
                            {
                                W_BEAM = new F_WEAVING_BEAM
                                {
                                    BEAM_NO = $"{g.F_PR_SIZING_PROCESS_ROPE_DETAILS.W_BEAM.BEAM_NO ?? g.F_PR_SLASHER_DYEING_DETAILS.W_BEAM.BEAM_NO} - {g.BEAM_LENGTH} Mtr."
                                }
                            }
                        }).ToList()
                    }).ToList(),
                    PROG_ = new PL_BULK_PROG_SETUP_D
                    {
                        SET_QTY = d.PROG_.SET_QTY,
                        BLK_PROG_ = new PL_BULK_PROG_SETUP_M
                        {
                            RndProductionOrder = new RND_PRODUCTION_ORDER
                            {
                                ORDER_QTY_YDS = d.PROG_.BLK_PROG_.RndProductionOrder.ORDER_QTY_YDS,
                                SO = new COM_EX_PI_DETAILS
                                {
                                    SO_NO = d.PROG_.BLK_PROG_.RndProductionOrder.SO.SO_NO,
                                    STYLE = new COM_EX_FABSTYLE
                                    {
                                        FABCODENavigation = new RND_FABRICINFO
                                        {
                                            STYLE_NAME = d.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.STYLE_NAME,
                                            GREPI = d.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.GREPI,
                                            GRPPI = d.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.GRPPI,
                                            WIGRBW = d.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.WIGRBW,
                                            WGGRBW = d.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.WGGRBW,
                                            TOTALENDS = d.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.TOTALENDS,
                                            TOTALWEFT = d.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.TOTALWEFT,
                                            REED_COUNT = d.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.REED_COUNT,
                                            DENT = d.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.DENT,
                                            REED_SPACE = d.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.REED_SPACE,
                                            RND_WEAVE = new RND_WEAVE
                                            {
                                                NAME = d.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.RND_WEAVE.NAME
                                            },
                                            COLORCODENavigation = new BAS_COLOR
                                            {
                                                COLOR = d.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.COLORCODENavigation.COLOR
                                            }
                                        }
                                    },
                                    PIMASTER = new COM_EX_PIMASTER
                                    {
                                        PINO = d.PROG_.BLK_PROG_.RndProductionOrder.SO.PIMASTER.PINO,
                                        BUYER = new BAS_BUYERINFO
                                        {
                                            BUYER_NAME = d.PROG_.BLK_PROG_.RndProductionOrder.SO.PIMASTER.BUYER.BUYER_NAME
                                        },
                                        BRAND = new BAS_BRANDINFO
                                        {
                                            BRANDNAME = d.PROG_.BLK_PROG_.RndProductionOrder.SO.PIMASTER.BRAND.BRANDNAME
                                        }
                                    }
                                }
                            }
                        },
                        OPT1 = string.Join(" + ",
                            d.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.RND_FABRIC_COUNTINFO.Where(c => c.YARNFOR.Equals(2))
                                .Select(p => p.LOT.LOTNO)),
                        OPT2 = string.Join(" + ",
                            d.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.RND_FABRIC_COUNTINFO.Where(c => c.YARNFOR.Equals(2))
                                .Select(p => p.SUPP.SUPPNAME)),
                        OPT3 = string.Join(":",
                            d.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.RND_FABRIC_COUNTINFO.Where(c => c.YARNFOR.Equals(2))
                                .Select(p => p.RATIO))
                    },
                    OPT1 = string.Join(" + ",
                        d.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.RND_FABRIC_COUNTINFO.Where(c => c.YARNFOR.Equals(1))
                            .Select(p => p.COUNT.RND_COUNTNAME)),
                    OPT2 = string.Join(" + ",
                        d.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.RND_FABRIC_COUNTINFO.Where(c => c.YARNFOR.Equals(2))
                            .Select(p => p.COUNT.RND_COUNTNAME))
                }).FirstOrDefaultAsync();
        }

        public async Task<F_PR_WEAVING_PROCESS_BEAM_DETAILS_B> GetByBeamIdAsync(int beamId)
        {
            return await DenimDbContext.F_PR_WEAVING_PROCESS_BEAM_DETAILS_B
                .Include(d => d.F_PR_WEAVING_PROCESS_DETAILS_B)
                .ThenInclude(d => d.LOOM_NONavigation)
                .Where(d => d.WV_BEAMID.Equals(beamId))
                .Select(d => new F_PR_WEAVING_PROCESS_BEAM_DETAILS_B
                {
                    BEAM_LENGTH = d.BEAM_LENGTH,
                    F_PR_WEAVING_PROCESS_DETAILS_B = d.F_PR_WEAVING_PROCESS_DETAILS_B.Select(f => new F_PR_WEAVING_PROCESS_DETAILS_B
                    {
                        LENGTH_BULK = f.LENGTH_BULK,
                        LOOM_NONavigation = new F_LOOM_MACHINE_NO
                        {
                            LOOM_NO = f.LOOM_NONavigation.LOOM_NO
                        }
                    }).ToList()
                }).FirstOrDefaultAsync();
        }

        public async Task<FQAFirstMtrAnalysisMViewModel> FindByIdIncludeAllAsync(int fmaId)
        {
            return await DenimDbContext.F_QA_FIRST_MTR_ANALYSIS_M
                    .Include(d => d.SET.PROG_)
                    .Include(d => d.EMP)
                    .Include(d => d.BEAM.F_PR_SIZING_PROCESS_ROPE_DETAILS.W_BEAM)
                    .Include(d => d.BEAM.F_PR_SLASHER_DYEING_DETAILS.W_BEAM)
                    .Include(d => d.F_QA_FIRST_MTR_ANALYSIS_D)
                    .ThenInclude(d => d.LOT)
                    .Include(d => d.F_QA_FIRST_MTR_ANALYSIS_D)
                    .ThenInclude(d => d.SUPPLIER)
                    .Where(d => d.FMID.Equals(fmaId))
                    .Select(d => new FQAFirstMtrAnalysisMViewModel
                    {
                        FirstMtrAnalysisM = new F_QA_FIRST_MTR_ANALYSIS_M
                        {
                            FMID = d.FMID,
                            EncryptedId = _protector.Protect(d.FMID.ToString()),
                            TRANS_DATE = d.TRANS_DATE,
                            EMPID = d.EMPID,
                            SETID = d.SETID,
                            BEAMID = d.BEAMID,
                            LENGTH_TRIAL = d.LENGTH_TRIAL,
                            ACT_REED = d.ACT_REED,
                            ACT_DENT = d.ACT_DENT,
                            ACT_RS = d.ACT_RS,
                            ACT_WIDTH = d.ACT_WIDTH,
                            RPM = d.RPM,
                            BYPASS_YARN = d.BYPASS_YARN,
                            REMARKS = d.REMARKS,
                            RPTNO = d.RPTNO,
                            ACT_RATIO = d.ACT_RATIO,
                            SET = new PL_PRODUCTION_SETDISTRIBUTION
                            {
                                PROG_ = new PL_BULK_PROG_SETUP_D
                                {
                                    PROG_NO = d.SET.PROG_.PROG_NO
                                }
                            },
                            EMP = new F_HRD_EMPLOYEE
                            {
                                EMPNO = $"{d.EMP.EMPNO} - {d.EMP.FIRST_NAME} {d.EMP.LAST_NAME}"
                            },
                            BEAM = new F_PR_WEAVING_PROCESS_BEAM_DETAILS_B
                            {
                                F_PR_SIZING_PROCESS_ROPE_DETAILS = new F_PR_SIZING_PROCESS_ROPE_DETAILS
                                {
                                    W_BEAM = new F_WEAVING_BEAM
                                    {
                                        BEAM_NO = $"{d.BEAM.F_PR_SIZING_PROCESS_ROPE_DETAILS.W_BEAM.BEAM_NO ?? d.BEAM.F_PR_SLASHER_DYEING_DETAILS.W_BEAM.BEAM_NO} - {d.BEAM.BEAM_LENGTH} Mtr."
                                    }
                                }
                            }
                        },
                        FQaFirstMtrAnalysisDsList = d.F_QA_FIRST_MTR_ANALYSIS_D.Select(f => new F_QA_FIRST_MTR_ANALYSIS_D
                        {
                            FM_D_ID = f.FM_D_ID,
                            FMID = f.FMID,
                            LOTID = f.LOTID,
                            SUPPLIERID = f.SUPPLIERID,
                            REMARKS = f.REMARKS,
                            LOT = new BAS_YARN_LOTINFO
                            {
                                LOTNO = f.LOT.LOTNO
                            },
                            SUPPLIER = new BAS_SUPPLIERINFO
                            {
                                SUPPNAME = f.SUPPLIER.SUPPNAME
                            }
                        }).ToList()
                    }).FirstOrDefaultAsync();
        }
    }
}
