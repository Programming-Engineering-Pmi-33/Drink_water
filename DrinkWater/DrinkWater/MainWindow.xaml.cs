namespace DrinkWater
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using DrinkWater.LogReg;
    using DrinkWater.MainServices;
    using DrinkWater.ProfileStatisticsServices;
    using DrinkWater.SettingServices;
    using LiveCharts;
    using LiveCharts.Wpf;

    /// <summary>
    /// Interaction logic for Main.xaml.
    /// </summary>
    public partial class MainWindow : Window
    {
        private static MainService main = new MainService();

        public SeriesCollection SeriesCollection { get; set; }

        private List<KeyValuePair<Label, TextBox>> labelBox = new List<KeyValuePair<Label, TextBox>>();
        private List<Image> pictureBox = new List<Image>();

        public MainWindow()
        {
            InitializeComponent();
        }

        public void GetSessionUser(SessionUser user)
        {
            main.GetSessionUser(user);
        }

        public void SetChart()
        {
            StatisticInfo statistic = main.GetStatistic();

            if (statistic.GetDailyStatistics().Count != 0)
            {
                foreach (var item in statistic.GetDailyStatistics())
                {
                    SeriesCollection.Add(new StackedColumnSeries
                    {
                        Title = (from f in main.GetFluids()
                                 where f.FluidId == item.FluidIdRef
                                 select f.Name).FirstOrDefault().ToString(),
                        Values = new ChartValues<double> { double.Parse(item.Sum.ToString()) },
                        StackMode = StackMode.Values,
                        DataLabels = true,
                        MaxColumnWidth = 206,
                    });
                }
            }
        }

        private void Add(object sender, RoutedEventArgs e)
        {
            foreach (var item in labelBox)
            {
                if (!string.IsNullOrEmpty(item.Value.Text))
                {
                    item.Value.BorderBrush = Brushes.LightGray;
                    ValidationLiquid validation = new ValidationLiquid(item.Key.Content.ToString(), item.Value.Text.ToString());
                    if (validation != null)
                    {
                        main.Add(validation.GetName(), validation.GetAmount());
                        item.Value.Clear();
                    }
                    else
                    {
                        item.Value.BorderBrush = Brushes.Red;
                    }
                }
            }

            SeriesCollection.Clear();
            SetChart();
        }

        private void Up_Click(object sender, RoutedEventArgs e)
        {
            int k = main.GetFluids().Count - 1;
            for (int i = 3; i >= 0; i--)
            {
                if (i - 1 < 0)
                {
                    labelBox[i].Key.Content = main.GetFluids()[k].Name;
                    pictureBox[i].Source = main.GetImages()[k].Source;
                    k--;
                }
                else
                {
                    labelBox[i].Key.Content = main.GetFluids()[i - 1].Name;
                    pictureBox[i].Source = main.GetImages()[i - 1].Source;
                }

                labelBox[i].Value.Text = string.Empty;
            }
        }

        private void Down_Click(object sender, RoutedEventArgs e)
        {
            int k = main.GetFluids().Count - 1;
            for (int i = 0; i < 4; i++)
            {
                if (i + 1 > main.GetFluids().Count)
                {
                    labelBox[i].Key.Content = main.GetFluids()[k].Name;
                    pictureBox[i].Source = main.GetImages()[k].Source;
                    k--;
                }
                else
                {
                    labelBox[i].Key.Content = main.GetFluids()[i + 1].Name;
                    pictureBox[i].Source = main.GetImages()[i + 1].Source;
                }

                labelBox[i].Value.Text = string.Empty;
            }
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            main.ListLiquids();
            SeriesCollection = new SeriesCollection();

            labelBox.Add(new KeyValuePair<Label, TextBox>(TypeOfLiquid1, Amount1));
            labelBox.Add(new KeyValuePair<Label, TextBox>(TypeOfLiquid2, Amount2));
            labelBox.Add(new KeyValuePair<Label, TextBox>(TypeOfLiquid3, Amount3));
            labelBox.Add(new KeyValuePair<Label, TextBox>(TypeOfLiquid4, Amount4));

            pictureBox.Add(ImageBox1);
            pictureBox.Add(ImageBox2);
            pictureBox.Add(ImageBox3);
            pictureBox.Add(ImageBox4);

            for (int i = 0; i < 4; i++)
            {
                labelBox[i].Key.Content = main.GetFluids()[i].Name;
                pictureBox[i].Source = main.GetImages()[i].Source;
            }

            Row.Width = 100;
            DataContext = this;

            BalanceLine.Value = (int)new UserData(main.GetUser()).GetDailyBalnace();
            SetChart();
        }

        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            ProfileStatistics profile = new ProfileStatistics();
            profile.GetSessionUser(main.GetUser());
            profile.Show();
            this.Close();
        }

        private void Setting_Click(object sender, RoutedEventArgs e)
        {
            Settings settings = new Settings();
            settings.GetSessionUser(main.GetUser());
            settings.Show();
            this.Close();
        }
    }
}
