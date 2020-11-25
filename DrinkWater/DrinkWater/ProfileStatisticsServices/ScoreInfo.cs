using System;
using System.Collections.Generic;
using Windows.System.UserProfile;

namespace DrinkWater.ProfileStatisticsServices
{
    public class ScoreInfo
    {
        public ScoreInfo() { }
        public int Score(int success, int total, int koef, List<double> items, long dailyBalance) 
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (dailyBalance * koef <= items[i])
                {
                    success++;
                }
            }
            return success;
        }
    }
}
