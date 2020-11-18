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

        public void GetWeekStatistic()
        {
        //    return (from weekQuery in db.
        //            where UserId == weekQuery.UserIdRef
        //            select weekQuery).ToList();
        }

        public void GetMonthStatistics()
        {
            //return (from monthQuery in db.Monthstatistic
            // where UserId == monthQuery.UserIdRef
            // select monthQuery).ToList();
        }

        public void GetYearStatistics()
        {
            //return (from yearQuery in db.Yearstatistic
            // where UserId == yearQuery.UserIdRef
            // select yearQuery).ToList();
        }
    }
}
