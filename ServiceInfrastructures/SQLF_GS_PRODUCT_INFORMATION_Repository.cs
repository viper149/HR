using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels;
using DenimERP.ViewModels.GeneralStore;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_GS_PRODUCT_INFORMATION_Repository : BaseRepository<F_GS_PRODUCT_INFORMATION>, IF_GS_PRODUCT_INFORMATION
    {
        private readonly IDataProtector _protector;

        public SQLF_GS_PRODUCT_INFORMATION_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<F_GS_PRODUCT_INFORMATION> GetSingleProductByProductId(int id)
        {
            return await DenimDbContext.F_GS_PRODUCT_INFORMATION
                .Include(e => e.UNITNavigation)
                .Where(e => e.PRODID.Equals(id))
                .Select(d => new F_GS_PRODUCT_INFORMATION
                {
                    UNITNavigation = new F_BAS_UNITS
                    {
                        UNAME = d.UNITNavigation.UNAME
                    },
                    Balance = (DenimDbContext.F_GEN_S_RECEIVE_DETAILS.Where(f => f.PRODUCTID.Equals(id)).Sum(f => f.FRESH_QTY) ?? 0) - (DenimDbContext.F_GEN_S_ISSUE_DETAILS.Where(f => f.PRODUCTID.Equals(id)).Sum(f => f.ISSUE_QTY) ?? 0)
                }).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<F_GS_PRODUCT_INFORMATION>> GetAllProductInformationAsync()
        {
            return await DenimDbContext.F_GS_PRODUCT_INFORMATION
                .Include(d=>d.SCAT.CAT)
                .Include(d=>d.UNITNavigation)
                .Select(d => new F_GS_PRODUCT_INFORMATION
                {
                    PRODID = d.PRODID,
                    EncryptedId = _protector.Protect(d.PRODID.ToString()),
                    PRODNAME = d.PRODNAME,
                    PARTNO = d.PARTNO,
                    DESCRIPTION = d.DESCRIPTION,
                    PROD_LOC = d.PROD_LOC,
                    REMARKS = d.REMARKS,
                    SCAT = new F_GS_ITEMSUB_CATEGORY
                    {
                        SCATNAME = d.SCAT.SCATNAME,
                        CAT = new F_GS_ITEMCATEGORY
                        {
                            CATNAME = d.SCAT.CAT.CATNAME
                        }
                    },
                    UNITNavigation = new F_BAS_UNITS
                    {
                        UNAME = d.UNITNavigation.UNAME
                    }
                }).ToListAsync();
        }

        public async Task<bool> FindByProdName(string prodName)
        {
            return !await DenimDbContext.F_GS_PRODUCT_INFORMATION.AnyAsync(d => d.PRODNAME.Equals(prodName));
        }

        public async Task<FGsProductInformationViewModel> GetInitObjByAsync(FGsProductInformationViewModel fGsProductInformationViewModel, bool edit)
        {
            fGsProductInformationViewModel.FGsItemcategoriesList = await DenimDbContext.F_GS_ITEMCATEGORY
                .Select(d => new F_GS_ITEMCATEGORY
                {
                    CATID = d.CATID,
                    CATNAME = d.CATNAME
                }).ToListAsync();

            fGsProductInformationViewModel.FBasUnitsList = await DenimDbContext.F_BAS_UNITS
                .Select(d => new F_BAS_UNITS
                {
                    UID = d.UID,
                    UNAME = d.UNAME
                }).ToListAsync();

            if (edit)
            {
                fGsProductInformationViewModel.FGsItemsubCategoriesList = await DenimDbContext.F_GS_ITEMSUB_CATEGORY
                    .Select(d => new F_GS_ITEMSUB_CATEGORY
                    {
                        SCATID = d.SCATID,
                        SCATNAME = d.SCATNAME
                    }).ToListAsync();
            }

            return fGsProductInformationViewModel;
        }

        public async Task<FGenSRequisitionViewModel> GetInitObjForDetailsByAsync(FGenSRequisitionViewModel fGenSRequisitionViewModel)
        {
            foreach (var item in fGenSRequisitionViewModel.FGenSIndentdetailsesList)
            {
                item.PRODUCT = await DenimDbContext.F_GS_PRODUCT_INFORMATION
                    .Include(d => d.UNITNavigation)
                    .Where(d=>d.PRODID.Equals(item.PRODUCTID))
                    .Select(d=>new F_GS_PRODUCT_INFORMATION
                    {
                        PRODNAME = $"{d.PRODID} - {d.PRODNAME} {(d.PARTNO != "" ? " - " + d.PARTNO : "")}",
                        UNITNavigation = new F_BAS_UNITS
                        {
                            UNAME = d.UNITNavigation.UNAME
                        }
                    }).FirstOrDefaultAsync();
            }

            return fGenSRequisitionViewModel;
        }

        public async Task<F_GS_PRODUCT_INFORMATION> GetIndentListByPId(int productId)
        {
            return await DenimDbContext.F_GS_PRODUCT_INFORMATION
                .Include(d => d.F_GEN_S_INDENTDETAILS)
                .ThenInclude(d => d.GIND)
                .Include(d => d.UNITNavigation)
                .Where(d => d.PRODID.Equals(productId))
                .Select(d=> new F_GS_PRODUCT_INFORMATION
                {
                    UNITNavigation = new F_BAS_UNITS
                    {
                        UNAME = d.UNITNavigation.UNAME
                    },
                    F_GEN_S_INDENTDETAILS = d.F_GEN_S_INDENTDETAILS.Select(f=>new F_GEN_S_INDENTDETAILS
                    {
                        GIND = new F_GEN_S_INDENTMASTER
                        {
                            GINDID = f.GIND.GINDID,
                            GINDNO = f.GIND.GINDNO
                        }
                    }).ToList()
                }).FirstOrDefaultAsync();
        }

        public async Task<FGsProductInformationViewModel> FindByIdIncludeAllAsync(int prodId)
        {
            return await DenimDbContext.F_GS_PRODUCT_INFORMATION
                .Include(d=>d.SCAT)
                .Where(d => d.PRODID.Equals(prodId))
                .Select(d => new FGsProductInformationViewModel
                {
                    FGsProductInformation = new F_GS_PRODUCT_INFORMATION
                    {
                        EncryptedId = _protector.Protect(d.PRODID.ToString()),
                        PRODNAME = d.PRODNAME,
                        CATID = d.SCAT.CATID ?? 0,
                        SCATID = d.SCATID,
                        UNIT = d.UNIT,
                        DESCRIPTION = d.DESCRIPTION,
                        REMARKS = d.REMARKS,
                        PARTNO = d.PARTNO,
                        PROD_LOC = d.PROD_LOC
                    }
                }).FirstOrDefaultAsync();
        }
    }
}
