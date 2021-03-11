namespace Hurace.Simulator.Models
{
    public class ClockSettings
    {
        public int MinInterval { get; set; }
        public int MaxInterval { get; set; }

        public int MinRealInterval { get; set; }
        public int MaxRealInterval { get; set; }

        public int SensorAmount { get; set; }

        public int TriggerSuccessWeight { get; set; }
        public int DoNotTriggerWeight { get; set; }
        public int SetWrongSensorIdWeight { get; set; }
        public int SetWrongDeltaTimeWeight { get; set; }
        public int TriggerMultipleWeight { get; set; }

        public static ClockSettings GetDefaultSettings() =>
            new ClockSettings
            {
                MinInterval = 1_000,
                MaxInterval = 1_500,
                MinRealInterval = 30_000,
                MaxRealInterval = 33_000,
                SensorAmount = 5,
                DoNotTriggerWeight = 0,
                SetWrongDeltaTimeWeight = 0,
                SetWrongSensorIdWeight = 0,
                TriggerMultipleWeight = 0,
                TriggerSuccessWeight = 100
            };
    }
}
