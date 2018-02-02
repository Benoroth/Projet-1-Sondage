using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms.DataVisualization.Charting;
using System.Web.UI.DataVisualization.Charting;


namespace Sondage.Models
{
    //Classe du sondage
    public class MSondage
    {
        public int IdSondage { get; set; }        
        public int NbVote { get; set; }
        public string NomQuest { get; set; }
        public bool ChoixMultiple { get; set; } 
        public string LienPartage { get; set; }
        public string CleSuppression { get; set; }
        public string LienResultat { get; set; } 
        public bool Actif { get; set; }       

        public MSondage(string nomQuest, bool choixM)
        {            
            NbVote = 0;
            NomQuest = nomQuest;
            ChoixMultiple = choixM;
            Actif = true;        
        }
    }

    // Représente un choix en base de données
    public class Choix
    {
        public int IdChoix { get; set; }
        public string NomChoix { get; set; }
        public int NbVoteChoix { get; set; }
        public int IdSondage { get; set; }        

        public Choix(string nomChoix, int idSondage)
        {            
            NomChoix = nomChoix;
            NbVoteChoix = 0;
            IdSondage = idSondage;            
        }
    }

    // Classe de la question et ses choix liés
    public class QuestionEtChoix
    {
        public string Question { get; set; }
        public List<string> ListeChoix { get; set; }
        public int Id { get; set; }
        public bool TypeChoix { get; set; }
        public List<int> IdChoix { get; set; }
        public int NbVotants { get; set; }

        public QuestionEtChoix(string question, List<string> choix, int id, bool typeChoix, List<int> idChoix, int nbVotants)
        {
            Question = question;
            ListeChoix = choix;
            Id = id;
            TypeChoix = typeChoix;
            IdChoix = idChoix;
            NbVotants = nbVotants;
        }
    }

    // Classe pour stocker le contact et le message
    public class Contact
    {
        public string NomContact { get; set; }
        public string PrenomContact { get; set; }
        public string EmailContact { get; set; }
        public string Message { get; set; }

        //Constructeur contact
        public Contact(string nomContact, string prenomContact, string emailContact, string message)
        {
            NomContact = nomContact;
            PrenomContact = prenomContact;
            EmailContact = emailContact;
            Message = message;
        }
    }

    public class nbVotesQuestionChoix
    {
        public string Question { get; set; }
        public List<string> ListeChoix { get; set; }
        public int NbVotesQuestion { get; set; }
        public List<int> NbVotesChoix { get; set; }
        public bool TypeChoix { get; set; }        

        public nbVotesQuestionChoix(string question, List<string> lChoix ,int nbVoteQuestion, List<int> nbVotesChoix, bool typeChoix)
        {
            Question = question;
            ListeChoix = lChoix;
            NbVotesQuestion = nbVoteQuestion;
            NbVotesChoix = nbVotesChoix;
            TypeChoix = typeChoix;
        }
    }

    public class QuestionsEtNbVotes
    {
        public List<string> ListeQuestions { get; set; }
        public List<int> ListeNbVotes { get; set; }
        public List<DateTime> ListeDate { get; set; }

        public QuestionsEtNbVotes(List<string> questions, List<int> nbVotes, List<DateTime> listeDate)
        {
            ListeQuestions = questions;
            ListeNbVotes = nbVotes;
            ListeDate = listeDate;
        }
    }
}