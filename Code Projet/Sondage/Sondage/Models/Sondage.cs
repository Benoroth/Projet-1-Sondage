using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sondage.Models
{
    public class Sondage
    {
        public int idSondage { get; set; }        
        public int nbVote { get; set; }
        public string nomQuest { get; set; }
        public bool choixMultiple { get; set; }                                           
    }

    public class Choix
    {
        public int idChoix { get; set; }
        public string nomChoix { get; set; }
        public int nbVoteChoix { get; set; }
        public int idSondage { get; set; }
    }

    public Sondage(int nbVote, string nomQuestion, bool choixMultiple)
    {
        Sondage Sondage = new Sondage();

        Sondage.nbVote = 0;
        Sondage.nomQuest = nomQuestion;       
    }

    public Choix(string nomChoix, int nbVoteChoix, int idSondage)
    {
        Choix Choix = new Choix();

        Choix.nomChoix = nomChoix;
        Choix.nbVoteChoix = 0;
        Choix.nomChoix = nomChoix;
    }
}