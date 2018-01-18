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

        public ActionResult Valider(string question, string choix1, string choix2, string choix3, string choix4)
        {
            Models.Sondage sondageweb = new Models.Sondage(question);

            Models.SQL.InsererSondageBDD(sondageweb); //insertion du sondage dans la BDD

            int idDernierSondage = Models.SQL.GetIdSondage();   

            Models.Choix choixun = new Models.Choix(choix1, idDernierSondage);
            Models.Choix choixdeux = new Models.Choix(choix2, idDernierSondage);
            Models.Choix choixtrois = new Models.Choix(choix3, idDernierSondage);
            Models.Choix choixquatre = new Models.Choix(choix4, idDernierSondage);

            Models.SQL.InsererChoixBDD(choixun);  //insertion des choix dans la BDD
            Models.SQL.InsererChoixBDD(choixdeux);
            Models.SQL.InsererChoixBDD(choixtrois);
            Models.SQL.InsererChoixBDD(choixquatre);

            return View();
        }
    }
}