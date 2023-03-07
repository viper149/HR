using System;
using System.Threading.Tasks;
using DenimERP.Security;
using DenimERP.ServiceInterfaces;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Controllers
{
    public class RndYarnConsumptionController : Controller
    {
        private readonly IRND_YARNCONSUMPTION _rndYarnConsumption;

        private readonly IDataProtector _protector;

        public RndYarnConsumptionController(IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            IRND_YARNCONSUMPTION rndYarnConsumption
        )
        {
            _rndYarnConsumption = rndYarnConsumption;
            this._protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<double> GetConsumption(int countId, int fabCode,int yarnFor)
        {
            try
            {
                var data = await _rndYarnConsumption.GetConsumptionByCountIdAndFabCodeAsync(countId, fabCode, yarnFor);
                return data;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}