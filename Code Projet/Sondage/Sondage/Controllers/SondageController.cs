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

        //Valider et insérer la question et les choix en bdd
        public ActionResult Valider(string question, string choix1, string choix2, string choix3, string choix4, string TypeChoix)
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

            Choix choixun = new Choix(choix1, idDernierSondage);
            Choix choixdeux = new Choix(choix2, idDernierSondage);
            Choix choixtrois = new Choix(choix3, idDernierSondage);
            Choix choixquatre = new Choix(choix4, idDernierSondage);

            SQL.InsererChoixBDD(choixun);  //insertion des choix dans la BDD
            SQL.InsererChoixBDD(choixdeux);
            SQL.InsererChoixBDD(choixtrois);
            SQL.InsererChoixBDD(choixquatre);

            Random rnd = new Random();
            int nombreRandom = rnd.Next(0, 64000); //génération d'un nombre aléatoire pour la clé de suppression
            Convert.ToString(nombreRandom); //convertion du nombre aléatoire en chaine de caractères

            string lienPartage = "localhost:1093/Sondage/Vote?id="; //lien vers la page de vote
            string lienSuppr = "localhost:1093/Sondage/Suppression?id="; //lien vers la page de suppression
            string lienResul = "localhost:1093/Sondage/Resultat?id="; //lien vers la page de résultat

            Convert.ToString(idDernierSondage); //convertir id du dernier sondage créé en string pour concaténer avec les liens 

            string cleSuppression = Convert.ToString(idDernierSondage) + nombreRandom; //création de la clé sécurisée

            lienPartage = lienPartage + idDernierSondage; //concaténation lien partage et id du derniere sondage créé
            lienSuppr = lienSuppr + cleSuppression; //concaténation lien suppression et id du derniere sondage créé
            lienResul = lienResul + idDernierSondage; //concaténation lien résultat et id du derniere sondage créé

            sondageweb._lienPartage = lienPartage;
            sondageweb._lienSuppression = cleSuppression;
            sondageweb._lienResultat = lienResul;

            SQL.InsertionLiensBDD(sondageweb, idDernierSondage); //insertion des liens partage, suppression et résultat dans la BDD

            return View("SondageCree", sondageweb); //renvoie vers la page affichant les liens (partage, suppression, résultats)
        }

        public ActionResult Vote(int id) //insère la question et ses choix dans la vue de Vote
        {
            List<int> lIdSondage = SQL.GetTousLesId(); //récupère la liste de tous les idSondage présents dans la BDD

            foreach(var idSondage in lIdSondage)
            {
                if(id == idSondage) //si l'id est dans la liste de tous les idSondage présents dans la BDD --> redirige vers la page de vote
                {
                    QuestionEtChoix questionchoix = SQL.GetQuestionEtChoix(id);
                    return View("Vote", questionchoix);
                }
            }
            return Redirect("Introuvable");            
        }


        //Renvoie vers la validation de suppression du sondage
        public ActionResult Suppression(string id)
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
            List<int> lIdSondage = SQL.GetTousLesId();

            foreach (var idSondage in lIdSondage)
            {
                if (id == idSondage)
                {
                    HttpCookie cookie = new HttpCookie("Cookie"); //Création d'un nouveau cookie
                    cookie.Value = "A voté le : " + DateTime.Now.ToShortTimeString();  //attribution d'une valeur à "cookie" ainsin que date création
                                                                                       // Vérification de la présence d'un cookie
                    if (this.ControllerContext.HttpContext.Request.Cookies.AllKeys.Contains("Cookie"))
                    {
                        return View("DejaVote", id); //si cookie présent envoie vers page erreur vote
                    }
                    else //si cookie absent, on en ajoute un, ensuite on vote
                    {
                        this.ControllerContext.HttpContext.Response.Cookies.Add(cookie);
                        SQL.Voter(id, vote);
                        return Redirect("Resultat?id=" + id); //envoi vers la page de résultats du sondage
                    }
                }
            }
            return Redirect("Introuvable");
        }

        public ActionResult VoterM(int id, int? valeur0, int? valeur1, int? valeur2, int? valeur3) //Vote pour un choix multiple
        {
            SQL.VoterMultiple(id, valeur0, valeur1, valeur2, valeur3);

            return Redirect("Resultat?id=" + id); //redirection vers la page de résultats du sondage
        }

        public ActionResult Resultat(int id)
        {
            if (id <= SQL.maxIdSondage()) //si id entré est supérieur à l'id max dans la BDD, l'utilisateur sera redirigé vers la page "Introuvable"
            {
                nbVotesQuestionChoix sondageEtNbVotes = SQL.GetNbVotesQuestionChoix(id);

                return View("Resultat", sondageEtNbVotes);
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

        public ActionResult SondagesPopulaires() //page affichant les 10 sondages comptant le plus de votants
        {
            QuestionsPopulaires questionsPopulaires = SQL.GetQuestionsPopulaires();

            return View("SondagesPopulaires", questionsPopulaires);
        }

        public ActionResult ChartTest(int id) //A SUPPRIMER
        {
            nbVotesQuestionChoix nouveauResultat = SQL.GetNbVotesQuestionChoix(id);

            return View("ChartTest", nouveauResultat);
        }

        public ActionResult SondagesRecents()
        {
            SondagesRecents sondagesRecents = SQL.GetSondagesRecents();

            return View("SondagesRecents", sondagesRecents);
        }
    }
}