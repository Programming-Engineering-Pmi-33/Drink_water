using DrinkWater.LogReg;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using LiveCharts;
using LiveCharts.Wpf;
using System.IO;
using System.Windows.Media.Imaging;

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
        List<Weekstatistic> weekstatistics;
        List<Monthstatistic> monthstatistics;
        List<Yearstatistic> yearstatistics;
        List<DateTime> week;
        List<DateTime> month;
        List<DateTime> year;
        List<double> weekWaterAmount;
        List<double> monthWaterAmount;
        List<double> yearWaterAmount;
        List<Fluids> fluids;
        int[] fluidAmount;
        int index;


        public ProfileStatistics()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            index = 0;
            
            weekWaterAmount = new List<double>();
            monthWaterAmount = new List<double>();
            yearWaterAmount = new List<double>();
            SeriesCollection = new SeriesCollection();
            
            weekstatistics = (from weekQuery in db.Weekstatistic
                              where SessionUser.UserId == weekQuery.UserIdRef
                              select weekQuery).ToList();
            monthstatistics = (from monthQuery in db.Monthstatistic
                               where SessionUser.UserId == monthQuery.UserIdRef
                               select monthQuery).ToList();
            yearstatistics = (from yearQuery in db.Yearstatistic
                              where SessionUser.UserId == yearQuery.UserIdRef
                              select yearQuery).ToList();
            
            fluids = (from fluid in db.Fluids
                      select fluid).ToList();
           
            SortedSet<DateTime> sortedWeek = new SortedSet<DateTime>();
            SortedSet<DateTime> sortedMonth = new SortedSet<DateTime>();
            SortedSet<DateTime> sortedYear = new SortedSet<DateTime>();
            for(int i =0; i< weekstatistics.Count;i++)
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
            week = new List<DateTime>(sortedWeek);
            month = new List<DateTime>(sortedMonth);
            year = new List<DateTime>(sortedYear);

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

            if (userInformatoin.Avatar != null)
            {
                var memoryStream = new MemoryStream(userInformatoin.Avatar);
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.StreamSource = memoryStream;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                bitmap.Freeze();
                Avatar.Source = bitmap;
            }

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
                        temp += (int)weekstatistics[j].Sum * fluids[(int)(weekstatistics[j].FluidIdRef) - 1].Koeficient;
                }
                weekWaterAmount.Add(temp);
            }
            //SeriesCollection.Clear();
            SeriesCollection.Add(
            new ColumnSeries
            {
                Title = "Water",
                Values = new ChartValues<double>(weekWaterAmount)
            });

            List<string> tempList = new List<string>();
            for (int i = 0; i < week.Count; i++)
            {
                tempList.Add(week[i].DayOfWeek.ToString());
            }
            Axisx.Labels = tempList;
            Formatter = value => value.ToString("N");

            DataContext = this;
            UpperElement1.Content = fluids[index].Name;
            LowerElement1.Content = fluidAmount[index];
            UpperElement2.Content = fluids[index + 1].Name;
            LowerElement2.Content = fluidAmount[index + 1];
        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }

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
                fluidAmount = new int[fluids.Count];
                for (int i = 0; i <fluids.Count;i++)
                {
                    for(int j = 0; j<weekstatistics.Count; j++)
                    {
                        if(fluids[i].FluidId == weekstatistics[j].FluidIdRef)
                        {
                            fluidAmount[i] += (int)weekstatistics[j].Sum;
                        }
                    }
                }
                for(int i =0; i < week.Count;i++)
                {
                    double temp = 0;
                   for(int j =0; j< weekstatistics.Count;j++)
                   {
                        if (week[i].Day == weekstatistics[j].Date.Value.Day)
                            temp += (int)weekstatistics[j].Sum * fluids[(int)(weekstatistics[j].FluidIdRef)-1].Koeficient;
                   }
                    weekWaterAmount.Add(temp);
                }
                SeriesCollection.Clear();
                SeriesCollection.Add(
                new ColumnSeries
                {
                    Title = "Water",
                    Values = new ChartValues<double>(weekWaterAmount)
                }) ;

                List<string> tempList = new List<string>();
                for (int i = 0; i < week.Count; i++)
                {
                    tempList.Add(week[i].DayOfWeek.ToString());
                }
                Axisx.Labels = tempList;

            }
            if (Period.Text == "Per year")
            {
                yearWaterAmount.Clear();
                for (int i = 0; i < year.Count; i++)
                {
                    double temp = 0;
                    for (int j = 0; j < yearstatistics.Count; j++)
                    {
                        if (year[i].Day == yearstatistics[j].Date.Value.Day && year[i].Month == yearstatistics[j].Date.Value.Month)
                            temp += (int)yearstatistics[j].Sum * fluids[(int)(yearstatistics[j].FluidIdRef-1)].Koeficient;
                    }
                    yearWaterAmount.Add(temp);
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
                SeriesCollection.Clear();
                SeriesCollection.Add(
                new ColumnSeries
                {
                    Title = "Water",
                    Values = new ChartValues<double>(yearWaterAmount)
                }) ;
                List<string> tempList = new List<string>();
                for (int i = 0; i < year.Count; i++)
                {
                    tempList.Add(month[i].ToString("MMM"));
                }
                Axisx.Labels = tempList;

            }

            if (Period.Text == "Per month")
            {
                monthWaterAmount.Clear();
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
                for(int i = 0; i < month.Count; i++)
                {
                    double temp = 0;
                    for (int j = 0; j < monthstatistics.Count; j++)
                    {
                        if (month[i].Day == monthstatistics[j].Date.Value.Day && year[i].Month == monthstatistics[j].Date.Value.Month)
                            temp += (int)monthstatistics[j].Sum * fluids[(int)(monthstatistics[j].FluidIdRef-1)].Koeficient;
                    }
                    monthWaterAmount.Add(temp);
                }
                SeriesCollection.Clear();
                SeriesCollection.Add(
                new ColumnSeries
                {
                    Title = "Water",
                    Values = new ChartValues<double> (monthWaterAmount)
                });
                List<string> tempList = new List<string>();
                for(int i =0; i<month.Count;i++)
                {
                    tempList.Add(month[i].Day.ToString());
                }
                Axisx.Labels = tempList;


            }
            Formatter = value => value.ToString("N");

            DataContext = this;

        }

        private void BackwardButton_Click(object sender, RoutedEventArgs e)
        {
            if (index == 0)
            {
                index = fluidAmount.Length - 1;
            }
            UpperElement1.Content = fluids[index-1].Name;
            LowerElement1.Content = fluidAmount[index-1];
            UpperElement2.Content = fluids[index].Name;
            LowerElement2.Content = fluidAmount[index];
            index--;

        }

        private void ForwardButton_Click(object sender, RoutedEventArgs e)
        {
            
            UpperElement1.Content = fluids[index].Name;
            LowerElement1.Content = fluidAmount[index];
            UpperElement2.Content = fluids[index+1].Name;
            LowerElement2.Content = fluidAmount[index+1];
            if (index == fluidAmount.Length - 2)
            {
                index = 0;
            }
            else
            {
                index++;
            }
        }
    }
}

     
    
