namespace F1TelemetryApp.View;

using ViewModel;

using System.Windows.Controls;

/// <summary>
/// Interaction logic for HomePage.xaml
/// </summary>
public partial class HomePage : Page
{
    private HomePageViewModel viewModel;
    public HomePage()
    {
        InitializeComponent();
        viewModel = (HomePageViewModel)DataContext;
    }
}
