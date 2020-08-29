using System;
using System.Collections.Generic;

namespace CountryInfoApplication
{
    /// <summary>
    /// Класс для взаимодействия с пользователем через консоль.
    /// </summary>
    public class UIConsole
    {
        private void PrintDividingLine()
        {
            Console.WriteLine("===================================================================================================================");
        }

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
            PrintDividingLine();
            Console.WriteLine("{0,30}   |{1,10}   |{2,25}   |{3,10}   |{4,10}   |{5,10}", "Название", "Код страны", "Столица", "Площадь", "Население", "Регион");
            PrintDividingLine();
        }

        private void PrintCountryInfo(List<string> countryInfo)
        {
            Console.WriteLine("{0,30}   |{1,10}   |{2,25}   |{3,10}   |{4,10}   |{5,10}", countryInfo[0], countryInfo[1], countryInfo[2], countryInfo[3], countryInfo[4], countryInfo[5]);
        }

        private void PrintRecordsFromDataBase(List<List<string>> recordsFromDatabase)
        {
            foreach (var record in recordsFromDatabase)
            {
                PrintCountryInfo(record);
            }
        }

        /// <summary>
        /// Запуск приложения.
        /// </summary>
        public void RunApplication()
        {
            var databaseTools = new DatabaseTools();
            if (!databaseTools.ConnectToSQLServer())
            {
                Console.WriteLine("Проверьте файл конфигурации.");
                return;
            }

            PrintDividingLine();
            Console.WriteLine("Подключение с базой данных открыто. Можете начинать работу.");
            PrintDividingLine();

            var apiTools = new RestCountriesAPITools();

            int operation = 1;
            while (operation != 0)
            {
                PrintMenu();

                while (!Int32.TryParse(Console.ReadLine(), out operation))
                {
                    PrintDividingLine();
                    Console.WriteLine("Ошибка ввода данных! Запрос должен являться целым числом.");
                    PrintDividingLine();
                    PrintMenu();
                }

                switch (operation)
                {
                    case 1:
                        Console.Write("Введите полное название страны на латинице (например, Russian Federation вместо Russia): ");
                        string country = Console.ReadLine();

                        List<string> countryInfo = apiTools.GetCountryInfo(country);

                        if (countryInfo.Count == 0)
                        {
                            PrintDividingLine();
                            Console.WriteLine("Страна с таким названием не найдена, или, возможно, какая-то проблема с API RestCountries. \n" +
                                "Напоминание: API работает с полным названием страны (например, Russian Federation вместо Russia).");
                            PrintDividingLine();
                        }
                        else
                        {
                            PrintNameOfColumns();
                            PrintCountryInfo(countryInfo);
                            PrintDividingLine();

                            int saveData;
                            PrintSaveDataRequest();

                            while (!Int32.TryParse(Console.ReadLine(), out saveData) && (saveData != 0 || saveData != 1))
                            {
                                PrintDividingLine();
                                Console.WriteLine("Такой операции нет. Введите целое число - номер операции.");
                                PrintDividingLine();
                                PrintSaveDataRequest();
                            }

                            if (saveData == 1)
                            {
                                databaseTools.AddCountryInfoToDatabase(countryInfo[0], countryInfo[1], countryInfo[2], countryInfo[3], countryInfo[4], countryInfo[5]);
                                PrintDividingLine();
                                Console.WriteLine("Информация о стране " + countryInfo[0] + " успешно добавлена.");
                                PrintDividingLine();
                            }
                        }
                        break;
                    case 2:
                        List<List<string>> recordsFromDataBase = databaseTools.GetRecordsFromDatabase();
                        if (recordsFromDataBase.Count == 0)
                        {
                            PrintDividingLine();
                            Console.WriteLine("На данный момент в базе данных нет записей.");
                            PrintDividingLine();
                        }
                        else
                        {
                            PrintNameOfColumns();
                            PrintRecordsFromDataBase(recordsFromDataBase);
                            PrintDividingLine();
                        }
                        break;
                    case 0:
                        break;
                    default:
                        PrintDividingLine();
                        Console.WriteLine("Данной операции нет в списке. Попробуйте еще раз. \n");
                        PrintDividingLine();
                        break;
                }
            }

            databaseTools.CloseConnection();

            Console.WriteLine("Подключение закрыто...");
        }
    }
}
