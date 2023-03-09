using Microsoft.AspNetCore.Http;

namespace HRMS.ServiceInterfaces.OtherInterfaces
{
    public interface IProcessUploadFile
    {
        string ProcessUploadFile(IFormFile formFile, string toFolder = "");
        string ProcessUploadFileToContentRootPath(IFormFile formFile, string toFolder = "unhandled_files");
        string ProcessUploadFileToProfilePhotos(IFormFile formFile, string toFolder = "profile_photos");
        byte[] ProceessFormFile(IFormFile file);
    }
}
