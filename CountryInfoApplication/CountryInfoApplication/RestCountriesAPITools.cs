using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace CountryInfoApplication
{
    class RestCountriesAPITools
    {
        public static List<string> GetInfoCountry(string country)
        {
            WebRequest request = WebRequest.Create("https://restcountries.eu/rest/v2/name/" + country);

            WebResponse response = request.GetResponse();

            List<string> result = new List<string>();

            using (StreamReader stream = new StreamReader(response.GetResponseStream()))
            {
                string myJsonResponse;
                if ((myJsonResponse = stream.ReadLine()) != null)
                {
                    RestCountriesJSONClass myDeserializedClass = JsonConvert.DeserializeObject<RestCountriesJSONClass>(myJsonResponse);
                    result.Add(myDeserializedClass.CountryInfo.name);
                    result.Add(myDeserializedClass.CountryInfo.alpha2Code);
                    result.Add(myDeserializedClass.CountryInfo.capital);
                    result.Add(myDeserializedClass.CountryInfo.area.ToString());
                    result.Add(myDeserializedClass.CountryInfo.population.ToString());
                    result.Add(myDeserializedClass.CountryInfo.region);
                }
            }

            return result;
        }
    }
}
