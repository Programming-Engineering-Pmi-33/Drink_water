using DrinkWater.LogReg;
using DrinkWater.ProfileStatisticsServices;
using DrinkWater.SettingServices;
using LiveCharts;
using LiveCharts.Wpf;
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

namespace DrinkWater
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static public MainService main = new MainService();
        static public SeriesCollection SeriesCollection = new SeriesCollection();

        public List<KeyValuePair<Label, TextBox>> LabelBox = new List<KeyValuePair<Label, TextBox>>();
        public List<Image> PictureBox = new List<Image>();
        public MainWindow()
        {
            InitializeComponent();
        }
        public void GetSessionUser(SessionUser user)
        {
            main.GetSessionUser(user);
            SetChart();
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

        public void SetChart()
        {
            StatisticInfo statistic = new StatisticInfo(main.GetUser().UserId);
            if (statistic.GetDailyStatistics().Count != 0)
            {
                foreach (var item in statistic.GetDailyStatistics())
                {
                    SeriesCollection.Add(new StackedColumnSeries
                    {
                        Title = (from f in main.Fluids
                                 where f.FluidId == item.FluidIdRef
                                 select f.Name).FirstOrDefault().ToString(),
                        Values = new ChartValues<double> { Double.Parse(item.Sum.ToString()) },
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
                if(!string.IsNullOrEmpty(item.Value.ToString()) & IsValidAmount(item.Value.ToString()))
                {
                    main.Add(item.Key.ToString(), int.Parse(item.Value.ToString()));
                }
            }
            SeriesCollection.Clear();
            SetChart();
        }

        private void Up_Click(object sender, RoutedEventArgs e)
        {
            int k = main.Fluids.Count-1;
            for (int i = 3; i >=0; i--)
            {
                if(i-1< 0)
                {
                    LabelBox[i].Key.Content = main.Fluids[k].Name;
                    PictureBox[i].Source = main.Images[k].Source;
                    k--;
                }
                else
                {
                    LabelBox[i].Key.Content = main.Fluids[i-1].Name;
                    PictureBox[i].Source = main.Images[i-1].Source;
                }
                LabelBox[i].Value.Text = "";

            }
        }

        private void Down_Click(object sender, RoutedEventArgs e)
        {
            int k = main.Fluids.Count - 1;
            for (int i = 0; i < 4; i++)
            {
                if (i + 1 > main.Fluids.Count )
                {
                    LabelBox[i].Key.Content = main.Fluids[k].Name;
                    PictureBox[i].Source = main.Images[k].Source;
                    k--;
                }
                else
                {
                    LabelBox[i].Key.Content = main.Fluids[i + 1].Name;
                    PictureBox[i].Source = main.Images[i+1].Source;
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
            main.ListLiquids();
            Row.Width = 100;
            DataContext = this;

            BalanceLine.Value = (int)new UserData(main.GetUser()).GetDailyBalnace();
        }

        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            ProfileStatistics profile = new ProfileStatistics();
            profile.SessionUser = main.GetUser();
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
