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

        private SimulatedRaceClock raceClock => (SimulatedRaceClock)clockService.RaceClock;

        public ClockSettingsEditViewModel Edit { get; set; }

        private bool isEnabled;
        public bool IsEnabled
        {
            get => isEnabled;
            set => Set(ref isEnabled, value);
        }

        public CommandViewModel StartCommandViewModel { get; }
        public CommandViewModel StopCommandViewModel { get; }
        
        public ObservableCollection<string> Logs { get; set; }
            = new ObservableCollection<string>();
        public Dispatcher Dispatcher { get; internal set; }

        public SimulatorSettingsEditViewModel()
        {
            mapper = App.Container.Resolve<IMapper>();
            Edit = mapper.Map<ClockSettingsEditViewModel>(ClockSettings.GetDefaultSettings());

            StartCommandViewModel = new CommandViewModel(
                "Start", "Start timer",
                () => Start(),
                () => CanStart());

            StopCommandViewModel = new CommandViewModel(
                "Stop", "Stop timer",
                () => Stop(),
                () => CanStop());
            
            clockService = App.Container.Resolve<IClockService>();
            clockService.OnRunUpdate += OnValidUpdate;
            clockService.OnInvalid += OnInvalidUpdate;

            IsEnabled = !raceClock.IsRunning;
        }      

        private bool CanStart() => !raceClock.IsRunning && !Edit.HasErrors;
        private Task Start()
        {
            IsEnabled = false;
            Logs.Clear();

            var settings = mapper.Map<ClockSettings>(Edit);
            Edit.Original = settings;

            raceClock.Start(settings);

            return Task.CompletedTask;
        }

        private bool CanStop() => raceClock.IsRunning;
        private Task Stop()
        {
            IsEnabled = true;
            raceClock.Stop();

            return Task.CompletedTask;
        }

        private void OnValidUpdate(Core.Models.RaceData raceData)
        {
            string message = string.Empty;
            if (raceData.SensorId == 1) message = "START";
            if (raceData.SensorId == Edit.SensorAmount) message = "END";

            Log(raceData.SensorId, raceData.TimeStamp, message);
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

            Log(sensorId, time, message, isValid: false);
        }

        private void Log(int sensorId, DateTime time, string message, bool isValid = true)
        {
            string valid = isValid ? string.Empty : "[NOT VALID]";
            Dispatcher.Invoke(() => Logs.Insert(0, $"{sensorId} {time.ToLongTimeString()} {valid} {message}"));
        }
    }
}
