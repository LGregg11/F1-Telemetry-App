namespace F1TelemetryApp.ViewModel;

using F1GameTelemetry.Converters;
using F1GameTelemetry.Enums;
using F1GameTelemetry.Events;
using F1GameTelemetry.Exporter;
using F1GameTelemetry.Listener;
using F1GameTelemetry.Models;
using F1GameTelemetry.Readers;

using log4net;
using Prism.Mvvm;

using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;

public class MainWindowViewModel : BindableBase
{
    private static readonly List<ITelemetryConverter> _converters = new()
    {
        new TelemetryConverter2021(),
        new TelemetryConverter2022(),
        new TelemetryConverter2023()
    };

    private const int _port = 20777;
    private bool _isSubscribedToReader = false;

    public MainWindowViewModel()
    {
        log4net.Config.XmlConfigurator.Configure();
        Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);
        Version = GameVersion.F12021;
        SingletonTelemetryReader.SetTelemetryListener(new TelemetryListener(_port));
        UpdateTelemetryConverter();
    }

    public ILog Log { get; set; }

    public static bool IsListenerRunning => SingletonTelemetryReader.IsListenerRunning;

    public static List<GameVersion> Versions => GetConverterVersions();
    private static List<GameVersion> GetConverterVersions()
    {
        var versions = new List<GameVersion>();
        foreach (var converter in _converters)
            versions.Add(converter.GameVersion);

        return versions;
    }


    private GameVersion _version;
    public GameVersion Version
    {
        get => _version;

        set
        {
            if (_version != value)
            {
                _version = value;
                RaisePropertyChanged(nameof(Version));
                UpdateTelemetryConverter();
            }
        }
    }

    public bool IsExportCheckboxEnabled => !SingletonTelemetryReader.IsListenerRunning && !_isImportCheckboxChecked;

    private bool _isExportCheckboxChecked;
    public bool IsExportCheckboxChecked
    {
        get => _isExportCheckboxChecked;

        set
        {
            if (IsExportCheckboxChecked != value)
            {
                _isExportCheckboxChecked = value;
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

    public bool IsImportCheckboxEnabled => !SingletonTelemetryReader.IsListenerRunning && !_isExportCheckboxChecked;

    private bool _isImportCheckboxChecked;
    public bool IsImportCheckboxChecked
    {
        get => _isImportCheckboxChecked;

        set
        {
            if (_isImportCheckboxChecked != value)
            {
                _isImportCheckboxChecked = value;
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

    private string _importTelemetryFilepath;
    public string ImportTelemetryFilepath
    {
        get => _importTelemetryFilepath;

        set
        {
            if (ImportTelemetryFilepath != value)
            {
                _importTelemetryFilepath = value;
                RaisePropertyChanged(nameof(ImportTelemetryFilepath));
                RaisePropertyChanged(nameof(IsSessionBtnEnabled));
            }
        }
    }

    private string _warningMessage;
    public string WarningMessage
    {
        get => _warningMessage;
        set
        {
            if (_warningMessage != value)
            {
                _warningMessage = value;
                RaisePropertyChanged(nameof(WarningMessage));
                RaisePropertyChanged(nameof(WarningMessageVisibility));
                RaisePropertyChanged(nameof(IsSessionBtnEnabled));
            }
        }
    }

    public Visibility WarningMessageVisibility => !string.IsNullOrEmpty(_warningMessage) ? Visibility.Visible : Visibility.Hidden;

    public bool IsSessionBtnEnabled => string.IsNullOrEmpty(_warningMessage);

    public string TrackName { get; private set; }
    public string WeatherStatus { get; private set; }
    public string TrackTemperature { get; private set; }
    public string AirTemperature { get; private set; }

    public void UpdateTelemetryConverter()
    {
        ITelemetryConverter? newConverter = null;
        foreach (var converter in _converters)
        {
            if (converter.GameVersion == Version)
                newConverter = converter;
        }

        if (_isSubscribedToReader) UnSubscribeFromCurrentConverter();
        SingletonTelemetryReader.SetTelemetryConverterByVersion(newConverter!);
        if (!_isSubscribedToReader) SubscribeToCurrentConverter();

        Log.Info($"Updated converter version to {Enum.GetName(Version)}");
        CheckWarnings();
    }

    private void SubscribeToCurrentConverter()
    {
        SingletonTelemetryReader.SessionReceived += OnSessionReceived;
        _isSubscribedToReader = true;
    }

    private void UnSubscribeFromCurrentConverter()
    {
        SingletonTelemetryReader.SessionReceived -= OnSessionReceived;
        _isSubscribedToReader = false;
    }

    private void CheckWarnings()
    {
        var warningMessage = string.Empty;
        if (IsImportCheckboxChecked && string.IsNullOrEmpty(_importTelemetryFilepath))
            warningMessage = $"No import file selected";

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
