using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace CountryInfoApplication
{
    class UIConsole
    {
        private void PrintMenu()
        {
            Console.WriteLine("Список доступных операций: \n" +
            "1 - ввести название страны для вывода информации; \n" +
            "2 - посмотреть информацию о всех странах с БД; \n" +
            "0 - выйти.");
            Console.Write("Введите номер операции: ");
        }

        private void PrintSaveDataRequest()
        {
            Console.Write("Желаете сохранить информацию по этой стране в базу данных? \n" +
            "0 - нет \n" +
            "1 - да \n" +
            "Введите номер операции: ");
        }

        private void PrintNameOfColumns()
        {
            Console.WriteLine("{0,30}   |{1,10}   |{2,10}   |{3,10}   |{4,10}   |{5,10}", "Название", "Код страны", "Столица", "Площадь", "Население", "Регион");
            Console.WriteLine("======================================================");
        }

        private void ShowCountryInfo(List<string> countryInfo)
        {
            PrintNameOfColumns();
            Console.WriteLine("{0,30}   |{1,10}   |{2,10}   |{3,10}   |{4,10}   |{5,10}", countryInfo[0], countryInfo[1], countryInfo[2], countryInfo[3], countryInfo[4], countryInfo[5]);
        }

        public void RunApplication()
        {
            var databaseTools = new DatabaseTools();
            if (!databaseTools.ConnectToSQLServer())
            {
                return;
            }

            var apiTools = new RestCountriesAPITools();

            int operation = 1;
            while (operation != 0)
            {
                PrintMenu();

                while (!Int32.TryParse(Console.ReadLine(), out operation))
                {
                    Console.WriteLine("Ошибка ввода данных! Запрос должен являться целым числом. \n");
                    PrintMenu();
                }

                switch (operation)
                {
                    case 1:
                        Console.Write("Введите название страны на латинице: ");
                        string country = Console.ReadLine();

                        List<string> countryInfo = apiTools.GetCountryInfo(country);

                        if (countryInfo.Count == 0)
                        {
                            Console.WriteLine("Страна с таким названием не найдена.");
                        }
                        else
                        {
                            ShowCountryInfo(countryInfo);
                        }

                        int saveData;
                        PrintSaveDataRequest();

                        while (!Int32.TryParse(Console.ReadLine(), out saveData) && (saveData != 0 || saveData != 1))
                        {
                            Console.WriteLine("Такой операции нет. Введите целое число - номер операции. \n");
                            PrintSaveDataRequest();
                        }

                        if (saveData == 1)
                        {
                            databaseTools.AddCountryInfoToDatabase(countryInfo[0], countryInfo[1], countryInfo[2], countryInfo[3], countryInfo[4], countryInfo[5]);
                        }

                        break;
                    case 2:
                        break;
                    case 0:
                        break;
                    default:
                        Console.WriteLine("Данной операции нет в списке. Попробуйте еще раз. \n");
                        break;
                }
            }

            databaseTools.CloseConnection();

            Console.WriteLine("Подключение закрыто...");
        }
    }
}
