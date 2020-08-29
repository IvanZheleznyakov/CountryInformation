using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Net;
using System.Text;

namespace CountryInfoApplication
{
    class RestCountriesAPITools
    {
        public List<string> GetCountryInfo(string country)
        {
            WebRequest request = WebRequest.Create("https://restcountries.eu/rest/v2/name/" + country);

            WebResponse response = request.GetResponse();

            List<string> result = new List<string>();

            using (StreamReader stream = new StreamReader(response.GetResponseStream()))
            {
                string myJsonResponse;
                if ((myJsonResponse = stream.ReadLine()) != null)
                {
                    myJsonResponse = myJsonResponse.Remove(0, 1);
                    myJsonResponse = myJsonResponse.Remove(myJsonResponse.Length - 1, 1);
                    RestCountriesJSONClass myDeserializedClass = JsonConvert.DeserializeObject<RestCountriesJSONClass>(myJsonResponse);
                    result.Add(myDeserializedClass.Name);
                    result.Add(myDeserializedClass.Alpha2Code);
                    result.Add(myDeserializedClass.Capital);
                    result.Add(myDeserializedClass.Area.ToString());
                    result.Add(myDeserializedClass.Population.ToString());
                    result.Add(myDeserializedClass.Region);
                }
            }

            return result;
        }
    }
}
