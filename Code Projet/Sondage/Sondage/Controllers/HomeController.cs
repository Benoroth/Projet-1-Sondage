using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;

namespace Sondage.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        //Adresse BDD SQL
        private const string SqlConnectionString = @"Server=.\SQLExpress;Initial Catalog=Projet; Trusted_Connection=Yes";

        ////Requètes SQL

        //Récupérer nombre de votants en BDD



    }
}