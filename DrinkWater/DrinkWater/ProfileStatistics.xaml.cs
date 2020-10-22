using DrinkWater.LogReg;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Linq;
using LiveCharts;
using LiveCharts.Wpf;

namespace DrinkWater
{
    /// <summary>
    /// Interaction logic for ProfileStatistics.xaml
    /// </summary>
    public partial class ProfileStatistics : Window
    {
        public SessionUser SessionUser = new SessionUser();
        static public dfkg9ojh16b4rdContext db = new dfkg9ojh16b4rdContext();
        static public Users userInformatoin;
        public SeriesCollection SeriesCollection;
        public string[] Labels;
        public Func<double, string> Formatter;
        public ProfileStatistics()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Users userInformatoin = (from user in db.Users
                                     where user.UserId == SessionUser.UserId
                                     select user).FirstOrDefault();
            UsernameInfo.Content = userInformatoin.Username;
            WeightInfo.Content = String.IsNullOrEmpty(userInformatoin.Weight.ToString()) ? "NULL" : userInformatoin.Weight.ToString();
            HeightInfo.Content = String.IsNullOrEmpty(userInformatoin.Height.ToString()) ? "NULL" : userInformatoin.Height.ToString();
            AgeInfo.Content = String.IsNullOrEmpty(userInformatoin.Age.ToString()) ? "NULL" : userInformatoin.Age.ToString();
            ActivityTimeInfo.Content = String.IsNullOrEmpty(userInformatoin.GoingToBed.ToString())
                                        || String.IsNullOrEmpty(userInformatoin.WakeUp.ToString())
                                        ? "NULL" : Math.Abs(userInformatoin.GoingToBed.Value.Hours - userInformatoin.WakeUp.Value.Hours).ToString();


           
            SeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Water",
                    Values = new ChartValues<double> { 1700, 2000, 1500, 1250, 1700, 2000, 1500, 1250, 1700, 2000, 1500, 1250 }
                }
            };

            Labels = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            Formatter = value => value.ToString("N");

            DataContext = this;
            ABBA.Series = SeriesCollection;
            Axisx.Labels = Labels;
            Axisy.LabelFormatter = Formatter;
        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
        }

        private void CartesianChart_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void PeriodLable1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Water",
                    Values = new ChartValues<double> { 1700, 2000, 1500, 1250, 1700, 2000, 1500}
                }
            };

            Labels = new[] { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };
            Formatter = value => value.ToString("N");

            DataContext = this;
            ABBA.Series = SeriesCollection;
            Axisx.Labels = Labels;
            Axisy.LabelFormatter = Formatter;

        }
    }
}

     
    

