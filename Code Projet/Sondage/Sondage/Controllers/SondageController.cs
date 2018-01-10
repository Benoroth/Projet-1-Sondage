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
    }
}