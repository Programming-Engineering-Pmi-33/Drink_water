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

        static public List<Statistics> Statistic = new List<Statistics>();
        static private SessionUser sessionUser = new SessionUser();
        static private dfkg9ojh16b4rdContext db = new dfkg9ojh16b4rdContext();
        public List<Fluids> Fluids=new List<Fluids>();
        public List<KeyValuePair<Label, TextBox>> LabelBox = new List<KeyValuePair<Label, TextBox>>();
        public MainWindow()
        {
            InitializeComponent();

            SessionUser sessionUser = new SessionUser(1, "Mamonchik");
            GetSessionUser(sessionUser);
            ListLiquids();
            ShowStatistic();

        }
        public void GetSessionUser(SessionUser user)
        {
            sessionUser = user;
            Statistic = (from searchingUser in db.Statistics
                         where searchingUser.UserIdRef == sessionUser.UserId
                         select searchingUser).ToList();
        }
        private void ListLiquids()
        {
            LabelBox.Add(new KeyValuePair<Label, TextBox>(Label1, TextBox1));
            LabelBox.Add(new KeyValuePair<Label, TextBox>(Label2, TextBox2));
            LabelBox.Add(new KeyValuePair<Label, TextBox>(Label3, TextBox3));
            LabelBox.Add(new KeyValuePair<Label, TextBox>(Label4, TextBox4));

            List<Fluids> fluids = db.Fluids.ToList();

            foreach (var item in fluids)
            {
                Fluids.Add(item);
            }
            
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
                if ( item.Date.Date == DateTime.Now.Date)
                {
                    SeriesCollection.Add(new StackedColumnSeries
                    {
                        Title = (from f in Fluids
                                 where ((f.FluidId == item.FluidIdRef))
                                 select f.Name).FirstOrDefault().ToString(),
                        Values = new ChartValues<double> { Double.Parse(item.FluidAmount.ToString()) },
                        StackMode = StackMode.Values,
                        DataLabels = true

                    });
                }
                
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
                    Statistics find = Statistic.Find(s => (s.FluidIdRef == fluidId & s.UserIdRef == sessionUser.UserId));

                    if (find != null)
                    {
                        Statistic.Find(s=>s.StatisticId==find.StatisticId).FluidAmount += long.Parse(text);
                        db.Statistics.Update(find);
                        //SeriesCollection.(StackedColumnSeries.)

                    }
                    else
                    {
                        Statistics statistics = new Statistics
                        {
                            UserIdRef = sessionUser.UserId,
                            FluidIdRef = fluidId,
                            FluidAmount = long.Parse(text),
                            Date = DateTime.Now,

                        };
                        Statistic.Add(statistics);
                        SeriesCollection.Add(new StackedColumnSeries
                        {
                            Title = (from f in Fluids
                                     where ((f.FluidId == statistics.FluidIdRef))
                                     select f.Name).FirstOrDefault().ToString(),
                            Values = new ChartValues<double> { Double.Parse(statistics.FluidAmount.ToString()) },
                            StackMode = StackMode.Values,
                            DataLabels = true
                        });
                        db.Statistics.Add(statistics);
                        
                    }
                    db.SaveChanges();
                }
            }
            ShowStatistic();
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

    }
}
