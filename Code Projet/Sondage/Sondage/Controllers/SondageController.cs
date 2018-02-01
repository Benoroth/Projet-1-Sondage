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
            return View();
        }

        int idDernierSondage;

        // Valider et insérer la question et les choix en bdd
        public ActionResult Valider(string question, List<string> choix, string TypeChoix) // string choix2, string choix3, string choix4, )
        {
            bool choixMultiple = false;
            if (TypeChoix == "ChoixM") //attribuer le type de choix à un boolean
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
                SQL.InsererChoixBDD(new Choix(nom, idDernierSondage));
            }

            Random rnd = new Random();
            int nombreRandom = rnd.Next(0, 64000); // génération d'un nombre aléatoire pour la clé de suppression
            Convert.ToString(nombreRandom); // convertion du nombre aléatoire en chaine de caractères        

            Convert.ToString(idDernierSondage); //convertir id du dernier sondage créé en string pour concaténer avec les liens 

            sondageweb._lienPartage = "localhost:1093/Sondage/Vote?id=" + idDernierSondage;
            sondageweb._cleSuppression = Convert.ToString(idDernierSondage) + nombreRandom;
            sondageweb._lienResultat = "localhost:1093/Sondage/Resultat?id=" + idDernierSondage;

            SQL.InsertionLiensBDD(sondageweb, idDernierSondage); //insertion des liens partage, suppression et résultat dans la BDD

            return View("SondageCree", sondageweb); //renvoie vers la page affichant les liens (partage, suppression, résultats)
        }

        public ActionResult Vote(int id) //insère la question et ses choix dans la vue de Vote
        {
            List<int> lIdSondage = SQL.GetTousLesId(); //récupère la liste de tous les idSondage présents dans la BDD

            foreach (var idSondage in lIdSondage)
            {
                if (id == idSondage) //si l'id est dans la liste de tous les idSondage présents dans la BDD --> redirige vers la page de vote
                {
                    QuestionEtChoix questionchoix = SQL.GetQuestionEtChoix(id);
                    return View("Vote", questionchoix);
                }
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
            Contact NouveauContact = new Models.Contact(nomBDD, prenomBDD, emailBDD, messageBDD); //création d'un nouveau contact
            SQL.InsererDonneesContact(NouveauContact); //insertion BDD

            return View("Contact", NouveauContact); //Envoi vers la vue correspondante
        }

        public ActionResult Voter(int id, int vote) //Vote pour choix unique
        {
            if (vote < 1)
            {
                throw new Exception("Pas de vote");
            }
            HttpCookie cookie = new HttpCookie("Cookie" + id); //Création d'un nouveau cookie
            cookie.Value = "A voté le : " + DateTime.Now.ToShortTimeString();  //attribution d'une valeur à "cookie" ainsin que date création

            if (this.ControllerContext.HttpContext.Request.Cookies.AllKeys.Contains("Cookie" + id)) // Vérification de la présence d'un cookie
            {
                return Redirect("Resultat?id=" + id); //si cookie présent envoie vers page erreur vote
            }
            else //si cookie absent, on en ajoute un, ensuite on vote
            {
                if (SQL.estActif(id) == 1)
                {
                    this.ControllerContext.HttpContext.Response.Cookies.Add(cookie);
                    SQL.Voter(id, vote);
                    return Redirect("Resultat?id=" + id); //envoi vers la page de résultats du sondage
                }
            }
            return Redirect("Resultat?id=" + id);
        }

        public ActionResult VoterM(int id, List<int> valeurs) // int? valeur0, int? valeur1, int? valeur2, int? valeur3) //Vote pour un choix multiple
        {
            HttpCookie cookie = new HttpCookie("Cookie" + id); //Création d'un nouveau cookie
            cookie.Value = "A voté le : " + DateTime.Now.ToShortTimeString();  //attribution d'une valeur à "cookie" ainsin que date création

            if (id == 0)//(this.ControllerContext.HttpContext.Request.Cookies.AllKeys.Contains("Cookie")) // Vérification de la présence d'un cookie
            {
                return Redirect("Resultat?id=" + id); //si cookie présent envoie vers page erreur vote
            }
            else
            {
                if (SQL.estActif(id) == 1)
                {
                    this.ControllerContext.HttpContext.Response.Cookies.Add(cookie);
                    SQL.VoterMultiple(id, valeurs);
                    return Redirect("Resultat?id=" + id); //redirection vers la page de résultats du sondage
                }                
            }
            return Redirect("Resultat?id=" + id);
        }

        public ActionResult Resultat(int id)
        {
            List<int> lIdSondage = SQL.GetTousLesId();

            foreach (var idSondage in lIdSondage)
            {
                if (id == idSondage)
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
            QuestionsPopulaires questionsPopulaires = SQL.GetQuestionsPopulaires();

            return View("SondagesPopulaires", questionsPopulaires);
        }

        public ActionResult SondagesRecents() //page affichant les 10 derniers sondages créés
        {
            SondagesRecents sondagesRecents = SQL.GetSondagesRecents();

            return View("SondagesRecents", sondagesRecents);
        }
    }
}