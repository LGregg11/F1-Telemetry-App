namespace F1TelemetryApp.View;

using Controls;
using Enums;
using ViewModel;

using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System;
using System.Windows;
using F1TelemetryApp.Misc;

/// <summary>
/// Interaction logic for TelemetryPage.xaml
/// </summary>
public partial class TelemetryPage : Page
{
    private readonly TelemetryPageViewModel vm;
    private readonly Dictionary<GraphDataType, CheckBox> graphTypeCheckboxMap = new();
    private Dictionary<CheckBox, TelemetryGraph> graphMap = new();
    private Dictionary<string, CheckBox> driverCheckboxMap = new();


    public TelemetryPage()
    {
        InitializeComponent();
        vm = (TelemetryPageViewModel)DataContext;

        graphTypeCheckboxMap = new Dictionary<GraphDataType, CheckBox> {
            { GraphDataType.Throttle, ThrottleCheckbox },
            { GraphDataType.Brake, BrakeCheckbox },
            { GraphDataType.Gear, GearCheckbox },
            { GraphDataType.Speed, SpeedCheckbox },
            { GraphDataType.Steer, SteerCheckbox },
        };

        foreach (var key in graphTypeCheckboxMap.Keys)
        {
            if (!vm.DataTypeSeriesCollectionMap.ContainsKey(key))
                graphTypeCheckboxMap[key].Visibility = Visibility.Hidden;
        }

        UpdateGraphMap();
        UpdateGrid();

        vm.LapUpdated += OnLapUpdated;
        vm.DriverUpdated += OnDriverUpdated;
    }

    private void OnLapUpdated(object? sender, EventArgs e)
    {
        UpdateGraphMap();
        UpdateGrid();
    }

    private void OnDriverUpdated(object? sender, EventArgs e)
    {
        var collections = vm.DataTypeSeriesCollectionMap.Values.FirstOrDefault();
        if (collections == null) return;

        int i = -1;
        foreach (var driverSeries in collections.Series)
        {
            i++;
            var name = driverSeries.Title;
            if (string.IsNullOrEmpty(name) || driverCheckboxMap.ContainsKey(name)) continue;

            var driverCheckBox = new CheckBox
            {
                Content = name,
                IsChecked = false,
                Style = this.FindResource("DefaultCheckboxStyle") as Style,
                Foreground = GraphColors.Colors[collections.Series.IndexOf(driverSeries)]
            };
            driverCheckBox.Click += DriverCheckBoxClick;

            if (i == vm.MyCarIndex)
            {
                driverCheckBox.IsChecked = true;
                driverCheckBox.Content = name + " (You)";
                DriversStackPanel.Children.Insert(0, driverCheckBox);
            }
            else
            {
                DriversStackPanel.Children.Add(driverCheckBox);
            }

            driverCheckboxMap.Add(name, driverCheckBox);
            UpdateVisibleDriverSeries(name);
        }

    }

    private TelemetryGraph CreateTelemetryGraph(GraphDataType type)
    {
        var vm = (TelemetryPageViewModel)DataContext;

        var graph = new TelemetryGraph
        {
            DataSeries = vm.DataTypeSeriesCollectionMap[type],
        };
        graph.YAxis.MinValue = 0;

        var percentageTypes = new List<GraphDataType> { GraphDataType.Throttle, GraphDataType.Brake };
        if (percentageTypes.Contains(type))
        {
            graph.YAxis.MaxValue = 1;
            graph.YAxis.LabelFormatter = v => v.ToString("P0");
        }
        else if (type == GraphDataType.Steer)
        {
            // Steer is -1 to 1
            graph.YAxis.MinValue = -1;
            graph.YAxis.MaxValue = 1;
        }
        else if (type == GraphDataType.Gear)
        {
            graph.YAxis.MinValue = -1;
            graph.YAxis.MaxValue = 8;
        }

        return graph;
    }

    private void UpdateGraphMap()
    {
        var vm = (TelemetryPageViewModel)DataContext;
        graphMap = new();

        foreach (var key in graphTypeCheckboxMap.Keys)
        {
            if (vm.DataTypeSeriesCollectionMap.ContainsKey(key))
                graphMap.Add(graphTypeCheckboxMap[key], CreateTelemetryGraph(key));
        }
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

    private void UpdateVisibleDriverSeries(string name)
    {
        var vm = (TelemetryPageViewModel)DataContext;
        if (!driverCheckboxMap.Keys.Contains(name)) return;

        var visible = (bool)driverCheckboxMap[name].IsChecked!;
        vm.DriverCollection.UpdateDriverVisibility(name, visible);
    }

    private void DataGraphCheckBoxClick(object sender, RoutedEventArgs e)
    {
        UpdateGrid();
    }

    private void DriverCheckBoxClick(object sender, RoutedEventArgs e)
    {
        UpdateVisibleDriverSeries((string)((CheckBox)sender).Content);
    }

    private void NewLapButton_Click(object sender, RoutedEventArgs e)
    {
        vm.DebugNewLap();
    }

    private void AddDrivers_Click(object sender, RoutedEventArgs e)
    {
        vm.DebugAddDrivers();
    }

    private void LapsComboBox_LostFocus(object sender, RoutedEventArgs e)
    {
        vm.RedrawLaps();
    }
}
