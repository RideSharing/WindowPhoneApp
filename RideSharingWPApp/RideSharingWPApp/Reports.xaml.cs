using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;

namespace RideSharingWPApp
{
    public partial class Reports : PhoneApplicationPage
    {
        public ObservableCollection<PChart> Data = new ObservableCollection<PChart>() {
            new PChart() { label = "Label 1", val = 30 },
            new PChart() { label = "Label 2", val = 40 },
            new PChart() { label = "Label 3", val = 50 },
            new PChart() { label = "Label 4", val = 60 },
            new PChart() { label = "Label 5", val = 70 },
        };


        private ObservableCollection<LineChart> _data = new ObservableCollection<LineChart>() {
            new LineChart() { label = "L1", val1=5, val2=15, val3=12},
            new LineChart() { label = "L2", val1=15.2, val2=1.5, val3=2.1M},
            new LineChart() { label = "L3", val1=25, val2=5, val3=2},
            new LineChart() { label = "L4", val1=8.1, val2=1, val3=22},
            new LineChart() { label = "L5", val1=8.1, val2=1, val3=22},
            new LineChart() { label = "L6", val1=8.1, val2=1, val3=22},
            new LineChart() { label = "L7", val1=4.1, val2=4, val3=2},
            new LineChart() { label = "L8", val1=8.1, val2=1, val3=22},
            new LineChart() { label = "L9", val1=8.1, val2=29, val3=22},
        };
        public Reports()
        {
            InitializeComponent();

            //chartDemo.DataContext = new ViewModel();
            //PC1.DataSource = Data;
            chart1.DataSource = _data;
            Uri uri = new Uri("https://chart.googleapis.com/chart?chs=250x100&chd=t:60,40&cht=p3&chl=Hello|World", UriKind.Absolute);
            imgChart.Source = new BitmapImage(uri);
            //chart1.cate
        }
    }

    public class PChart
    {
        public string label { get; set; }
        public double val { get; set; }
    }

    public class LineChart
    {
        public string label { get; set; }
        public double val1 { get; set; }
        public double val2 { get; set; }
        public decimal val3 { get; set; }
    }

    public class Model
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Model(double x, double y)
        {
            X = x;
            Y = y;
        }
    }

    // Create a ViewModel
    public class ViewModel
    {
        public ObservableCollection<Model> Collection { get; set; }
        public ViewModel()
        {
            Collection = new ObservableCollection<Model>();
            GenerateDatas();
        }
        private void GenerateDatas()
        {
            this.Collection.Add(new Model(0, 1));
            this.Collection.Add(new Model(1, 2));
            this.Collection.Add(new Model(2, 3));
            this.Collection.Add(new Model(3, 3));
        }
    }

}