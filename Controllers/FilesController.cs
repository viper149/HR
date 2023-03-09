using System;
using System.IO;
using System.Linq;
using HRMS.ServiceInterfaces.OtherInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Controllers
{
    [Authorize]
    public class FilesController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IRequestFiles _requestFiles;

        public FilesController(IHostingEnvironment hostingEnvironment,
            IRequestFiles requestFiles)
        {
            _hostingEnvironment = hostingEnvironment;
            _requestFiles = requestFiles;

        }

        [HttpGet]
        public IActionResult DrivingLicense(string fileName)
        {
            try
            {
                return PhysicalFile(_requestFiles.RequestedFilePath(fileName, "hr_assets/employees_files/driving_license"), $"application/{fileName.Split('.').Last()}");
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpGet]
        public IActionResult Passport(string fileName)
        {
            try
            {
                return PhysicalFile(_requestFiles.RequestedFilePath(fileName, "hr_assets/employees_files/passport"), $"application/{fileName.Split('.').Last()}");
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpGet]
        public IActionResult Nid(string fileName)
        {
            try
            {
                return PhysicalFile(_requestFiles.RequestedFilePath(fileName, "hr_assets/employees_files/nid"), $"application/{fileName.Split('.').Last()}");
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"> Image file name. Must have to match with the existing one.</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult ProfilePhotos(string fileName)
        {
            try
            {
                var filePath = Path.Combine(_hostingEnvironment.ContentRootPath, "RequestFiles", $"profile_photos\\{fileName}");
                var physicalFileResult = PhysicalFile(filePath, $"image/{filePath.Split('.').Last()}");
                physicalFileResult.EnableRangeProcessing = true;

                return physicalFileResult;
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpGet]
        public IActionResult lc_files(string fileName)
        {
            try
            {
                return PhysicalFile(_requestFiles.RequestedFilePath(fileName, "lc_files"), $"application/{fileName.Split('.').Last()}");
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpGet]
        public IActionResult pi_files(string fileName)
        {
            try
            {
                return PhysicalFile(_requestFiles.RequestedFilePath(fileName, "lc_files/pi_files"), "application/pdf");
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpGet]
        public IActionResult imp_invoice_files(string fileName)
        {
            try
            {
                var filePath = Path.Combine(_hostingEnvironment.ContentRootPath, "RequestFiles", $"imp_invoice_files\\{fileName}");
                return PhysicalFile(filePath, "application/pdf");
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpGet]
        public IActionResult com_ex_invoice_files(string fileName)
        {
            try
            {
                var filePath = Path.Combine(_hostingEnvironment.ContentRootPath, "RequestFiles", $"com_ex_invoice_files\\{fileName}");
                return PhysicalFile(filePath, "application/pdf");
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }
    }
}