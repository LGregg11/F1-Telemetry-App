namespace F1TelemetryApp.View;

using Controls;
using Interfaces;
using ViewModel;

using System.Windows;
using System.Windows.Controls;
using System;
using System.Windows.Navigation;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>

public partial class MainWindow : Window
{
    private const string PACK_URI = "pack://application:,,,/F1TelemetryApp;component";
    private MainWindowViewModel viewModel;

    public MainWindow()
    {
        InitializeComponent();
        viewModel = (MainWindowViewModel)DataContext;
        NavFrame.Navigated += OnNavigate;
        NavFrame.Navigate(new Uri($"{PACK_URI}/View/HomePage.xaml", UriKind.Absolute));
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

    private void NavBar_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selected = NavBar.SelectedItem as NavigationButton;
        NavFrame.Navigate(selected!.NavigationLink);
    }

    private void OnNavigate(object sender, NavigationEventArgs e)
    {
        var content = (Page)NavFrame.Content;
        ((IPageViewModel)content.DataContext).MainWindowViewModel = viewModel;
    }
}
