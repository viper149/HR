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
    public class SQLF_CHEM_STORE_PRODUCTINFO_Repository : BaseRepository<F_CHEM_STORE_PRODUCTINFO>, IF_CHEM_STORE_PRODUCTINFO
    {
        private readonly IDataProtector _protector;

        public SQLF_CHEM_STORE_PRODUCTINFO_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<dynamic> GetSingleProductDetailsByPID(int id)
        {
            return await DenimDbContext.F_CHEM_STORE_PRODUCTINFO
                .Include(e => e.UNITNAVIGATION)
                .Where(e => e.PRODUCTID.Equals(id))
                .Select(e => new
                {
                    UID = e.UNITNAVIGATION.UID,
                    UNAME = e.UNITNAVIGATION.UNAME,
                    BALANCE = DenimDbContext.F_CHEM_TRANSECTION.Where(f => f.PRODUCTID.Equals(e.PRODUCTID)).Sum(f => f.BALANCE)
                }).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<F_CHEM_STORE_PRODUCTINFO>> GetProductDD()
        {
            return await DenimDbContext.F_CHEM_STORE_PRODUCTINFO
                .Include(e => e.UNITNAVIGATION)
                .Include(e => e.TYPENAVIGATION)
                .Include(e => e.COUNTRIES)
                .Select(e => new F_CHEM_STORE_PRODUCTINFO
                {
                    EncryptedId = _protector.Protect(e.PRODUCTID.ToString()),
                    PRODUCTNAME = e.PRODUCTNAME,
                    SIZE = e.SIZE,
                    REMARKS = e.REMARKS,
                    UNITNAVIGATION = new F_BAS_UNITS
                    {
                        UNAME = e.UNITNAVIGATION.UNAME
                    },
                    TYPENAVIGATION = new F_CHEM_TYPE
                    {
                        CTYPE = e.TYPENAVIGATION.CTYPE
                    },
                    COUNTRIES = new COUNTRIES
                    {
                        COUNTRY_NAME = e.COUNTRIES.COUNTRY_NAME
                    }
                }).ToListAsync();
        }

        public async Task<dynamic> GetProducts()
        {
            return await DenimDbContext.F_CHEM_TRANSECTION
                .Include(e => e.PRODUCT)
                .Include(e => e.CRCV)
                .Where(e => e.BALANCE > 0)
                .Select(e => new
                {
                    TRNSID = e.CRCV.TRNSID,
                    PRODUCTNAME = $"{e.PRODUCT.PRODUCTNAME}, Batch no: {e.CRCV.BATCHNO}"
                }).Distinct().ToListAsync();
        }

        public async Task<FChemicalRequisitionViewModel> GetInitObjForDetailsByAsync(FChemicalRequisitionViewModel requisitionViewModel)
        {
            foreach (var item in requisitionViewModel.FChemStoreIndentdetailsList)
            {
                item.PRODUCT = await DenimDbContext.F_CHEM_STORE_PRODUCTINFO.FirstOrDefaultAsync(e => e.PRODUCTID.Equals(item.PRODUCTID));
                item.FBasUnits = await DenimDbContext.F_BAS_UNITS.FirstOrDefaultAsync(e => e.UID.Equals(item.UNIT));
            }

            return requisitionViewModel;
        }

        public async Task<FChemProductEntryViewModel> GetInitObjByAsync(FChemProductEntryViewModel fChemProductEntryViewModel)
        {
            fChemProductEntryViewModel.FBasUnitsList = await DenimDbContext.F_BAS_UNITS.Select(e => new F_BAS_UNITS
            {
                UID = e.UID,
                UNAME = e.UNAME
            }).OrderBy(e => e.UNAME).ToListAsync();

            fChemProductEntryViewModel.FChemTypeList = await DenimDbContext.F_CHEM_TYPE.Select(e => new F_CHEM_TYPE
            {
                CTID = e.CTID,
                CTYPE = e.CTYPE
            }).OrderBy(e => e.CTYPE).ToListAsync();

            fChemProductEntryViewModel.Countries = await DenimDbContext.COUNTRIES.Select(e => new COUNTRIES
            {
                ID = e.ID,
                COUNTRY_NAME = e.COUNTRY_NAME
            }).OrderBy(e => e.COUNTRY_NAME).ToListAsync();

            return fChemProductEntryViewModel;
        }

        public async Task<FChemProductEntryViewModel> FindByIdIncludeAllAsync(int productId)
        {
            return await DenimDbContext.F_CHEM_STORE_PRODUCTINFO
                .Where(e => e.PRODUCTID.Equals(productId))
                .Select(e => new FChemProductEntryViewModel
                {
                    FChemStoreProductinfo = new F_CHEM_STORE_PRODUCTINFO
                    {
                        EncryptedId = _protector.Protect(e.PRODUCTID.ToString()),
                        PRODUCTNAME = e.PRODUCTNAME,
                        UNIT = e.UNIT,
                        TYPE = e.TYPE,
                        ORIGIN = e.ORIGIN,
                        SIZE = e.SIZE,
                        REMARKS = e.REMARKS
                    }
                }).FirstOrDefaultAsync();
        }

        public async Task<bool> FindByProdName(string prodName)
        {
            return !await DenimDbContext.F_CHEM_STORE_PRODUCTINFO.AnyAsync(d => d.PRODUCTNAME.Equals(prodName));
        }

        public async Task<FChemProductEntryViewModel> FindByIdIncludeAllForDetailsAsync(int productId)
        {
            return await DenimDbContext.F_CHEM_STORE_PRODUCTINFO
                .Include(e => e.TYPENAVIGATION)
                .Include(e => e.UNITNAVIGATION)
                .Include(e => e.COUNTRIES)
                .Where(e => e.PRODUCTID.Equals(productId))
                .Select(e => new FChemProductEntryViewModel
                {
                    FChemStoreProductinfo = new F_CHEM_STORE_PRODUCTINFO
                    {
                        EncryptedId = _protector.Protect(e.PRODUCTID.ToString()),
                        PRODUCTNAME = e.PRODUCTNAME,
                        SIZE = e.SIZE,
                        REMARKS = e.REMARKS,
                        UNITNAVIGATION = new F_BAS_UNITS
                        {
                            UNAME = e.UNITNAVIGATION.UNAME
                        },
                        TYPENAVIGATION = new F_CHEM_TYPE
                        {
                            CTYPE = e.TYPENAVIGATION.CTYPE
                        },
                        COUNTRIES = new COUNTRIES
                        {
                            COUNTRY_NAME = e.COUNTRIES.COUNTRY_NAME
                        }
                    }
                }).FirstOrDefaultAsync();
        }
    }
}
