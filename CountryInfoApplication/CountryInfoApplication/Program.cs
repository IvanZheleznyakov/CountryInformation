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
            var uiConsole = new UIConsole();
            uiConsole.RunApplication();
        }
    }
}
