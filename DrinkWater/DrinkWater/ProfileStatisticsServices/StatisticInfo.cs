namespace DrinkWater.ProfileStatisticsServices
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Class for getting different statistic from database.
    /// </summary>
    public class StatisticInfo
    {
        private static dfkg9ojh16b4rdContext db = new dfkg9ojh16b4rdContext();

        private long UserId { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StatisticInfo"/> class.
        /// </summary>
        /// <param name="userId">Id of user.</param>
        public StatisticInfo(long userId)
        {
            UserId = userId;
        }

        /// <summary>
        /// Gets statistic of current day for user.
        /// </summary>
        /// <param name="today">Current day.</param>
        /// <returns>List of statistic records.</returns>
        public List<Statistic> GetStatistics(DateTime today)
        {
            return (from statQuery in db.Statistics
                    where UserId == statQuery.UserIdRef && statQuery.Date == today
                    select statQuery).ToList();
        }

        /// <summary>
        /// Get statistic of current day converted as water amount.
        /// </summary>
        /// <returns>List of water amount records.</returns>
        public List<Dailystatistic> GetDailyStatistics()
        {
            return (from dayQuery in db.Dailystatistics
                                      where UserId == dayQuery.UserIdRef
                                      select dayQuery).ToList();
        }

        /// <summary>
        /// Get how much water user has drunk during a week.
        /// </summary>
        /// <returns>List of week statistic records.</returns>
        public List<Totalweekstatistic> GetTotalWeekStatistics()
        {
            return (from totalWeekQuery in db.Totalweekstatistics
                    where UserId == totalWeekQuery.UserIdRef
                    select totalWeekQuery).ToList();
        }

        /// <summary>
        /// Get how much water user has drunk during a month.
        /// </summary>
        /// <returns>List of month statistic records.</returns>
        public List<Totalmonthstatistic> GetTotalMonthStatistics()
        {
            return (from totalMonthQuery in db.Totalmonthstatistics
                    where UserId == totalMonthQuery.UserIdRef
                    select totalMonthQuery).ToList();
        }

        /// <summary>
        /// Get how much water user has drunk during a year.
        /// </summary>
        /// <returns>List of week statistic year.</returns>
        public List<Totalyearstatistic> GetTotalYearStatistics()
        {
            return (from totalYearQuery in db.Totalyearstatistics
                    where UserId == totalYearQuery.UserIdRef
                    select totalYearQuery).ToList();
        }

        /// <summary>
        /// Get what luquids and in what amounts user has drunk during a week.
        /// </summary>
        /// <returns>List of week statistic records.</returns>
        public List<Waterweekstatistic> GetWeekStatistic()
        {
            return (from weekQuery in db.Waterweekstatistics
                    where UserId == weekQuery.UserIdRef
                    select weekQuery).ToList();
        }

        /// <summary>
        /// Get what luquids and in what amounts user has drunk during a month.
        /// </summary>
        /// <returns>List of month statistic records.</returns>
        public List<Watermonthstatistic> GetMonthStatistics()
        {
            return (from monthQuery in db.Watermonthstatistics
                    where UserId == monthQuery.UserIdRef
                    select monthQuery).ToList();
        }

        /// <summary>
        /// Get what luquids and in what amounts user has drunk during a year.
        /// </summary>
        /// <returns>List of year statistic records.</returns>
        public List<Wateryearstatistic> GetYearStatistics()
        {
            return (from yearQuery in db.Wateryearstatistics
                    where UserId == yearQuery.UserIdRef
                    select yearQuery).ToList();
        }
    }
}