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
        public DateTime dateSond { get; set; }
        public TimeSpan dureeSond { get; set; }
        public int nbVote { get; set; }
        public string nomQuest { get; set; }
        public string lienResult { get; set; }
        public string lienPartage { get; set; }
        public string lienSuppr { get; set; }
        public bool choixMultiple { get; set; }        
        public bool PublicOuPrive { get; set; }
        public int noteSond { get; set; }               
    }

    public class Choix
    {
        public int idChoix { get; set; }
        public string nomChoix { get; set; }
        public int nbVoteChoix { get; set; }
    }
}