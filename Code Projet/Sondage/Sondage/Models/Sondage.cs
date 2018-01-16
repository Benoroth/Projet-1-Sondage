using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sondage.Models
{
    public class Sondage
    {
        public int _idSondage { get; set; }        
        public int _nbVote { get; set; }
        public string _nomQuest { get; set; }
        public bool _choixMultiple { get; set; }

        public Sondage(int idSondage, int nbVote, string nomQuest, bool choixMultiple)
        {
            _idSondage = idSondage;
            _nbVote = nbVote;
            _nomQuest = nomQuest;
            _choixMultiple = choixMultiple;
        }
    }

    public class Choix
    {
        public int _idChoix { get; set; }
        public string _nomChoix { get; set; }
        public int _nbVoteChoix { get; set; }
        public int _idSondage { get; set; }

        public Choix(int idChoix, string nomChoix, int nbVoteChoix, int idSondage)
        {
            _idChoix = idChoix;
            _nomChoix = nomChoix;
            _nbVoteChoix = 0;
            _idSondage = idSondage;
        }
    }

    

    
}