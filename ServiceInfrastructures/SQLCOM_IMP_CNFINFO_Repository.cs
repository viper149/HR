using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels.Com.CnfInfo;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLCOM_IMP_CNFINFO_Repository : BaseRepository<COM_IMP_CNFINFO>, ICOM_IMP_CNFINFO
    {
        private readonly IDataProtector _protector;

        public SQLCOM_IMP_CNFINFO_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<IEnumerable<COM_IMP_CNFINFO>> GetAllForDataTables()
        {
            return await DenimDbContext.COM_IMP_CNFINFO.Select(e => new COM_IMP_CNFINFO
            {
                EncryptedId = _protector.Protect(e.CNFID.ToString()),
                CNFNAME = e.CNFNAME,
                ADDRESS = e.ADDRESS,
                C_PERSON = e.C_PERSON,
                REMARKS = e.REMARKS
            }).ToListAsync();
        }

        public async Task<ComImpCnfInfoViewModel> FindByIdIncludeAllAsync(int cnfId)
        {
            return await DenimDbContext.COM_IMP_CNFINFO.Select(e => new ComImpCnfInfoViewModel
            {
                ComImpCnfinfo = new COM_IMP_CNFINFO
                {
                    CNFID = e.CNFID,
                    EncryptedId = _protector.Protect(e.CNFID.ToString()),
                    CNFNAME = e.CNFNAME,
                    ADDRESS = e.ADDRESS,
                    C_PERSON = e.C_PERSON,
                    CELL_NO = e.CELL_NO,
                    REMARKS = e.REMARKS,
                    OPT1 = e.OPT1,
                    OPT2 = e.OPT2,
                    OPT3 = e.OPT3
                }
            }).FirstOrDefaultAsync(e => e.ComImpCnfinfo.CNFID.Equals(cnfId));
        }
    }
}
