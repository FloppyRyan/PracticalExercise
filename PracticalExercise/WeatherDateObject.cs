//-----------------------------------------------------------------------
// <copyright file="WeatherDateObject.cs" company="CompanyName">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------
using System;

namespace PracticalExercise
{
    /// <summary>
    /// Class for holding shorthand information
    /// </summary>
    public class WeatherDateObject
    {
        /// <summary>
        /// The date
        /// </summary>
        private DateTime date;

        /// <summary>
        /// The maximum temperature
        /// </summary>
        private double maxTemp;

        /// <summary>
        /// The weather id
        /// </summary>
        private long weatherId;

        /// <summary>
        /// Initializes a new instance of the <see cref="WeatherDateObject"/> class
        /// </summary>
        public WeatherDateObject()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WeatherDateObject"/> class
        /// </summary>
        /// <param name="date">The date</param>
        /// <param name="maxTemp">The maximum temperature</param>
        /// <param name="weatherId">The weather id</param>
        public WeatherDateObject(DateTime date, double maxTemp, long weatherId)
        {
            this.Date = date;
            this.MaxTemp = maxTemp;
            this.WeatherId = weatherId;
        }

        /// <summary>
        /// Gets or sets the date
        /// </summary>
        public DateTime Date { get => this.date; set => this.date = value; }

        /// <summary>
        /// Gets or sets the maximum temperature
        /// </summary>
        public double MaxTemp { get => this.maxTemp; set => this.maxTemp = value; }

        /// <summary>
        /// Gets or sets the weather id
        /// </summary>
        public long WeatherId { get => this.weatherId; set => this.weatherId = value; }
    }
}
