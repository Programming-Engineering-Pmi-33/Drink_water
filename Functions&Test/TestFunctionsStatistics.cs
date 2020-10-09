
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Drink_water
{
    public class TestFunctionsStatistics
    {
        private long UserId { get; set; }
        public bool Exist { get; set; } = false;
        dfkg9ojh16b4rdContext db = new dfkg9ojh16b4rdContext();
        TestFunctionsStatistics() { }
        public TestFunctionsStatistics(int userId)
        {
            UserId = userId;
        }
        public void DoesExist()
        {
            var Users = db.Users.ToList();
            User searched_user = (from User user in Users
                                  where user.UserId == UserId
                                  select user).FirstOrDefault();
            if (searched_user != null)
            {
                Exist = true;
            }
        }
        public void Update(long _amount)
        {
            var daily = db.DailyStatistics.ToList();
            DailyStatistic searched_user = (from DailyStatistic ds in daily
                                            where ds.UserId == UserId
                                            select ds).FirstOrDefault();
            searched_user.DailyBalance = _amount;
            db.SaveChanges();
        }
        public void Delete()
        {
            var statistics = db.Statistics.ToList();
            Statistic searched_statistic = (from Statistic ds in statistics
                                            where ds.UserId == UserId
                                            select ds).FirstOrDefault();
            db.Statistics.Remove(searched_statistic);
            db.SaveChanges();
        }
        public void AddStatistic(DateTime date, string liquidType, long amount)
        {
            Statistic newStatistic = new Statistic { UserId = UserId, Date = date, LiqiudType = liquidType, Amount = amount };
            db.Statistics.Add(newStatistic);
            db.SaveChanges();
        }
        public void AddDailyStatistic(DateTime date, long dailyBalance, bool achieved)
        {
            DailyStatistic newDailyStatistic = new DailyStatistic { UserId = UserId, Date = date, DailyBalance = dailyBalance, WasAchieved = achieved };
            db.DailyStatistics.Add(newDailyStatistic);
            db.SaveChanges();
        }
    }
}
