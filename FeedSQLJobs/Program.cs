using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace FeedSQLJobs
{
    class Program
    {
        static void Main(string[] args)
        {
            ChangeMealStatuses();
        }

        static void ChangeMealStatuses()
        {
            RunSQLquery("EXECUTE [dbo].[CloseHourOldMeals];");
        }

        static void RunSQLquery(string sqlQuery)
        {
            // Change status of meals older than 1 hour from 'Live' to 'Closed'
            string connectionString;
            try
            {
                connectionString = ConfigurationManager.ConnectionStrings["FeedMeDataBaseConnectionString"].ConnectionString;
            }
            catch
            {
                try
                {
                    connectionString = ConfigurationManager.ConnectionStrings["LocalFeedMeDataBaseConnectionString"].ConnectionString;
                }
                catch
                {
                    Console.WriteLine("Connection string error...probs.");
                    return;
                }
            }
            SqlConnection sqlConnection1 = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = sqlQuery; // "EXECUTE [dbo].[CloseHourOldMeals] ;";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnection1;

            sqlConnection1.Open();

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                // Data is accessible through the DataReader object here.
                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        Console.WriteLine(reader.GetValue(i).ToString());
                    }
                }
                reader.Close();
            }
            sqlConnection1.Close();
        }
    }
}
