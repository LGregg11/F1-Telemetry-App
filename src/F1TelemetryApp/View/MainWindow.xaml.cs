namespace F1TelemetryApp.View;

using ViewModel;

using System.Windows;
using System;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private MainWindowViewModel _viewModel;

    public MainWindow()
    {
        InitializeComponent();
        _viewModel = (MainWindowViewModel)DataContext;
    }

    public void StartTelemetryFeed(object sender, RoutedEventArgs e)
    {
        _viewModel.StartTelemetryFeed();
        UpdateTelemetryFeedBtn();
    }

    public void StopTelemetryFeed(object sender, RoutedEventArgs e)
    {
        _viewModel.StopTelemetryFeed();
        UpdateTelemetryFeedBtn();
    }

    public void ImportTelemetry(object sender, RoutedEventArgs e)
    {
        _viewModel.ImportTelemetry();
    }

    public void OpenTestInputsWindow_Click(object sender, RoutedEventArgs e)
    {
        TestWindow w = new();
        ((TestWindowViewModel)w.DataContext).MainWindowViewModel = _viewModel; // Do I need this?
        w.Show();
    }

    public void OpenTelemetryWindow_Click(object sender, RoutedEventArgs e)
    {
        LapTelemetryWindow w = new();
        ((LapTelemetryWindowViewModel)w.DataContext).MainWindowViewModel = _viewModel; // Do I need this?
        w.Show();
    }

    public void OpenTimesheetWindow_Click(object sender, RoutedEventArgs e)
    {
        TimesheetWindow w = new();
        ((TimesheetWindowViewModel)w.DataContext).MainWindowViewModel = _viewModel; // Do I need this?
        w.Show();
    }

    private void UpdateTelemetryFeedBtn()
    {
        if (MainWindowViewModel.IsListenerRunning)
        {
            TelemetryFeedBtn.Click -= StartTelemetryFeed;
            TelemetryFeedBtn.Click += StopTelemetryFeed;
        }
        else
        {
            TelemetryFeedBtn.Click -= StopTelemetryFeed;
            TelemetryFeedBtn.Click += StartTelemetryFeed;
        }
    }

    private void OnWindowClosed(object sender, System.EventArgs e)
    {
        _viewModel.StopTelemetryFeed();
        Environment.Exit(0);
    }
}
