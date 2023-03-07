using DenimERP.Models;
using Microsoft.AspNetCore.Http;

namespace DenimERP.ViewModels
{
    public class CompanyInfoViewModel
    {
        public IFormFile Logo { get; set; }

        public COMPANY_INFO CompanyInfo { get; set; }
    }
}
