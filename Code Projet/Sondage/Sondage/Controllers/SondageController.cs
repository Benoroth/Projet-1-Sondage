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
        public ActionResult Home() //return vers la page de création de sondage (home)
        {
            return View("Home");
        }

        public ActionResult Creation()
        {
            return View();
        }
        int idDernierSondage;
        // Valider et insérer la question et les choix en bdd
        public ActionResult Valider(string question, List<string> choix, string typeChoix) // string choix2, string choix3, string choix4, )
        {
            bool choixMultiple = false;
            if (typeChoix == "ChoixM") //attribuer le type de choix à un boolean
            {
                choixMultiple = true;
            }
            else
            {
                choixMultiple = false;
            }           

            MSondage sondageweb = new MSondage(question, choixMultiple);

            idDernierSondage = SQL.InsererSondageBDD(sondageweb); //insertion du sondage dans la BDD

            foreach (string nom in choix)
            {
                if (nom != "")
                {
                    SQL.InsererChoixBDD(new Choix(nom, idDernierSondage));
                }
            }

            Random rnd = new Random();
            int nombreRandom = rnd.Next(0, 64000); // génération d'un nombre aléatoire pour la clé de suppression
            Convert.ToString(nombreRandom); // convertion du nombre aléatoire en chaine de caractères        

            Convert.ToString(idDernierSondage); //convertir id du dernier sondage créé en string pour concaténer avec les liens 

            sondageweb.LienPartage = "172.19.240.12/Sondage/Vote?id=" + idDernierSondage;
            sondageweb.CleSuppression = Convert.ToString(idDernierSondage) + nombreRandom;
            sondageweb.LienResultat = "172.19.240.12:1093/Sondage/Resultat?id=" + idDernierSondage;

            SQL.InsertionLiensBDD(sondageweb, idDernierSondage); //insertion des liens partage, suppression et résultat dans la BDD

            return View("SondageCree", sondageweb); //renvoie vers la page affichant les liens (partage, suppression, résultats)
        }

        public ActionResult Vote(int id) //insère la question et ses choix dans la vue de Vote
        {
                List<int> listeIdSondage = SQL.GetTousLesId(); //récupère la liste de tous les idSondage présents dans la BDD
                if (SQL.EstActif(id)) //si le sondage est désactivé, redirige vers la page de résultat du sondage
                {
                    foreach (var idSondage in listeIdSondage)
                    {
                        if (id == idSondage) //si l'id est dans la liste de tous les idSondage présents dans la BDD --> redirige vers la page de vote
                        {
                            QuestionEtChoix questionchoix = SQL.GetQuestionEtChoix(id);
                            return View("Vote", questionchoix);
                        }
                    }
                }
                else
                {
                    return Redirect("Resultat?id=" + id);
                }
            return Redirect("Introuvable");
        }        
        
        public ActionResult Suppression(string id) //Renvoie vers la validation de suppression du sondage
        {
            SQL.SuppressionSondage(id); //rend inactif un sondage dans la BDD (impossible de voter, résultats consultables)

            return View();
        }
        //Renvoie vers la page de contact
        public ActionResult Contact()
        {
            return View();
        }
        //Envoie en BDD les informations rentrées dans le formulaire de contact
        public ActionResult Contacter(string nomBDD, string prenomBDD, string emailBDD, string messageBDD)
        {
            Contact nouveauContact = new Models.Contact(nomBDD, prenomBDD, emailBDD, messageBDD);
            SQL.InsererDonneesContact(nouveauContact); //insertion BDD

            return View("Contact", nouveauContact); //Envoi vers la vue correspondante
        }

        public ActionResult Voter(int id, int vote) //Vote pour choix unique
        {            
            HttpCookie cookie = Request.Cookies["Cookie" + id]; //Vérification si cookie existe

            if (cookie==null) // Si pas de cookie
            {
                cookie = new HttpCookie("Cookie" + id);         // On créé le cookie
                SQL.Voter(id, vote);                            // On vote
                Response.Cookies.Add(cookie);                   // On ajoute le cookie
                return Redirect("Resultat?id=" + id);           //envoi vers la page de résultats du sondage
            }
            else
            { 
                    return View("DejaVote", id);                // Envoi vers la page d'erreur
            }

        }

        public ActionResult VoterM(int id, List<int> valeurs) 
        {
            HttpCookie cookie = Request.Cookies["Cookie" + id]; //Vérification si cookie existe
            if (cookie==null) // Vérification de la présence d'un cookie
            {
                cookie = new HttpCookie("Cookie" + id);         // On créé le cookie
                SQL.VoterMultiple(id, valeurs);
                Response.Cookies.Add(cookie);
                return Redirect("Resultat?id=" + id); //redirection vers la page de résultats du sondage
            }
            else
            {
                return View("DejaVote", id); //si cookie présent envoie vers page erreur vote           
            }
        }

        public ActionResult Resultat(int id)
        {
            List<int> listeIdSondage = SQL.GetTousLesId();

            foreach (var idSondage in listeIdSondage)
            {
                if (id == idSondage) //vérifie si l'id du sondage demandé est contenu dans la BDD
                {
                    nbVotesQuestionChoix sondageEtNbVotes = SQL.GetNbVotesQuestionChoix(id);
                    return View("Resultat", sondageEtNbVotes);
                }
            }
            return Redirect("Introuvable");
        }

        public ActionResult Introuvable() //lorqu'un sondage n'est pas dans la BDD, l'utilisateur est rédirigé vers cette page
        {
            return View("Introuvable");
        }

        public ActionResult SondagesPopulaires() //page affichant les 10 sondages comptant le plus de votants
        {
            QuestionsEtNbVotes sondagesPopulaires = SQL.GetQuestionsPopulaires();

            return View("SondagesPopulaires", sondagesPopulaires);
        }

        public ActionResult SondagesRecents() //page affichant les 10 derniers sondages créés
        {
            QuestionsEtNbVotes sondagesRecents = SQL.GetSondagesRecents();

            return View("SondagesRecents", sondagesRecents);
        }
    }
}