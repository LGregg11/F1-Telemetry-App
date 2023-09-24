namespace F1TelemetryApp.View;

using ViewModel;

using System.Windows;

/// <summary>
/// Interaction logic for TimesheetDriverWindow.xaml
/// </summary>
public partial class TimesheetDriverWindow : Window
{
    private TimesheetDriverWindowViewModel _viewModel;
    public TimesheetDriverWindow()
    {
        InitializeComponent();
        _viewModel = (TimesheetDriverWindowViewModel)DataContext;
    }
}
