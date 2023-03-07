using System;
using System.Collections.Generic;
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
    public class SQLF_GEN_S_RECEIVE_DETAILS_Repository : BaseRepository<F_GEN_S_RECEIVE_DETAILS>, IF_GEN_S_RECEIVE_DETAILS
    {
        public SQLF_GEN_S_RECEIVE_DETAILS_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public IEnumerable<F_GEN_S_RECEIVE_DETAILS> FindAllGenSByReceiveIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<FGenSReceiveViewModel> GetInitObjForDetails(FGenSReceiveViewModel fGenSReceiveViewModel)
        {
            foreach (var item in fGenSReceiveViewModel.FGenSReceiveDetailsesList)
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

                item.GIND = await DenimDbContext.F_GEN_S_INDENTMASTER
                    .Where(d => d.GINDID.Equals(item.GINDID))
                    .Select(d => new F_GEN_S_INDENTMASTER
                    {
                        GINDNO = d.GINDNO
                    })
                    .FirstOrDefaultAsync();
            }

            return fGenSReceiveViewModel;
        }
    }
}
