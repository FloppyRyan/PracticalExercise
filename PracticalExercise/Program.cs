using System;
using System.Configuration;

namespace PracticalExercise
{
    public class Program
    {
        static void Main(string[] args)
        {
            var uri = ConfigurationManager.AppSettings.Get("uri");
            try
            {
                WeatherResponse response = WeatherGetter.GetWeather(uri);

                if (string.IsNullOrEmpty(uri))
                {
                    throw new NullReferenceException("The uri is null");
                }
                else if (response == null)
                {
                    throw new NullReferenceException("The response was null");
                }
                else
                {
                    Console.WriteLine(response.City.Name);
                    foreach(List l in response.List)
                    {
                        DateTime date = UnixTimeStampToDateTime(l.Dt);
                    }
                }
            }catch(Exception e)
            {
                Console.WriteLine("An exception occurred: " + e.Message);
                Console.WriteLine("An exception occurred: " + e.InnerException);
            }

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
    }
}
