namespace F1TelemetryApp.DataHandlers;

using Model;

using F1GameTelemetry.Models;

internal static class DriversHandler
{
    private const int _numSectors = 3;

    public static ObservableDriverCollection Drivers = new();

    public static void UpdateParticipant(Participant data)
    {
        InvokeDispatch.Invoke(() =>
        {

            // Participant packets will give us the names of the drivers
            Drivers.Clear();

            for (byte i = 0; i < data.numActiveCars; i++)
            {
                var participantName = data.participants[i].name;
                if (string.IsNullOrEmpty(participantName))
                    return;

                Drivers.Add(new Driver(participantName.Replace("\0", string.Empty), i));
            }
        });
    }

    public static void UpdateSessionHistory(SessionHistory data)
    {
        // Session History will give us the updated sector data
        // Session History packets are one per driver.

        var driverIndex = data.carIdx;
        if (!CheckDriverExists(driverIndex)) return;

        InvokeDispatch.Invoke(() =>
        {
            var driver = Drivers[driverIndex];
            var lapOfFastestSectors = new int[_numSectors];
            for (byte i = 0; i < _numSectors; i++)
                lapOfFastestSectors[i] = driver.LapData.BestSectorIndexes[i];

            Drivers[driverIndex].UpdateLapHistoryData(
                data.numLaps,
                data.lapHistoryData);

            if (driver.LapData.BestLapIndex == data.bestLapTimeLapNum - 1)
                Drivers.UpdateFastestLap(driverIndex);

            for (byte i = 0; i < _numSectors; i++)
            {
                if (lapOfFastestSectors[i] == data.bestSectorTimeLapNums[i] - 1)
                    Drivers.UpdateFastestSector(i, driverIndex);
            }

            Drivers[driverIndex].UpdateTyreStintHistoryData(
                data.numTyreStints,
                data.tyreStintHistoryData);
        });
    }

    public static void UpdateLapData(LapData data)
    {
        // Only care about the position from this one for now
        InvokeDispatch.Invoke(() =>
        {
            for (int i = 0; i < Drivers.Count; i++)
                Drivers[i].SetPosition(data.carLapData[i].carPosition);
        });
    }

    private static bool CheckDriverExists(byte index) => Drivers.Count > index;
}
