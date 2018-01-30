using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sondage.Models;




namespace Sondage.Controllers
{
    public class SondageController : Controller
    {        
        // GET: Sondage
        public ActionResult Home()
        {
            return View();
        }

        int idDernierSondage;

        //Valider et insérer la question et les choix en bdd
        public ActionResult Valider(string question, string choix1, string choix2, string choix3, string choix4)
        {
            MSondage sondageweb = new MSondage(question);            

            idDernierSondage = SQL.InsererSondageBDD(sondageweb); //insertion du sondage dans la BDD
                        
            Choix choixun = new Choix(choix1, idDernierSondage);
            Choix choixdeux = new Choix(choix2, idDernierSondage);
            Choix choixtrois = new Choix(choix3, idDernierSondage);                       
            Choix choixquatre = new Choix(choix4, idDernierSondage);            

            SQL.InsererChoixBDD(choixun);  //insertion des choix dans la BDD
            SQL.InsererChoixBDD(choixdeux);
            SQL.InsererChoixBDD(choixtrois);             
            SQL.InsererChoixBDD(choixquatre);

            Random rnd = new Random();
            int cleSuppression = rnd.Next(0, 64000);
            Convert.ToString(cleSuppression);

            string lienPartage = "localhost:1093/Sondage/Vote?id="; //lien vers la page de vote
            string lienSuppr = "localhost:1093/Sondage/Suppression?id="; //lien vers la page de suppression
            string lienResul = "localhost:1093/Sondage/Resultat?id="; //lien vers la page de résultat

            Convert.ToString(idDernierSondage); //convertir id du dernier sondage créé en string pour concaténer avec les liens 

            lienPartage = lienPartage + idDernierSondage; //concaténation lien partage et id du derniere sondage créé
            lienSuppr = lienSuppr + idDernierSondage + cleSuppression; //concaténation lien suppression et id du derniere sondage créé
            lienResul = lienResul + idDernierSondage; //concaténation lien résultat et id du derniere sondage créé

            sondageweb._lienPartage = lienPartage;
            sondageweb._lienSuppression = lienSuppr;
            sondageweb._lienResultat = lienResul;

            SQL.InsertionLiensBDD(sondageweb, idDernierSondage); //insertion des liens partage, suppression et résultat dans la BDD

            return View("SondageCree", sondageweb); //renvoie vers la page affichant les liens (partage, suppression, résultats)
        }
        
        public ActionResult Vote(int id) //insère la question et ses choix dans la vue de Vote
        {
            if (id < SQL.maxIdSondage())
            {
                if (SQL.estActif(id) == 1)
                {
                    QuestionEtChoix questionchoix = SQL.GetQuestionEtChoix(id);
                    return View("Vote", questionchoix);
                }
                else
                {
                    return Redirect("Introuvable");
                }
            }
            else
            {
                return Redirect("Introuvable");
            }
        }


        //Renvoie vers la validation de suppression du sondage
        //public ActionResult Suppression(int id)
        //{
            //SQL.SuppressionSondage(id);

          //  return View();
        //}

        public ActionResult Contact()
        {
            return View();
        }
        public ActionResult Contacter(string nomBDD, string prenomBDD, string emailBDD, string messageBDD)
        {
            Contact NouveauContact = new Models.Contact(nomBDD, prenomBDD, emailBDD, messageBDD);
            SQL.InsererDonneesContact(NouveauContact);

            return View("Contact", NouveauContact);
        }

        public ActionResult Voter(int id, string vote)
        {           
            SQL.Voter(id, vote);

            return Redirect("Resultat");
        }

        public ActionResult Resultat(int id)
        {
            if (id < SQL.maxIdSondage())
            {
                if (SQL.estActif(id) == 1)
                {
                    nbVotesQuestionChoix sondageEtNbVotes = SQL.GetNbVotesQuestionChoix(id);

                    return View("Resultat", sondageEtNbVotes);
                }
                else
                {
                    return Redirect("Introuvable");
                }
            }
            else
            {
                return Redirect("Introuvable");
            }
        }

        public ActionResult Introuvable()
        {
            return View("Introuvable");
        }

        public ActionResult ChartTest(int id)
        {
            nbVotesQuestionChoix nouveauResultat = SQL.GetNbVotesQuestionChoix(id);

            return View("ChartTest", nouveauResultat);
        }


    }
}