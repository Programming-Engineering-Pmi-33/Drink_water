namespace DrinkWater
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media.Imaging;
    using DrinkWater.LogReg;
    using DrinkWater.MainServices;
    using DrinkWater.ProfileStatisticsServices;
    using DrinkWater.SettingServices;

    public class MainService
    {
        private static StatisticInfo statisticInfo;
        private static SessionUser sessionUser = new SessionUser();
        private static dfkg9ojh16b4rdContext db = new dfkg9ojh16b4rdContext();
        private static List<Fluid> fluids = new List<Fluid>();
        private static List<Image> images = new List<Image>();

        public MainService()
        {
        }

        public void GetSessionUser(SessionUser user)
        {
            sessionUser = user;
            statisticInfo = new StatisticInfo(user.UserId);
            fluids = new FliudInfo().GetFluids();
        }

        public SessionUser GetUser()
        {
            return sessionUser;
        }

        public List<Fluid> GetFluids()
        {
            return fluids;
        }

        public List<Image> GetImages()
        {
            return images;
        }

        public StatisticInfo GetStatistic()
        {
            return statisticInfo;
        }

        public void ListLiquids()
        {
            for (int i = 0; i < fluids.Count; i++)
            {
                images.Add(new Image { Source = new ImageHandler().GetImagefromDB(fluids[i].FliudImage) });
            }
        }

        public void Add(string liquidName, long liquidAmount)
        {
            List<Statistic> fullStatistic = statisticInfo.GetStatistics(DateTime.Now);
            long fluidId = (from fl in fluids
                             where fl.Name == liquidName
                             select fl.FluidId).FirstOrDefault();
            if (fluidId != 0)
            {
                Statistic find = fullStatistic.Find(s => (s.FluidIdRef == fluidId & s.UserIdRef == sessionUser.UserId & s.Date.Day == DateTime.Now.Day));

                if (find != null)
                {
                    statisticInfo.GetStatistics(DateTime.Now).Find(s => s.StatisticId == find.StatisticId).FluidAmount += liquidAmount;
                    db.Statistics.Update(find);
                }
                else
                {
                    Statistic statistics = new Statistic
                    {
                        UserIdRef = (int)sessionUser.UserId,
                        FluidIdRef = fluidId,
                        FluidAmount = (long)Math.Round((double)liquidAmount, 0),
                        Date = DateTime.Now,
                    };
                    fullStatistic.Add(statistics);

                    db.Statistics.Add(statistics);
                }

                db.SaveChanges();
            }
        }
    }
}
