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

    /// <summary>
    /// Main window UI handlers.
    /// </summary>
    public class MainService
    {
        private static StatisticInfo statisticInfo;
        private static SessionUser sessionUser = new SessionUser();

        /// <summary>
        /// Instance of db context object.
        /// </summary>
        public dfkg9ojh16b4rdContext Db = new dfkg9ojh16b4rdContext();
        private static List<Fluid> fluids = new List<Fluid>();
        private static List<Image> images = new List<Image>();

        /// <summary>
        /// Initializes a new instance of the <see cref="MainService"/> class.
        /// </summary>
        public MainService()
        {
        }

        /// <summary>This method changes the session user and get data about statistic and fluids.
        /// </summary>
        /// <param name="user">is the current user.</param>
        public void SetSessionUser(SessionUser user)
        {
            sessionUser = user;
            statisticInfo = new StatisticInfo(user.UserId);
            fluids = new FliudInfo().GetFluids();
        }

        /// <summary>Property <c>GetUser</c> represents the session user.
        /// </summary>
        /// <returns>Session user.</returns>
        public SessionUser GetUser()
        {
            return sessionUser;
        }

        /// <summary>Property <c>GetFluids</c> represents the data list about fluids.
        /// </summary>
        /// <returns>List of Fluid.</returns>
        public List<Fluid> GetFluids()
        {
            return fluids;
        }

        /// <summary>Property <c>GetImages</c> represents the data list about images.
        /// </summary>
        /// <returns>List of images.</returns>
        public List<Image> GetImages()
        {
            return images;
        }

        /// <summary>Property <c>GetStatistic</c> represents all statistic of session user.
        /// </summary>
        /// <returns>StatisticInfo.</returns>
        public StatisticInfo GetStatistic()
        {
            return statisticInfo;
        }

        /// <summary>This method gets data about images
        ///  from the database.</summary>
        public void ListLiquids()
        {
            for (int i = 0; i < fluids.Count; i++)
            {
                images.Add(new Image { Source = new ImageHandler().GetImagefromDB(fluids[i].FliudImage) });
            }
        }

        /// <summary>
        /// This method add data about liquid in Statistic database ,
        /// if fluid data already exists, the database is updated, otherwise
        /// a new Statistics item is created and added to the database.
        /// </summary>
        /// <param name="liquidName">The name of added liquid.</param>
        /// <param name="liquidAmount">The amount of this liquid.</param>
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
                    Db.Statistics.Update(find);
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

                    Db.Statistics.Add(statistics);
                }

                Db.SaveChanges();
        }
            else
            {
                MessageBox.Show("There is no such liquid", "Data error", MessageBoxButton.OK);
            }
}
    }
}
