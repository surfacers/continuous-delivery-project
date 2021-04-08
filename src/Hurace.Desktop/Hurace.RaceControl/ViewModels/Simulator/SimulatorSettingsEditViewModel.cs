using System;
using System.Collections.ObjectModel;
using System.Drawing.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using AutoMapper;
using Hurace.Mvvm;
using Hurace.Mvvm.ViewModels;
using Hurace.Mvvm.ViewModels.Controls;
using Hurace.RaceControl.Services;
using Hurace.Simulator;
using Hurace.Simulator.Models;
using Unity;

namespace Hurace.RaceControl.ViewModels.Simulator
{
    public class SimulatorSettingsEditViewModel : NotifyPropertyChanged
    {
        private readonly IMapper mapper;
        private readonly IClockService clockService;

        private SimulatedRaceClock raceClock => (SimulatedRaceClock)this.clockService.RaceClock;

        public ClockSettingsEditViewModel Edit { get; set; }

        private bool isEnabled;
        public bool IsEnabled
        {
            get => this.isEnabled;
            set => this.Set(ref this.isEnabled, value);
        }

        public CommandViewModel StartCommandViewModel { get; }
        public CommandViewModel StopCommandViewModel { get; }
        
        public ObservableCollection<string> Logs { get; set; }
            = new ObservableCollection<string>();
        public Dispatcher Dispatcher { get; internal set; }

        public SimulatorSettingsEditViewModel()
        {
            this.mapper = App.Container.Resolve<IMapper>();
            this.Edit = this.mapper.Map<ClockSettingsEditViewModel>(ClockSettings.GetDefaultSettings());

            this.StartCommandViewModel = new CommandViewModel(
                "Start", 
                "Start timer",
                () => this.Start(),
                () => this.CanStart());

            this.StopCommandViewModel = new CommandViewModel(
                "Stop", 
                "Stop timer",
                () => this.Stop(),
                () => this.CanStop());
            
            this.clockService = App.Container.Resolve<IClockService>();
            this.clockService.OnRunUpdate += this.OnValidUpdate;
            this.clockService.OnInvalid += this.OnInvalidUpdate;

            this.IsEnabled = !this.raceClock.IsRunning;
        }      

        private bool CanStart() => !this.raceClock.IsRunning && !this.Edit.HasErrors;
        private Task Start()
        {
            this.IsEnabled = false;
            this.Logs.Clear();

            var settings = this.mapper.Map<ClockSettings>(this.Edit);
            this.Edit.Original = settings;

            this.raceClock.Start(settings);

            return Task.CompletedTask;
        }

        private bool CanStop() => this.raceClock.IsRunning;
        private Task Stop()
        {
            this.IsEnabled = true;
            this.raceClock.Stop();

            return Task.CompletedTask;
        }

        private void OnValidUpdate(Core.Models.RaceData raceData)
        {
            string message = string.Empty;
            if (raceData.SensorId == 1)
            {
                message = "START";
            }
            else if (raceData.SensorId == this.Edit.SensorAmount)
            {
                message = "END";
            }

            this.Log(raceData.SensorId, raceData.TimeStamp, message);
        }

        private void OnInvalidUpdate(InvalidRaceDataReason reason, int sensorId, DateTime time)
        {
            string message = reason switch
            {
                InvalidRaceDataReason.NotEnoughTimeBetween => "{not enough time between}",
                InvalidRaceDataReason.NotGranted => "{not granted}",
                InvalidRaceDataReason.NotInSensorRange => "{not in sensor range}",
                InvalidRaceDataReason.NotStartedYet => "{not started yet}",
                _ => string.Empty
            };

            this.Log(sensorId, time, message, isValid: false);
        }

        private void Log(int sensorId, DateTime time, string message, bool isValid = true)
        {
            string valid = isValid ? string.Empty : "[NOT VALID]";
            this.Dispatcher.Invoke(() => this.Logs.Insert(0, $"{sensorId} {time.ToLongTimeString()} {valid} {message}"));
        }
    }
}
