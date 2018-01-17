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

        public ActionResult Valider(string question, string choix1, string choix2, string choix3, string choix4, bool multiple)
        {
            Models.Sondage sondageweb = new Models.Sondage(question, multiple);

            InsererSondageBDD(sondageweb); //insertion du sondage dans la BDD

            int idDernierSondage = GetIdSondage();

            Models.Choix choixun = new Model.Choix(choix1, idDernierSondage);
            Models.Choix choixdeux = new Model.Choix(choix2, idDernierSondage);
            Models.Choix choixtrois = new Model.Choix(choix3, idDernierSondage);
            Models.Choix choixquatre = new Model.Choix(choix4, idDernierSondage);

            InsererChoixBDD(choixun);  //insertion des choix dans la BDD
            InsererChoixBDD(choixdeux);
            InsererChoixBDD(choixtrois);
            InsererChoixBDD(choixquatre);



            return View();
        }
    }
}