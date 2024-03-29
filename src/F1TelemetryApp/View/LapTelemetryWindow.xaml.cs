﻿namespace F1TelemetryApp.View;

using Controls;
using Enums;
using ViewModel;

using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System;
using System.Windows;

/// <summary>
/// Interaction logic for TelemetryPage.xaml
/// </summary>
public partial class LapTelemetryWindow : Window
{
    private readonly LapTelemetryWindowViewModel _viewModel;
    private readonly Dictionary<DataGraphType, CheckBox> _checkboxMap = new();
    private Dictionary<CheckBox, TelemetryGraph> _graphMap = new();


    public LapTelemetryWindow()
    {
        InitializeComponent();
        _viewModel = (LapTelemetryWindowViewModel)DataContext;

        _checkboxMap = new Dictionary<DataGraphType, CheckBox> {
            { DataGraphType.Throttle, ThrottleCheckbox },
            { DataGraphType.Brake, BrakeCheckbox },
            { DataGraphType.Gear, GearCheckbox },
            { DataGraphType.Speed, SpeedCheckbox },
            { DataGraphType.Steer, SteerCheckbox },
        };

        foreach (var key in _checkboxMap.Keys)
        {
            //if (!_viewModel.GraphPointCollectionMap.ContainsKey(key))
            //    _checkboxMap[key].Visibility = System.Windows.Visibility.Hidden;
        }

        UpdateGraphMap();
        UpdateGrid();

        //_viewModel.LapUpdated += OnLapUpdated;
    }

    private void OnLapUpdated(object? sender, EventArgs e)
    {
        UpdateGraphMap();
        UpdateGrid();
    }

    private void UpdateGraphMap()
    {
        var vm = (LapTelemetryWindowViewModel)DataContext;
        _graphMap = new();

        foreach (var key in _checkboxMap.Keys)
        {
            //if (vm.GraphPointCollectionMap.ContainsKey(key))
            //    _graphMap.Add(_checkboxMap[key], CreateTelemetryGraph(key));
        }
    }

    private TelemetryGraph CreateTelemetryGraph(DataGraphType type)
    {
        var vm = (LapTelemetryWindowViewModel)DataContext;

        var graph = new TelemetryGraph
        {
            //DataSeries = vm.GraphPointCollectionMap[type],
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
        var checkBoxes = _graphMap.Keys.ToList().Where(c => c.IsChecked != null && (bool)c.IsChecked!);
        DataGraphGrid.Children.Clear();
        DataGraphGrid.RowDefinitions.Clear();
        int i = 0;
        foreach (var checkBox in checkBoxes)
        {
            DataGraphGrid.RowDefinitions.Add(new RowDefinition());
            DataGraphGrid.Children.Add(_graphMap[checkBox]);
            Grid.SetRow(_graphMap[checkBox], i);
            i++;
        }
    }

    private void NewLapButton_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        //_viewModel.DebugNewLap();
    }

    private void LapsComboBox_LostFocus(object sender, System.Windows.RoutedEventArgs e)
    {
        //_viewModel.RedrawLaps();
    }
}
