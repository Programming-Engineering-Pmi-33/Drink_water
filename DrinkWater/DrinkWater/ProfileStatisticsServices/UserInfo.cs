namespace DrinkWater.ProfileStatisticsServices
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Class fot activity time calculation.
    /// </summary>
    public class UserInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserInfo"/> class.
        /// </summary>
        public UserInfo()
        {
        }

        /// <summary>
        /// Calculate user activity time.
        /// </summary>
        /// <param name="bedtime">Time that user is going to bed.</param>
        /// <param name="wakeupTime">Time that user wakes up.</param>
        /// <returns>Time that user is active through daytime.</returns>
        public int GetUserActivityTime(int bedtime, int wakeupTime)
        {
            if (bedtime == null || wakeupTime == null)
            {
                return 0;
            }

            int time;
            if (wakeupTime > bedtime)
            {
                return time = 24 - (wakeupTime - bedtime);
            }

            return time = bedtime - wakeupTime;
        }
    }
}
