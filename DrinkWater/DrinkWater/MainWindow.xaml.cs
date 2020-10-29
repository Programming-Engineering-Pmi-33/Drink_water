using DrinkWater.LogReg;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public SeriesCollection SeriesCollection { get; set; }
        public List<string> Labels { get; set; }
        public Func<string,string> Formatter { get; set; }

        static public List<Dailystatistic> Statistic = new List<Dailystatistic>();
        static public List<Statistics> FullStatistics = new List<Statistics>();
        static private SessionUser sessionUser = new SessionUser();
        static private dfkg9ojh16b4rdContext db = new dfkg9ojh16b4rdContext();
        public List<Fluids> Fluids=new List<Fluids>();
        public List<KeyValuePair<Label, TextBox>> LabelBox = new List<KeyValuePair<Label, TextBox>>();
        public MainWindow()
        {
            InitializeComponent();

        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           
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
            LabelBox.Add(new KeyValuePair<Label, TextBox>(Label1, TextBox1));
            LabelBox.Add(new KeyValuePair<Label, TextBox>(Label2, TextBox2));
            LabelBox.Add(new KeyValuePair<Label, TextBox>(Label3, TextBox3));
            LabelBox.Add(new KeyValuePair<Label, TextBox>(Label4, TextBox4));

            Fluids = db.Fluids.ToList();

            for(int i=0; i<4; i++)
            {
                LabelBox[i].Key.Content = Fluids[i].Name;
            }
        }

        public void ShowStatistic()
        {
            SeriesCollection = new SeriesCollection();
            Row.Title = sessionUser.Username.ToString();
            Row.Width = 100;

            foreach (var item in Statistic)
            {
                SeriesCollection.Add(new StackedColumnSeries
                {
                    Title = (from f in Fluids
                                where ((f.FluidId == item.FluidIdRef))
                                select f.Name).FirstOrDefault().ToString(),
                    Values = new ChartValues<double> {Double.Parse(item.Sum.ToString())},
                    StackMode = StackMode.Values,
                    DataLabels = true

                });
            }
            SeriesCollection[0].Values.Add(4d);

            Formatter = value => value + "ml";
            DataContext = this;

        }
        private void Add(object sender, RoutedEventArgs e)
        {

            foreach (var item in LabelBox)
            {
                string text = item.Value.Text;
                long fluidId= (from fl in Fluids
                              where fl.Name == item.Key.Content.ToString()
                              select fl).First().FluidId;
                if (!string.IsNullOrEmpty(item.Value.Text))
                {
                    Statistics find = FullStatistics.Find(s => (s.FluidIdRef == fluidId & s.UserIdRef == sessionUser.UserId & s.Date.Day == DateTime.Now.Day));

                    if (find != null)
                    {
                        FullStatistics.Find(s=>s.StatisticId==find.StatisticId).FluidAmount += long.Parse(text);
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
                                     where ((f.FluidId == dailystatistic.FluidIdRef))
                                     select f.Name).FirstOrDefault().ToString(),
                            Values = new ChartValues<double> { Double.Parse(dailystatistic.Sum.ToString()) },
                            StackMode = StackMode.Values,
                            DataLabels = true
                        });
                    }
                }
            }
            //ShowStatistic();
        }

        private void Up_Click(object sender, RoutedEventArgs e)
        {
            int k = Fluids.Count-1;
            for (int i = 3; i >=0; i--)
            {
                if(i-1< 0)
                {
                    LabelBox[i].Key.Content = Fluids[k].Name;
                    LabelBox[i].Value.Text = "";
                    k--;
                }
                else
                {
                    LabelBox[i].Key.Content = Fluids[i-1].Name;
                }
                
            }
        }

        private void Down_Click(object sender, RoutedEventArgs e)
        {
            int k = 0;
            for (int i = 0; i < 4; i++)
            {
                if (i + 1 >= Fluids.Count - 1)
                {
                    LabelBox[i].Key.Content = Fluids[k].Name;
                    LabelBox[i].Value.Text = "";
                    k++;
                }
                else
                {
                    LabelBox[i].Key.Content = Fluids[i + 1].Name;
                }

            }
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            ListLiquids();
            ShowStatistic();
        }
    }
}
