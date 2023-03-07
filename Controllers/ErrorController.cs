using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DenimERP.Controllers
{
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }

        [Route("/Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            var statusCodeResult = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

            switch (statusCode)
            {
                case 404:
                    ViewBag.ErrorMessage = "Sorry but the page you are looking for does not exist, have been removed, Name changed or is temporarily unavailable. For Any Query please call to IT Helpline (1322)";
                    _logger.LogWarning($"404 error occurred. Path = {statusCodeResult.OriginalPath}" +
                        $"and Query string = {statusCodeResult.OriginalQueryString}");
                    break;

                case 502:
                    ViewBag.ErrorMessage = "Poor Internet Connection or the server is temporarily unavailable. Please wait and reload the page after some times. For Any Query please call to IT Helpline (1322)";
                    _logger.LogWarning($"404 error occurred. Path = {statusCodeResult.OriginalPath}" +
                                       $"and Query string = {statusCodeResult.OriginalQueryString}");
                    break;

                default:
                    ViewBag.ErrorMessage = "For Any Query please call to IT Helpline (1322)";
                    _logger.LogWarning($"404 error occurred. Path = {statusCodeResult.OriginalPath}" +
                                       $"and Query string = {statusCodeResult.OriginalQueryString}");
                    break;
            }

            return View($"NotFound");
        }

        [Route("Error")]
        [AllowAnonymous]
        public IActionResult Error()
        {
            var exceptionDetails = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            if (exceptionDetails != null)
            {
                _logger.LogError($"The path {exceptionDetails.Path} threw an exception + " +
                    $"{exceptionDetails.Error}");
            }

            return View($"Error");
        }
    }
}