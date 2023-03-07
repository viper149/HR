using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.Models.Chart;
using DenimERP.ServiceInterfaces;
using DenimERP.ServiceInterfaces.Hubs;
using DenimERP.ViewModels.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace DenimERP.Controllers
{
    [Authorize]
    public class HomeController: Controller
    {
        public IF_CHEM_ISSUE_DETAILS ChemIssueDetails;
        private readonly IF_LCB_PRODUCTION_ROPE_MASTER _fLcbProductionRopeMaster;
        private readonly IF_DYEING_PROCESS_ROPE_MASTER _fDyeingProcessRopeMaster;
        private readonly IF_PR_WARPING_PROCESS_SW_MASTER _fPrWarpingProcessSwMaster;
        private readonly IF_PR_WARPING_PROCESS_ROPE_MASTER _fPrWarpingProcessRopeMaster;
        private readonly ICOM_IMP_LCINFORMATION _comImpLcinformation;
        private readonly IF_PR_FINISHING_PROCESS_MASTER _fPrFinishingProcessMaster;
        private readonly IF_DYEING_PROCESS_ROPE_DETAILS _fDyeingProcessRopeDetails;
        private readonly IF_PR_WARPING_PROCESS_ECRU_MASTER _fPrWarpingProcessEcruMaster;
        private readonly IF_PR_WARPING_PROCESS_DW_MASTER _fPrWarpingProcessDwMaster;
        private readonly IF_PR_WARPING_PROCESS_DW_MASTER _warpingProcessDwMaster;
        private readonly IF_CHEM_ISSUE_DETAILS _chemIssueDetails;
        private readonly IF_YS_YARN_ISSUE_DETAILS _ysYarnIssueDetails;
        private readonly IACC_EXPORT_REALIZATION _accExportRealization;
        private readonly IF_FS_DELIVERYCHALLAN_PACK_DETAILS _fFsDeliverychallanPackDetails;
        private readonly IF_PR_FINISHING_FNPROCESS _fPrFinishingFnprocess;
        private readonly IF_PR_INSPECTION_PROCESS_DETAILS _fPrInspectionProcessDetails;
        private readonly IF_PR_WEAVING_PRODUCTION _fPrWeavingProduction;
        private readonly IF_PR_SIZING_PROCESS_ROPE_DETAILS _fPrSizingProcessRopeDetails;
        private readonly IF_PR_SIZING_PROCESS_ROPE_MASTER _fPrSizingProcessRopeMaster;
        private readonly ICOM_EX_LCINFO _comExLcinfo;
        private readonly IRND_FABRICINFO _rndFabricinfo;
        private readonly ICOM_EX_PIMASTER _comExPimaster;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMESSAGE _message;
        private readonly IMESSAGE_INDIVIDUAL _messageIndividual;

        public HomeController(ICOM_IMP_LCINFORMATION comImpLcinformation,
            IF_PR_FINISHING_PROCESS_MASTER fPrFinishingProcessMaster,
            IF_DYEING_PROCESS_ROPE_DETAILS fDyeingProcessRopeDetails,
            IF_PR_WARPING_PROCESS_ECRU_MASTER fPrWarpingProcessEcruMaster,
            IF_PR_WARPING_PROCESS_DW_MASTER fPrWarpingProcessDwMaster,
            IF_CHEM_ISSUE_DETAILS chemIssueDetails,
            IF_YS_YARN_ISSUE_DETAILS ysYarnIssueDetails,
            IACC_EXPORT_REALIZATION accExportRealization,
            IF_FS_DELIVERYCHALLAN_PACK_DETAILS fFsDeliverychallanPackDetails,
            IF_PR_FINISHING_FNPROCESS fPrFinishingFnprocess,
            IF_PR_INSPECTION_PROCESS_DETAILS fPrInspectionProcessDetails,
            IF_PR_WEAVING_PRODUCTION fPrWeavingProduction,
            IF_PR_SIZING_PROCESS_ROPE_MASTER fPrSizingProcessRopeMaster,
            IF_PR_SIZING_PROCESS_ROPE_DETAILS fPrSizingProcessRopeDetails,
            IF_LCB_PRODUCTION_ROPE_MASTER fLcbProductionRopeMaster,
            IF_PR_WARPING_PROCESS_ROPE_MASTER warpingProcessRopeMaster,
            IF_DYEING_PROCESS_ROPE_MASTER fDyeingProcessRopeMaster,
            IF_PR_WARPING_PROCESS_SW_MASTER fPrWarpingProcessSwMaster,
            ICOM_EX_LCINFO comExLcinfo,
            IRND_FABRICINFO rndFabricinfo,
            ICOM_EX_PIMASTER comExPimaster,
            UserManager<ApplicationUser> userManager,
            IMESSAGE message,
            IMESSAGE_INDIVIDUAL messageIndividual)
        {

            _fLcbProductionRopeMaster = fLcbProductionRopeMaster;
            _fPrWarpingProcessRopeMaster = warpingProcessRopeMaster;
            _fDyeingProcessRopeMaster = fDyeingProcessRopeMaster;
            _fPrWarpingProcessSwMaster = fPrWarpingProcessSwMaster;
            _comImpLcinformation = comImpLcinformation;
            _fPrFinishingProcessMaster = fPrFinishingProcessMaster;
            _fDyeingProcessRopeDetails = fDyeingProcessRopeDetails;
            _fPrWarpingProcessEcruMaster = fPrWarpingProcessEcruMaster;
            _fPrWarpingProcessDwMaster = fPrWarpingProcessDwMaster;
            _chemIssueDetails = chemIssueDetails;
            _ysYarnIssueDetails = ysYarnIssueDetails;
            _accExportRealization = accExportRealization;
            _fFsDeliverychallanPackDetails = fFsDeliverychallanPackDetails;
            _fPrFinishingFnprocess = fPrFinishingFnprocess;
            _fPrInspectionProcessDetails = fPrInspectionProcessDetails;
            _fPrWeavingProduction = fPrWeavingProduction;
            _fPrSizingProcessRopeMaster = fPrSizingProcessRopeMaster;
            _fPrSizingProcessRopeDetails = fPrSizingProcessRopeDetails;
            _comExLcinfo = comExLcinfo;
            _rndFabricinfo = rndFabricinfo;
            _comExPimaster = comExPimaster;
            _userManager = userManager;
            _message = message;
            _messageIndividual = messageIndividual;
        }

        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            try
            {
                return View();
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        [HttpPost]
        public IActionResult CultureManagement(string culture, string returnUrl)
        {
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.Now.AddDays(30) });
            return LocalRedirect(returnUrl);
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var messages = await _message.GetAllIncludeOtherObjects();
                var users = await _userManager.Users.ToListAsync();
                var currentUser = await _userManager.GetUserAsync(User);

                ViewBag.CurrentUserId = currentUser.Id;

                return View(new ChatUsersMesaages<MESSAGE, ApplicationUser>(messages, users, currentUser));
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetUsersName(string recipient)
        {
            try
            {
                return PartialView("GetUsersName", await _userManager.Users
                    .Where(e => e.UserName.Contains(recipient))
                    .Select(e => new ApplicationUser
                    {
                        Id = e.Id,
                        UserName = e.UserName
                    }).ToListAsync());
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetSpecificUserMessage(string userId)
        {
            try
            {
                var task = await _messageIndividual.All();
                var user = await _userManager.GetUserAsync(User);
                var messageIndividuals = task.Where(e => e.ReceiverId.Equals(userId) && e.SenderId.Equals(user.Id)).ToList();

                return PartialView("GetSpecificUserMessage", new ChatUsersMesaages<MESSAGE_INDIVIDUAL, ApplicationUser>(messageIndividuals, null, user));
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        [HttpGet]
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        [HttpGet]
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        [HttpGet]
        public IActionResult Privacy()
        {
            return View();
        }

    }

}

