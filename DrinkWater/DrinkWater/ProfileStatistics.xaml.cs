namespace DrinkWater
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Media.Animation;
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
        public SeriesCollection SeriesCollection { get; set; }

        public string[] Labels { get; set; }

        public Func<double, string> Formatter { get; set; }

        private SessionUser sessionUser = new SessionUser();
        private static User userInformation;
        private List<string> week = new List<string>();
        private List<string> month = new List<string>();
        private List<string> year = new List<string>();
        private List<double> weekWaterAmount = new List<double>();
        private List<double> monthWaterAmount = new List<double>();
        private List<double> yearWaterAmount = new List<double>();
        private List<Fluid> fluids;
        private List<BitmapImage> images;
        private List<double> fluidAmount = new List<double>();
        private int index;

        public ProfileStatistics()
        {
            InitializeComponent();
        }

        public void GetSessionUser(SessionUser sesUser)
        {
            sessionUser = sesUser;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SeriesCollection = new SeriesCollection();
            ShowUserInfo();
            GetFluids();
            GetConsumedWaterPerWeek();
            DrawChart(weekWaterAmount, week, (int)userInformation.DailyBalance);
            ShowScore(0, 7, 1, weekWaterAmount);
            Scroll();
        }

        private void GetFluids()  
        {
            ScrollClass scrollClass = new ScrollClass("week", userInformation.UserId);
            fluids = scrollClass.Fluids;
            fluidAmount = scrollClass.FluidsAmount;
            images = scrollClass.Images;
        }

        private void DrawChart(List<double> waterAmount, List<string> period, int userWaterBalance)
        {
            SeriesCollection.Add(
          new ColumnSeries
          {
              Title = "Water",
              Values = new ChartValues<double>(waterAmount),
          });
            BalanceLine.Value = userWaterBalance;
            Axisx.Labels = period;
            Formatter = value => value.ToString("N");

            DataContext = this;
        }

        private void GetConsumedWaterPerWeek()
        {
            List<Waterweekstatistic> weekstatistics = new StatisticInfo(sessionUser.UserId).GetWeekStatistic();
            week.Clear();
            weekWaterAmount.Clear();
            foreach (var value in weekstatistics)
            {
                weekWaterAmount.Add((double)value.Amount);
            }

            foreach (var date in weekstatistics)
            {
                week.Add(date.Date.ToString());
            }
        }

        private void GetConsumedWaterPerYear()
        {
            List<Wateryearstatistic> yearstatistics = new StatisticInfo(sessionUser.UserId).GetYearStatistics();
            year.Clear();
            yearWaterAmount.Clear();
            foreach (var value in yearstatistics)
            {
                yearWaterAmount.Add((double)value.Amount);
            }

            foreach (var date in yearstatistics)
            {
                year.Add(date.Date.ToString());
            }
        }

        private void GetConsumedWaterPerMonth()
        {
            List<Watermonthstatistic> monthstatistics = new StatisticInfo(sessionUser.UserId).GetMonthStatistics();
            month.Clear();
            monthWaterAmount.Clear();
            foreach (var value in monthstatistics)
            {
                monthWaterAmount.Add((double)value.Amount);
            }

            foreach (var date in monthstatistics)
            {
                month.Add(date.Date.ToString());
            }
        }

        private void ShowUserInfo()
        { 
            UserData userData = new UserData(sessionUser);
            userInformation = userData.GetData();
            UserInfo uInfo = new UserInfo();
            UsernameInfo.Content = userInformation.Username;
            WeightInfo.Content = userInformation.Weight.ToString();
            HeightInfo.Content = userInformation.Height.ToString();
            AgeInfo.Content = userInformation.Age.ToString();
            ActivityTimeInfo.Content = uInfo.GetUserActivityTime(userInformation.GoingToBed.Value.Hours, userInformation.WakeUp.Value.Hours).ToString();
            ShowAvatar();
        }

        private void Scroll()
        {
            ImageElement1.Source = images[index];
            UpperElement1.Content = fluids[index].Name;
            LowerElement1.Content = fluidAmount[index];
            ImageElement2.Source = images[index + 1];
            UpperElement2.Content = fluids[index + 1].Name;
            LowerElement2.Content = fluidAmount[index + 1];
            ImageElement3.Source = images[index + 2];
            UpperElement3.Content = fluids[index + 2].Name;
            LowerElement3.Content = fluidAmount[index + 2];
            ImageElement4.Source = images[index + 3];
            UpperElement4.Content = fluids[index + 3].Name;
            LowerElement4.Content = fluidAmount[index + 3];
        }

        public void ShowScore(int success, int total, int koef, List<double> items)
        {
            ScoreInfo score = new ScoreInfo();

            Score2.Content = score.Score(success, total, koef, items, (long)userInformation.DailyBalance);
            Score4.Content = total;
        }

        private void ShowAvatar()
        { 
            if (userInformation.Avatar != null)
            {
                AvatarImage.Source = new ImageHandler().GetImagefromDB(userInformation.Avatar);
            }
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            Settings settings = new Settings();
            settings.GetSessionUser(sessionUser);
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
                DrawChart(weekWaterAmount, week, (int)userInformation.DailyBalance);
                ShowScore(0, 7, 1, weekWaterAmount);
            }

            if (Period.Text == "Per year")
            {
                Axisy.MaxValue = 150000;

                GetConsumedWaterPerYear();
                SeriesCollection.Clear();

                DrawChart(yearWaterAmount, year, (int)userInformation.DailyBalance * 30);
                ShowScore(0, 12, 12, yearWaterAmount);
            }

            if (Period.Text == "Per month")
            {
                Axisy.MaxValue = 5000;
                monthWaterAmount.Clear();
                GetConsumedWaterPerMonth();

                SeriesCollection.Clear();
                DrawChart(monthWaterAmount, month, (int)userInformation.DailyBalance);

                ShowScore(0, 30, 1, monthWaterAmount);
            }
        }

        private void BackwardButton_Click(object sender, RoutedEventArgs e)
        {
            if (index <= 2 || index == 0)
            {
                index = fluids.Count() - 1;
            }

            ImageElement1.Source = images[index - 3];
            UpperElement1.Content = fluids[index - 3].Name;
            LowerElement1.Content = fluidAmount[index - 3];
            ImageElement2.Source = images[index - 2];
            UpperElement2.Content = fluids[index - 2].Name;
            LowerElement2.Content = fluidAmount[index - 2];
            ImageElement3.Source = images[index - 1];
            UpperElement3.Content = fluids[index - 1].Name;
            LowerElement3.Content = fluidAmount[index - 1];
            ImageElement4.Source = images[index];
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

            ImageElement1.Source = images[index];
            UpperElement1.Content = fluids[index].Name;
            LowerElement1.Content = fluidAmount[index];
            ImageElement2.Source = images[index + 1];
            UpperElement2.Content = fluids[index + 1].Name;
            LowerElement2.Content = fluidAmount[index + 1];
            ImageElement3.Source = images[index + 2];
            UpperElement3.Content = fluids[index + 2].Name;
            LowerElement3.Content = fluidAmount[index + 2];
            ImageElement4.Source = images[index + 3];
            UpperElement4.Content = fluids[index + 3].Name;
            LowerElement4.Content = fluidAmount[index + 3];
            index++;
        }

        private void Main_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.GetSessionUser(sessionUser);
            main.Show();
            this.Close();
        }
    }
}