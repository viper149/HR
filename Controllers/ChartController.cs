using System;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces;
using DenimERP.ServiceInterfaces.Hubs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Controllers
{
    public class ChartController : Controller
    {
        private readonly ICOM_EX_LCINFO _comExLcinfo;
        private readonly ICOM_EX_PIMASTER _comExPiMaster;
        private readonly IF_DYEING_PROCESS_ROPE_DETAILS _fDyeingProcessRopeDetails;
        private readonly IF_PR_FINISHING_FNPROCESS _fPrFinishingFnprocess;
        private readonly IF_PR_INSPECTION_PROCESS_DETAILS _fPrInspectionProcessDetails;
        private readonly IF_PR_WEAVING_PRODUCTION _fPrWeavingProduction;
        private readonly IF_PR_SIZING_PROCESS_ROPE_MASTER _fPrSizingProcessRopeMaster;
        private readonly IF_PR_SIZING_PROCESS_ROPE_DETAILS _fPrSizingProcessRopeDetails;
        private readonly IF_LCB_PRODUCTION_ROPE_MASTER _fLcbProductionRopeMaster;
        private readonly IF_PR_WARPING_PROCESS_ROPE_MASTER _fPrwarpingProcessRopeMaster;
        private readonly IF_PR_WARPING_PROCESS_ECRU_MASTER _fPrEcruProcessRopeMaster;
        private readonly IF_DYEING_PROCESS_ROPE_MASTER _fDyeingProcessRopeMaster;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMESSAGE _message;
        private readonly IMESSAGE_INDIVIDUAL _messageIndividual;
        private readonly IF_PR_WARPING_PROCESS_ECRU_MASTER _fPrWarpingProcessEcruMaster;
        private readonly IF_PR_WARPING_PROCESS_DW_MASTER _fPrWarpingProcessDwMaster;
        private readonly IF_PR_WARPING_PROCESS_SW_MASTER _fPrWarpingProcessSwMaster;

        public ChartController(
            ICOM_EX_LCINFO comExLcinfo,
            ICOM_EX_PIMASTER comExPiMaster,
            IF_DYEING_PROCESS_ROPE_DETAILS fDyeingProcessRopeDetails,
            IF_PR_FINISHING_FNPROCESS fPrFinishingFnprocess,
            IF_PR_INSPECTION_PROCESS_DETAILS fPrInspectionProcessDetails,
            IF_PR_WEAVING_PRODUCTION fPrWeavingProduction,
            IF_PR_SIZING_PROCESS_ROPE_MASTER fPrSizingProcessRopeMaster,
            IF_PR_SIZING_PROCESS_ROPE_DETAILS fPrSizingProcessRopeDetails,
            IF_LCB_PRODUCTION_ROPE_MASTER fLcbProductionRopeMaster,
            IF_PR_WARPING_PROCESS_ROPE_MASTER fPrwarpingProcessRopeMaster,
            IF_PR_WARPING_PROCESS_ECRU_MASTER fPrWarpingProcessEcruMaster,
            IF_PR_WARPING_PROCESS_DW_MASTER fPrWarpingProcessDwMaster,
            IF_PR_WARPING_PROCESS_SW_MASTER fPrWarpingProcessSwMaster,
            IF_DYEING_PROCESS_ROPE_MASTER fDyeingProcessRopeMaster,
            UserManager<ApplicationUser> userManager,
            IMESSAGE message,
            IMESSAGE_INDIVIDUAL messageIndividual

            )
        {
            _comExLcinfo = comExLcinfo;
            _comExPiMaster = comExPiMaster;
            _fDyeingProcessRopeDetails = fDyeingProcessRopeDetails;
            _fPrFinishingFnprocess = fPrFinishingFnprocess;
            _fPrInspectionProcessDetails = fPrInspectionProcessDetails;
            _fPrWeavingProduction = fPrWeavingProduction;
            _fPrSizingProcessRopeMaster = fPrSizingProcessRopeMaster;
            _fPrSizingProcessRopeDetails = fPrSizingProcessRopeDetails;
            _fLcbProductionRopeMaster = fLcbProductionRopeMaster;
            _fPrwarpingProcessRopeMaster = fPrwarpingProcessRopeMaster;
            _fPrWarpingProcessEcruMaster = fPrWarpingProcessEcruMaster;
            _fPrWarpingProcessDwMaster = fPrWarpingProcessDwMaster;
            _fPrWarpingProcessSwMaster = fPrWarpingProcessSwMaster;
            _fDyeingProcessRopeMaster = fDyeingProcessRopeMaster;
            _userManager = userManager;
            _message = message;
            _messageIndividual = messageIndividual;
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> SectionalWarpingProductionList()
        {
            var result = await _fPrWarpingProcessSwMaster.GetSectionalWarpingProductionList();
            var x = result.Select(item => item.TotalSectionalWarping ?? 0).ToList();
            return Ok(x);
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> DirectWarpingProductionList()
        {
            var result = await _fPrWarpingProcessDwMaster.GetDirectWarpingProductionList();
            var x = result.Select(item => item.TotalDirectWarping ?? 0).ToList();
            return Ok(x);
        }


        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> EcruWarpingProductionList()
        {
            var result = await _fPrWarpingProcessEcruMaster.GetEcruWarpingProductionList();
            var x = result.Select(item => item.TotalEcruWarping ?? 0).ToList();
            return Ok(x);
        }

        //[AcceptVerbs("Get", "Post")]
        //public async Task<IActionResult> WarpingChartData()
        //{
        //    var result = await _fPrwarpingProcessRopeMaster.GetWarpingDateWiseLengthGraph();
        //    var x = result.Select(item => item.TotalWarping ?? 0).ToList();
        //    return Ok(x);
        //}

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> WarpingChartData()
        {

            //return View(await _fPrSizingProcessRopeDetails.GetSizingDateWiseLengthGraph());
            var result = await _fPrwarpingProcessRopeMaster.GetWarpingDateWiseLengthGraph();
            var x = result.Select(item => item.TotalWarping ?? 0).ToList();
            return Ok(x);
        }
        //[AcceptVerbs("Get", "Post")]
        //public async Task<IActionResult> SizingChartData()
        //{
        //    var result = await _fPrSizingProcessRopeMaster.GetSizingProductionList();
        //    var x = result.Select(item => item.TotalProduction ?? 0).ToList();
        //    return Ok(x);
        //}

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> RopeWarpingProductionList()
        {
            var result = await _fPrwarpingProcessRopeMaster.GetRopeWarpingProductionList();
            var x = result.Select(item => item.TotalWarping ?? 0).ToList();
            return Ok(x);
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> DyeingChartData()
        {
            return View(await _fDyeingProcessRopeMaster.GetDyeingDateWiseLength());
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> LCBProductionList()
        {
            try
            {
                return Ok(await _fLcbProductionRopeMaster.GetLCBProductionList());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> SizingProductionList()
        {
            try
            {
                return Ok(await _fPrSizingProcessRopeMaster.GetSizingProductionList());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> WeavingProductionList()
        {
            try
            {
                return Ok(await _fPrWeavingProduction.GetWeavingProductionList());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> DyeingChart()
        {
            try
            {
                return Ok(await _fDyeingProcessRopeMaster.GetDyeingDateWiseLengthGraph());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> InspectionChart()
        {
            try
            {
                return Ok(await _fPrInspectionProcessDetails.GetInspectionDateWiseLengthGraph());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> WarpingProductionList()
        {
            try
            {
                return Ok(await _fPrwarpingProcessRopeMaster.GetWarpingProductionList());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> DyeingProductionList()
        {
            try
            {
                return Ok(await _fDyeingProcessRopeDetails.GetDyeingProductionList());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> WeavingChart()
        {
            try
            {
                return Ok(await _fPrWeavingProduction.GetWeavingDateWiseLengthGraph());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [AcceptVerbs("Get","post")]
        public async Task<IActionResult> LCBChartData()
        {
            return View(await _fLcbProductionRopeMaster.GetLCBDateWiseLengthGraph());
        }

        //[AcceptVerbs("Get","Post")]
        //public async Task<IActionResult> SizingChartData()
        //{

        //    //return View(await _fPrSizingProcessRopeDetails.GetSizingDateWiseLengthGraph());
        //    var result = await _fPrSizingProcessRopeMaster.GetSizingDateWiseLengthGraph();
        //    var x = result.Select(item => item.TotalSizing ?? 0).ToList();
        //    return Ok(x);
        //}

        //[AcceptVerbs("Get","Post")]
        //public async Task<IActionResult> WeavingChartData()
        //{
        //    return View(await _fPrWeavingProduction.GetWeavingDateWiseLengthGraph());
        //}

        [AcceptVerbs("Get","Post")]
        public async Task<IActionResult> FinishingChartData()
        {
            //return View(await _fPrFinishingFnprocess.GetFinishingDateWiseLengthGraph());
            var result = await _fPrFinishingFnprocess.GetFinishingDateWiseLengthGraph();
            var x = result.Select(item => item.TotalFinishing ?? 0).ToList();
            return Ok(x);
        }

        //[AcceptVerbs("Get", "Post")]
        //public async Task<IActionResult> InspectionChartData()
        //{
        //    {
        //        return View(await _fPrInspectionProcessDetails.GetInspectionDateWiseLengthGraph());
        //    }
        //}

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> PIChartData()
        {
            {
                return View(await _comExPiMaster.GetPIChartData());
            }
        }

        //[AcceptVerbs("Get", "Post")]
        //public async Task<IActionResult> LCChartData()
        //{
        //    {
        //        return View(await _comExLcinfo.GetLCChartData());
        //    }
        //}

    }
}
