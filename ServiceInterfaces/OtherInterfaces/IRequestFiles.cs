﻿namespace DenimERP.ServiceInterfaces.OtherInterfaces
{
    public interface IRequestFiles
    {
        string RequestedFilePath(string fileName = "", string toFolder = "");
    }
}
