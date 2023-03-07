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
    public class HomeController : Controller
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
            //var totalPercentageOfComImpLcInformationList = await _comImpLcinformation.TotalPercentageOfComImpLcInformationList(DateTime.Now, 30 * 12);
            var totalPercentageOfComExLcInfoList = await _comExLcinfo.TotalPercentageOfComExLcInfoList(DateTime.Now, 30 * 12);
            var totalNumberOfFabricInfo = await _rndFabricinfo.TotalNumberOfFabricInfoExcludingAllByAsync();
            var totalPercentageOfComExPiMaster = await _comExPimaster.TotalPercentageOfComExPiMaster(30);
            var result = await _fPrWarpingProcessRopeMaster.GetWarpingDateWiseLengthGraph();




            var dataPoints = new List<DataPoint>
            {
                //new DataPoint("L/C (Import)",Convert.ToDouble(totalPercentageOfComImpLcInformationList)),
                new DataPoint("L/C (Export)",Convert.ToDouble(totalPercentageOfComExLcInfoList)),
                new DataPoint("RnD Fabric Information", Convert.ToDouble(totalNumberOfFabricInfo)),
                new DataPoint("Commercial PI (Export)",Convert.ToDouble(totalPercentageOfComExPiMaster)),
            };


            var dataPoints2 = new List<DataPoint2>
            {
                new(110, 44),
                new (116, 31),
                new (124, 43),
                new (129, 45),
                new (131, 56),
                new (138, 79),
                new (142, 57),
                new (150, 56),
                new (153, 58),
                new (155, 92),
                new (156, 78),
                new (159, 64),
                new (164, 88),
                new (168, 112),
                new (174, 101)
            };

            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);
            ViewBag.DataPoints2 = JsonConvert.SerializeObject(dataPoints2);
            ViewBag.PIChartData = await _comExPimaster.GetPIChartData();
            ViewBag.LCChartData = await _comExLcinfo.GetLCChartData();
            ViewBag.DyeingChartData = await _fDyeingProcessRopeMaster.GetDyeingDateWiseLength();
            ViewBag.InspectionChartData = await _fPrInspectionProcessDetails.GetInspectionTotalLength();
            ViewBag.FabricDeliveryChartData = await _fFsDeliverychallanPackDetails.GetFabricDeliveryChallanLength();
            ViewBag.RealizationChartData = await _accExportRealization.GetRealizatioData();
            ViewBag.WeavingChartData = await _fPrWeavingProduction.GetWeavingDateWiseLengthGraph();
            ViewBag.TopStyleProductionDataList = await _fPrInspectionProcessDetails.GetTopStyleProductionData();
            ViewBag.SizingChartData = await _fPrSizingProcessRopeDetails.GetSizingDateWiseLengthGraph();
            ViewBag.IssuedCountData = await _ysYarnIssueDetails.GetIssuedCountData();
            ViewBag.IssuedChemicalData = await _chemIssueDetails.GetIssuedChemicalData();
            ViewBag.WarpingProductionData = await _fPrWarpingProcessRopeMaster.GetWarpingDateWiseLengthGraph();
            ViewBag.WarpingData = await _fPrWarpingProcessRopeMaster.GetWarpingDataDayMonthAsync();
            ViewBag.SizingProductionData = await _fPrSizingProcessRopeMaster.GetSizingDateWiseLengthGraph();
            ViewBag.SizingData = await _fPrSizingProcessRopeMaster.GetSizingDataDayMonthAsync();
            ViewBag.FinishingProductionData = await _fPrFinishingFnprocess.GetFinishingDateWiseLengthGraph();
            ViewBag.FinishingData = await _fPrFinishingFnprocess.GetFinishingDataDayMonthAsync();

            return View();
        }

        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> DashboardWarping()
        {
            ViewBag.WarpingData = await _fPrWarpingProcessRopeMaster.GetWarpingDataDayMonthAsync();
            ViewBag.WarpingPendingSets = await _fPrWarpingProcessRopeMaster.GetWarpingPendingSets();
            ViewBag.WarpingPendingSetList = await _fPrWarpingProcessRopeMaster.GetWarpingPendingSetList();
            ViewBag.BudgetConsumedYarn = await _fPrWarpingProcessRopeMaster.GetBudgetConsumedYarn();
            ViewBag.RopeWarpingProductionData = await _fPrWarpingProcessRopeMaster.GetRopeWarpingProductionData();
            ViewBag.DirectWarpingProductionData = await _fPrWarpingProcessDwMaster.GetDirectWarpingProductionData();
            ViewBag.EcruWarpingProductionData = await _fPrWarpingProcessEcruMaster.GetEcruWarpingProductionData();
            ViewBag.RopeWarpingProductionList = await _fPrWarpingProcessRopeMaster.GetRopeWarpingProductionList();
            ViewBag.EcruWarpingProductionList = await _fPrWarpingProcessEcruMaster.GetEcruWarpingProductionList();
            ViewBag.DirectWarpingProductionList = await _fPrWarpingProcessDwMaster.GetDirectWarpingProductionList();
            ViewBag.SectionalWarpingProductionList = await _fPrWarpingProcessSwMaster.GetSectionalWarpingProductionList();
            ViewBag.WarpingProductionList = await _fPrWarpingProcessRopeMaster.GetWarpingProductionList();
            ViewBag.MonthlyWarpingPendingsAndCompleteSets = await _fPrWarpingProcessRopeMaster.MonthlyWarpingPendingsAndCompleteSets();



            try
            {
                return View();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [AcceptVerbs("GET","POST")]
        public async Task<IActionResult> DashboardDyeing()
        {
            ViewBag.DyeingPendingSets = await _fDyeingProcessRopeDetails.GetDyeingPendingSets();
            ViewBag.DyeingPendingSetList = await _fDyeingProcessRopeDetails.GetDyeingPendingSetList();
            ViewBag.DyeingProductionList = await _fDyeingProcessRopeDetails.GetDyeingProductionList();
            ViewBag.MonthlyDyeingPendingAndCompleteSets = await _fDyeingProcessRopeDetails.GetMonthlyDyeingPendingAndCompleteSets();
            ViewBag.DyeingChemicalConsumed = await _fDyeingProcessRopeDetails.GetDyeingChemicalConsumed();
            ViewBag.DyeingProductionData = await _fDyeingProcessRopeDetails.GetDyeingProductionData();

            try
            {
                return View();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> DashboardLCB()
        {
            ViewBag.LCBProductionData = await _fLcbProductionRopeMaster.GetLCBProductionData();
            ViewBag.LCBProductionList = await _fLcbProductionRopeMaster.GetLCBProductionList();
            ViewBag.MonthlyLCBPendingAndCompleteSets = await _fLcbProductionRopeMaster.GetMonthlyLCBPendingAndCompleteSets();
            ViewBag.LcbPendingSetList = await _fLcbProductionRopeMaster.GeLcbPendingSetList();


            try
            {
                return View();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> DashboardSizing()
        {
            ViewBag.SizingroductionData = await _fPrSizingProcessRopeMaster.GetSizingProductionData();
            ViewBag.SizingProductionList = await _fPrSizingProcessRopeMaster.GetSizingProductionList();
            ViewBag.SizingPendingSetList = await _fPrSizingProcessRopeMaster.GetSizingPendingSetList();


            try
            {
                return View();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> DashboardWeaving()
        {
            ViewBag.WeavingProductionData = await _fPrWeavingProduction.GetWeavingProductionData();
            ViewBag.WeavingProductionList = await _fPrWeavingProduction.GetWeavingProductionList();
            ViewBag.WeavingPendingList = await _fPrWeavingProduction.GetWeavingPendingList();



            try
            {
                return View();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> DashboardFinishing()
        {
            ViewBag.FinishingProductionData = await _fPrFinishingFnprocess.GetFinishingProductionData();


            ViewBag.WarpingData = await _fPrWarpingProcessRopeMaster.GetWarpingDataDayMonthAsync();
            ViewBag.WarpingPendingSets = await _fPrWarpingProcessRopeMaster.GetWarpingPendingSets();
            ViewBag.WarpingPendingSetList = await _fPrWarpingProcessRopeMaster.GetWarpingPendingSetList();
            ViewBag.BudgetConsumedYarn = await _fPrWarpingProcessRopeMaster.GetBudgetConsumedYarn();
            ViewBag.RopeWarpingProductionData = await _fPrWarpingProcessRopeMaster.GetRopeWarpingProductionData();
            ViewBag.DirectWarpingProductionData = await _fPrWarpingProcessDwMaster.GetDirectWarpingProductionData();
            ViewBag.EcruWarpingProductionData = await _fPrWarpingProcessEcruMaster.GetEcruWarpingProductionData();
            ViewBag.RopeWarpingProductionList = await _fPrWarpingProcessRopeMaster.GetRopeWarpingProductionList();
            ViewBag.EcruWarpingProductionList = await _fPrWarpingProcessEcruMaster.GetEcruWarpingProductionList();
            ViewBag.DirectWarpingProductionList = await _fPrWarpingProcessDwMaster.GetDirectWarpingProductionList();
            ViewBag.SectionalWarpingProductionList = await _fPrWarpingProcessSwMaster.GetSectionalWarpingProductionList();
            ViewBag.WarpingProductionList = await _fPrWarpingProcessRopeMaster.GetWarpingProductionList();
            ViewBag.MonthlyWarpingPendingsAndCompleteSets = await _fPrWarpingProcessRopeMaster.MonthlyWarpingPendingsAndCompleteSets();



            try
            {
                return View();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> DashboardInspection()
        {
            ViewBag.WarpingData = await _fPrWarpingProcessRopeMaster.GetWarpingDataDayMonthAsync();
            ViewBag.WarpingPendingSets = await _fPrWarpingProcessRopeMaster.GetWarpingPendingSets();
            ViewBag.WarpingPendingSetList = await _fPrWarpingProcessRopeMaster.GetWarpingPendingSetList();
            ViewBag.BudgetConsumedYarn = await _fPrWarpingProcessRopeMaster.GetBudgetConsumedYarn();
            ViewBag.RopeWarpingProductionData = await _fPrWarpingProcessRopeMaster.GetRopeWarpingProductionData();
            ViewBag.DirectWarpingProductionData = await _fPrWarpingProcessDwMaster.GetDirectWarpingProductionData();
            ViewBag.EcruWarpingProductionData = await _fPrWarpingProcessEcruMaster.GetEcruWarpingProductionData();
            ViewBag.RopeWarpingProductionList = await _fPrWarpingProcessRopeMaster.GetRopeWarpingProductionList();
            ViewBag.EcruWarpingProductionList = await _fPrWarpingProcessEcruMaster.GetEcruWarpingProductionList();
            ViewBag.DirectWarpingProductionList = await _fPrWarpingProcessDwMaster.GetDirectWarpingProductionList();
            ViewBag.SectionalWarpingProductionList = await _fPrWarpingProcessSwMaster.GetSectionalWarpingProductionList();
            ViewBag.WarpingProductionList = await _fPrWarpingProcessRopeMaster.GetWarpingProductionList();
            ViewBag.MonthlyWarpingPendingsAndCompleteSets = await _fPrWarpingProcessRopeMaster.MonthlyWarpingPendingsAndCompleteSets();



            try
            {
                return View();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
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
                return View($"Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetUsersName(string recipient)
        {
            try
            {
                return PartialView($"GetUsersName", await _userManager.Users
                    .Where(e => e.UserName.Contains(recipient))
                    .Select(e => new ApplicationUser
                    {
                        Id = e.Id,
                        UserName = e.UserName
                    }).ToListAsync());
            }
            catch (Exception)
            {
                return View($"Error");
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

                return PartialView($"GetSpecificUserMessage", new ChatUsersMesaages<MESSAGE_INDIVIDUAL, ApplicationUser>(messageIndividuals, null, user));
            }
            catch (Exception)
            {
                return View($"Error");
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

