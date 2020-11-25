namespace DrinkWater
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Navigation;
    using System.Windows.Shapes;
    using DrinkWater.LogReg;
    using DrinkWater.ProfileStatisticsServices;
    using DrinkWater.SettingServices;
    using LiveCharts;
    using LiveCharts.Wpf;

    /// <summary>
    /// Interaction logic for Main.xaml.
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainService Main = new MainService();
        public static SeriesCollection SeriesCollection = new SeriesCollection();

        public List<KeyValuePair<Label, TextBox>> LabelBox = new List<KeyValuePair<Label, TextBox>>();
        public List<Image> PictureBox = new List<Image>();

        public MainWindow()
        {
            InitializeComponent();
        }

        public void GetSessionUser(SessionUser user)
        {
            Main.GetSessionUser(user);
            SetChart();
        }

        public bool IsValidAmount(string text)
        {
            bool isValid = false;
            long test;
            if (long.TryParse(text, out test) && test > 0 && test < 6000)
            {
                isValid = true;
            }

            return isValid;
        }

        public void SetChart()
        {
            StatisticInfo statistic = new StatisticInfo(Main.GetUser().UserId);
            if (statistic.GetDailyStatistics().Count != 0)
            {
                foreach (var item in statistic.GetDailyStatistics())
                {
                    SeriesCollection.Add(new StackedColumnSeries
                    {
                        Title = (from f in Main.Fluids
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
            foreach (var item in LabelBox)
            {
                if (!string.IsNullOrEmpty(item.Value.ToString()) & IsValidAmount(item.Value.ToString()))
                {
                    Main.Add(item.Key.ToString(), int.Parse(item.Value.ToString()));
                }
            }

            SeriesCollection.Clear();
            SetChart();
        }

        private void Up_Click(object sender, RoutedEventArgs e)
        {
            int k = Main.Fluids.Count - 1;
            for (int i = 3; i >= 0; i--)
            {
                if (i - 1 < 0)
                {
                    LabelBox[i].Key.Content = Main.Fluids[k].Name;
                    PictureBox[i].Source = Main.Images[k].Source;
                    k--;
                }
                else
                {
                    LabelBox[i].Key.Content = Main.Fluids[i - 1].Name;
                    PictureBox[i].Source = Main.Images[i - 1].Source;
                }

                LabelBox[i].Value.Text = "";
            }
        }

        private void Down_Click(object sender, RoutedEventArgs e)
        {
            int k = Main.Fluids.Count - 1;
            for (int i = 0; i < 4; i++)
            {
                if (i + 1 > Main.Fluids.Count)
                {
                    LabelBox[i].Key.Content = Main.Fluids[k].Name;
                    PictureBox[i].Source = Main.Images[k].Source;
                    k--;
                }
                else
                {
                    LabelBox[i].Key.Content = Main.Fluids[i + 1].Name;
                    PictureBox[i].Source = Main.Images[i + 1].Source;
                }

                LabelBox[i].Value.Text = "";
            }
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            LabelBox.Add(new KeyValuePair<Label, TextBox>(TypeOfLiquid1, Amount1));
            LabelBox.Add(new KeyValuePair<Label, TextBox>(TypeOfLiquid2, Amount2));
            LabelBox.Add(new KeyValuePair<Label, TextBox>(TypeOfLiquid3, Amount3));
            LabelBox.Add(new KeyValuePair<Label, TextBox>(TypeOfLiquid4, Amount4));

            PictureBox.Add(ImageBox1);
            PictureBox.Add(ImageBox2);
            PictureBox.Add(ImageBox3);
            PictureBox.Add(ImageBox4);

            SetChart();
            Main.ListLiquids();
            Row.Width = 100;
            DataContext = this;

            BalanceLine.Value = (int)new UserData(Main.GetUser()).GetDailyBalnace();
        }

        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            ProfileStatistics profile = new ProfileStatistics();
            profile.GetSessionUser(Main.GetUser());
            profile.Show();
            this.Close();
        }

        private void Setting_Click(object sender, RoutedEventArgs e)
        {
            Settings settings = new Settings();
            settings.GetSessionUser(Main.GetUser());
            settings.Show();
            this.Close();
        }
    }
}
