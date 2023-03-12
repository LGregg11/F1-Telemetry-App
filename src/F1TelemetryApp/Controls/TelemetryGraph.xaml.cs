namespace F1TelemetryApp.Controls;

using Model;

using System;
using System.Windows;
using System.Windows.Controls;
using LiveCharts;

/// <summary>
/// Interaction logic for TelemetryGraphControl.xaml
/// </summary>
public partial class TelemetryGraph : UserControl
{
    public TelemetryGraph()
    {
        InitializeComponent();
    }

    public string DataType { get; set; }
    public double MinY { get; set; }
    public double MaxY { get; set; }
    public Func<double, string> DataFormatter { get; set; }

    public GraphPointCollection DataSeries
    {
        get { return (GraphPointCollection)GetValue(DataSeriesProperty); }
        set { SetValue(DataSeriesProperty, value); }
    }

    public static readonly DependencyProperty DataSeriesProperty =
        DependencyProperty.Register("DataSeries", typeof(GraphPointCollection), typeof(TelemetryGraph), new PropertyMetadata(null));
}