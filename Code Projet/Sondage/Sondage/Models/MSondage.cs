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
        public int _idSondage { get; set; }        
        public int _nbVote { get; set; }
        public string _nomQuest { get; set; }
        public bool _choixMultiple { get; set; } 
        public string _lienPartage { get; set; }
        public string _cleSuppression { get; set; }
        public string _lienResultat { get; set; } 
        public int _actif { get; set; }       

        public MSondage(string nomQuest, bool choixM)
        {            
            _nbVote = 0;
            _nomQuest = nomQuest;
            _choixMultiple = choixM;
            _actif = 1;        
        }
    }

    // Représente un choix en base de données
    public class Choix
    {
        public int _idChoix { get; set; }
        public string _nomChoix { get; set; }
        public int _nbVoteChoix { get; set; }
        public int _idSondage { get; set; }        

        public Choix(string nomChoix, int idSondage)
        {            
            _nomChoix = nomChoix;
            _nbVoteChoix = 0;
            _idSondage = idSondage;            
        }
    }

    // Classe de la question et ses choix liés
    public class QuestionEtChoix
    {
        public string _Question { get; set; }
        public List<string> _ListeChoix { get; set; }
        public int _id { get; set; }
        public bool _TypeChoix { get; set; }
        public List<int> _idChoix { get; set; }

        public QuestionEtChoix(string Question, List<string> Choix, int id, bool typeChoix, List<int> idChoix)
        {
            _Question = Question;
            _ListeChoix = Choix;
            _id = id;
            _TypeChoix = typeChoix;
            _idChoix = idChoix;
        }
    }

    // Classe pour stocker le contact et le message
    public class Contact
    {
        public string _nomContact { get; set; }
        public string _prenomContact { get; set; }
        public string _emailContact { get; set; }
        public string _message { get; set; }

        //Constructeur contact
        public Contact(string nomContact, string prenomContact, string emailContact, string message)
        {
            _nomContact = nomContact;
            _prenomContact = prenomContact;
            _emailContact = emailContact;
            _message = message;
        }
    }

    public class nbVotesQuestionChoix
    {
        public string _Question { get; set; }
        public List<string> _ListeChoix { get; set; }
        public int _NbVotesQuestion { get; set; }
        public List<int> _NbVotesChoix { get; set; }
        public bool _TypeChoix { get; set; }        

        public nbVotesQuestionChoix(string question, List<string> lChoix ,int nbVoteQuestion, List<int> nbVotesChoix, bool typeChoix)
        {
            _Question = question;
            _ListeChoix = lChoix;
            _NbVotesQuestion = nbVoteQuestion;
            _NbVotesChoix = nbVotesChoix;
            _TypeChoix = typeChoix;
        }
    }

    public class QuestionsPopulaires
    {
        public List<string> _ListeQuestions { get; set; }
        public List<int> _ListeNbVotes { get; set; }

        public QuestionsPopulaires(List<string> questions, List<int> nbVotes)
        {
            _ListeQuestions = questions;
            _ListeNbVotes = nbVotes;
        }
    }

    public class SondagesRecents
    {
        public List<string> _ListeQuestions { get; set; }
        public List<int> _ListeNbVotes { get; set; }

        public SondagesRecents(List<string> questions, List<int> nbVotes)
        {
            _ListeQuestions = questions;
            _ListeNbVotes = nbVotes;
        }
    }
}