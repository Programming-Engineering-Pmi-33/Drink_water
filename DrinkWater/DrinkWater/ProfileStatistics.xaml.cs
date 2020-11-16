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
    using DrinkWater.ProfileStatisticsServices;
    using DrinkWater.SettingServices;
    using LiveCharts;
    using LiveCharts.Wpf;

    /// <summary>
    /// Interaction logic for ProfileStatistics.xaml.
    /// </summary>
    ///
    public partial class ProfileStatistics : Window
    {
        public SessionUser SessionUser = new SessionUser();

        

        private static Users userInformation;

        public SeriesCollection SeriesCollection { get; set; }

        public string[] Labels { get; set; }

        public Func<double, string> Formatter { get; set; }

        private List<Weekstatistic> weekstatistics;
        private List<Monthstatistic> monthstatistics;
        private List<Yearstatistic> yearstatistics;
        private List<DateTime> week;
        private List<DateTime> month;
        private List<DateTime> year;
        private List<int> yearMonth;
        private List<double> weekWaterAmount = new List<double>();
        private List<double> monthWaterAmount = new List<double>();
        private List<double> yearWaterAmount = new List<double>();
        private List<Fluids> fluids;
        private List<BitmapImage> Images;
        private int[] fluidAmount;
        private int index;

        public ProfileStatistics()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SeriesCollection = new SeriesCollection();
            GetStatistics();
            GetFluids();
            SortPeriod();
            ShowUserInfo();
            ShowFluidFPhotos();
            GetConsumedWaterPerWeek();
            DrawWeekChart();
            ShowScore(0, 7, 1, weekWaterAmount);
            Scroll();
        }

        private void GetStatistics()
        {
            StatisticInfo statisticInfo = new StatisticInfo(SessionUser.UserId);
            weekstatistics = statisticInfo.GetWeekStatistic();
            monthstatistics = statisticInfo.GetMonthStatistics();
            yearstatistics = statisticInfo.GetYearStatistics();
        }

        private void GetFluids()
        {
            fluids = new FliudInfo().GetFluids();
        }

        private void SortPeriod()
        {
            SortedSet<DateTime> sortedWeek = new SortedSet<DateTime>();
            SortedSet<DateTime> sortedMonth = new SortedSet<DateTime>();
            SortedSet<DateTime> sortedYear = new SortedSet<DateTime>();
            SortedSet<int> sortedYearMonth = new SortedSet<int>();
            for (int i = 0; i < weekstatistics.Count; i++)
            {
                sortedWeek.Add((DateTime)weekstatistics[i].Date);
            }

            for (int i = 0; i < monthstatistics.Count; i++)
            {
                sortedMonth.Add((DateTime)monthstatistics[i].Date);
            }

            for (int i = 0; i < yearstatistics.Count; i++)
            {
                sortedYear.Add((DateTime)yearstatistics[i].Date);
            }

            for (int i = 0; i < yearstatistics.Count; i++)
            {
                sortedYearMonth.Add(yearstatistics[i].Date.Value.Month);
            }

            week = new List<DateTime>(sortedWeek);
            month = new List<DateTime>(sortedMonth);
            year = new List<DateTime>(sortedYear);
            yearMonth = new List<int>(sortedYearMonth);
        }

        private void DrawWeekChart()
        {
            SeriesCollection.Add(
          new ColumnSeries
          {
              Title = "Water",
              Values = new ChartValues<double>(weekWaterAmount),
          });
            BalanceLine.Value = (int)userInformation.DailyBalance;
            List<string> tempList = new List<string>();
            for (int i = 0; i < week.Count; i++)
            {
                tempList.Add(week[i].DayOfWeek.ToString());
            }

            Axisx.Labels = tempList;
            Formatter = value => value.ToString("N");

            DataContext = this;
        }

        private void DrawMonthChart()
        {
            SeriesCollection.Add(
                new ColumnSeries
                {
                    Title = "Water",
                    Values = new ChartValues<double>(monthWaterAmount),
                });
            List<string> tempList = new List<string>();
            for (int i = 0; i < month.Count; i++)
            {
                tempList.Add(month[i].Day.ToString());
            }

            Axisx.Labels = tempList;
            BalanceLine.Value = (int)userInformation.DailyBalance;
            Formatter = value => value.ToString("N");

            DataContext = this;
        }

        private void DrawYearChart()
        {
            SeriesCollection.Add(
               new ColumnSeries
               {
                   Title = "Water",
                   Values = new ChartValues<double>(yearWaterAmount),
               });
            List<string> tempList = new List<string>();
            for (int i = 0; i < year.Count; i++)
            {
                tempList.Add(month[i].ToString("MMM"));
            }

            Axisx.Labels = tempList;
            BalanceLine.Value = (int)userInformation.DailyBalance * 30;
            Formatter = value => value.ToString("N");

            DataContext = this;
        }

        private void GetConsumedWaterPerWeek()
        {
            fluidAmount = new int[fluids.Count];
            for (int i = 0; i < fluids.Count; i++)
            {
                for (int j = 0; j < weekstatistics.Count; j++)
                {
                    if (fluids[i].FluidId == weekstatistics[j].FluidIdRef)
                    {
                        fluidAmount[i] += (int)weekstatistics[j].Sum;
                    }
                }
            }

            for (int i = 0; i < week.Count; i++)
            {
                double temp = 0;
                for (int j = 0; j < weekstatistics.Count; j++)
                {
                    if (week[i].Day == weekstatistics[j].Date.Value.Day)
                    {
                        temp += (int)weekstatistics[j].Sum * fluids[index: (int)weekstatistics[j].FluidIdRef - 1].Koeficient;
                    }
                }

                weekWaterAmount.Add(temp);
            }
        }

        private void GetConsumedWaterPerYear()
        {
            yearWaterAmount.Clear();
            for (int l = 0; l < yearMonth.Count; l++)
            {
                double result = 0;
                for (int i = 0; i < year.Count; i++)
                {
                    double temp = 0;
                    for (int j = 0; j < yearstatistics.Count; j++)
                    {
                        if (year[i].Day == yearstatistics[j].Date.Value.Day && year[i].Month == yearstatistics[j].Date.Value.Month)
                        {
                            temp += (int)yearstatistics[j].Sum * fluids[(int)(yearstatistics[j].FluidIdRef - 1)].Koeficient;
                        }
                    }

                    if (yearMonth[l] == year[i].Month)
                    {
                        result += temp;
                    }
                }

                yearWaterAmount.Add(result);
            }

            fluidAmount = new int[fluids.Count];
            for (int i = 0; i < fluids.Count; i++)
            {
                for (int j = 0; j < yearstatistics.Count; j++)
                {
                    if (fluids[i].FluidId == yearstatistics[j].FluidIdRef)
                    {
                        fluidAmount[i] += (int)yearstatistics[j].Sum;
                    }
                }
            }
        }

        private void GetConsumedWaterPerMonth()
        {
            fluidAmount = new int[fluids.Count];
            for (int i = 0; i < fluids.Count; i++)
            {
                for (int j = 0; j < monthstatistics.Count; j++)
                {
                    if (fluids[i].FluidId == monthstatistics[j].FluidIdRef)
                    {
                        fluidAmount[i] += (int)monthstatistics[j].Sum;
                    }
                }
            }

            for (int i = 0; i < month.Count; i++)
            {
                double temp = 0;
                for (int j = 0; j < monthstatistics.Count; j++)
                {
                    if (month[i].Day == monthstatistics[j].Date.Value.Day && year[i].Month == monthstatistics[j].Date.Value.Month)
                    {
                        temp += (int)monthstatistics[j].Sum * fluids[(int)(monthstatistics[j].FluidIdRef - 1)].Koeficient;
                    }
                }

                monthWaterAmount.Add(temp);
            }
        }

        private void ShowUserInfo()
        {
            UserData userData = new UserData(SessionUser);
            userInformation = userData.GetData();
            UsernameInfo.Content = userInformation.Username;
            WeightInfo.Content = userInformation.Weight.ToString();
            HeightInfo.Content = userInformation.Height.ToString();
            AgeInfo.Content = userInformation.Age.ToString();
            ActivityTimeInfo.Content = Math.Abs(userInformation.GoingToBed.Value.Hours - userInformation.WakeUp.Value.Hours).ToString();
            ShowAvatar();
        }

        private void Scroll()
        {
            ImageElement1.Source = Images[index];
            UpperElement1.Content = fluids[index].Name;
            LowerElement1.Content = fluidAmount[index];
            ImageElement2.Source = Images[index + 1];
            UpperElement2.Content = fluids[index + 1].Name;
            LowerElement2.Content = fluidAmount[index + 1];
            ImageElement3.Source = Images[index + 2];
            UpperElement3.Content = fluids[index + 2].Name;
            LowerElement3.Content = fluidAmount[index + 2];
            ImageElement4.Source = Images[index + 3];
            UpperElement4.Content = fluids[index + 3].Name;
            LowerElement4.Content = fluidAmount[index + 3];
        }

        private void ShowScore(int m, int n, int koef, List<double> items)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (userInformation.DailyBalance * koef <= items[i])
                {
                    m++;
                }
            }

            Score2.Content = m;
            Score4.Content = n;
        }

        private void ShowFluidFPhotos()
        {
            Images = new List<BitmapImage>();
            for (int i = 0; i < fluids.Count; i++)
            {
                Images.Add(new ImageHandler().GetImagefromDB(fluids[i].FliudImage));
            }
        }

        private void ShowAvatar()
        {
            if (userInformation.Avatar != null)
            {
                var memoryStream = new MemoryStream(userInformation.Avatar);
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.StreamSource = memoryStream;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                bitmap.Freeze();
                AvatarImage.Source = bitmap;
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            Settings settings = new Settings();
            settings.GetSessionUser(SessionUser);
            settings.Show();
            this.Close();
        }

        private void Period_DropDownClosed(object sender, EventArgs e)
        {
            weekWaterAmount.Clear();

            if (Period.Text == "Per week")
            {
                Axisy.MaxValue = 5000;
                GetConsumedWaterPerWeek();
                SeriesCollection.Clear();
                DrawWeekChart();
                ShowScore(0, 7, 1, weekWaterAmount);
            }

            if (Period.Text == "Per year")
            {
                Axisy.MaxValue = 150000;

                GetConsumedWaterPerYear();
                SeriesCollection.Clear();

                DrawYearChart();
                ShowScore(0, 12, 12, yearWaterAmount);
            }

            if (Period.Text == "Per month")
            {
                Axisy.MaxValue = 5000;
                monthWaterAmount.Clear();
                GetConsumedWaterPerMonth();

                SeriesCollection.Clear();
                DrawMonthChart();

                ShowScore(0, 30, 1, monthWaterAmount);
            }
        }

        private void BackwardButton_Click(object sender, RoutedEventArgs e)
        {
            if (index <= 2 || index == 0)
            {
                index = fluidAmount.Length - 1;
            }

            ImageElement1.Source = Images[index - 3];
            UpperElement1.Content = fluids[index - 3].Name;
            LowerElement1.Content = fluidAmount[index - 3];
            ImageElement2.Source = Images[index - 2];
            UpperElement2.Content = fluids[index - 2].Name;
            LowerElement2.Content = fluidAmount[index - 2];
            ImageElement3.Source = Images[index - 1];
            UpperElement3.Content = fluids[index - 1].Name;
            LowerElement3.Content = fluidAmount[index - 1];
            ImageElement4.Source = Images[index];
            UpperElement4.Content = fluids[index].Name;
            LowerElement4.Content = fluidAmount[index];
            index--;
        }

        private void ForwardButton_Click(object sender, RoutedEventArgs e)
        {
            if (index >= 2)
            {
                index = 0;
            }

            ImageElement1.Source = Images[index];
            UpperElement1.Content = fluids[index].Name;
            LowerElement1.Content = fluidAmount[index];
            ImageElement2.Source = Images[index + 1];
            UpperElement2.Content = fluids[index + 1].Name;
            LowerElement2.Content = fluidAmount[index + 1];
            ImageElement3.Source = Images[index + 2];
            UpperElement3.Content = fluids[index + 2].Name;
            LowerElement3.Content = fluidAmount[index + 2];
            ImageElement4.Source = Images[index + 3];
            UpperElement4.Content = fluids[index + 3].Name;
            LowerElement4.Content = fluidAmount[index + 3];
            index++;
        }

        private void Main_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.main.GetSessionUser(SessionUser);
            mainWindow.Show();
            this.Close();
        }
    }
}