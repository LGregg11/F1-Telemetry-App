using UnityEngine;
using TMPro;
using F1GameTelemetry.Readers;
using F1GameTelemetry.Listener;
using F1GameTelemetry.Converters;

public class TelemetryManager : MonoBehaviour
{
    private static TelemetryManager _instance;
    //private static DataManager _dataManager;
    private int participantPacketTally;

    public TMP_InputField port;

    void Awake()
    {
        if (_instance != null)
        {
            Debug.Log("Awake called on TelemetryManager again .. This shouldn't have happened");
            Destroy(_instance);
        }

        _instance = this;
        Debug.Log("TelemetryManager instance created");

        participantPacketTally = 0;
        SingletonTelemetryReader.ParticipantReceived += (o, e) =>
        {
            var sessionTime = e.Header.sessionTime.ToTelemetryTime().ToString();
            participantPacketTally++;
            Debug.Log($"Participant packet {participantPacketTally} received - session time: {sessionTime}");
        };
    }

    private void OnApplicationQuit()
    {
        StopTelemetry();
    }

    public void StartTelemetry()
    {
        // Start Telemetry Listener if not running
        if (!SingletonTelemetryReader.IsListenerRunning)
        {
            Debug.Log("Starting Telemetry feed");
            //if (IsImportCheckboxChecked && string.IsNullOrEmpty(WarningMessage))

            // For now, hard code everything to use the existing replay
            if (!int.TryParse(port.text, out int portInt))
            {
                Debug.Log($"{port.text} is not a valid port number");
                return;
            }

            SingletonTelemetryReader.SetTelemetryListener(new TelemetryListener(portInt));
            SingletonTelemetryReader.SetTelemetryConverterByVersion(new TelemetryConverter2021());
            string defaultReplayFilepath = @"D:/Coding/Projects/F1-Telemetry-App/TestData/Austria_5laps_Leclerc.txt";
            SingletonTelemetryReader.SetTelemetryListener(new TelemetryImporter(defaultReplayFilepath));

            SingletonTelemetryReader.StartListener();
        }
    }

    public void StopTelemetry()
    {
        // Stop TelemetryListener if running
        if (SingletonTelemetryReader.IsListenerRunning)
        {
            Debug.Log("Stopping Telemetry feed");
            SingletonTelemetryReader.StopListener();
            participantPacketTally = 0;
        }
    }
}
