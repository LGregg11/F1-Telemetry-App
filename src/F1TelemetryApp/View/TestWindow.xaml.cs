namespace F1TelemetryApp.View;

using System.Windows;
using ViewModel;

/// <summary>
/// Interaction logic for TelemetryWindow.xaml
/// </summary>
public partial class TestWindow : Window
{
    private TestWindowViewModel _viewModel;

    public TestWindow()
    {
        InitializeComponent();
        _viewModel = (TestWindowViewModel)DataContext;
    }
}
