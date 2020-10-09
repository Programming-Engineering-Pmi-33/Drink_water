using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrinkWater
{
    class DataOutput
    {
        public dfkg9ojh16b4rdContext db = new dfkg9ojh16b4rdContext();

        public void UserOutput()
        {
            List<User> output = db.User.ToList();
            Console.WriteLine("User table");
            foreach (User user in output)
            {
                Console.WriteLine(user.ToString());
            }
        }
        public void UserParametersOutput()
        {
            List<UserParameters> output = db.UserParameters.ToList();
            Console.WriteLine("User Parameters table");
            foreach (UserParameters userParameters in output)
            {
                Console.WriteLine(userParameters.ToString());
            }
        }
        public void StatisticOutput()
        {
            List<Statistic> output = db.Statistic.ToList();
            Console.WriteLine("Statistic table");
            foreach (Statistic statistic in output)
            {
                Console.WriteLine(statistic.ToString());
            }
        }
        public void NotificationOutput()
        {
            List<Notifications> output = db.Notifications.ToList();
            Console.WriteLine("Notifications table");
            foreach (Notifications notifications in output)
            {
                Console.WriteLine(notifications.ToString());
            }
        }
        public void DailuStatisticOutput()
        {
            List<DailyStatistic> output = db.DailyStatistic.ToList();
            Console.WriteLine("DailyStatistic table");
            foreach (DailyStatistic dailyStatistic in output)
            {
                Console.WriteLine(dailyStatistic.ToString());
            }
        }
        public void ActivityOutput()
        {
            List<ActivityTime> output = db.ActivityTime.ToList();
            Console.WriteLine("ActivityTime table");
            foreach (ActivityTime activityTime in output)
            {
                Console.WriteLine(activityTime.ToString());
            }
        }
    }
}
