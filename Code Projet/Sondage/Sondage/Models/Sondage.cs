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
        private DateTime dateSond { get; set; }
        private TimeSpan dureeSond { get; set; }
        private int nbVote { get; set; }
        private string nomQuest { get; set; }
        public string lienResult { get; set; }
        public string lienPartage { get; set; }
        private string lienSuppr { get; set; }
        public bool choixMultiple { get; set; }        
        private bool PublicOuPrive { get; set; }
        public int noteSond { get; set; }               
    }

    public class Choix
    {
        public int idChoix { get; set; }
        public string nomChoix { get; set; }
        public int nbVoteChoix { get; set; }
    }
}