using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Sondage.Controllers
{
    public class SondageController : Controller
    {
        // GET: Sondage
        public ActionResult Home()
        {
            return View();
        }

        public ActionResult Home(string question, string choix1, string choix2, string choix3, string choix4, bool multiple)
        {
            Models.Sondage sondageweb = new Models.Sondage(question, multiple);
            

            return View();
        }
    }
}