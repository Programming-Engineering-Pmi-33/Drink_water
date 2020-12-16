namespace DrinkWater
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
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
        /// <summary>
        /// Gets or sets chart values.
        /// </summary>
        public SeriesCollection SeriesCollection { get; set; }

        /// <summary>
        /// Gets or sets chart labels.
        /// </summary>
        public string[] Labels { get; set; }

        private Func<double, string> Formatter { get; set; }

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

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileStatistics"/> class.
        /// </summary>
        public ProfileStatistics()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Set session user.
        /// </summary>
        /// <param name="sesUser"> Session user.</param>
        public void SetSessionUser(SessionUser sesUser)
        {
            sessionUser = sesUser;
        }

        /// <summary>
        /// Load vindow and show components on load.
        /// </summary>
        /// <param name="sender"> sender object.</param>
        /// <param name="e"> Arguments.</param>
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

        /// <summary>
        /// Get fluids list with their images and amount.
        /// </summary>
        private void GetFluids()
        {
            ScrollClass scrollClass = new ScrollClass("week", userInformation.UserId);
            fluids = scrollClass.Fluids;
            fluidAmount = scrollClass.FluidsAmount;
            images = scrollClass.Images;
        }

        /// <summary>
        /// Draw chart for consumed water.
        /// </summary>
        /// <param name="waterAmount">Water consumed per certain period.</param>
        /// <param name="period"> Certain period for showing statistics.</param>
        /// <param name="userWaterBalance"> Normal water amount for user.</param>
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

        /// <summary>
        /// Get user's consumed water per week.
        /// </summary>
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

        /// <summary>
        /// Get user's consumed water per year.
        /// </summary>
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

        /// <summary>
        /// Get user's consumed water per month.
        /// </summary>
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

        /// <summary>
        /// Show user informaton and avatar.
        /// </summary>
        private void ShowUserInfo()
        {
            UserData userData = new UserData(sessionUser);
            userInformation = userData.GetData();
            UserInfo uInfo = new UserInfo();
            UsernameInfo.Content = userInformation.Username;
            WeightInfo.Content = userInformation.Weight.ToString();
            HeightInfo.Content = userInformation.Height.ToString();
            AgeInfo.Content = userInformation.Age.ToString();
            userInformation.DailyBalance = userData.GetDailyBalnace();
            if (userInformation.GoingToBed == null || userInformation.WakeUp == null)
            {
                ActivityTimeInfo.Content = "";
            }
            else
            {
                ActivityTimeInfo.Content = uInfo.GetUserActivityTime(userInformation.GoingToBed.Value.Hours, userInformation.WakeUp.Value.Hours).ToString();
            }

            ShowAvatar();
        }

        /// <summary>
        /// Scroll liquids list.
        /// </summary>
        private void Scroll()
        {
            InitFluidsVisuals(index);
        }

        /// <summary>
        /// Show user's keeping water balance score.
        /// </summary>
        /// <param name="keepingBalanceDays">Number of days keeping daily balance.</param>
        /// <param name="totalDayNumber">Total number of days in period.</param>
        /// <param name="koef">Constant for identification of period.</param>
        /// <param name="waterAmountPerPeriod">Array of water consumed per day for every period.</param>
        public void ShowScore(int keepingBalanceDays, int totalDayNumber, int koef, List<double> waterAmountPerPeriod)
        {
            ScoreInfo score = new ScoreInfo();

            Score2.Content = score.Score(keepingBalanceDays, totalDayNumber, koef, waterAmountPerPeriod, (long)userInformation.DailyBalance);
            Score4.Content = totalDayNumber;
        }

        /// <summary>
        /// Show user avatar.
        /// </summary>
        private void ShowAvatar()
        {
            if (userInformation.Avatar != null)
            {
                AvatarImage.Source = new ImageHandler().GetImagefromDB(userInformation.Avatar);
            }
        }

        /// <summary>
        /// Redirect to Settings Window.
        /// </summary>
        /// <param name="sender">sender object.</param>
        /// <param name="e">Arguments.</param>
        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            Settings settings = new Settings();
            settings.SetSessionUser(sessionUser);
            settings.Show();
            this.Close();
        }

        /// <summary>
        /// Show chart according to chosen period.
        /// </summary>
        /// <param name="sender">sender object.</param>
        /// <param name="e">Arguments.</param>
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

        /// <summary>
        /// Scroll lquids list up.
        /// </summary>
        /// <param name="sender">sender object.</param>
        /// <param name="e">Arguments.</param>
        private void BackwardButton_Click(object sender, RoutedEventArgs e)
        {
            if (index <= 2 || index == 0)
            {
                index = fluids.Count() - 1;
            }

            InitFluidsVisuals(index - 3);
            index--;
        }

        private void InitFluidsVisuals(int index)
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

        /// <summary>
        /// Scroll lquids list down.
        /// </summary>
        /// <param name="sender">sender object.</param>
        /// <param name="e">Arguments.</param>
        private void ForwardButton_Click(object sender, RoutedEventArgs e)
        {
            if (index >= 2)
            {
                index = 0;
            }

            InitFluidsVisuals(index);
            index++;
        }

        /// <summary>
        /// Redirect to Main Window.
        /// </summary>
        /// <param name="sender">sender object.</param>
        /// <param name="e">Arguments.</param>
        private void Main_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.SetSessionUser(sessionUser);
            main.Show();
            this.Close();
        }
    }
}