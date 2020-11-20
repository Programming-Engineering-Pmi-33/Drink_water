using DrinkWater.SettingServices;
using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace DrinkWater.ProfileStatisticsServices
{
    public class ScrollClass
    {
        public List<Fluid> Fluids;
        public List<double> FluidsAmount;
        public List<BitmapImage> Images;

        public ScrollClass(string period, int userId)
        {
            Fluids = new FliudInfo().GetFluids();
            Images = new List<BitmapImage>();
            FluidsAmount = new List<double>();
            foreach (var fluid in Fluids)
            {
                Images.Add(new ImageHandler().GetImagefromDB(fluid.FliudImage));
            }

            GetTotalAmount(period, userId);
            while (FluidsAmount.Count <= 5)
            {
                FluidsAmount.Add(0);
            }
        }

        public void GetTotalAmount(string period, int userId)
        {
            StatisticInfo statisticInfo = new StatisticInfo(userId);
            FluidsAmount.Clear();
            switch (period)
            {
                case "week":
                    {
                        foreach (var fluid in statisticInfo.GetTotalWeekStatistics())
                        {
                            FluidsAmount.Add((double)fluid.Sum);
                        }

                        return;
                    }

                case "month":
                    {
                        foreach (var fluid in statisticInfo.GetTotalMonthStatistics())
                        {
                            FluidsAmount.Add((double)fluid.Sum);
                        }

                        return;
                    }

                case "year":
                    {
                        foreach (var fluid in statisticInfo.GetTotalYearStatistics())
                        {
                            FluidsAmount.Add((double)fluid.Sum);
                        }

                        return;
                    }

                default:
                    throw new Exception("Period is invalid");
            }

        }
    }
}
