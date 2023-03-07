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
    public class SQLF_CHEM_STORE_INDENTMASTER_Repositiry : BaseRepository<F_CHEM_STORE_INDENTMASTER>, IF_CHEM_STORE_INDENTMASTER
    {
        private readonly IDataProtector _protector;

        public SQLF_CHEM_STORE_INDENTMASTER_Repositiry(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<IEnumerable<F_CHEM_STORE_INDENTMASTER>> GetAllChemicalIndentAsync()
        {
            return await DenimDbContext.F_CHEM_STORE_INDENTMASTER
                    .Include(e => e.INDSL)
                    .Select(m => new F_CHEM_STORE_INDENTMASTER
                    {
                        CINDID = m.CINDID,
                        CINDDATE = m.CINDDATE,
                        CINDNO = m.CINDNO,
                        INDSLID = m.INDSLID,
                        FChemStoreIndentType = new F_CHEM_STORE_INDENT_TYPE
                        {
                            INDENTTYPE = m.FChemStoreIndentType.INDENTTYPE
                        },
                        REMARKS = m.REMARKS,
                        STATUS = m.STATUS,
                        IsLocked = m.CREATED_AT < DateTime.Now.AddDays(-2)
                    }).ToListAsync();
        }

        public async Task<F_CHEM_STORE_INDENTMASTER> GetIndentByCindid(int id)
        {
            return await DenimDbContext.F_CHEM_STORE_INDENTMASTER
                .Select(e => new F_CHEM_STORE_INDENTMASTER
                {
                    CINDID = e.CINDID,
                    CINDDATE = e.CINDDATE
                }).FirstOrDefaultAsync(e => e.CINDID.Equals(id));
        }

        /// <summary>
        /// 200000001 => Identity Seed
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetLastChmIndNo()
        {
            var fChemStoreIndentmaster = DenimDbContext.F_CHEM_STORE_INDENTMASTER.AsQueryable();
            return fChemStoreIndentmaster.Any() ? await fChemStoreIndentmaster.MaxAsync(e => e.CINDID) + 1 : 200000001;
        }

        public async Task<FChemicalRequisitionViewModel> FindByIdIncludeAllAsync(int cindId)
        {
            var rs = await DenimDbContext.F_CHEM_STORE_INDENTMASTER
                    .Include(e => e.INDSL.F_CHEM_STORE_INDENTDETAILS)
                    .ThenInclude(e => e.PRODUCT)
                    .Include(e => e.F_CHEM_STORE_INDENTDETAILS)
                    .ThenInclude(e => e.PRODUCT)
                    .Where(e => e.CINDID.Equals(cindId))
                    .Select(e => new FChemicalRequisitionViewModel
                    {
                        FChemStoreIndentmaster = new F_CHEM_STORE_INDENTMASTER
                        {
                            CINDID = e.CINDID,
                            EncryptedId = _protector.Protect(e.CINDNO.ToString()),
                            CINDDATE = e.CINDDATE,
                            CINDNO = e.CINDNO,
                            INDSLID = e.INDSLID,
                            INDTYPE = e.INDTYPE,
                            REMARKS = e.REMARKS,
                            OPT1 = e.OPT1,
                            OPT2 = e.OPT2,
                            OPT3 = e.OPT3,
                            OPT4 = e.OPT4,
                            STATUS = e.STATUS,
                            INDSL = new F_CHEM_PURCHASE_REQUISITION_MASTER
                            {
                                F_CHEM_STORE_INDENTDETAILS = e.INDSL.F_CHEM_STORE_INDENTDETAILS.Select(f => new F_CHEM_STORE_INDENTDETAILS
                                {
                                    PRODUCT = f.PRODUCT
                                }).ToList()
                            }
                        },

                        FChemStoreIndentdetailsList = e.F_CHEM_STORE_INDENTDETAILS.Select(f => new F_CHEM_STORE_INDENTDETAILS
                        {
                            TRNSID = f.TRNSID,
                            TRNSDATE = f.TRNSDATE,
                            CINDID = f.CINDID,
                            INDSLID = f.INDSLID,
                            PRODUCTID = f.PRODUCTID,
                            UNIT = f.UNIT,
                            QTY = f.QTY,
                            VALIDITY = f.VALIDITY,
                            FULL_QTY = f.FULL_QTY,
                            ADD_QTY = f.ADD_QTY,
                            BAL_QTY = f.BAL_QTY,
                            LOCATION = f.LOCATION,
                            REMARKS = f.REMARKS,
                            OPT1 = f.OPT1,
                            OPT2 = f.OPT2,
                            OPT3 = f.OPT3,
                            PRODUCT = f.PRODUCT
                        }).ToList()
                    }).FirstOrDefaultAsync();

            return rs;
        }

        public async Task<FChemicalRequisitionViewModel> GetInitObjByAsync(FChemicalRequisitionViewModel fChemicalRequisition)
        {
            fChemicalRequisition.FChemStoreIndentTypes = await DenimDbContext.F_CHEM_STORE_INDENT_TYPE
                .OrderBy(e => e.INDENTTYPE).ToListAsync();

            fChemicalRequisition.FChemPurchaseRequisitionMasterList = await DenimDbContext.F_CHEM_PURCHASE_REQUISITION_MASTER
                .Include(e => e.Employee)
                .Include(e => e.ConcernEmployee)
                .Include(e => e.F_CHEM_STORE_INDENTDETAILS)
                .ThenInclude(e => e.PRODUCT)
                .Select(e => new F_CHEM_PURCHASE_REQUISITION_MASTER
                {
                    INDSLID = e.INDSLID,
                    INDSLDATE = e.INDSLDATE,
                    DEPTID = e.DEPTID,
                    SECID = e.SECID,
                    Employee = e.Employee,
                    ConcernEmployee = e.ConcernEmployee,
                    STATUS = e.STATUS,
                    REMARKS = e.REMARKS,
                    F_CHEM_STORE_INDENTDETAILS = e.F_CHEM_STORE_INDENTDETAILS
                }).Where(e => e.STATUS.Equals(false)).ToListAsync();

            return fChemicalRequisition;
        }
    }
}
