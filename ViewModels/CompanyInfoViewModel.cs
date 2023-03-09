using HRMS.Models;
using Microsoft.AspNetCore.Http;

namespace HRMS.ViewModels
{
    public class CompanyInfoViewModel
    {
        public IFormFile Logo { get; set; }

        public COMPANY_INFO CompanyInfo { get; set; }
    }
}
