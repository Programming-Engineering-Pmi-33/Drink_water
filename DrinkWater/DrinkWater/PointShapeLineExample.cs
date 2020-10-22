using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Windows;

namespace DrinkWater
{
    public partial class BasicColumn : Window
    {
        public BasicColumn()
        {

            SeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Water",
                    Values = new ChartValues<double> { 10, 50, 39, 50 }
                }
            };

            //adding series will update and animate the chart automatically
            SeriesCollection.Add(new ColumnSeries
            {
                Title = "Vodka",
                Values = new ChartValues<double> { 11, 56, 42, 15 }
            });

            //also adding values updates and animates the chart automatically
            SeriesCollection[1].Values.Add(48d);

            Labels = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            Formatter = value => value.ToString("N");

            DataContext = this;
        }

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }


    }
}
