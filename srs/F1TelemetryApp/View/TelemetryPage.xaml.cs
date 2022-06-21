namespace F1TelemetryApp.View;

using System.Windows.Controls;
using ViewModel;

/// <summary>
/// Interaction logic for TelemetryPage.xaml
/// </summary>
public partial class TelemetryPage : Page
{
    private TelemetryPageViewModel viewModel;

    public TelemetryPage()
    {
        InitializeComponent();
        viewModel = (TelemetryPageViewModel)DataContext;
    }
}
