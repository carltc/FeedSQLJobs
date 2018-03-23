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
            int mealsClosed = RunSQLStoredProcedure("CloseHourOldMeals");

            int mealsAdded = RunSQLStoredProcedure("GenerateNewMeals");
        }

        static void RunSQLquery(string sqlQuery)
        {
            // Change status of meals older than 1 hour from 'Live' to 'Closed'
            string connectionString;
            try
            {
                connectionString = ConfigurationManager.ConnectionStrings["feedWebAppUser"].ConnectionString;
            }
            catch
            {
                Console.WriteLine("Connection string error...probs.");
                return;
            }

            try
            {
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
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.ToString());
            }
        }

        static int RunSQLStoredProcedure(string sqlStoredProcedure)
        {
            int result = 0;
            // Change status of meals older than 1 hour from 'Live' to 'Closed'
            string connectionString;
            try
            {
                connectionString = ConfigurationManager.ConnectionStrings["feedWebAppUser"].ConnectionString;
            }
            catch (Exception e)
            {
                Console.WriteLine("Connection string error...probs.");
                Console.WriteLine(e.ToString());
                return result;
            }

            using (SqlConnection sqlConnection1 = new SqlConnection(connectionString))
            {
                try
                {
                
                    sqlConnection1.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "EXECUTE [dbo].[" + sqlStoredProcedure + "] ;";
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = sqlConnection1;
                    result = cmd.ExecuteNonQuery();
                    sqlConnection1.Close();

                    if (result > 0)
                    {
                        Console.WriteLine("Rows affected by '" + sqlStoredProcedure + "': " + result.ToString() + ".");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e.ToString());
                }
            }
            return result;
        }
    }
}
