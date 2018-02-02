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
        private const string SqlConnectionString = @"Server=172.19.240.123; Database=ProjetERTG; User Id=sa; password=pf68*CCI";

        private static SqlConnection connexion = new SqlConnection(SqlConnectionString);
        ////Requètes SQL
                
        public static int InsererSondageBDD(MSondage sondageAInserer) //insertion de la question, du nombre de votants (initialisé à 0), du type de sondage (choix unique/multiple), le statut du sondage (actif = true), et la date actuelle
        {            
            connexion.Open();
            SqlCommand insererSondage = new SqlCommand(@"INSERT INTO TSondage(nomQuestion, nbVote, choixMultiple, actif, dateSondage) 
                                                         VALUES (@question, @nbVote, @choixMultiple, @actif, GETDATE()); SELECT SCOPE_IDENTITY()", connexion);            
            insererSondage.Parameters.AddWithValue("@question", sondageAInserer.NomQuest);
            insererSondage.Parameters.AddWithValue("@nbVote", sondageAInserer.NbVote);
            insererSondage.Parameters.AddWithValue("@choixMultiple", sondageAInserer.ChoixMultiple);
            insererSondage.Parameters.AddWithValue("@actif", sondageAInserer.Actif);                     
            int lastId = Convert.ToInt32(insererSondage.ExecuteScalar()); //récupère l'id du sondage venant d'être créé      

            connexion.Close();

            return lastId;                 
        }

        public static void InsertionLiensBDD(MSondage sondageAInserer, int id) //insertion des liens de partage, résultat et suppression dans la base de données
        {            
            connexion.Open();
            SqlCommand insererLiens = new SqlCommand(@"UPDATE TSondage 
                                                       SET lienPartage = @lienpartage, lienResult = @lienresult, lienSuppr = @liensuppr 
                                                       WHERE idSondage = @id", connexion);
            insererLiens.Parameters.AddWithValue("@lienpartage", sondageAInserer.LienPartage);
            insererLiens.Parameters.AddWithValue("@liensuppr", sondageAInserer.CleSuppression);
            insererLiens.Parameters.AddWithValue("@lienresult", sondageAInserer.LienResultat);
            insererLiens.Parameters.AddWithValue("@id", id);
            insererLiens.ExecuteNonQuery();

            connexion.Close();
        }

        public static void InsererChoixBDD(Choix ChoixAInserer)  //insertion des choix BDD
        {            
            connexion.Open();
            SqlCommand insererChoix = new SqlCommand(@"INSERT INTO TChoix(nomChoix, idSondage, nbVoteChoix) 
                                                       VALUES (@nomChoix, @idSondage,@nbVoteChoix)", connexion);
            insererChoix.Parameters.AddWithValue("@nomChoix", ChoixAInserer.NomChoix);
            insererChoix.Parameters.AddWithValue("@idSondage", ChoixAInserer.IdSondage);
            insererChoix.Parameters.AddWithValue("@nbVoteChoix", ChoixAInserer.NbVoteChoix);
            insererChoix.ExecuteNonQuery();

            connexion.Close();
        }

        public static QuestionEtChoix GetQuestionEtChoix(int id) //methode pour obtenir la question et les choix liés à cette question
        {            
            connexion.Open();

            SqlCommand getQuestion = new SqlCommand(@"SELECT nomQuestion 
                                                      FROM TSondage 
                                                      WHERE idSondage = @id", connexion);
            getQuestion.Parameters.AddWithValue("@id", id);
            string Question = (string)getQuestion.ExecuteScalar(); //récupère la question dans la BDD

            //====================================================================================================================

            SqlCommand getChoix = new SqlCommand(@"SELECT nomChoix
                                                   FROM TChoix
                                                   WHERE idSondage = @id", connexion);
            getChoix.Parameters.AddWithValue("@id", id);

            SqlDataReader recordset = getChoix.ExecuteReader(); 
            List<string> lChoix = new List<string>();            
               
            while(recordset.Read())
            {
                lChoix.Add((string)recordset["nomChoix"]); //récupère les choix et les insère dans une liste
            }
            connexion.Close();

            //====================================================================================================================

            connexion.Open();

            SqlCommand getTypeChoix = new SqlCommand(@"SELECT choixMultiple
                                                       FROM TSondage
                                                       WHERE idSondage = @id", connexion);
            getTypeChoix.Parameters.AddWithValue("@id", id);

            bool typeChoix = (bool)getTypeChoix.ExecuteScalar();

            connexion.Close();

            //====================================================================================================================

            connexion.Open();

            SqlCommand getIdChoix = new SqlCommand(@"SELECT idChoix
                                                     FROM TChoix
                                                     WHERE idSondage = @id", connexion);
            getIdChoix.Parameters.AddWithValue("@id", id);

            SqlDataReader lecteur = getIdChoix.ExecuteReader();
            List<int> lIdChoix = new List<int>();

            while(lecteur.Read())
            {
                lIdChoix.Add((int)lecteur["idChoix"]); //récupère les idChoix et les insère dans une liste
            }
            connexion.Close();

            //====================================================================================================================

            connexion.Open();

            SqlCommand getNbVotants = new SqlCommand(@"SELECT nbVote
                                                       FROM TSondage
                                                       WHERE idSondage = @id", connexion);
            getNbVotants.Parameters.AddWithValue("@id", id);
            int nbVotants = (int)getNbVotants.ExecuteScalar();

            connexion.Close();

            QuestionEtChoix questionChoix = new QuestionEtChoix(Question, lChoix, id, typeChoix, lIdChoix, nbVotants);
                
            return questionChoix; //retourne la question, ses choix, l'id de la question, les id des choix, et le type du sondage (choix unique ou multiple)
        }

        public static void SuppressionSondage(string id) //désactivation d'un sondage
        {            
            connexion.Open();

            SqlCommand desactiveSondage = new SqlCommand(@"UPDATE TSondage
                                                           SET actif = 0
                                                           WHERE lienSuppr = @id", connexion);
            desactiveSondage.Parameters.AddWithValue("@id", id);

            desactiveSondage.ExecuteNonQuery(); //rend inactif un sondage (vote impossible, résultats consultables)

            connexion.Close();
        }        

        //Incrémenter le nombre de vote total et le nombre de vote par choix, pour un sondage à choix unique
        public static void Voter(int id, int idChoix)
        {
            connexion.Open();

            SqlCommand incrementerNbVotesSondage = new SqlCommand(@"UPDATE TSondage 
                                                                    SET nbVote = nbVote + 1 
                                                                    WHERE idSondage = @id", connexion);
            incrementerNbVotesSondage.Parameters.AddWithValue("@id", id);
            incrementerNbVotesSondage.ExecuteNonQuery(); //incrémente le nombre de votants total d'un sondage de 1

            //====================================================================================================================

            SqlCommand incrementerNbVotesChoix = new SqlCommand(@"UPDATE TChoix
                                                                  SET nbVoteChoix = nbVoteChoix + 1
                                                                  WHERE idSondage = @id AND idChoix = @idChoix", connexion);
            incrementerNbVotesChoix.Parameters.AddWithValue("@id", id);
            incrementerNbVotesChoix.Parameters.AddWithValue("@idChoix", idChoix);

            incrementerNbVotesChoix.ExecuteNonQuery(); //incrémente de 1 le nombre de votes d'un choix

            connexion.Close();
        }

        //Incrémenter le nombre de vote total et le nombre de vote par choix, pour un sondage à choix multiple
        public static void VoterMultiple(int id, List<int> votes_id)
        {
            connexion.Open();
            SqlCommand incrementerNbVotesSondage = new SqlCommand(@"UPDATE TSondage
                                                                    SET nbVote = nbVote + 1
                                                                    WHERE idSondage = @id", connexion);
            incrementerNbVotesSondage.Parameters.AddWithValue("@id", id);
            incrementerNbVotesSondage.ExecuteNonQuery(); //incrémente le nombre de votants total d'un sondage de 1

            foreach (int vote_id in votes_id)
            {
                SqlCommand incrementerNbVotesChoix = new SqlCommand(@"UPDATE TChoix
                                                                      SET nbVoteChoix = nbVoteChoix +1
                                                                      WHERE idChoix = @id", connexion);
                incrementerNbVotesChoix.Parameters.AddWithValue("@id", vote_id);
                incrementerNbVotesChoix.ExecuteNonQuery();
            }
            connexion.Close();
        }
        
        //Insérer données contact
        public static void InsererDonneesContact(Contact NouveauContact)
        {
            connexion.Open();
            SqlCommand insererDonneesContact = new SqlCommand(@"INSERT INTO Contact( nomContact, prenomContact, emailContact, messageContact) VALUES (@nom, @prenom, @email, @message)", connexion);
            insererDonneesContact.Parameters.AddWithValue("@nom", NouveauContact.NomContact);
            insererDonneesContact.Parameters.AddWithValue("@prenom", NouveauContact.PrenomContact);
            insererDonneesContact.Parameters.AddWithValue("@email", NouveauContact.EmailContact);
            insererDonneesContact.Parameters.AddWithValue("@message", NouveauContact.Message);
            insererDonneesContact.ExecuteNonQuery();
            connexion.Close();
        }

        public static nbVotesQuestionChoix GetNbVotesQuestionChoix(int id)
        {
            connexion.Open();

            SqlCommand getQuestion = new SqlCommand(@"SELECT nomQuestion
                                                      FROM TSondage
                                                      WHERE idSondage = @id", connexion);
            getQuestion.Parameters.AddWithValue("@id", id);

            string question = (string)getQuestion.ExecuteScalar();  //récupère la question du sondage

            connexion.Close();

            //=================================================================================================

            connexion.Open();
            SqlCommand getChoix = new SqlCommand(@"SELECT nomChoix
                                                   FROM TChoix
                                                   WHERE idSondage = @id
                                                   ORDER BY nbVoteChoix DESC", connexion);
            getChoix.Parameters.AddWithValue("@id", id);

            SqlDataReader reader = getChoix.ExecuteReader();
            List<string> lChoix = new List<string>();

            while(reader.Read())
            {
                lChoix.Add((string)reader["nomChoix"]); //récupère la liste des choix liés à un sondage et les insère dans une liste
            }
            connexion.Close();

            //=================================================================================================

            connexion.Open();
            SqlCommand getVotesQuestion = new SqlCommand(@"SELECT nbVote 
                                                           FROM TSondage
                                                           WHERE idSondage= @id", connexion);
            getVotesQuestion.Parameters.AddWithValue("@id", id);

            int nbVotesQuestion = (int)getVotesQuestion.ExecuteScalar(); //récupère le nombre de votants total pour un sondage
            connexion.Close();

            //=================================================================================================

            connexion.Open();
            SqlCommand getVotesChoix = new SqlCommand(@"SELECT nbVoteChoix 
                                                         FROM TChoix 
                                                         WHERE idSondage = @id
                                                         ORDER BY nbVoteChoix DESC", connexion);
            getVotesChoix.Parameters.AddWithValue("@id", id);

            SqlDataReader recordset = getVotesChoix.ExecuteReader();
            List<int> lNbVotesChoix = new List<int>();            

            while (recordset.Read())
            {
                lNbVotesChoix.Add((int)recordset["nbVoteChoix"]); //récupère le nombre de votes pour chaque choix et les insère dans une liste
            }
            
            connexion.Close();

            //==================================================================================================

            connexion.Open();
            SqlCommand getTypeChoix = new SqlCommand(@"SELECT choixMultiple
                                                       FROM TSondage
                                                       WHERE idSondage = @id", connexion);
            getTypeChoix.Parameters.AddWithValue("@id", id);

            bool typeChoix = (bool)getTypeChoix.ExecuteScalar(); //récupère un boolean pour savoir si le sondage est à choix unique ou multiple

            nbVotesQuestionChoix nbVotesQuestionEtChoix = new nbVotesQuestionChoix(question, lChoix, nbVotesQuestion, lNbVotesChoix, typeChoix);

            connexion.Close();

            return nbVotesQuestionEtChoix;
        }

        public static bool EstActif(int id)
        {
            connexion.Open();

            SqlCommand getStatut = new SqlCommand(@"SELECT actif
                                                  FROM TSondage
                                                  WHERE idSondage = @id", connexion);
            getStatut.Parameters.AddWithValue("@id", id);

            bool actif = (bool)getStatut.ExecuteScalar(); //récupère un boolean pour savoir si le sondage est activé

            connexion.Close();
            return actif;
        }        

        public static QuestionsEtNbVotes GetQuestionsPopulaires()
        {
            connexion.Open();

            SqlCommand getQuestions = new SqlCommand(@"SELECT TOP 10 nomQuestion
                                                  FROM TSondage
                                                  ORDER BY nbVote DESC", connexion);
            SqlDataReader reader = getQuestions.ExecuteReader();            

            List<string> listeQuestionsPopulaires = new List<string>();

            while(reader.Read())
            {
                listeQuestionsPopulaires.Add((string)reader["nomQuestion"]); //récupère les 10 questions comptant le plus de votants et les insère dans une liste
            }

            connexion.Close();
            //====================================================================================================================
            connexion.Open();

            SqlCommand getNbVotes = new SqlCommand(@"SELECT TOP 10 nbVote
                                                     FROM TSondage
                                                     ORDER BY nbVote DESC", connexion);
            SqlDataReader recordset = getNbVotes.ExecuteReader();

            List<int> listeNbVote = new List<int>();

            while(recordset.Read())
            {
                listeNbVote.Add((int)recordset["nbVote"]); //récupère le nombre de votants des 10 questions comptant le plus de votants et les insère dans une liste
            }

            connexion.Close();

            //====================================================================================================================

            connexion.Open();

            SqlCommand getDate = new SqlCommand(@"SELECT TOP 10 dateSondage
                                                  FROM TSondage
                                                  ORDER BY nbVote DESC", connexion);
            SqlDataReader lecteur = getDate.ExecuteReader();

            List<DateTime> listeDates = new List<DateTime>();

            while (lecteur.Read())
            {
                listeDates.Add((DateTime)lecteur["dateSondage"]); //récupère les dates de création des 10 dernières questions créées et les insère dans une liste
            }

            connexion.Close();

            QuestionsEtNbVotes questionsPopu = new QuestionsEtNbVotes(listeQuestionsPopulaires, listeNbVote, listeDates);

            return questionsPopu;
        }

        public static QuestionsEtNbVotes GetSondagesRecents() //récupère les derniers sondages créés
        {
            connexion.Open();

            SqlCommand getQuestions = new SqlCommand(@"SELECT TOP 10 nomQuestion
                                                       FROM TSondage
                                                       ORDER BY idSondage DESC", connexion);
            SqlDataReader reader = getQuestions.ExecuteReader();

            List<string> listeQuestionsRecentes = new List<string>();

            while(reader.Read())
            {
                listeQuestionsRecentes.Add((string)reader["nomQuestion"]); //récupère les 10 dernières questions créées et les insère dans une liste
            }

            connexion.Close();

            //====================================================================================================================

            connexion.Open();

            SqlCommand getNbVotes = new SqlCommand(@"SELECT TOP 10 nbVote
                                                    FROM TSondage
                                                    ORDER BY idSondage DESC", connexion);
            SqlDataReader recordset = getNbVotes.ExecuteReader();

            List<int> listeNbVote = new List<int>();

            while(recordset.Read())
            {
                listeNbVote.Add((int)recordset["nbVote"]); //récupère le nombre de votes des 10 dernières questions créées et les insère dans une liste
            }

            connexion.Close();

            //====================================================================================================================

            connexion.Open();

            SqlCommand getDate = new SqlCommand(@"SELECT TOP 10 dateSondage
                                                  FROM TSondage
                                                  ORDER BY idSondage DESC", connexion);
            SqlDataReader lecteur = getDate.ExecuteReader();

            List<DateTime> listeDates = new List<DateTime>();

            while(lecteur.Read())
            {
                listeDates.Add((DateTime)lecteur["dateSondage"]); //récupère les dates de création des 10 dernières questions créées et les insère dans une liste
            }
            

            connexion.Close();

            QuestionsEtNbVotes sondagesRecents = new QuestionsEtNbVotes(listeQuestionsRecentes, listeNbVote, listeDates);            

            return sondagesRecents;            
        }

        public static List<int> GetTousLesId() //récupère la liste de tous les idSondage présents dans la BDD
        {
            connexion.Open();

            SqlCommand getTousLesIdBDD = new SqlCommand(@"SELECT idSondage
                                                  FROM TSondage", connexion);
            SqlDataReader reader = getTousLesIdBDD.ExecuteReader();

            List<int> listeIdSondage = new List<int>();

            while (reader.Read())
            {
                listeIdSondage.Add((int)reader["idSondage"]);
            }

            connexion.Close();

            return listeIdSondage;
        }
    }
}