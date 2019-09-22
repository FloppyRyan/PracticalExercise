using Newtonsoft.Json;
using System.IO;
using System.Net;

namespace PracticalExercise
{
    public static class WeatherGetter
    {
        public static WeatherResponse GetWeather(string uri)
        {
            WebRequest request = WebRequest.Create(uri);
            request.Method = "GET";
            WebResponse response = request.GetResponse();

            HttpWebResponse webResponse = (HttpWebResponse)response;


            if (!webResponse.StatusDescription.Equals("OK"))
            {
                throw new WebException("Expected a 200 response, got " + ((HttpWebResponse)response).StatusDescription);
            }

            Stream stream = response.GetResponseStream();
            using(StreamReader streamReader = new StreamReader(stream))
            {
                string json = streamReader.ReadToEnd();
                return JsonConvert.DeserializeObject<WeatherResponse>(json);
            }
        }
    }
}
