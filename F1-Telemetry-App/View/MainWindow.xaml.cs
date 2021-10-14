namespace F1_Telemetry_App.View
{
    using System.Windows;
    using F1_Telemetry_App.ViewModel;

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
        }

        public void StopTelemetryFeed(object sender, RoutedEventArgs e)
        {
            viewModel.StopTelemetryFeed();
        }
    }
}
