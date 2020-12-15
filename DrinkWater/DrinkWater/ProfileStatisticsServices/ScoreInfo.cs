using System;
using System.Collections.Generic;
using Windows.System.UserProfile;

namespace DrinkWater.ProfileStatisticsServices
{
    /// <summary>
    /// Class that calculate number of days that user has achieved his water balance.
    /// </summary>
    public class ScoreInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScoreInfo"/> class.
        /// </summary>
        public ScoreInfo()
        {
        }

        /// <summary>
        /// Calculate keeping daily balance score for certain period.
        /// </summary>
        /// <param name="keepingBalanceDays">Number of days keeping daily balance.</param>
        /// <param name="totalDayNumber">Total number of days in period.</param>
        /// <param name="koef">Constant for identification of period.</param>
        /// <param name="waterAmountPerPeriod">Array of water consumed per day for every period.</param>
        /// <param name="dailyBalance">Normal amount of water for user.</param>
        /// <returns>Number of days keeping daily balance per period.</returns>
        public int Score(int keepingBalanceDays, int totalDayNumber, int koef, List<double> waterAmountPerPeriod, long dailyBalance)
        {
            for (int i = 0; i < waterAmountPerPeriod.Count; i++)
            {
                if (dailyBalance * koef <= waterAmountPerPeriod[i])
                {
                    keepingBalanceDays++;
                }
            }

            return keepingBalanceDays;
        }
    }
}
