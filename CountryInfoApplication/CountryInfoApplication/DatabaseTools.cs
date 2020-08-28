using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;

namespace CountryInfoApplication
{
    class DatabaseTools
    {
        private SqlConnection connection;
        public bool ConnectToSQLServer()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                Console.WriteLine("Подключение открыто");
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Проверьте файл конфигурации.");
                return false;
            }

            return true;
        }

        public void AddCountryInfoToDatabase(string name, string countryCode, string capital, string area, string population, string region)
        {

        }

        public void CloseConnection()
        {
            connection.Close();
        }
    }
}
