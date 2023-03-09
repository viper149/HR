namespace HRMS.ServiceInterfaces.OtherInterfaces
{
    public interface IDeleteFileFromFolder
    {
        bool DeleteFile(string fileName = "", string folderName = "");
        bool DeleteFileFromContentRootPath(string fileName = "", string folderName = "");
    }
}
