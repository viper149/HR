using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Controllers
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
