using System;

namespace DenimERP.Common
{
    public static class Common
    {
        public static string GetFileExtension(string base64String)
        {
            var data = base64String.Substring(0, 5);

            switch (data.ToUpper())
            {
                case "IVBOR":
                    return "png";
                case "/9J/4":
                    return "jpg";
                case "AAAAF":
                    return "mp4";
                case "JVBER":
                    return "pdf";
                case "AAABA":
                    return "ico";
                case "UMFYI":
                    return "rar";
                case "E1XYD":
                    return "rtf";
                case "U1PKC":
                    return "txt";
                case "MQOWM":
                case "77U/M":
                    return "srt";
                default:
                    return string.Empty;
            }
        }

        public static DateTime GetDate()
        {
            //return DateTime.Now;
            return Convert.ToDateTime("2022-02-27");
        }
        public static DateTime GetDefaultDate()
        {
            //return DateTime.Now;
            return Convert.ToDateTime("1501-01-01 00:00:00.000");
        }
    }
}
