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

            string lienPartage = "localhost:1093/Sondage/Vote?id="; //lien vers la page de vote
            string lienSuppr = "localhost:1093/Sondage/Suppression?id="; //lien vers la page de suppression
            string lienResul = "localhost:1093/Sondage/Resultat?id="; //lien vers la page de résultat

            Convert.ToString(idDernierSondage); //convertir id du dernier sondage créé en string pour concaténer avec les liens 

            lienPartage = lienPartage + idDernierSondage; //concaténation lien partage et id du derniere sondage créé
            lienSuppr = lienSuppr + idDernierSondage; //concaténation lien suppression et id du derniere sondage créé
            lienResul = lienResul + idDernierSondage; //concaténation lien résultat et id du derniere sondage créé

            sondageweb._lienPartage = lienPartage;
            sondageweb._lienSuppression = lienSuppr;
            sondageweb._lienResultat = lienResul;

            Models.SQL.InsertionLiensBDD(sondageweb); //insertion des liens partage, suppression et résultat dans la BDD

            return View("SondageCree");
        }

        public ActionResult SondageCree()
        {
            int idDernierSondage = Models.SQL.GetIdSondage();

            Models.SQL.GetLienPSondage(idDernierSondage);
            Models.SQL.GetLienSSondage(idDernierSondage);
            Models.SQL.GetLienRSondage(idDernierSondage);

            return View("SondageCree");
        }

        public ActionResult Vote(int id)
        {
            return View("Vote");
        }


        public ActionResult Suppression(int id)
        {
            Models.SQL.SuppressionSondage(id);

            return View();
        }
    }
}