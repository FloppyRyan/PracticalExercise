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
                        int dayDiff = (date - DateTime.Now).Days;
                        Console.Write($"The difference in days is {dayDiff}\t");
                        Console.WriteLine($"The date and time: {date.Year}/{date.Month}/{date.Day} | {date.Hour}:{date.Minute}:{date.Second}\t");
                        Console.Write($"Current Temp: {l.Main.Temp} | Temp Min: {l.Main.TempMin} | Temp Max: {l.Main.TempMax}");
                        foreach(Weather w in l.Weather)
                        {
                            Console.WriteLine($"Weather conditions: {w.Main} | {w.Id} | {w.Description}");
                            if (w.Main.ToString().ToLower().Trim().Contains("rain"))
                            {
                                Console.WriteLine($"Rain: {l.Rain.The3H}");
                            }
                        }
                        Console.WriteLine();
                        Console.WriteLine();
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

        /// <summary>
        /// Converts a Unix timestamp in seconds from the epoch to a DateTime object
        /// Stolen from https://stackoverflow.com/questions/249760/how-can-i-convert-a-unix-timestamp-to-datetime-and-vice-versa
        /// </summary>
        /// <param name="unixTimeStamp">The Unix timestamp</param>
        /// <returns>A new DateTime object</returns>
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        public void CalculateMaxTempAndConditionForDate(List list)
        {
            var tad = new { date = DateTime.Now, high = 90, Condition = Conditions.Sunny };
        }
    }
}
