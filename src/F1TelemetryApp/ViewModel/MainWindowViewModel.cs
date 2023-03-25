namespace F1TelemetryApp.ViewModel;

using Converters;

using F1GameTelemetry.Listener;
using F1GameTelemetry.Enums;
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

    private bool isExportEnabled;
    public bool IsExportEnabled
    {
        get => isExportEnabled;

        set
        {
            if (isExportEnabled != value)
            {
                isExportEnabled = value;
                RaisePropertyChanged(nameof(IsExportEnabled));
                TelemetryReader.IsExportEnabled = isExportEnabled;
            }
        }
    }

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
            }
        }
    }

    public ITelemetryReader TelemetryReader { get; set; }

    private string readerWarningMessage = "No reader specified";
    public string ReaderWarningMessage
    {
        get => readerWarningMessage;
        set
        {
            if (readerWarningMessage != value)
            {
                readerWarningMessage = value;
                RaisePropertyChanged(nameof(ReaderWarningMessage));
            }
        }
    }

    private Visibility readerWarningMessageVisibility = Visibility.Hidden;
    public Visibility ReaderWarningMessageVisibility
    {
        get => readerWarningMessageVisibility;
        set
        {
            if (readerWarningMessageVisibility != value)
            {
                readerWarningMessageVisibility = value;
                RaisePropertyChanged(nameof(ReaderWarningMessageVisibility));
            }
        }
    }

    public bool IsFeedBtnEnabled => TelemetryReader != null && TelemetryReader.IsSupported;

    public void UpdateTelemetryReader()
    {
        TelemetryReader = readerFactory.GetTelemetryReader(Version)!;
        VersionUpdated?.Invoke(this, new EventArgs());

        if (TelemetryReader == null)
        {
            var msg = $"{EnumConverter.GetEnumDescription(Version)} reader not found";
            Log?.Warn(msg);
            ReaderWarningMessage = msg;
            ReaderWarningMessageVisibility = Visibility.Visible;

            RaisePropertyChanged(nameof(IsFeedBtnEnabled));
            return;
        }

        if (TelemetryReader.IsSupported)
        {
            ReaderWarningMessage = "";
            ReaderWarningMessageVisibility = Visibility.Hidden;
        }
        else
        {
            var msg = $"{TelemetryReader.Name} is not supported";
            Log?.Warn(msg);
            ReaderWarningMessage = msg;
            ReaderWarningMessageVisibility = Visibility.Visible;
        }

        RaisePropertyChanged(nameof(IsFeedBtnEnabled));
    }

    public void StartTelemetryFeed()
    {
        if (!telemetryListener.IsListenerRunning)
        {
            Log?.Info("Starting Telemetry feed");
            telemetryListener.Start();
        }
    }

    public void StopTelemetryFeed()
    {
        if (telemetryListener.IsListenerRunning)
        {
            Log?.Info("Stopping Telemetry feed");
            telemetryListener.Stop();
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
    }
}
