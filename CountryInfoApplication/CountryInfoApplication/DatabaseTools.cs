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

        private void AddRecordToSingleTable(string tableName, string record, ref object id)
        {
            string sqlSelectQuery = "SELECT" + tableName + ".Id FROM " + tableName + " WHERE " + tableName + ".Name = '" + record + "'";

            SqlCommand command = new SqlCommand(sqlSelectQuery, connection);
            SqlDataReader reader = command.ExecuteReader();

            if (!reader.HasRows)
            {
                reader.Close();
                string sqlInsertQuery = "INSERT " + tableName + " VALUES ('" + record + "')";
                command = new SqlCommand(sqlInsertQuery, connection);
                command.ExecuteNonQuery();

                command = new SqlCommand(sqlSelectQuery, connection);
                reader = command.ExecuteReader();
            }

            while (reader.Read())
            {
                id = reader.GetValue(0);
            }

            reader.Close();
        }

        public void AddCountryInfoToDatabase(string name, string countryCode, string capital, string area, string population, string region)
        {
            string tableNameCapitals = "Capitals";
            object idCapital = null;

            AddRecordToSingleTable(tableNameCapitals, capital, ref idCapital);

            string tableNameRegion = "Regions";
            object idRegion = null;

            AddRecordToSingleTable(tableNameRegion, region, ref idRegion);

            string sqlSelectCountryCode = "SELECT Countries.Id FROM Countries WHERE Countries.CountryCode = '" + countryCode + "'";

            SqlCommand command = new SqlCommand(sqlSelectCountryCode, connection);
            SqlDataReader reader = command.ExecuteReader();

            if (!reader.HasRows)
            {
                reader.Close();
                string sqlInsertNewCountry = "INSERT Countries VALUES ('" + name + "'), " +
                    "('" + countryCode + "'), " +
                    "('" + idCapital.ToString() + "'), " +
                    "('" + area + "'), " +
                    "('" + population + "'), " +
                    "('" + idRegion.ToString() + "')";
                command = new SqlCommand(sqlInsertNewCountry, connection);
                command.ExecuteNonQuery();
            }
            else
            {
                reader.Close();
                string sqlUpdateCountry = "UPDATE Countries SET Name = '" + name + 
                    "', Capital = '" + idCapital.ToString() + 
                    "', Area = '" + area + 
                    "', Population = '" + population + 
                    "', Region = '" + idRegion.ToString() + "'" +
                    "WHERE CountryCode = '" + countryCode + "'";
                command = new SqlCommand(sqlUpdateCountry, connection);
                command.ExecuteNonQuery();
            }
        }

        public void CloseConnection()
        {
            connection.Close();
        }
    }
}
