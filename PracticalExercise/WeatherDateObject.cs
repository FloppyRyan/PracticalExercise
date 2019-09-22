using System;

namespace PracticalExercise
{
    /// <summary>
    /// Class for holding shorthand information
    /// </summary>
    public class WeatherDateObject
    {
        /// <summary>
        /// Instantiates a new instance of the <see cref="WeatherDateObject"/>
        /// </summary>
        public WeatherDateObject()
        {

        }

        /// <summary>
        /// Instantiates a new isntance of the <see cref="WeatherDateObject"/>
        /// </summary>
        /// <param name="date">The date</param>
        /// <param name="maxTemp">The maximum temperature</param>
        /// <param name="weatherId">The weather id</param>
        public WeatherDateObject(DateTime date, double maxTemp, long weatherId)
        {
            this.date = date;
            this.maxTemp = maxTemp;
            this.weatherId = weatherId;
        }

        public DateTime date;
        public double maxTemp;
        public long weatherId;
    }
}
}
