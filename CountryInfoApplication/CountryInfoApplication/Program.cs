using Newtonsoft.Json;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Configuration;

namespace CountryInfoApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            Console.WriteLine(connectionString);

            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                // Открываем подключение
                connection.Open();
                Console.WriteLine("Подключение открыто");
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Проверьте файл конфигурации.");
                return;
            }
            int operation = 1;
            while (operation != 0)
            {
                UIConsole.PrintMenu();

                while (!Int32.TryParse(Console.ReadLine(), out operation))
                {
                    Console.WriteLine("Ошибка ввода данных! Запрос должен являться целым числом. \n");
                    UIConsole.PrintMenu();
                }

                switch (operation)
                {
                    case 1:
                        break;
                    case 2:
                        break;
                    case 0:
                        break;
                    default:
                        Console.WriteLine("Данной операции нет в списке. Попробуйте еще раз. \n");
                        break;
                }

                string eur = "europe";
                string afc = "afc";
                string sqlExpr = "select Regions.Id from Regions where Regions.Name = '" + eur + "'";

                SqlCommand command = new SqlCommand(sqlExpr, connection);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows) 
                {
                    Console.WriteLine("{0}", reader.GetName(0));

                    while (reader.Read()) 
                    {
                        object id = reader.GetValue(0);

                        Console.WriteLine("{0}", id);
                    }
                }

                reader.Close();
            }

            Console.WriteLine("Подключение закрыто...");
        }
    }
}
