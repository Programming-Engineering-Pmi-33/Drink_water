namespace DrinkWater
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media.Imaging;
    using DrinkWater.LogReg;
    using LiveCharts;
    using LiveCharts.Wpf;

    /// <summary>
    /// Interaction logic for MainWindow.xaml.
    /// </summary>
    public partial class MainWindow : Window
    {
        public SeriesCollection SeriesCollection { get; set; }

        public Func<string, string> Formatter { get; set; }

        public static List<Dailystatistic> Statistic = new List<Dailystatistic>();
        public static List<Statistics> FullStatistics = new List<Statistics>();
        private static SessionUser sessionUser = new SessionUser();
        private static dfkg9ojh16b4rdContext db = new dfkg9ojh16b4rdContext();
        public List<Fluids> Fluids = new List<Fluids>();
        public List<KeyValuePair<Label, TextBox>> LabelBox = new List<KeyValuePair<Label, TextBox>>();
        public List<Image> PictureBox = new List<Image>();
        private List<Image> Images = new List<Image>();

        public MainWindow()
        {
            InitializeComponent();
        }

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

        private void ListLiquids()
        {
            LabelBox.Add(new KeyValuePair<Label, TextBox>(TypeOfLiquid1, Amount1));
            LabelBox.Add(new KeyValuePair<Label, TextBox>(TypeOfLiquid2, Amount2));
            LabelBox.Add(new KeyValuePair<Label, TextBox>(TypeOfLiquid3, Amount3));
            LabelBox.Add(new KeyValuePair<Label, TextBox>(TypeOfLiquid4, Amount4));

            PictureBox.Add(ImageBox1);
            PictureBox.Add(ImageBox2);
            PictureBox.Add(ImageBox3);
            PictureBox.Add(ImageBox4);

            Fluids = db.Fluids.ToList();
            for (int i = 0; i < Fluids.Count; i++)
            {
                byte[] img = Fluids[i].FliudImage;
                if (img != null)
                {
                    var memoryStream = new MemoryStream(img);
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

        public void ShowStatistic()
        {
            SeriesCollection = new SeriesCollection();
            Row.Title = sessionUser.Username.ToString();
            int balance = (int)db.Users.ToList().First(x => x.UserId == sessionUser.UserId).DailyBalance;
            BalanceLine.Value = balance;
            Milliliters.MaxValue = balance + 1000;
            Row.Width = 100;

            if (Statistic.Count != 0)
            {
                foreach (var item in Statistic)
                {
                    SeriesCollection.Add(new StackedColumnSeries
                    {
                        Title = (from f in Fluids
                                 where f.FluidId == item.FluidIdRef
                                 select f.Name).FirstOrDefault().ToString(),

                        Values = new ChartValues<double> { double.Parse(item.Sum.ToString()) },
                        StackMode = StackMode.Values,
                        DataLabels = true,
                        MaxColumnWidth = 206,
                    });
                }

                SeriesCollection[0].Values.Add(4d);
            }

            Formatter = value => value + "ml";
            DataContext = this;
        }

        private void Add(object sender, RoutedEventArgs e)
        {
            foreach (var item in LabelBox)
            {
                string text = item.Value.Text;
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
                    List<Dailystatistic> temp = (from US in db.Dailystatistic
                                                 where US.UserIdRef == sessionUser.UserId
                                                 select US).ToList();
                    SeriesCollection.Clear();
                    foreach (Dailystatistic dailystatistic in temp)
                    {
                        SeriesCollection.Add(new StackedColumnSeries
                        {
                            Title = (from f in Fluids
                                     where f.FluidId == dailystatistic.FluidIdRef
                                     select f.Name).FirstOrDefault().ToString(),
                            Values = new ChartValues<double> { double.Parse(dailystatistic.Sum.ToString()) },
                            StackMode = StackMode.Values,
                            DataLabels = true,
                        });
                    }
                }
            }

            // ShowStatistic();
        }

        private void Up_Click(object sender, RoutedEventArgs e)
        {
            int k = Fluids.Count - 1;
            for (int i = 3; i >= 0; i--)
            {
                if (i - 1 < 0)
                {
                    LabelBox[i].Key.Content = Fluids[k].Name;
                    PictureBox[i].Source = Images[k].Source;
                    k--;
                }
                else
                {
                    LabelBox[i].Key.Content = Fluids[i - 1].Name;
                    PictureBox[i].Source = Images[i - 1].Source;
                }

                LabelBox[i].Value.Text = string.Empty;
            }
        }

        private void Down_Click(object sender, RoutedEventArgs e)
        {
            int k = Fluids.Count - 1;
            for (int i = 0; i < 4; i++)
            {
                if (i + 1 > Fluids.Count - 1)
                {
                    LabelBox[i].Key.Content = Fluids[k].Name;
                    PictureBox[i].Source = Images[k].Source;
                    k--;
                }
                else
                {
                    LabelBox[i].Key.Content = Fluids[i + 1].Name;
                    PictureBox[i].Source = Images[i + 1].Source;
                }

                LabelBox[i].Value.Text = string.Empty;
            }
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            GetSessionUser(sessionUser);
            ListLiquids();
            ShowStatistic();
        }
    }
}
