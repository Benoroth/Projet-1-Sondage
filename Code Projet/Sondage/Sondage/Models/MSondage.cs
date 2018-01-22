using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sondage.Models
{
    public class MSondage
    {
        public int _idSondage { get; set; }        
        public int _nbVote { get; set; }
        public string _nomQuest { get; set; }
        public bool _choixMultiple { get; set; } 
        public string _lienPartage { get; set; }
        public string _lienSuppression { get; set; }
        public string _lienResultat { get; set; }      

        public MSondage(string nomQuest)
        {            
            _nbVote = 0;
            _nomQuest = nomQuest;
            _choixMultiple = false;                                
        }
    }

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

    

    
}