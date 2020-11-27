namespace DrinkWater.ProfileStatisticsServices
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class UserInfo
    {
        public UserInfo()
        {
        }

        public int GetUserActivityTime(int goingtobed, int wakeup)
        {
            int time = Math.Abs(goingtobed - wakeup);
            return time;
        }
    }
}
