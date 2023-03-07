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
    public class SQLACC_PHYSICAL_INVENTORY_FAB_Repository: BaseRepository<ACC_PHYSICAL_INVENTORY_FAB>, IACC_PHYSICAL_INVENTORY_FAB
    {
        private readonly IDataProtector _protector;

        public SQLACC_PHYSICAL_INVENTORY_FAB_Repository (DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector=dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<IEnumerable<ACC_PHYSICAL_INVENTORY_FAB>> GetAllAsync (string userId)
        {

            var accPhysicalInventoryFabs = await DenimDbContext.ACC_PHYSICAL_INVENTORY_FAB
                .Include(d => d.ROLL_.ROLL_)
                .Include(d => d.ROLL_.FABCODENavigation)
                .Where(d => d.CREATED_BY.Equals(userId))
                .Select(d => new ACC_PHYSICAL_INVENTORY_FAB
                {
                    FPI_ID=d.FPI_ID,
                    EncryptedId=_protector.Protect(d.FPI_ID.ToString()),
                    FPI_DATE=d.FPI_DATE,
                    ROLL_=new F_FS_FABRIC_RCV_DETAILS
                    {
                        ROLL_=new F_PR_INSPECTION_PROCESS_DETAILS
                        {
                            ROLLNO=d.ROLL_.ROLL_.ROLLNO,
                            FAB_GRADE=d.ROLL_.ROLL_.FAB_GRADE
                        },
                        FABCODENavigation=new RND_FABRICINFO
                        {
                            STYLE_NAME=d.ROLL_.FABCODENavigation.STYLE_NAME
                        }
                    }
                }).OrderBy(d => d.FPI_ID).Distinct().ToListAsync();

            var index = 1;

            foreach (var item in accPhysicalInventoryFabs)
            {
                item.INDEX=index;
                index++;
            }

            return accPhysicalInventoryFabs;
        }

        public async Task<int> GetRcvIdByRollNoAsync (string rollNo)
        {
            return await DenimDbContext.F_FS_FABRIC_RCV_DETAILS
                .Include(d => d.ROLL_)
                .Where(d => d.ROLL_.ROLLNO.Equals(rollNo))
                .Select(d => d.TRNSID).FirstOrDefaultAsync();
        }

        public async Task<bool> FindReceivedByRoll (string roll)
        {
            return await DenimDbContext.F_FS_FABRIC_RCV_DETAILS
                .Include(d => d.ROLL_)
                .AnyAsync(d => d.ROLL_.ROLLNO.Equals(roll));
        }

        public async Task<bool> FindDuplicateByRoll (string roll)
        {
            return !await DenimDbContext.ACC_PHYSICAL_INVENTORY_FAB
                .Include(d => d.ROLL_.ROLL_)
                .AnyAsync(d => d.ROLL_.ROLL_.ROLLNO.Equals(roll));
        }
    }
}
