using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace Sondage.Models
{
    public class SQLSD
    {
        private const string SqlConnectionString = @"Server=.\SQLExpress;Initial Catalog=Projet; Trusted_Connection=Yes";

        public static void getIdLiens()
        {
            SqlConnection connexion = new SqlConnection(SqlConnectionString);
            connexion.Open();

            SqlCommand command = new SqlCommand(@"SELECT MAX(idSondage) FROM TSondage ", connexion);
            SqlDataReader recordset = command.ExecuteReader(); //cherche l'id du dernier sondage créé


            connexion.Close();
            ;              
        }

    }
}