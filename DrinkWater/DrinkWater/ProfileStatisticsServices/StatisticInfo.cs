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

        public List<Weekstatistic> GetWeekStatistic()
        {
            return (from weekQuery in db.Weekstatistic
                    where UserId == weekQuery.UserIdRef
                    select weekQuery).ToList();
        }

        public List<Monthstatistic> GetMonthStatistics()
        {
            return (from monthQuery in db.Monthstatistic
             where UserId == monthQuery.UserIdRef
             select monthQuery).ToList();
        }

        public List<Yearstatistic> GetYearStatistics()
        {
            return (from yearQuery in db.Yearstatistic
             where UserId == yearQuery.UserIdRef
             select yearQuery).ToList();
        }
    }
}
