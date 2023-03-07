using System;
using System.IO;
using DenimERP.ServiceInterfaces.OtherInterfaces;
using DevExpress.Data.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using static System.String;

namespace DenimERP.ServiceInfrastructures.OtherInfrastructures
{
    public class ProcessUploadFileToFolderRepository : IProcessUploadFile
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public ProcessUploadFileToFolderRepository(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public string ProcessUploadFile(IFormFile formFile, string toFolder = "")
        {
            try
            {
                var uniqueFileName = Empty;

                if (formFile == null) return uniqueFileName;
                var extension = Path.GetExtension(formFile.FileName);

                if (extension.ToLower() != ".pdf") return uniqueFileName;
                var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, toFolder);
                uniqueFileName = Guid.NewGuid().ToString() + "_" + formFile.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    formFile.CopyTo(fileStream);
                }
                return uniqueFileName;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string ProcessUploadFileToContentRootPath(IFormFile formFile, string toFolder = "unhandled_files")
        {
            try
            {
                var uniqueFileName = Empty;

                if (formFile == null) return uniqueFileName;
                var extension = Path.GetExtension(formFile.FileName);

                if (extension.ToLower() != ".pdf") return uniqueFileName;
                var uploadsFolder = Path.Combine(_hostingEnvironment.ContentRootPath, $"RequestFiles\\{toFolder}");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + formFile.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    formFile.CopyTo(fileStream);
                }
                return uniqueFileName;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string ProcessUploadFileToProfilePhotos(IFormFile formFile, string toFolder = "profile_photos")
        {
            try
            {
                var uniqueFileName = Empty;

                if (formFile == null) return uniqueFileName;
                var extension = Path.GetExtension(formFile.FileName);

                if (AcceptExtensionForImage().FindIndex(extension.Equals) < 0) return null;

                var uploadsFolder = Path.Combine(_hostingEnvironment.ContentRootPath, $"RequestFiles\\{toFolder}");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + formFile.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    formFile.CopyTo(fileStream);
                }
                return uniqueFileName;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public byte[] ProceessFormFile(IFormFile file)
        {
            if (file == null) return null;

            if (file.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    var s = Convert.ToBase64String(fileBytes);
                    return fileBytes;
                }
            }
            return null;
        }

        private static string[] AcceptExtensionForImage()
        {
            return new[] {
                ".jpg", ".JPG", ".jpeg", ".JPEG" ,".png", ".PNG"
            };
        }
    }
}
