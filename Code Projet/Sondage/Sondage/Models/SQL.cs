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
        public static int InsererSondageBDD(MSondage sondageAInserer)
        {
            SqlConnection connexion = new SqlConnection(SqlConnectionString);
            connexion.Open();
            SqlCommand InsererSondage = new SqlCommand(@"INSERT INTO TSondage(nomQuestion, nbVote, choixMultiple) VALUES (@question, @nbVote, @choixMultiple); SELECT SCOPE_IDENTITY()", connexion);            
            InsererSondage.Parameters.AddWithValue("@question", sondageAInserer._nomQuest);
            InsererSondage.Parameters.AddWithValue("@nbVote", sondageAInserer._nbVote);
            InsererSondage.Parameters.AddWithValue("@choixMultiple", sondageAInserer._choixMultiple);               
            int lastId = Convert.ToInt32(InsererSondage.ExecuteScalar());            

            connexion.Close();

            return lastId;                 
        }

        public static void InsertionLiensBDD(MSondage sondageAInserer, int id) //insertion des liens de partage, résultat et suppression dans la base de données
        {
            SqlConnection connexion = new SqlConnection(SqlConnectionString);
            connexion.Open();
            SqlCommand InsererLiens = new SqlCommand(@"UPDATE TSondage SET lienPartage = @lienpartage, lienResult = @lienresult, lienSuppr = @liensuppr WHERE idSondage = @id", connexion);
            InsererLiens.Parameters.AddWithValue("@lienpartage", sondageAInserer._lienPartage);
            InsererLiens.Parameters.AddWithValue("@liensuppr", sondageAInserer._lienSuppression);
            InsererLiens.Parameters.AddWithValue("@lienresult", sondageAInserer._lienResultat);
            InsererLiens.Parameters.AddWithValue("@id", id);
            InsererLiens.ExecuteNonQuery();

            connexion.Close();
        }

        public static void InsererChoixBDD(Choix ChoixAInserer)  //insertion des choix BDD
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

        public static QuestionEtChoix GetQuestionEtChoix(int id)
        {
            SqlConnection connexion = new SqlConnection(SqlConnectionString);
            connexion.Open();

            SqlCommand command = new SqlCommand(@"SELECT nomQuestion FROM TSondage WHERE idSondage = @id", connexion);
            command.Parameters.AddWithValue("@id", id);

            
            
                        
        }

        public static void SuppressionSondage(int id) //suppression d'un sondage
        {
            SqlConnection connexion = new SqlConnection(SqlConnectionString);
            connexion.Open();

            SqlCommand SupprimerQuestion = new SqlCommand(@"DELETE FROM TSondage WHERE idSondage = @id", connexion); //supprime une question de la BDD
            SupprimerQuestion.Parameters.AddWithValue("@id", id);

            SupprimerQuestion.ExecuteNonQuery();

            SqlCommand SupprimerChoix = new SqlCommand(@"DELETE FROM TChoix WHERE idSondage = @id", connexion); //supprime les choix liés à la question, de la BDD
            SupprimerChoix.Parameters.AddWithValue("@id", id);

            SupprimerChoix.ExecuteNonQuery();

            connexion.Close();
        }

        public static string GetLienPSondage(int id) //obtenir les liens du dernier sondage
        {
            SqlConnection connexion = new SqlConnection(SqlConnectionString);
            connexion.Open();

            SqlCommand GetLien = new SqlCommand(@"SELECT lienPartage FROM TSondage WHERE idSondage = @id");
            GetLien.Parameters.AddWithValue("@id", id);

            string lienpartage = (string)GetLien.ExecuteScalar();
            connexion.Close();

            return lienpartage;
        }

        public static string GetLienSSondage(int id) //obtenir les liens du dernier sondage
        {
            SqlConnection connexion = new SqlConnection(SqlConnectionString);
            connexion.Open();

            SqlCommand GetLien = new SqlCommand(@"SELECT lienSuppr FROM TSondage WHERE idSondage = @id");
            GetLien.Parameters.AddWithValue("@id", id);

            string liensuppr = (string)GetLien.ExecuteScalar();
            connexion.Close();

            return liensuppr;
        }

        public static string GetLienRSondage(int id) //obtenir les liens du dernier sondage
        {
            SqlConnection connexion = new SqlConnection(SqlConnectionString);
            connexion.Open();

            SqlCommand GetLien = new SqlCommand(@"SELECT lienResult FROM TSondage WHERE idSondage = @id");
            GetLien.Parameters.AddWithValue("@id", id);

            string lienresu = (string)GetLien.ExecuteScalar();
            connexion.Close();

            return lienresu;
        }
    }
}