using DrinkWater.LogReg;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DrinkWater
{
    public class MainService
    {
        public SeriesCollection SeriesCollection { get; set; }

        static public List<Dailystatistic> Statistic = new List<Dailystatistic>();
        static public List<Statistics> FullStatistics = new List<Statistics>();
        static private SessionUser sessionUser = new SessionUser();
        static private dfkg9ojh16b4rdContext db = new dfkg9ojh16b4rdContext();
        public List<Fluids> Fluids = new List<Fluids>();
        public int balance;
        public List<KeyValuePair<Label, TextBox>> LabelBox = new List<KeyValuePair<Label, TextBox>>();
        public List<Image> PictureBox = new List<Image>();
        public List<Image> Images = new List<Image>();

        public MainService() { }
        public void GetSessionUser(SessionUser user)
        {
            sessionUser = user;
            Statistic = (from US in db.Dailystatistic
                         where US.UserIdRef == sessionUser.UserId
                         select US).ToList();
            FullStatistics = (from Fullstat in db.Statistics
                              where Fullstat.UserIdRef == sessionUser.UserId
                              select Fullstat).ToList();

        }
        public SessionUser GetUser()
        {
            return sessionUser;
        }

        public void ShowStatistic(SeriesCollection series)
        {
            SeriesCollection =series;
            balance = (int)db.Users.ToList().First(x => x.UserId == sessionUser.UserId).DailyBalance;

            if (Statistic.Count != 0)
            {
                foreach (var item in Statistic)
                {
                    SeriesCollection.Add(new StackedColumnSeries
                    {
                        Title = (from f in Fluids
                                 where ((f.FluidId == item.FluidIdRef))
                                 select f.Name).FirstOrDefault().ToString(),
                        Values = new ChartValues<double> { Double.Parse(item.Sum.ToString()) },
                        StackMode = StackMode.Values,
                        DataLabels = true,
                        MaxColumnWidth = 206

                    }); 
                }

            }

        }

        public bool IsValidAmount(string text)
        {
            bool IsValid = false;
            long test;
            if (long.TryParse(text, out test) && test > 0 && test < 6000)
            {
                IsValid = true;
            }
            return IsValid;
        }
        public void ListLiquids()
        {
            Fluids = db.Fluids.ToList();
            for (int i = 0; i < Fluids.Count; i++)
            {
                byte[] img = Fluids[i].FliudImage;
                if (img != null)
                {
                    var memoryStream = new System.IO.MemoryStream(img);
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.StreamSource = memoryStream;
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    bitmap.Freeze();
                    Images.Add(new Image { Source = bitmap });
                }
            }

            for (int i = 0; i < 4; i++)
            {
                LabelBox[i].Key.Content = Fluids[i].Name;
                PictureBox[i].Source = Images[i].Source;
            }
        }
        public void Add()
        {
            foreach (var item in LabelBox)
            {
                item.Value.BorderBrush = Brushes.LightGray;
                string text = item.Value.Text;
                if (IsValidAmount(text))
                {
                    long fluidId = (from fl in Fluids
                                    where fl.Name == item.Key.Content.ToString()
                                    select fl).First().FluidId;
                    if (!string.IsNullOrEmpty(item.Value.Text))
                    {
                        Statistics find = FullStatistics.Find(s => (s.FluidIdRef == fluidId & s.UserIdRef == sessionUser.UserId & s.Date.Day == DateTime.Now.Day));

                        if (find != null)
                        {
                            FullStatistics.Find(s => s.StatisticId == find.StatisticId).FluidAmount += long.Parse(text);
                            db.Statistics.Update(find);

                        }
                        else
                        {
                            Statistics statistics = new Statistics
                            {
                                UserIdRef = (int)sessionUser.UserId,
                                FluidIdRef = fluidId,
                                FluidAmount = long.Parse(text),
                                Date = DateTime.Now,

                            };
                            FullStatistics.Add(statistics);

                            db.Statistics.Add(statistics);

                        }
                        db.SaveChanges();
                        List<Dailystatistic> daily_statisticList = (from US in db.Dailystatistic
                                                                    where US.UserIdRef == sessionUser.UserId
                                                                    select US).ToList();
                        SeriesCollection.Clear();
                        foreach (Dailystatistic dailystatistic in daily_statisticList)
                        {
                            SeriesCollection.Add(new StackedColumnSeries
                            {
                                Title = (from f in Fluids
                                         where ((f.FluidId == dailystatistic.FluidIdRef))
                                         select f.Name).FirstOrDefault().ToString(),
                                Values = new ChartValues<double> { Double.Parse(dailystatistic.Sum.ToString()) },
                                StackMode = StackMode.Values,
                                DataLabels = true,
                                MaxColumnWidth = 206
                            });
                        }
                    }
                    item.Value.Text = "";
                }
                else if (!IsValidAmount(text) && text != "")
                {
                    item.Value.BorderBrush = Brushes.Red;
                }

            }
        }
    }
}
