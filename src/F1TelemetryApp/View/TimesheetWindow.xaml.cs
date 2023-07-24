namespace F1TelemetryApp.View;

using ViewModel;

using System.Windows;
using System.Threading;
using System.ComponentModel;
using System;

/// <summary>
/// Interaction logic for TimesheetWindow.xaml
/// </summary>
public partial class TimesheetWindow : Window
{
    private TimesheetWindowViewModel _viewModel;
    private bool _isThreadAlive;
    private readonly Thread _uiThread;
    private readonly int _sleepTimeMs = 5000;

    public TimesheetWindow()
    {
        InitializeComponent();
        _viewModel = (TimesheetWindowViewModel)DataContext;

        _isThreadAlive = true;
        _uiThread = new Thread(PositionOrderUpdateLoop);
        _uiThread.Start();
    }

    private void OnWindowClosed(object sender, EventArgs e)
    {
        _isThreadAlive = false;
        lock (this)
        {
            Monitor.Pulse(this);
        }
        _uiThread.Join();
    }

    private void PositionOrderUpdateLoop()
    {
        while (_isThreadAlive)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                TimesheetDataGrid.Items.SortDescriptions.Add(new SortDescription("Position", ListSortDirection.Ascending));
            });

            lock(this)
            {
                Monitor.Wait(this, TimeSpan.FromMilliseconds(_sleepTimeMs));
            }
        }
    }
}
