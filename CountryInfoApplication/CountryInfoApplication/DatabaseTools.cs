using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;

namespace CountryInfoApplication
{
    /// <summary>
    /// Класс для взаимодействия с сервером БД.
    /// </summary>
    public class DatabaseTools
    {
        private SqlConnection connection;
        /// <summary>
        /// Установка соединения.
        /// </summary>
        /// <returns>Булевая переменная - успешно ли произведено соединение.</returns>
        public bool ConnectToSQLServer()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
            }
            catch (SqlException exception)
            {
                Console.WriteLine(exception.Message);
                return false;
            }

            return true;
        }

        private void AddRecordToSingleTable(string tableName, string record, ref object id)
        {
            string sqlSelectQuery = "SELECT " + tableName + ".Id FROM " + tableName + " WHERE " + tableName + ".Name = '" + record + "'";

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

        /// <summary>
        /// Добавление информации о стране в базу данных.
        /// </summary>
        /// <param name="name">Название страны.</param>
        /// <param name="countryCode">Код страны.</param>
        /// <param name="capital">Столица.</param>
        /// <param name="area">Площадь.</param>
        /// <param name="population">Население.</param>
        /// <param name="region">Регион.</param>
        public void AddCountryInfoToDatabase(string name, string countryCode, string capital, string area, string population, string region)
        {
            string tableNameCapitals = "Capitals";
            object idCapital = null;

            AddRecordToSingleTable(tableNameCapitals, capital, ref idCapital);

            string tableNameRegion = "Regions";
            object idRegion = null;

            AddRecordToSingleTable(tableNameRegion, region, ref idRegion);

            if (name.Contains("'") || name.Contains("(") || name.Contains(")"))
            {
                name = name.Replace("'", "");
                name = name.Replace("(", "");
                name = name.Replace(")", "");
            }

            string sqlSelectCountryCode = "SELECT Countries.Id FROM Countries WHERE Countries.CountryCode = '" + countryCode + "'";

            SqlCommand command = new SqlCommand(sqlSelectCountryCode, connection);
            SqlDataReader reader = command.ExecuteReader();

            if (!reader.HasRows)
            {
                reader.Close();
                string sqlInsertNewCountry = "INSERT Countries VALUES ('" + name + "', " +
                    "'" + countryCode + "', " +
                    "'" + idCapital.ToString() + "', " +
                    "'" + area + "', " +
                    "'" + population + "', " +
                    "'" + idRegion.ToString() + "')";
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

        /// <summary>
        /// Получение текущей информации о странах в БД.
        /// </summary>
        /// <returns>Список из List<string>, каждый из которых содержит информации об одной стране.</returns>
        public List<List<string>> GetRecordsFromDatabase()
        {
            List<List<string>> result = new List<List<string>>();
            string sqlSelectQuery = "SELECT Countries.Name, Countries.CountryCode, Capitals.Name, Countries.Area, Countries.Population, Regions.Name FROM Countries " +
                "JOIN Capitals ON Countries.Capital = Capitals.Id " +
                "JOIN Regions ON Countries.Region = Regions.Id";

            SqlCommand command = new SqlCommand(sqlSelectQuery, connection);
            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    List<string> countryInfo = new List<string>();
                    countryInfo.Add(reader.GetValue(0).ToString());
                    countryInfo.Add(reader.GetValue(1).ToString());
                    countryInfo.Add(reader.GetValue(2).ToString());
                    countryInfo.Add(reader.GetValue(3).ToString());
                    countryInfo.Add(reader.GetValue(4).ToString());
                    countryInfo.Add(reader.GetValue(5).ToString());
                    result.Add(countryInfo);
                }
            }

            reader.Close();

            return result;
        }

        /// <summary>
        /// Закрытие соединения.
        /// </summary>
        public void CloseConnection()
        {
            connection.Close();
        }
    }
}
