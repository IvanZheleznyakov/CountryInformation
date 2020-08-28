using System;
using System.Collections.Generic;
using System.Text;

namespace CountryInfoApplication
{
    class UIConsole
    {
        public static void PrintMenu()
        {
            Console.WriteLine("Список доступных операций: \n" +
            "1 - ввести название страны для вывода информации; \n" +
            "2 - посмотреть информацию о всех странах с БД; \n" +
            "0 - выйти.");
            Console.Write("Введите номер операции: ");
        }
    }
}
