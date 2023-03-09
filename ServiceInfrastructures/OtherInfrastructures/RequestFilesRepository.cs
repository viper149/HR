using System;
using System.IO;
using HRMS.ServiceInterfaces.OtherInterfaces;
using Microsoft.AspNetCore.Hosting;

namespace HRMS.ServiceInfrastructures.OtherInfrastructures
{
    public class RequestFilesRepository : IRequestFiles
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public RequestFilesRepository(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public string RequestedFilePath(string fileName = "", string toFolder = "")
        {
            try
            {
                var filePath = Path.Combine(_hostingEnvironment.ContentRootPath, "RequestFiles", $"{toFolder}\\{fileName}");
                return filePath;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
