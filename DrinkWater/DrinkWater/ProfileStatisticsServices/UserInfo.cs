using System;
using System.Collections.Generic;
using System.Text;

namespace DrinkWater.ProfileStatisticsServices
{
    public class UserInfo
    {
        public UserInfo() { }

        /// <summary>
        /// Calculate user activity time.
        /// </summary>
        /// <param name="bedtime"> Time that user is going to bed.</param>
        /// <param name="wakeupTime">Time that user wakes up.</param>
        /// <returns>Time that user is active through daytime.</returns>
        public int GetUserActivityTime(int bedtime, int wakeupTime)
        {
            int time;
            if (wakeupTime > bedtime)
            {
                return time = 24 - (wakeupTime - bedtime);
            }

            return time = bedtime - wakeupTime;
        }
    }
}
