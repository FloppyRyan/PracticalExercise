//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="CompanyName">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace PracticalExercise
{
    /// <summary>
    /// The main program
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The main operation
        /// </summary>
        /// <param name="args">The arguments</param>
        public static void Main(string[] args)
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
                    foreach (var w in weatherObjs)
                    {
                        Console.WriteLine($"For {w.Date}\tHigh {w.MaxTemp}F\tWeather ID {w.WeatherId}");

                        if (w.MaxTemp < 55 || (w.WeatherId >= 500 && w.WeatherId <= 599))
                        {
                        // When it is less than 55F or it is raining
                            Console.WriteLine("Send Customers an SMS Message");
                        }
                        else if (w.MaxTemp >= 55 && w.MaxTemp <= 75)
                        {
                        // When it is not raining, but between 55F and 75F
                            Console.WriteLine("Send Customers an Email");
                        }
                        else if (w.MaxTemp > 75 && w.WeatherId == 800)
                        {
                        // When it is sunny and > 75F
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
        /// Taken from https://stackoverflow.com/questions/249760/how-can-i-convert-a-unix-timestamp-to-datetime-and-vice-versa
        /// </summary>
        /// <param name="unixTimeStamp">The Unix timestamp</param>
        /// <returns>A new DateTime object</returns>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed.")]
        private static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime newDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            newDateTime = newDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return newDateTime;
        }

        /// <summary>
        /// Calculates the maximum temperature for a day, as well as the worst conditions
        /// </summary>
        /// <param name="list">The <see cref="List"/></param>
        /// <returns>A <see cref="WeatherDateObject"/></returns>
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

                var x = ret.Find(j => j.Date == t.Date);
                if (x != null)
                {
                    if (x.MaxTemp > t.MaxTemp)
                    {
                        if (t.WeatherId < x.WeatherId)
                        {
                            x.WeatherId = t.WeatherId;
                        }
                    }
                    else
                    {
                        x.MaxTemp = t.MaxTemp;
                        if (t.WeatherId < x.WeatherId)
                        {
                            x.WeatherId = t.WeatherId;
                        }
                    }
                }
                else
                {
                    ret.Add(t);
                }
            }

            // Remove the information for today
            foreach (var r in ret)
            {
                if (r.Date == DateTime.Today)
                {
                    ret.Remove(r);
                    break;
                }
            }

            return ret.OrderBy(j => j.Date).ToList();
        }
    }
}
