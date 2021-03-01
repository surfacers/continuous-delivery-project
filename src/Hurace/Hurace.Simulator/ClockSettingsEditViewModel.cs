using AutoMapper;
using Hurace.Mvvm.ViewModels;
using Hurace.Simulator.Models;
using Hurace.Simulator.Validators;

namespace Hurace.Simulator
{
    public class ClockSettingsEditViewModel : EditViewModel<ClockSettings, ClockSettingsValidator>
    {
        public ClockSettingsEditViewModel(IMapper mapper)
            : base(mapper)
        {
        }

        private int minInterval;
        public int MinInterval 
        {
            get => minInterval;
            set => Set(ref minInterval, value);
        }

        private int maxInterval;
        public int MaxInterval
        {
            get => maxInterval;
            set => Set(ref maxInterval, value);
        }

        private int minRealInterval;
        public int MinRealInterval
        {
            get => minRealInterval;
            set => Set(ref minRealInterval, value);
        }

        private int maxRealInterval;
        public int MaxRealInterval
        {
            get => maxRealInterval;
            set => Set(ref maxRealInterval, value);
        }

        private int sensorAmount;
        public int SensorAmount
        {
            get => sensorAmount;
            set => Set(ref sensorAmount, value);
        }

        private int triggerSuccessWeight;
        public int TriggerSuccessWeight
        {
            get => triggerSuccessWeight;
            set => Set(ref triggerSuccessWeight, value);
        }

        private int doNotTriggerWeight;
        public int DoNotTriggerWeight
        {
            get => doNotTriggerWeight;
            set => Set(ref doNotTriggerWeight, value);
        }

        private int setWrongSensorIdWeight;
        public int SetWrongSensorIdWeight
        {
            get => setWrongSensorIdWeight;
            set => Set(ref setWrongSensorIdWeight, value);
        }

        private int setWrongDeltaTimeWeight;
        public int SetWrongDeltaTimeWeight
        {
            get => setWrongDeltaTimeWeight;
            set => Set(ref setWrongDeltaTimeWeight, value);
        }

        private int triggerMultipleWeight;
        public int TriggerMultipleWeight
        {
            get => triggerMultipleWeight;
            set => Set(ref triggerMultipleWeight, value);
        }
    }
}
