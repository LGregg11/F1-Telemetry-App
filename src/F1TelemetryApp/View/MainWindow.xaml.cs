namespace F1TelemetryApp.View;

using ViewModel;

using System.Windows;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private MainWindowViewModel viewModel;

    public MainWindow()
    {
        InitializeComponent();
        viewModel = (MainWindowViewModel)DataContext;
    }

    public void StartTelemetryFeed(object sender, RoutedEventArgs e)
    {
        viewModel.StartTelemetryFeed();
        UpdateTelemetryFeedBtn();
    }

    public void StopTelemetryFeed(object sender, RoutedEventArgs e)
    {
        viewModel.StopTelemetryFeed();
        UpdateTelemetryFeedBtn();
    }

    public void ImportTelemetry(object sender, RoutedEventArgs e)
    {
        viewModel.ImportTelemetry();
    }

    public void OpenTestInputsWindow_Click(object sender, RoutedEventArgs e)
    {
        TestWindow w = new();
        ((TestWindowViewModel)w.DataContext).MainWindowViewModel = viewModel; // Do I need this?
        w.Show();
    }

    public void OpenTelemetryWindow_Click(object sender, RoutedEventArgs e)
    {
        LapTelemetryWindow w = new();
        ((LapTelemetryWindowViewModel)w.DataContext).MainWindowViewModel = viewModel; // Do I need this?
        w.Show();
    }

    private void UpdateTelemetryFeedBtn()
    {
        if (viewModel.IsListenerRunning)
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
}
