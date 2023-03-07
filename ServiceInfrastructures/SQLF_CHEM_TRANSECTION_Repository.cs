using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_CHEM_TRANSECTION_Repository : BaseRepository<F_CHEM_TRANSECTION>, IF_CHEM_TRANSECTION
    {
        private readonly IDataProtector _protector;

        public SQLF_CHEM_TRANSECTION_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<double> GetLastBalanceByProductIdAsync(int? id,int cRcvId)
        {
            var result = await DenimDbContext.F_CHEM_TRANSECTION
                .Where(c => c.PRODUCTID.Equals(id) && c.CRCVID.Equals(cRcvId))
                .OrderBy(e => e.CTRID)
                .Select(e => e.BALANCE)
                .LastOrDefaultAsync();

            return result ?? 0;
        }

        public async Task<IEnumerable<F_CHEM_TRANSECTION>> GetAllTransactions()
        {
            try
            {
                var fChemTransections = await DenimDbContext.F_CHEM_TRANSECTION
                    .Include(e => e.ISSUE)
                    .Include(e => e.RCVT)
                    .Include(e => e.PRODUCT)
                    .Include(c=>c.CRCV)
                    .Select(e => new F_CHEM_TRANSECTION
                    {
                        CTRID = e.CTRID,
                        EncryptedId = _protector.Protect(e.CTRID.ToString()),
                        PRODUCT = new F_CHEM_STORE_PRODUCTINFO
                        {
                            PRODUCTNAME = e.PRODUCT.PRODUCTNAME
                        },
                        CTRDATE = e.CTRDATE,
                        CRCVID = e.CRCVID,
                        RCVT = new F_BAS_RECEIVE_TYPE
                        {
                            RCVTYPE = e.RCVT.RCVTYPE
                        },
                        CISSUEID = e.CISSUEID,
                        ISSUE = new F_BAS_ISSUE_TYPE
                        {
                            ISSUTYPE = e.ISSUE.ISSUTYPE
                        },
                        OP_BALANCE = e.OP_BALANCE,
                        RCV_QTY = e.RCV_QTY,
                        ISSUE_QTY = e.ISSUE_QTY,
                        BALANCE = e.BALANCE,
                        REMARKS = e.REMARKS,
                        CRCV = new F_CHEM_STORE_RECEIVE_DETAILS
                        {
                            BATCHNO = e.CRCV.BATCHNO
                        }
                    }).ToListAsync();

                return fChemTransections;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
