namespace DrinkWater
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using DrinkWater.LogReg;
    using DrinkWater.ProfileStatisticsServices;
    using DrinkWater.SettingServices;
    using LiveCharts;
    using LiveCharts.Wpf;

    public class MainService
    {
        static public SeriesCollection SeriesCollection { get; set; }

        public static void Add(List<KeyValuePair<string, int>> liquidsAmount)
        {
            throw new NotImplementedException();
        }

        static public List<Dailystatistic> Statistic = new List<Dailystatistic>();
        static public List<Statistic> FullStatistics = new List<Statistic>();
        static private SessionUser sessionUser = new SessionUser();
        static private dfkg9ojh16b4rdContext db = new dfkg9ojh16b4rdContext();
        public List<Fluid> Fluids = new List<Fluid>();
        public List<Image> Images = new List<Image>();

        public MainService()
        {
        }

        public void GetSessionUser(SessionUser user)
        {
            sessionUser = user;
            StatisticInfo statisticInfo = new StatisticInfo(user.UserId);
            Statistic = statisticInfo.GetDailyStatistics();
            FullStatistics = statisticInfo.GetStatistics(DateTime.Now);
        }

        public SessionUser GetUser()
        {
            return sessionUser;
        }

        public void ListLiquids()
        {
            Fluids = new FliudInfo().GetFluids();
            for (int i = 0; i < Fluids.Count; i++)
            {
                Images.Add(new Image { Source = new ImageHandler().GetImagefromDB(Fluids[i].FliudImage) });
            }
        }

        public void Add(string liquidName, long liquidAmount)
        {
            long fluidId = (from fl in Fluids
                        where fl.Name == liquidName
                        select fl).First().FluidId;
            Statistic find = FullStatistics.Find(s => (s.FluidIdRef == fluidId & s.UserIdRef == sessionUser.UserId & s.Date.Day == DateTime.Now.Day));

            if (find != null)
            {
                FullStatistics.Find(s => s.StatisticId == find.StatisticId).FluidAmount += liquidAmount;
                db.Statistics.Update(find);
            }
            else
            {
                Statistic statistics = new Statistic
                {
                    UserIdRef = (int)sessionUser.UserId,
                    FluidIdRef = fluidId,
                    FluidAmount = liquidAmount,
                    Date = DateTime.Now,
                };
                FullStatistics.Add(statistics);

                db.Statistics.Add(statistics);
            }

            db.SaveChanges();
        }
    }
}
