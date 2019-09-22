using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace PracticalExercise
{
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var uri = ConfigurationManager.AppSettings.Get("uri");
                if (string.IsNullOrEmpty(uri))
                {
                    throw new NullReferenceException("The uri is null, no service exists");
                }

                WeatherResponse response = WeatherGetter.GetWeather(uri);

                if (response == null)
                {
                    throw new NullReferenceException("The response from the service was null");
                }
                else
                {
                    var weatherObjs = CalculateMaxTempAndConditionForDate(response.List);
                    foreach(var w in weatherObjs)
                    {
                        Console.WriteLine($"For {w.date}\tHigh {w.maxTemp}F\tWeather ID {w.weatherId}");

                        // When it is less than 55F or it is raining
                        if (w.maxTemp < 55 || (w.weatherId >= 500 && w.weatherId <= 599))
                        {
                            Console.WriteLine("Send Customers an SMS Message");
                        }
                        // When it is not raining, but between 55F and 75F
                        else if(w.maxTemp >= 55 && w.maxTemp <= 75)
                        {
                            Console.WriteLine("Send Customers an Email");
                        }
                        // When it is sunny and > 75F
                        else if (w.maxTemp > 75 && w.weatherId == 800)
                        {
                            Console.WriteLine("Call Customers");
                        }
                        else
                        {
                            Console.WriteLine("No Customer Action to be Taken");
                        }

                        Console.WriteLine();
                    }
                }
            }
            catch (Exception e)
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
        private static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        private static List<WeatherDateObject> CalculateMaxTempAndConditionForDate(List<List> list)
        {
            var tempDate = UnixTimeStampToDateTime(list[0].Dt);
            var tad = new WeatherDateObject(new DateTime(tempDate.Year, tempDate.Month, tempDate.Day), list[0].Main.TempMax, list[0].Weather[0].Id);

            List<WeatherDateObject> ret = new List<WeatherDateObject>();
            ret.Add(tad);

            foreach (var k in list)
            {
                var date = UnixTimeStampToDateTime(k.Dt);
                var t = new WeatherDateObject(new DateTime(date.Year, date.Month, date.Day), k.Main.TempMax, k.Weather[0].Id);

                var x = ret.Find(j => j.date == t.date);
                if (x != null)
                {
                    if (x.maxTemp > t.maxTemp)
                    {
                        if (t.weatherId < x.weatherId)
                        {
                            x.weatherId = t.weatherId;
                        }
                    }
                    else
                    {
                        x.maxTemp = t.maxTemp;
                        if (t.weatherId < x.weatherId)
                        {
                            x.weatherId = t.weatherId;
                        }
                    }
                }
                else
                {
                    ret.Add(t);
                }
            }

            // Remove the information for today
            foreach(var r in ret)
            {
                if(r.date == DateTime.Today)
                {
                    ret.Remove(r);
                    break;
                }
            }

            return ret.OrderBy(j => j.date).ToList();
        }
    }
}
