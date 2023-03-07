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
    public class SQLRND_BOM_Repository : BaseRepository<RND_BOM>, IRND_BOM
    {
        private readonly IDataProtector _protector;

        public SQLRND_BOM_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<RndBomViewModel> GetInitObjects(RndBomViewModel rndBomViewModel)
        {

            try
            {
                int[] sec = { 158, 160, 163 };
                rndBomViewModel.RndFabricInfos = await DenimDbContext.RND_FABRICINFO
                    .Select(c => new RND_FABRICINFO()

                    {
                        FABCODE = c.FABCODE,
                        STYLE_NAME = c.STYLE_NAME

                    }).ToListAsync();

                rndBomViewModel.FChemStoreProductInfo = await DenimDbContext.F_CHEM_STORE_PRODUCTINFO
                    .Select(c => new F_CHEM_STORE_PRODUCTINFO()
                    {
                        PRODUCTID = c.PRODUCTID,
                        PRODUCTNAME = c.PRODUCTNAME

                    }).ToListAsync();

                rndBomViewModel.FBasSections = await DenimDbContext.F_BAS_SECTION
                    .Select(c => new F_BAS_SECTION()
                    {
                        SECID = c.SECID,
                        SECNAME = c.SECNAME
                    })
                    .Where(c => sec.Any(e => e.Equals(c.SECID)))
                    .ToListAsync();

                return rndBomViewModel;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IEnumerable<RND_BOM>> GetAllRndBomInfoAsync()
        {
            return await DenimDbContext.RND_BOM
                .Include(d => d.FABCODENavigation)
                .Include(d => d.COLORNavigation)
                .Include(d => d.FINISH_TYPENavigation)
                .Select(d => new RND_BOM()
                {
                    TRNSDATE = d.TRNSDATE,
                    BOMID = d.BOMID,
                    EncryptedId = _protector.Protect(d.BOMID.ToString()),
                    FABCODE = d.FABCODE,
                    FINISH_TYPE = d.FINISH_TYPE,
                    COLOR = d.COLOR,
                    TOTAL_ENDS = d.TOTAL_ENDS,
                    LOT_RATIO = d.LOT_RATIO,
                    WIDTH = d.WIDTH,
                    SETNO = d.SETNO,
                    PROG_NO = d.PROG_NO,
                    REMARKS = d.REMARKS,
                    OPT1 = d.FABCODENavigation.STYLE_NAME,

                    COLORNavigation = new BAS_COLOR
                    {
                        COLOR = d.COLORNavigation.COLOR
                    },
                    FINISH_TYPENavigation = new RND_FINISHTYPE
                    {
                        TYPENAME = d.FINISH_TYPENavigation.TYPENAME
                    }
                }).ToListAsync();

        }

        public async Task<RND_FABRICINFO> GetAllByStyleIdAsync(int styleId)
        {
            var result = await DenimDbContext.RND_FABRICINFO
                .Include(d => d.RND_FINISHTYPE)
                .Include(d => d.COLORCODENavigation)
                .Include(c => c.RND_FABRIC_COUNTINFO)
                .ThenInclude(c => c.COUNT)
                .Include(c => c.RND_FABRIC_COUNTINFO)
                .ThenInclude(c => c.LOT)
                .Where(d => d.FABCODE.Equals(styleId))
                .FirstOrDefaultAsync();




            result.OPT1 = string.Join(" + ",
                result.RND_FABRIC_COUNTINFO.Where(c => c.YARNFOR.Equals(1))
                    .Select(p => p.COUNT.RND_COUNTNAME));
            result.OPT2 = string.Join(" + ",
                result.RND_FABRIC_COUNTINFO.Where(c => c.YARNFOR.Equals(2))
                    .Select(p => p.COUNT.RND_COUNTNAME));
            result.OPT1 =
                $"{result.OPT1} X {result.OPT2} / {result.FNEPI}X{result.FNPPI}";

            result.OPT2 = string.Join(" + ",
                result.RND_FABRIC_COUNTINFO.Where(c => c.YARNFOR.Equals(1))
                    .Select(p => $"{p.LOT.LOTNO}(R-{p.RATIO})"));
            result.OPT3 = string.Join(" + ",
                result.RND_FABRIC_COUNTINFO.Where(c => c.YARNFOR.Equals(2))
                    .Select(p => $"{p.LOT.LOTNO} & (R-{p.RATIO})"));
            result.OPT2 =
                $"{result.OPT2} X {result.OPT3}";

            return result;
        }

        public async Task<RndBomViewModel> FindByIdIncludeAllAsync(int rbId)
        {
            return await DenimDbContext.RND_BOM
                .Include(d => d.RND_BOM_MATERIALS_DETAILS)
                .Include(d => d.COLORNavigation)
                .Include(d => d.FINISH_TYPENavigation)
                .Where(d =>d.BOMID.Equals(rbId))
                //.ThenInclude(d => d.WP)
                //.Where(d => d.WRID.Equals(wrId))
                .Select(d => new RndBomViewModel
                {
                    RndBom = new RND_BOM
                    {
                        
                        TRNSDATE = d.TRNSDATE,
                        BOMID = d.BOMID,
                        EncryptedId = _protector.Protect(d.BOMID.ToString()),
                        FABCODE = d.FABCODE,
                        FINISH_TYPE = d.FINISH_TYPE,
                        COLOR = d.COLOR,
                        TOTAL_ENDS = d.TOTAL_ENDS,
                        LOT_RATIO = d.LOT_RATIO,
                        WIDTH = d.WIDTH,
                        SETNO = d.SETNO,
                        PROG_NO = d.PROG_NO,
                        CONSTRUCTION = d.CONSTRUCTION,
                        FINISH_WEIGHT = d.FINISH_WEIGHT,
                        REMARKS = d.REMARKS,
                        OPT1 = d.FABCODENavigation.STYLE_NAME,
                        INDIGO_GPL = d.INDIGO_GPL,
                        INDIGO_BOX = d.INDIGO_BOX,
                        SULPHURE_GPL = d.SULPHURE_GPL,
                        SULPHURE_BOX = d.SULPHURE_BOX,
                        OTHERS_GPL = d.OTHERS_GPL,
                        OTHERS_BOX = d.OTHERS_BOX,
                        OTHERS_REMARKS = d.OTHERS_REMARKS,
                        COLORNavigation = new BAS_COLOR
                        {
                            COLOR = d.COLORNavigation.COLOR
                            
                        },
                        FINISH_TYPENavigation = new RND_FINISHTYPE
                        {
                            TYPENAME = d.FINISH_TYPENavigation.TYPENAME,
                            
                        },

                    },
                    RndBomMaterialsDetailsList = d.RND_BOM_MATERIALS_DETAILS.Select(e=> new RND_BOM_MATERIALS_DETAILS
                    {
                        SECTION = e.SECTION,
                        CHEM_PROD_ID = e.CHEM_PROD_ID,
                        DOSING = e.DOSING,
                        CONC = e.CONC,
                        SPEED = e.SPEED,
                        NO_OF_SETS = e.NO_OF_SETS,
                        REQ_QTY = e.REQ_QTY,
                        ADD_10_FOR_BOX = e.ADD_10_FOR_BOX,
                        REMARKS = e.REMARKS,
                        BOM_D_ID = e.BOM_D_ID,
                        SECTIONNavigation = new F_BAS_SECTION
                        {
                            SECNAME = e.SECTIONNavigation.SECNAME
                        },
                        CHEM_PROD_ = new F_CHEM_STORE_PRODUCTINFO
                        {
                            PRODUCTNAME = e.CHEM_PROD_.PRODUCTNAME
                        }

                    }).ToList()
                }).FirstOrDefaultAsync();
        }
    }
}
