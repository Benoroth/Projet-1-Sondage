using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;

namespace Sondage.Models
{
    public class SQL
    {

        //Adresse BDD SQL
        private const string SqlConnectionString = @"Server=.\SQLExpress;Initial Catalog=Projet; Trusted_Connection=Yes";
////Requètes SQL

        //Récupérer nombre de votants en BDD
        private int NombreVotesSondage()
        {
            SqlConnection connexion = new SqlConnection(SqlConnectionString);
            connexion.Open();
            SqlCommand NbVotesBDD = new SqlCommand(@"SELECT nbVote FROM TSondage", connexion);
            int nombreVoteRecup = (int)NbVotesBDD.ExecuteScalar();
            connexion.Close();
            return nombreVoteRecup;
        }
        //Récupérer nom de la question et nom des choix correspondants
        private string NomQuestionEtChoix()
        {
            //Nom Question
            SqlConnection connexion = new SqlConnection(SqlConnectionString);
            connexion.Open();
            SqlCommand NomQuestion = new SqlCommand(@"SELECT nomQuestion FROM TSondage", connexion);
            string nomQuestionRecup = (string)NomQuestion.ExecuteScalar();
            //Nom Choix
            SqlCommand NomChoixQuestion = new SqlCommand(@"SELECT nomChoix FROM TChoix, TSondage WHERE TSondage.idChoix=TChoix.idChoix 
                                                            AND nomQuestion=@question", connexion);
            SqlParameter.Equals("@question", nomQuestionRecup);


            connexion.Close();
            return nomQuestionRecup;
        }
    }
}