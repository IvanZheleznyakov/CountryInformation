using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace CountryInfoApplication
{
    /// <summary>
    /// Класс для взаимодействия с API RestCountries.
    /// </summary>
    public class RestCountriesAPITools
    {
        /// <summary>
        /// Получение информации о стране.
        /// </summary>
        /// <param name="country">Полное название страны.</param>
        /// <returns>Список, в котором содержится информация. Пустой, если запрос не удался.</returns>
        public List<string> GetCountryInfo(string country)
        {
            WebRequest request = WebRequest.Create("https://restcountries.eu/rest/v2/name/" + country + "?fullText=true");

            List<string> result = new List<string>();

            WebResponse response;

            try
            {
                response = request.GetResponse();
            }
            catch (WebException exception)
            {
                Console.WriteLine(exception.Message);
                return result;
            }

            using (StreamReader stream = new StreamReader(response.GetResponseStream()))
            {
                string myJsonResponse;
                if ((myJsonResponse = stream.ReadLine()) != null)
                {
                    var myDeserializedClass = JsonConvert.DeserializeObject<List<ArrayOfJSON>>(myJsonResponse);
                    result.Add(myDeserializedClass[0].Name);
                    result.Add(myDeserializedClass[0].Alpha2Code);
                    result.Add(myDeserializedClass[0].Capital);
                    result.Add(myDeserializedClass[0].Area.ToString());
                    result.Add(myDeserializedClass[0].Population.ToString());
                    result.Add(myDeserializedClass[0].Region);
                }
            }

            return result;
        }
    }
}
