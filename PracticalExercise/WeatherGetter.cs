using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace PracticalExercise
{
    public static class WeatherGetter
    {
        public static WeatherResponse GetWeather()
        {
            WebRequest request = WebRequest.Create("http://api.openweathermap.org/data/2.5/forecast?q=minneapolis,us&units=imperial&APPID=09110e603c1d5c272f94f64305c09436");
            request.Method = "GET";
            WebResponse response = request.GetResponse();

            HttpWebResponse webResponse = (HttpWebResponse)response;


            if (!webResponse.StatusDescription.Equals(200))
            {
                throw new WebException("Expected a 200 response, got " + ((HttpWebResponse)response).StatusDescription);
            }


            Stream stream = response.GetResponseStream();
            StreamReader streamReader = new StreamReader(stream);
            string json = streamReader.ReadToEnd();
            return JsonConvert.DeserializeObject<WeatherResponse>(json);
        }
    }
}
