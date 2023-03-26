namespace F1TelemetryApp.ViewModel;

using Converters;

using F1GameTelemetry.Enums;
using F1GameTelemetry.Exporter;
using F1GameTelemetry.Listener;
using F1GameTelemetry.Readers;

using log4net;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Windows;
using Microsoft.Win32;

public class MainWindowViewModel : BindableBase
{
    private const int port = 20777;

    private readonly TelemetryReaderFactory readerFactory;
    private ITelemetryListener telemetryListener;

    public MainWindowViewModel()
    {
        log4net.Config.XmlConfigurator.Configure();
        Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        telemetryListener = new TelemetryListener(port);
        readerFactory = new TelemetryReaderFactory(telemetryListener);
        Version = GameVersion.F12021;
        UpdateTelemetryReader();
    }

    public event EventHandler? VersionUpdated;

    public ILog Log { get; set; }
    public ITelemetryReader TelemetryReader { get; set; }
    public bool IsListenerRunning => telemetryListener.IsListenerRunning;

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

    public bool IsExportCheckboxEnabled => !telemetryListener.IsListenerRunning && !isImportCheckboxChecked;

    private bool isExportCheckboxChecked;
    public bool IsExportCheckboxChecked
    {
        get => isExportCheckboxChecked;

        set
        {
            if (IsExportCheckboxChecked != value)
            {
                isExportCheckboxChecked = value;
                TelemetryReader.IsExportEnabled = IsExportCheckboxChecked;
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

    public bool IsImportCheckboxEnabled => !telemetryListener.IsListenerRunning && !isExportCheckboxChecked;

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

    public void UpdateTelemetryReader()
    {
        TelemetryReader = readerFactory.GetTelemetryReader(Version)!;
        VersionUpdated?.Invoke(this, new EventArgs());
        CheckWarnings();
    }

    private void CheckWarnings()
    {
        if (TelemetryReader == null)
        {
            UpdateWarning($"{EnumConverter.GetEnumDescription(version)} reader not found");
            return;
        }

        var warningMessage = string.Empty;
        if (IsImportCheckboxChecked && string.IsNullOrEmpty(importTelemetryFilepath))
            warningMessage = $"No import file selected";

        // Override any previous warnings - this should take precedence
        if (!TelemetryReader.IsSupported)
            warningMessage = $"{TelemetryReader.Name} is not supported";

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
        if (!telemetryListener.IsListenerRunning)
        {
            Log?.Info("Starting Telemetry feed");
            telemetryListener.Start();
            RaisePropertyChanged(nameof(IsExportCheckboxEnabled));
            RaisePropertyChanged(nameof(IsImportCheckboxEnabled));
            RaisePropertyChanged(nameof(IsImportBtnEnabled));
        }
    }

    public void StopTelemetryFeed()
    {
        if (telemetryListener.IsListenerRunning)
        {
            Log?.Info("Stopping Telemetry feed");
            telemetryListener.Stop();
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
        telemetryListener = new TelemetryImporter(ImportTelemetryFilepath);
        var readerFactory = new TelemetryReaderFactory(telemetryListener);
        TelemetryReader = readerFactory.GetTelemetryReader(Version)!;
        RaisePropertyChanged(nameof(TelemetryReader));
        CheckWarnings();
    }
}
