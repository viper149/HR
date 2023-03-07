using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_PR_FN_CHEMICAL_CONSUMPTION_Repository:BaseRepository<F_PR_FN_CHEMICAL_CONSUMPTION>, IF_PR_FN_CHEMICAL_CONSUMPTION
    {
        public SQLF_PR_FN_CHEMICAL_CONSUMPTION_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<IEnumerable<F_PR_FN_CHEMICAL_CONSUMPTION>> GetInitChemData(
            List<F_PR_FN_CHEMICAL_CONSUMPTION> fPrFnChemicalConsumptions)
        {
            try
            {
                foreach (var item in fPrFnChemicalConsumptions)
                {
                    item.CHEM_PROD_ = await DenimDbContext.F_CHEM_STORE_PRODUCTINFO.FirstOrDefaultAsync(c=>c.PRODUCTID.Equals(item.CHEM_PROD_ID));
                }
                return fPrFnChemicalConsumptions;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        //public async Task<IEnumerable<F_PR_FN_CHEMICAL_CONSUMPTION>> GetChemList(int? setId)
        //{
        //    try
        //    {
        //        var fPrFnChemicalConsumptionsList = await _denimDbContext.F_PR_FN_CHEMICAL_CONSUMPTION
        //            .Where(c => c.SETID.Equals(setId)).ToListAsync();
        //        return fPrFnChemicalConsumptionsList;
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e);
        //        throw;
        //    }
        //}
    }
}
