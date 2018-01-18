using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using Sondage.Models;

namespace Sondage.Models
{
    public class SQL
    {

        //Adresse BDD SQL
        private const string SqlConnectionString = @"Server=.;Initial Catalog=Projet; Trusted_Connection=Yes";
        ////Requètes SQL

        //1) Création d'un sondage

        //a) insérer nouveau sondage BDD
        public static void InsererSondageBDD(Sondage sondageAInserer)
        {
            SqlConnection connexion = new SqlConnection(SqlConnectionString);
            connexion.Open();
            SqlCommand InsererSondage = new SqlCommand(@"INSERT INTO TSondage(nomQuestion) VALUES (@question)", connexion);            
            InsererSondage.Parameters.AddWithValue("@question", sondageAInserer._nomQuest);            
            InsererSondage.ExecuteNonQuery();

            connexion.Close();                  
        }

        public static void InsererChoixBDD(Choix ChoixAInserer)
        {
            SqlConnection connexion = new SqlConnection(SqlConnectionString);
            connexion.Open();
            SqlCommand InsererChoix = new SqlCommand(@"INSERT INTO TChoix(nomChoix, idSondage, nbVoteChoix) VALUES (@nomChoix, @idSondage,@nbVoteChoix)", connexion);
            InsererChoix.Parameters.AddWithValue("@nomChoix", ChoixAInserer._nomChoix);
            InsererChoix.Parameters.AddWithValue("@idSondage", ChoixAInserer._idSondage);
            InsererChoix.Parameters.AddWithValue("@nbVoteChoix", ChoixAInserer._nbVoteChoix);
            InsererChoix.ExecuteNonQuery();

            connexion.Close();
        }

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

        public static int GetIdSondage()
        {
            SqlConnection connexion = new SqlConnection(SqlConnectionString);
            connexion.Open();
            SqlCommand GetId = new SqlCommand(@"SELECT MAX(idSondage) FROM TSondage", connexion); //obtenir l'id du dernier sondage créé
            int id = (int)GetId.ExecuteScalar();

            connexion.Close();
            return id;
        }
    }
}