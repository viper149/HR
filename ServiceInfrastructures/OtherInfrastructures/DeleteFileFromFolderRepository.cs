using System;
using DenimERP.ServiceInterfaces.OtherInterfaces;
using Microsoft.AspNetCore.Hosting;

namespace DenimERP.ServiceInfrastructures.OtherInfrastructures
{
    public class DeleteFileFromFolderRepository : IDeleteFileFromFolder
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public DeleteFileFromFolderRepository(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public bool DeleteFile(string fileName = "", string folderName = "")
        {
            try
            {
                var webRootPath = _hostingEnvironment.WebRootPath;
                var fullPath = webRootPath + $"/{folderName}/" + fileName;

                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteFileFromContentRootPath(string fileName = "", string folderName = "")
        {
            try
            {
                var contentRootPath = _hostingEnvironment.ContentRootPath;
                var fullPath = contentRootPath + $"/RequestFiles/{folderName}/" + fileName;

                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
