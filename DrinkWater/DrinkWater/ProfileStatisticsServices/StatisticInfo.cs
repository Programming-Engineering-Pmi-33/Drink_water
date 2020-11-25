using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace DrinkWater.ProfileStatisticsServices
{
    class StatisticInfo
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

        public List<Totalweekstatistic> GetWeekStatistic()
        {
            return (from weekQuery in db.Totalweekstatistics
                    where UserId == weekQuery.UserIdRef
                    select weekQuery).ToList();
        }

        public List<Totalmonthstatistic> GetMonthStatistics()
        {
            return (from monthQuery in db.Totalmonthstatistics
                    where UserId == monthQuery.UserIdRef
             select monthQuery).ToList();
        }

        public List<Totalyearstatistic> GetYearStatistics()
        {
            return (from yearQuery in db.Totalyearstatistics
                    where UserId == yearQuery.UserIdRef
             select yearQuery).ToList();
        }
    }
}
