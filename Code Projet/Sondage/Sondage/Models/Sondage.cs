using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sondage
{
    //sondage public ou privé
    public enum TypeSondage
    {
        Public,
        Private
    }
    //choix unique ou multiple
    public enum TypeChoix
    {
        Unique,
        Multiple
    }
    public class Sondage
    {
        private int nbVote { get; set; }
        private string nomQuest { get; set; }
        public string lienResult { get; set; }
        public string lienPartage { get; set; }
        private string lienSuppr { get; set; }
        private bool PublicOuPrive { get; set; }
        public int noteSond { get; set; }
        private DateTime dateSond { get; set; }
        private TimeSpan dureeSond { get; set; }

    }
}