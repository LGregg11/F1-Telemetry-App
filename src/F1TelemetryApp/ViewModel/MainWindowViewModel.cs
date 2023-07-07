namespace F1TelemetryApp.ViewModel;

using F1GameTelemetry.Enums;
using F1GameTelemetry.Exporter;
using F1GameTelemetry.Listener;
using F1GameTelemetry.Readers;

using log4net;
using Prism.Mvvm;

using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Windows;
using F1GameTelemetry.Packets.Standard;
using F1GameTelemetry.Events;

public class MainWindowViewModel : BindableBase
{
    private const int port = 20777;
    private bool isSubscribedToReader = false;

    public MainWindowViewModel()
    {
        log4net.Config.XmlConfigurator.Configure();
        Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);
        Version = GameVersion.F12021;
        SingletonTelemetryReader.SetTelemetryListener(new TelemetryListener(port));
        UpdateTelemetryReader();
    }

    public ILog Log { get; set; }

    public static bool IsListenerRunning => SingletonTelemetryReader.IsListenerRunning;

    public static List<GameVersion> Versions => new((IEnumerable<GameVersion>)Enum.GetValues(typeof(GameVersion)));

    private GameVersion version;
    public GameVersion Version
    {
        get => version;

        set
        {
            if (version != value)
            {
                version = value;
                RaisePropertyChanged(nameof(Version));
                UpdateTelemetryReader();
            }
        }
    }

    public bool IsExportCheckboxEnabled => !SingletonTelemetryReader.IsListenerRunning && !isImportCheckboxChecked;

    private bool isExportCheckboxChecked;
    public bool IsExportCheckboxChecked
    {
        get => isExportCheckboxChecked;

        set
        {
            if (IsExportCheckboxChecked != value)
            {
                isExportCheckboxChecked = value;
                // TODO: Exporting is broken!!
                //TelemetryReader.IsExportEnabled = IsExportCheckboxChecked;
                RaisePropertyChanged(nameof(IsExportCheckboxChecked));
                RaisePropertyChanged(nameof(IsExportCheckboxEnabled));
                RaisePropertyChanged(nameof(IsImportCheckboxChecked));
                RaisePropertyChanged(nameof(IsImportCheckboxEnabled));
                RaisePropertyChanged(nameof(ExportDirectoryVisibility));
            }
        }
    }
    public Visibility ExportDirectoryVisibility => IsExportCheckboxChecked ? Visibility.Visible : Visibility.Hidden;
    public static string ExportDirectory => $"Data will be stored at\n{TelemetryExporter.TELEMETRY_EXPORTER_DIRECTORY}";

    public bool IsImportCheckboxEnabled => !SingletonTelemetryReader.IsListenerRunning && !isExportCheckboxChecked;

    private bool isImportCheckboxChecked;
    public bool IsImportCheckboxChecked
    {
        get => isImportCheckboxChecked;

        set
        {
            if (isImportCheckboxChecked != value)
            {
                isImportCheckboxChecked = value;
                RaisePropertyChanged(nameof(IsImportCheckboxChecked));
                RaisePropertyChanged(nameof(IsImportCheckboxEnabled));
                RaisePropertyChanged(nameof(IsExportCheckboxChecked));
                RaisePropertyChanged(nameof(IsExportCheckboxEnabled));
                RaisePropertyChanged(nameof(IsImportBtnEnabled));
                CheckWarnings();
            }
        }
    }

    public bool IsImportBtnEnabled => !IsListenerRunning && IsImportCheckboxChecked;

    private string importTelemetryFilepath;
    public string ImportTelemetryFilepath
    {
        get => importTelemetryFilepath;

        set
        {
            if (ImportTelemetryFilepath != value)
            {
                importTelemetryFilepath = value;
                RaisePropertyChanged(nameof(ImportTelemetryFilepath));
                RaisePropertyChanged(nameof(IsSessionBtnEnabled));
            }
        }
    }

    private string warningMessage;
    public string WarningMessage
    {
        get => warningMessage;
        set
        {
            if (warningMessage != value)
            {
                warningMessage = value;
                RaisePropertyChanged(nameof(WarningMessage));
                RaisePropertyChanged(nameof(WarningMessageVisibility));
                RaisePropertyChanged(nameof(IsSessionBtnEnabled));
            }
        }
    }

    public Visibility WarningMessageVisibility => !string.IsNullOrEmpty(warningMessage) ? Visibility.Visible : Visibility.Hidden;

    public bool IsSessionBtnEnabled => string.IsNullOrEmpty(warningMessage);

    public string TrackName { get; private set; }
    public string WeatherStatus { get; private set; }
    public string TrackTemperature { get; private set; }
    public string AirTemperature { get; private set; }

    public void UpdateTelemetryReader()
    {
        if (isSubscribedToReader) UnSubscribeFromCurrentReader();
        SingletonTelemetryReader.SetTelemetryConverterByVersion(Version);
        if (!isSubscribedToReader) SubscribeToCurrentReader();
        CheckWarnings();
    }

    private void SubscribeToCurrentReader()
    {
        SingletonTelemetryReader.SessionReceived += OnSessionReceived;
        isSubscribedToReader = true;
    }

    private void UnSubscribeFromCurrentReader()
    {
        SingletonTelemetryReader.SessionReceived -= OnSessionReceived;
        isSubscribedToReader = false;
    }

    private void CheckWarnings()
    {
        var warningMessage = string.Empty;
        if (IsImportCheckboxChecked && string.IsNullOrEmpty(importTelemetryFilepath))
            warningMessage = $"No import file selected";

        // Override any previous warnings - this should take precedence
        if (!SingletonTelemetryReader.IsConverterSupported())
            warningMessage = $"Converter is not supported";

        UpdateWarning(warningMessage);
    }

    private void UpdateWarning(string message = "")
    {
        if (!string.IsNullOrEmpty(message))
            Log?.Warn(message);
        WarningMessage = message;
    }

    public void StartTelemetryFeed()
    {
        if (!SingletonTelemetryReader.IsListenerRunning)
        {
            Log?.Info("Starting Telemetry feed");
            SingletonTelemetryReader.StartListener();
            RaisePropertyChanged(nameof(IsExportCheckboxEnabled));
            RaisePropertyChanged(nameof(IsImportCheckboxEnabled));
            RaisePropertyChanged(nameof(IsImportBtnEnabled));
        }
    }

    public void StopTelemetryFeed()
    {
        if (SingletonTelemetryReader.IsListenerRunning)
        {
            Log?.Info("Stopping Telemetry feed");
            SingletonTelemetryReader.StopListener();
            RaisePropertyChanged(nameof(IsExportCheckboxEnabled));
            RaisePropertyChanged(nameof(IsImportCheckboxEnabled));
            RaisePropertyChanged(nameof(IsImportBtnEnabled));
        }
    }

    public void ImportTelemetry()
    {
        OpenFileDialog dialog = new();
        // Apparently can only be true or false - https://learn.microsoft.com/en-us/dotnet/api/microsoft.win32.commondialog.showdialog
        if (!(bool)dialog.ShowDialog()!)
            return;

        ImportTelemetryFilepath = dialog.FileName;
        SingletonTelemetryReader.SetTelemetryListener(new TelemetryImporter(ImportTelemetryFilepath));
        CheckWarnings();
    }

    private void OnSessionReceived(object? sender, PacketEventArgs<Session> e)
    {
        var session = e.Packet;
        App.Current.Dispatcher.Invoke(() =>
        {
            TrackName = Enum.GetName(typeof(Track), session.trackId)!;
            WeatherStatus = Enum.GetName(typeof(Weather), session.weather)!;
            TrackTemperature = $"{Convert.ToInt16(session.trackTemperature)}";
            AirTemperature = $"{Convert.ToInt16(session.airTemperature)}";
        });
        RaisePropertyChanged(nameof(TrackName));
        RaisePropertyChanged(nameof(WeatherStatus));
        RaisePropertyChanged(nameof(TrackTemperature));
        RaisePropertyChanged(nameof(AirTemperature));
    }
}
