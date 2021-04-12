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
            get => this.minInterval;
            set => this.Set(ref this.minInterval, value);
        }

        private int maxInterval;
        public int MaxInterval
        {
            get => this.maxInterval;
            set => this.Set(ref this.maxInterval, value);
        }

        private int minRealInterval;
        public int MinRealInterval
        {
            get => this.minRealInterval;
            set => this.Set(ref this.minRealInterval, value);
        }

        private int maxRealInterval;
        public int MaxRealInterval
        {
            get => this.maxRealInterval;
            set => this.Set(ref this.maxRealInterval, value);
        }

        private int sensorAmount;
        public int SensorAmount
        {
            get => this.sensorAmount;
            set => this.Set(ref this.sensorAmount, value);
        }

        private int triggerSuccessWeight;
        public int TriggerSuccessWeight
        {
            get => this.triggerSuccessWeight;
            set => this.Set(ref this.triggerSuccessWeight, value);
        }

        private int doNotTriggerWeight;
        public int DoNotTriggerWeight
        {
            get => this.doNotTriggerWeight;
            set => this.Set(ref this.doNotTriggerWeight, value);
        }

        private int setWrongSensorIdWeight;
        public int SetWrongSensorIdWeight
        {
            get => this.setWrongSensorIdWeight;
            set => this.Set(ref this.setWrongSensorIdWeight, value);
        }

        private int setWrongDeltaTimeWeight;
        public int SetWrongDeltaTimeWeight
        {
            get => this.setWrongDeltaTimeWeight;
            set => this.Set(ref this.setWrongDeltaTimeWeight, value);
        }

        private int triggerMultipleWeight;
        public int TriggerMultipleWeight
        {
            get => this.triggerMultipleWeight;
            set => this.Set(ref this.triggerMultipleWeight, value);
        }
    }
}
