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
            string sqlSelectCapital = "SELECT Capitals.Id FROM Capitals WHERE Capitals.Name = '" + capital + "'";
            object idCapital = null;

            SqlCommand command = new SqlCommand(sqlSelectCapital, connection);
            SqlDataReader reader = command.ExecuteReader();

            if (!reader.HasRows)
            {
                reader.Close();
                string sqlInsertNewCapital = "INSERT Capitals VALUES ('" + capital + "')";
                command = new SqlCommand(sqlInsertNewCapital, connection);
                command.ExecuteNonQuery();

                command = new SqlCommand(sqlSelectCapital, connection);
                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    idCapital = reader.GetValue(0);
                }
            }

            string sqlSelectRegion = "SELECT Regions.Id FROM Regions WHERE Regions.Name = '" + region + "'";
            object idRegion = null;

            command = new SqlCommand(sqlSelectRegion, connection);
            reader = command.ExecuteReader();

            if (!reader.HasRows)
            {
                string sqlInsertNewCapital = "INSERT Regions VALUES ('" + region + "')";
                /*     Console.WriteLine("{0}", reader.GetName(0));

                     while (reader.Read())
                     {
                         object id = reader.GetValue(0);

                         Console.WriteLine("{0}", id);
                     }*/
            }
            else
            {
                while (reader.Read())
                {
                    idRegion = reader.GetValue(0);
                }
            }



            reader.Close();
        }

        public void CloseConnection()
        {
            connection.Close();
        }
    }
}
