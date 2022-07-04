namespace F1TelemetryApp.View;

using Controls;
using Enums;
using ViewModel;

using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

/// <summary>
/// Interaction logic for TelemetryPage.xaml
/// </summary>
public partial class TelemetryPage : Page
{
    private TelemetryPageViewModel _vm;
    private Dictionary<CheckBox, TelemetryGraph> graphMap = new();
    private Dictionary<DataGraphType, CheckBox> checkboxMap = new();


    public TelemetryPage()
    {
        InitializeComponent();
        _vm = (TelemetryPageViewModel)DataContext;

        checkboxMap = new Dictionary<DataGraphType, CheckBox> {
            { DataGraphType.Throttle, ThrottleCheckbox },
            { DataGraphType.Brake, BrakeCheckbox },
            { DataGraphType.Gear, GearCheckbox },
            { DataGraphType.Speed, SpeedCheckbox },
            { DataGraphType.Steer, SteerCheckbox },
        };

        foreach (var key in checkboxMap.Keys)
        {
            if (!_vm.GraphPointCollections.ContainsKey(key))
                checkboxMap[key].Visibility = System.Windows.Visibility.Hidden;
        }

        UpdateGraphMap();
        UpdateGrid();
    }

    private void UpdateGraphMap()
    {
        var vm = (TelemetryPageViewModel)DataContext;
        graphMap = new();

        foreach (var key in checkboxMap.Keys)
        {
            if (vm.GraphPointCollections.ContainsKey(key))
                graphMap.Add(checkboxMap[key], CreateTelemetryGraph(key));
        }
    }

    private TelemetryGraph CreateTelemetryGraph(DataGraphType type)
    {
        var vm = (TelemetryPageViewModel)DataContext;

        var graph = new TelemetryGraph
        {
            DataSeries = vm.GraphPointCollections[type],
        };
        graph.YAxis.MinValue = 0;

        var percentageTypes = new List<DataGraphType> { DataGraphType.Throttle, DataGraphType.Brake };
        if (percentageTypes.Contains(type))
        {
            graph.YAxis.MaxValue = 1;
            graph.YAxis.LabelFormatter = v => v.ToString("P0");
        }
        else if (type == DataGraphType.Steer)
        {
            // Steer is -1 to 1
            graph.YAxis.MinValue = -1;
            graph.YAxis.MaxValue = 1;
        }
        else if (type == DataGraphType.Gear)
        {
            graph.YAxis.MaxValue = 8;
        }

        return graph;
    }

    private void DataGraphCheckBoxClick(object sender, System.Windows.RoutedEventArgs e)
    {
        UpdateGrid();
    }

    private void UpdateGrid()
    {
        var checkBoxes = graphMap.Keys.ToList().Where(c => c.IsChecked != null && (bool)c.IsChecked!);
        DataGraphGrid.Children.Clear();
        DataGraphGrid.RowDefinitions.Clear();
        int i = 0;
        foreach (var checkBox in checkBoxes)
        {
            DataGraphGrid.RowDefinitions.Add(new RowDefinition());
            DataGraphGrid.Children.Add(graphMap[checkBox]);
            Grid.SetRow(graphMap[checkBox], i);
            i++;
        }
    }
}
