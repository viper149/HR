using Microsoft.AspNetCore.Mvc;

namespace HRMS.Controllers
{
    public class MailController : Controller
    {

        public IActionResult Inbox()
        {
            return View();
        }

        //public IActionResult Compose()
        //{
        //    return View();
        //}

        //public IActionResult Read()
        //{
        //    return View();
        //}
    }
}
