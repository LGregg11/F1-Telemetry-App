namespace F1TelemetryApp.Controls;

using System.Windows;
using System.Windows.Controls;

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

    public string DataValue
    {
        get { return (string)GetValue(DataValueProperty); }
        set { SetValue(DataValueProperty, value); }
    }

    public static readonly DependencyProperty DataValueProperty =
        DependencyProperty.Register("DataValue", typeof(string), typeof(TelemetryGraph), new PropertyMetadata(null));

}
