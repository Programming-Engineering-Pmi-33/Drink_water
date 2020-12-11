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

        public StatisticInfo(long userId)
        {
            UserId = userId;
        }

        public List<Statistic> GetStatistics(DateTime today)
        {
            return (from statQuery in db.Statistics
                    where UserId == statQuery.UserIdRef && statQuery.Date == today
                    select statQuery).ToList();
        }

        public List<Dailystatistic> GetDailyStatistics()
        {
            return (from dayQuery in db.Dailystatistics
                                      where UserId == dayQuery.UserIdRef
                                      select dayQuery).ToList();
        }

        public List<Totalweekstatistic> GetTotalWeekStatistics()
        {
            return (from totalWeekQuery in db.Totalweekstatistics
                    where UserId == totalWeekQuery.UserIdRef
                    select totalWeekQuery).ToList();
        }

        public List<Totalmonthstatistic> GetTotalMonthStatistics()
        {
            return (from totalMonthQuery in db.Totalmonthstatistics
                    where UserId == totalMonthQuery.UserIdRef
                    select totalMonthQuery).ToList();
        }

        public List<Totalyearstatistic> GetTotalYearStatistics()
        {
            return (from totalYearQuery in db.Totalyearstatistics
                    where UserId == totalYearQuery.UserIdRef
                    select totalYearQuery).ToList();
        }

        public List<Waterweekstatistic> GetWeekStatistic()
        {
            return (from weekQuery in db.Waterweekstatistics
                    where UserId == weekQuery.UserIdRef
                    select weekQuery).ToList();
        }

        public List<Watermonthstatistic> GetMonthStatistics()
        {
            return (from monthQuery in db.Watermonthstatistics
                    where UserId == monthQuery.UserIdRef
                    select monthQuery).ToList();
        }

        public List<Wateryearstatistic> GetYearStatistics()
        {
            return (from yearQuery in db.Wateryearstatistics
                    where UserId == yearQuery.UserIdRef
                    select yearQuery).ToList();
        }
    }
}