namespace F1GameTelemetry.Models
{
    using Enums;

    public struct CarStatus
    {
        public CarStatus(CarStatusData[] carStatusData)
        {
            this.carStatusData = carStatusData;
        }

        public CarStatusData[] carStatusData;
    }

    public struct CarStatusData
    {
        public CarStatusData(
            float fuelRemainingLaps,
            TyreVisual visualTyreCompound,
            byte tyresAgeLaps)
        {
            this.fuelRemainingLaps = fuelRemainingLaps;
            this.visualTyreCompound = visualTyreCompound;
            this.tyresAgeLaps = tyresAgeLaps;
        }

        public float fuelRemainingLaps;
        public TyreVisual visualTyreCompound;
        public byte tyresAgeLaps;
    }
}
