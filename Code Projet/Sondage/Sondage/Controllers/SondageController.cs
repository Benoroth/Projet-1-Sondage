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

            SQL.InsertionLiensBDD(sondageweb, idDernierSondage); //insertion des liens partage, suppression et résultat dans la BDD

            return View("SondageCree", sondageweb);
        }



        //Renvoie vers la page où le sondage est créé
        public ActionResult SondageCree(int id)
        {            
            return View("SondageCree");
        }

        public ActionResult Vote(int id) //insère la question et ses choix dans la vue de Vote
        {
            QuestionEtChoix questionchoix = SQL.GetQuestionEtChoix(id); 
            return View("Vote", questionchoix);
        }


        //Renvoie vers la validation de suppression du sondage
        public ActionResult Suppression(int id)
        {
            SQL.SuppressionSondage(id);

            return View();
        }
        //public ActionResult Contact(Contact NouveauContact)
        //{
        //    SQL.InsererDonneesContact(NouveauContact);

        //    return RedirectToAction("Home");        
        //}

        //Renvoie vers la page de contact


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


        //test et validation d'un choix, et ajout BDD
        public ActionResult Valider(int id, bool choix1, bool choix2, bool choix3, bool choix4)
        {
            string chien = "Chien";
            string chat = "Chat";
            string dauphin = "Dauphin";
            string loup = "Loup";
            if (choix1)
            {
                SQL.Voter(id, chien);
            }
            else
            {
                if (choix2)
                {
                    SQL.Voter(id, chat);
                }
                else
                {
                    if(choix3)
                    {
                        SQL.Voter(id, dauphin);
                    }
                    else
                    {
                        if(choix4)
                        {
                            SQL.Voter(id, loup);
                        }
                    }
                }
            }


            

            return View("Resultat");

        }
    }
}