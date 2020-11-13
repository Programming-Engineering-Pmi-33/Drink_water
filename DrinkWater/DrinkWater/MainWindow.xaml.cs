using DrinkWater.LogReg;
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
        public MainService main=new MainService();
        public SeriesCollection SeriesCollection { get; set; }
        public MainWindow()
        {
            InitializeComponent();
        }
       
        private void Add(object sender, RoutedEventArgs e)
        {
            main.Add();
        }

        private void Up_Click(object sender, RoutedEventArgs e)
        {
            int k = main.Fluids.Count-1;
            for (int i = 3; i >=0; i--)
            {
                if(i-1< 0)
                {
                    main.LabelBox[i].Key.Content = main.Fluids[k].Name;
                    main.PictureBox[i].Source = main.Images[k].Source;
                    k--;
                }
                else
                {
                    main.LabelBox[i].Key.Content = main.Fluids[i-1].Name;
                    main.PictureBox[i].Source = main.Images[i-1].Source;
                }
                main.LabelBox[i].Value.Text = "";

            }
        }

        private void Down_Click(object sender, RoutedEventArgs e)
        {
            int k = main.Fluids.Count - 1;
            for (int i = 0; i < 4; i++)
            {
                if (i + 1 > main.Fluids.Count )
                {
                    main.LabelBox[i].Key.Content = main.Fluids[k].Name;
                    main.PictureBox[i].Source = main.Images[k].Source;
                    k--;
                }
                else
                {
                    main.LabelBox[i].Key.Content = main.Fluids[i + 1].Name;
                    main.PictureBox[i].Source = main.Images[i+1].Source;
                }
                main.LabelBox[i].Value.Text = "";


            }
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            SeriesCollection = new SeriesCollection();
            main.LabelBox.Add(new KeyValuePair<Label, TextBox>(TypeOfLiquid1, Amount1));
            main.LabelBox.Add(new KeyValuePair<Label, TextBox>(TypeOfLiquid2, Amount2));
            main.LabelBox.Add(new KeyValuePair<Label, TextBox>(TypeOfLiquid3, Amount3));
            main.LabelBox.Add(new KeyValuePair<Label, TextBox>(TypeOfLiquid4, Amount4));

            main.PictureBox.Add(ImageBox1);
            main.PictureBox.Add(ImageBox2);
            main.PictureBox.Add(ImageBox3);
            main.PictureBox.Add(ImageBox4);

            main.ListLiquids();
            main.ShowStatistic(this.SeriesCollection);
            Row.Width = 100;
            DataContext = this;

            BalanceLine.Value = main.balance;
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
