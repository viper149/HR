using System;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_GEN_S_INDENTDETAILS_Repository : BaseRepository<F_GEN_S_INDENTDETAILS>, IF_GEN_S_INDENTDETAILS
    {
        public SQLF_GEN_S_INDENTDETAILS_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<F_GEN_S_INDENTDETAILS> FindFGenSIndentListByIdAsync(int id, int prdId, bool edit=false)
        {
            return await DenimDbContext.F_GEN_S_INDENTDETAILS
                .Include(d => d.PRODUCT)
                .ThenInclude(d=>d.UNITNavigation)
                .Where(d => d.INDSLID.Equals(id) && d.PRODUCTID.Equals(prdId) && (!edit|| d.CREATED_AT < DateTime.Now.AddDays(-2)))
                .FirstOrDefaultAsync();
        }

        public async Task<FGenSRequisitionViewModel> GetInitObjForDetails(FGenSRequisitionViewModel fGenSRequisitionViewModel)
        {
            foreach (var item in fGenSRequisitionViewModel.FGenSIndentdetailsesList)
            {
                item.PRODUCT = await DenimDbContext.F_GS_PRODUCT_INFORMATION
                    .Include(d => d.UNITNavigation)
                    .Where(d => d.PRODID.Equals(item.PRODUCTID))
                    .Select(d => new F_GS_PRODUCT_INFORMATION
                    {
                        PRODNAME = $"{d.PRODID} - {d.PRODNAME} - {d.PARTNO}",
                        UNITNavigation = new F_BAS_UNITS
                        {
                            UNAME = d.UNITNavigation.UNAME
                        }
                    }).FirstOrDefaultAsync();
            }

            return fGenSRequisitionViewModel;
        }
    }
}
