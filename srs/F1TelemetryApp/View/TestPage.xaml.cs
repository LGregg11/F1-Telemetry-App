namespace F1TelemetryApp.View;

using System.Windows.Controls;
using ViewModel;

/// <summary>
/// Interaction logic for TelemetryPage.xaml
/// </summary>
public partial class TestPage : Page
{
    private TestPageViewModel viewModel;

    public TestPage()
    {
        InitializeComponent();
        viewModel = (TestPageViewModel)DataContext;
    }
}
