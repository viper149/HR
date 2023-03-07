using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Controllers
{
    public class FGsReturnGpRcvController : Controller
    {
        private readonly IF_HRD_EMPLOYEE _fHrEmployee;
        private readonly IF_GS_GATEPASS_INFORMATION_M _fGsGatepassInformationM;
        private readonly IF_GS_GATEPASS_INFORMATION_D _fGsGatepassInformationD;
        private readonly IDataProtector _protector;
        private readonly UserManager<ApplicationUser> _userManager;

        public FGsReturnGpRcvController(
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            IF_HRD_EMPLOYEE fHrEmployee,
            IF_GS_GATEPASS_INFORMATION_M fGsGatepassInformationM,
            IF_GS_GATEPASS_INFORMATION_D fGsGatepassInformationD,
            UserManager<ApplicationUser> userManager
        )
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
            _fHrEmployee = fHrEmployee;
            _fGsGatepassInformationM = fGsGatepassInformationM;
            _fGsGatepassInformationD = fGsGatepassInformationD;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> CreateFGsReturnRcvGatePass()
        {
            var fGsReturnRcvGatePassViewModel = await GetInfo(new FGsReturnRcvGatePassViewModel());
            return View(fGsReturnRcvGatePassViewModel);
        }

        public async Task<FGsReturnRcvGatePassViewModel> GetInfo(FGsReturnRcvGatePassViewModel fGsReturnRcvGatePassViewModel)
        {
            var employees = await _fHrEmployee.GetAllEmployeesAsync();
            var gatePassIssue = await _fGsGatepassInformationM.GetAllGsGatePassAsync();

            fGsReturnRcvGatePassViewModel.GetFHrEmployeeViewModels = employees.ToList();
            fGsReturnRcvGatePassViewModel.FGsGatepassInformationMs = gatePassIssue.ToList();

            return fGsReturnRcvGatePassViewModel;
        }

        [HttpGet]
        public async Task<IEnumerable<F_GS_GATEPASS_INFORMATION_D>> GetGsGatePassInfoByGpId(int gpId)
        {
            return await _fGsGatepassInformationD.GetGsGatePassInfoByGpId(gpId);
        }
    }
}