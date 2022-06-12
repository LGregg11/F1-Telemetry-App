namespace F1TelemetryApp.View
{
    using System.Windows;
    using F1TelemetryApp.ViewModel;

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

        private void UpdateTelemetryFeedBtn()
        {
            if (_viewModel.IsListenerRunning)
            {
                TelemetryFeedBtn.Click -= StartTelemetryFeed;
                TelemetryFeedBtn.Click += StopTelemetryFeed;
                TelemetryFeedBtn.Content = "Stop Telemetry Feed";
            }
            else
            {
                TelemetryFeedBtn.Click -= StopTelemetryFeed;
                TelemetryFeedBtn.Click += StartTelemetryFeed;
                TelemetryFeedBtn.Content = "Start Telemetry Feed";
            }
        }
    }
}
