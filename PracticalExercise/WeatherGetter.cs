//-----------------------------------------------------------------------
// <copyright file="WeatherGetter.cs" company="CompanyName">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace PracticalExercise
{
    /// <summary>
    /// The class to get the weather from the specified uri
    /// </summary>
    public static class WeatherGetter
    {
        /// <summary>
        /// Gets the weather from a specified uri
        /// </summary>
        /// <param name="uri">The uri to get the weather from</param>
        /// <returns>A WeatherResponse</returns>
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
            using (StreamReader streamReader = new StreamReader(stream))
            {
                string json = streamReader.ReadToEnd();
                return JsonConvert.DeserializeObject<WeatherResponse>(json);
            }
        }
    }
}
