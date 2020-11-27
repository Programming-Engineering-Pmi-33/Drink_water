namespace DrinkWater.ProfileStatisticsServices
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class StatisticInfo
    {
        private static dfkg9ojh16b4rdContext db = new dfkg9ojh16b4rdContext();

        private long UserId { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StatisticInfo"/> class.
        /// </summary>
        /// <param name="userId"> User's id.</param>
        public StatisticInfo(long userId)
        {
            UserId = userId;
        }

        /// <summary>
        /// Get total week statistics of consumed fluids.
        /// </summary>
        /// <returns>Total week statistics.</returns>
        public List<Totalweekstatistic> GetTotalWeekStatistics()
        {
            return (from totalWeekQuery in db.Totalweekstatistics
                    where UserId == totalWeekQuery.UserIdRef
                    select totalWeekQuery).ToList();
        }

        /// <summary>
        /// Get total month statistics of consumed fluids.
        /// </summary>
        /// <returns>Total month statistics.</returns>
        public List<Totalmonthstatistic> GetTotalMonthStatistics()
        {
            return (from totalMonthQuery in db.Totalmonthstatistics
                    where UserId == totalMonthQuery.UserIdRef
                    select totalMonthQuery).ToList();
        }

        /// <summary>
        /// Get total year statistics of consumed fluids.
        /// </summary>
        /// <returns>Total year statistics.</returns>
        public List<Totalyearstatistic> GetTotalYearStatistics()
        {
            return (from totalYearQuery in db.Totalyearstatistics
                    where UserId == totalYearQuery.UserIdRef
                    select totalYearQuery).ToList();
        }

        /// <summary>
        /// Get week statistics of consumed water.
        /// </summary>
        /// <returns>Week statistics.</returns>
        public List<Waterweekstatistic> GetWeekStatistic()
        {
            return (from weekQuery in db.Waterweekstatistics
                    where UserId == weekQuery.UserIdRef
                    select weekQuery).ToList();
        }

        /// <summary>
        /// Get month statistics of consumed water.
        /// </summary>
        /// <returns>Month statistics.</returns>
        public List<Watermonthstatistic> GetMonthStatistics()
        {
            return (from monthQuery in db.Watermonthstatistics
             where UserId == monthQuery.UserIdRef
             select monthQuery).ToList();
        }

        /// <summary>
        /// Get year statistics of consumed water.
        /// </summary>
        /// <returns>Year statistics.</returns>
        public List<Wateryearstatistic> GetYearStatistics()
        {
            return (from yearQuery in db.Wateryearstatistics
             where UserId == yearQuery.UserIdRef
             select yearQuery).ToList();
        }
    }
}
