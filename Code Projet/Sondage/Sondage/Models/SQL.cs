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
        private const string SqlConnectionString = @"Server=172.19.240.12;Initial Catalog=Projet; Trusted_Connection=Yes";
        ////Requètes SQL

        //1) Création d'un sondage

        //a) insérer nouveau sondage BDD
        public static int InsererSondageBDD(MSondage sondageAInserer)
        {
            SqlConnection connexion = new SqlConnection(SqlConnectionString);
            connexion.Open();
            SqlCommand InsererSondage = new SqlCommand(@"INSERT INTO TSondage(nomQuestion, nbVote, choixMultiple, actif) VALUES (@question, @nbVote, @choixMultiple, @actif); SELECT SCOPE_IDENTITY()", connexion);            
            InsererSondage.Parameters.AddWithValue("@question", sondageAInserer._nomQuest);
            InsererSondage.Parameters.AddWithValue("@nbVote", sondageAInserer._nbVote);
            InsererSondage.Parameters.AddWithValue("@choixMultiple", sondageAInserer._choixMultiple);
            InsererSondage.Parameters.AddWithValue("@actif", sondageAInserer._actif);                     
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
        public static int NombreVotesSondage(int id)
        {
            SqlConnection connexion = new SqlConnection(SqlConnectionString);
            connexion.Open();
            SqlCommand NbVotesBDD = new SqlCommand(@"SELECT nbVote 
                                                     FROM TSondage
                                                     WHERE idSondage =@id", connexion);
            NbVotesBDD.Parameters.AddWithValue("@id", id);
            int nombreVoteRecup = (int)NbVotesBDD.ExecuteScalar();
            connexion.Close();
            return nombreVoteRecup;
        }

        public static QuestionEtChoix GetQuestionEtChoix(int id) //methode pour obtenir la question et les choix liés à cette question
        {
            SqlConnection connexion = new SqlConnection(SqlConnectionString);
            connexion.Open();

            SqlCommand getQuestion = new SqlCommand(@"SELECT nomQuestion 
                                                      FROM TSondage 
                                                      WHERE idSondage = @id", connexion);
            getQuestion.Parameters.AddWithValue("@id", id);
            string Question = (string)getQuestion.ExecuteScalar(); //cherche la question dans la BDD

            SqlCommand getChoix = new SqlCommand(@"SELECT nomChoix
                                                   FROM TChoix
                                                   WHERE idSondage = @id", connexion);
            getChoix.Parameters.AddWithValue("@id", id);

            SqlDataReader recordset = getChoix.ExecuteReader(); //cherche les choix dans la BDD
            List<string> lChoix = new List<string>();            
               
            while(recordset.Read())
            {
                lChoix.Add((string)recordset["nomChoix"]); //insère les choix dans une liste
            }
            connexion.Close();
            connexion.Open();

            SqlCommand getTypeChoix = new SqlCommand(@"SELECT choixMultiple
                                                       FROM TSondage
                                                       WHERE idSondage = @id", connexion);
            getTypeChoix.Parameters.AddWithValue("@id", id);

            bool typeChoix = (bool)getTypeChoix.ExecuteScalar();

            connexion.Close();
            connexion.Open();

            SqlCommand getIdChoix = new SqlCommand(@"SELECT idChoix
                                                     FROM TChoix
                                                     WHERE idSondage = @id", connexion);
            getIdChoix.Parameters.AddWithValue("@id", id);

            SqlDataReader lecteur = getIdChoix.ExecuteReader();
            List<int> lIdChoix = new List<int>();

            while(lecteur.Read())
            {
                lIdChoix.Add((int)lecteur["idChoix"]); //insère les idChoix dans une liste
            }
            connexion.Close();

            QuestionEtChoix questionChoix = new QuestionEtChoix(Question, lChoix, id, typeChoix, lIdChoix);            
                
            return questionChoix; //retourne la question et ses choix
        }

        public static void SuppressionSondage(string id) //suppression d'un sondage
        {
            SqlConnection connexion = new SqlConnection(SqlConnectionString);
            connexion.Open();

            SqlCommand desactiveSondage = new SqlCommand(@"UPDATE TSondage
                                                           SET actif = 0
                                                           WHERE lienSuppr = @id", connexion);
            desactiveSondage.Parameters.AddWithValue("@id", id);

            desactiveSondage.ExecuteNonQuery();

            connexion.Close();
        }

        public static string GetLienPSondage(int id) //obtenir les liens du dernier sondage
        {
            SqlConnection connexion = new SqlConnection(SqlConnectionString);
            connexion.Open();

            SqlCommand GetLien = new SqlCommand(@"SELECT lienPartage 
                                                  FROM TSondage 
                                                  WHERE idSondage = @id");
            GetLien.Parameters.AddWithValue("@id", id);

            string lienpartage = (string)GetLien.ExecuteScalar();
            connexion.Close();

            return lienpartage;
        }

        public static string GetLienSSondage(int id) //obtenir le lien de suppression du dernier sondage
        {
            SqlConnection connexion = new SqlConnection(SqlConnectionString);
            connexion.Open();

            SqlCommand GetLien = new SqlCommand(@"SELECT lienSuppr 
                                                  FROM TSondage 
                                                  WHERE idSondage = @id");
            GetLien.Parameters.AddWithValue("@id", id);

            string liensuppr = (string)GetLien.ExecuteScalar();
            connexion.Close();

            return liensuppr;
        }

        public static string GetLienRSondage(int id) //obtenir le lien de résultat du dernier sondage
        {
            SqlConnection connexion = new SqlConnection(SqlConnectionString);
            connexion.Open();

            SqlCommand GetLien = new SqlCommand(@"SELECT lienResult 
                                                  FROM TSondage 
                                                  WHERE idSondage = @id");
            GetLien.Parameters.AddWithValue("@id", id);

            string lienresu = (string)GetLien.ExecuteScalar();
            connexion.Close();

            return lienresu;
        }
        //Incrémenter le nombre de vote total et le nombre de vote par choix
        public static void Voter(int id, int idChoix)
        {
            SqlConnection connexion = new SqlConnection(SqlConnectionString);
            connexion.Open();

            SqlCommand incrementerNbVotesSondage = new SqlCommand(@"UPDATE TSondage 
                                                                    SET nbVote = nbVote + 1 
                                                                    WHERE idSondage = @id", connexion);
            incrementerNbVotesSondage.Parameters.AddWithValue("@id", id);
            incrementerNbVotesSondage.ExecuteNonQuery();

            SqlCommand incrementerNbVotesChoix = new SqlCommand(@"UPDATE TChoix
                                                                  SET nbVoteChoix = nbVoteChoix + 1
                                                                  WHERE idSondage = @id AND idChoix = @idChoix", connexion);
            incrementerNbVotesChoix.Parameters.AddWithValue("@id", id);
            incrementerNbVotesChoix.Parameters.AddWithValue("@idChoix", idChoix);

            incrementerNbVotesChoix.ExecuteNonQuery();

            connexion.Close();
        }

        public static void VoterMultiple(int id, int? voteun, int? votedeux, int? votetrois, int? votequatre)
        {
            SqlConnection connexion = new SqlConnection(SqlConnectionString);
            connexion.Open();

            SqlCommand incrementerNbVotesSondage = new SqlCommand(@"UPDATE TSondage
                                                                    SET nbVote = nbVote + 1
                                                                    WHERE idSondage = @id", connexion);
            incrementerNbVotesSondage.Parameters.AddWithValue("@id", id);
            incrementerNbVotesSondage.ExecuteNonQuery();
            
            SqlCommand incrementerNbVotesChoixUn = new SqlCommand(@"UPDATE TChoix
                                                                  SET nbVoteChoix = nbVoteChoix +1
                                                                  WHERE idChoix = @id", connexion);
            incrementerNbVotesChoixUn.Parameters.AddWithValue("@id", voteun);
            
            SqlCommand incrementerNbVotesChoixDeux = new SqlCommand(@"UPDATE TChoix
                                                                  SET nbVoteChoix = nbVoteChoix +1
                                                                  WHERE idChoix = @id", connexion);
            incrementerNbVotesChoixDeux.Parameters.AddWithValue("@id", votedeux);

            SqlCommand incrementerNbVotesChoixTrois = new SqlCommand(@"UPDATE TChoix
                                                                  SET nbVoteChoix = nbVoteChoix +1
                                                                  WHERE idChoix = @id", connexion);
            incrementerNbVotesChoixTrois.Parameters.AddWithValue("@id", votetrois);

            SqlCommand incrementerNbVotesChoixQuatre = new SqlCommand(@"UPDATE TChoix
                                                                  SET nbVoteChoix = nbVoteChoix +1
                                                                  WHERE idChoix = @id", connexion);
            incrementerNbVotesChoixQuatre.Parameters.AddWithValue("@id", votequatre);
            
            if(voteun != null)
            { 
            incrementerNbVotesChoixUn.ExecuteNonQuery();
            }

            if(votedeux != null)
            {
                incrementerNbVotesChoixDeux.ExecuteNonQuery();
            }

            if(votetrois != null)
            { 
            incrementerNbVotesChoixTrois.ExecuteNonQuery();
            }

            if(votequatre != null)
            { 
            incrementerNbVotesChoixQuatre.ExecuteNonQuery();
            }

            connexion.Close();
        }

        //Lier les données de sondage et de vote à un graphique

            //Lier données ci dessus à un graph


        //Insérer données contact
        public static void InsererDonneesContact(Contact NouveauContact)
        {
            SqlConnection connexion = new SqlConnection(SqlConnectionString);
            connexion.Open();
            SqlCommand InsererDonneesContact = new SqlCommand(@"INSERT INTO Contact( nomContact, prenomContact, emailContact, messageContact) VALUES (@nom, @prenom, @email, @message)", connexion);
            InsererDonneesContact.Parameters.AddWithValue("@nom", NouveauContact._nomContact);
            InsererDonneesContact.Parameters.AddWithValue("@prenom", NouveauContact._prenomContact);
            InsererDonneesContact.Parameters.AddWithValue("@email", NouveauContact._emailContact);
            InsererDonneesContact.Parameters.AddWithValue("@message", NouveauContact._message);
            InsererDonneesContact.ExecuteNonQuery();
            connexion.Close();
        }

        public static nbVotesQuestionChoix GetNbVotesQuestionChoix(int id)
        {
            SqlConnection connexion = new SqlConnection(SqlConnectionString);
            connexion.Open();

            SqlCommand getQuestion = new SqlCommand(@"SELECT nomQuestion
                                                      FROM TSondage
                                                      WHERE idSondage = @id", connexion);
            getQuestion.Parameters.AddWithValue("@id", id);

            string question = (string)getQuestion.ExecuteScalar();

            connexion.Close();

            //=================================================================================================

            connexion.Open();
            SqlCommand getChoix = new SqlCommand(@"SELECT nomChoix
                                                   FROM TChoix
                                                   WHERE idSondage = @id", connexion);
            getChoix.Parameters.AddWithValue("@id", id);

            SqlDataReader reader = getChoix.ExecuteReader();
            List<string> lChoix = new List<string>();

            while(reader.Read())
            {
                lChoix.Add((string)reader["nomChoix"]);
            }
            connexion.Close();

            //=================================================================================================

            connexion.Open();
            SqlCommand getVotesQuestion = new SqlCommand(@"SELECT nbVote 
                                                           FROM TSondage
                                                           WHERE idSondage= @id", connexion);
            getVotesQuestion.Parameters.AddWithValue("@id", id);

            int nbVotesQuestion = (int)getVotesQuestion.ExecuteScalar();
            connexion.Close();

            //=================================================================================================

            connexion.Open();
            SqlCommand getVotesChoix = new SqlCommand(@"SELECT nbVoteChoix 
                                                         FROM TChoix 
                                                         WHERE idSondage = @id", connexion);
            getVotesChoix.Parameters.AddWithValue("@id", id);

            SqlDataReader recordset = getVotesChoix.ExecuteReader();
            List<int> lNbVotesChoix = new List<int>();            

            while (recordset.Read())
            {
                lNbVotesChoix.Add((int)recordset["nbVoteChoix"]); 
            }
            nbVotesQuestionChoix nbVotesQuestionEtChoix = new nbVotesQuestionChoix(question, lChoix, nbVotesQuestion, lNbVotesChoix);

            connexion.Close();
            return nbVotesQuestionEtChoix;
        }
        public static int estActif(int id)
        {
            SqlConnection connexion = new SqlConnection(SqlConnectionString);
            connexion.Open();

            SqlCommand command = new SqlCommand(@"SELECT actif
                                                  FROM TSondage
                                                  WHERE idSondage = @id", connexion);
            command.Parameters.AddWithValue("@id", id);

            int actif = (int)command.ExecuteScalar();

            connexion.Close();
            return actif;
        }

        public static int maxIdSondage()
        {
            SqlConnection connexion = new SqlConnection(SqlConnectionString);
            connexion.Open();

            SqlCommand command = new SqlCommand(@"SELECT MAX(idSondage)
                                                  FROM TSondage", connexion);

            int maxId = (int)command.ExecuteScalar();
            connexion.Close();

            return maxId;
        }

        public static QuestionsPopulaires GetQuestionsPopulaires()
        {
            SqlConnection connexion = new SqlConnection(SqlConnectionString);
            connexion.Open();

            SqlCommand getQuestions = new SqlCommand(@"SELECT TOP 10 nomQuestion
                                                  FROM TSondage
                                                  ORDER BY nbVote DESC", connexion);
            SqlDataReader reader = getQuestions.ExecuteReader();            

            List<string> lQuestionsPopulaires = new List<string>();

            while(reader.Read())
            {
                lQuestionsPopulaires.Add((string)reader["nomQuestion"]);
            }

            connexion.Close();
            //====================================================================================================================
            connexion.Open();

            SqlCommand getNbVotes = new SqlCommand(@"SELECT TOP 10 nbVote
                                                     FROM TSondage
                                                     ORDER BY nbVote DESC", connexion);
            SqlDataReader recordset = getNbVotes.ExecuteReader();

            List<int> lNbVote = new List<int>();

            while(recordset.Read())
            {
                lNbVote.Add((int)recordset["nbVote"]);
            }

            connexion.Close();

            QuestionsPopulaires questionsPopu = new QuestionsPopulaires(lQuestionsPopulaires, lNbVote);

            return questionsPopu;
        }
    }
}