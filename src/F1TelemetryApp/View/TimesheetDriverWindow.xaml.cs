namespace F1TelemetryApp.View;

using ViewModel;

using System.Windows;

/// <summary>
/// Interaction logic for TimesheetDriverWindow.xaml
/// </summary>
public partial class TimesheetDriverWindow : Window
{
    public TimesheetDriverWindow(byte driverIndex=0)
    {
        InitializeComponent();
        ((TimesheetDriverWindowViewModel)DataContext).SubscribeToDriver(driverIndex);
    }
}
