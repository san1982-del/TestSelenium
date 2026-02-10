using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumFrameworkCSharp.Utils
{
    internal class DBManager
    {
        public static MySqlConnection connection = null;


        public static void SetMySQLDBConnection()
        {
            string connectionString = "server=127.0.0.1;user=root;password=Yuvi@2014;database=carwale";
            connection = new MySqlConnection(connectionString);

            try
            {
                connection.Open();
                Console.WriteLine("MySQL database connection established.");

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error establishing MySQL database connection: " + ex.Message);
            }


        }

        public static List<string> GetQuery(string query)
        {
            List<string> results = new List<string>();

            try
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Assuming the SELECT statement returns a single string column
                        string value = reader.GetString(0);
                        results.Add(value);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error executing SELECT statement: " + ex.Message);
            }

            return results;
        }
    }
}
