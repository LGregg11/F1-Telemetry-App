namespace F1TelemetryApp.View;

using System.Windows;
using ViewModel;

/// <summary>
/// Interaction logic for TelemetryWindow.xaml
/// </summary>
public partial class TestWindow : Window
{
    private TestWindowViewModel viewModel;

    public TestWindow()
    {
        InitializeComponent();
        viewModel = (TestWindowViewModel)DataContext;
    }
}
