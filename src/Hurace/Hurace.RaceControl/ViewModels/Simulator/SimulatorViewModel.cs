using Unity;
using System.Threading.Tasks;
using Hurace.RaceControl.ViewModels.Controls;
using Hurace.RaceControl.Services;
using Hurace.RaceControl.Views.Simulator;
using System;
using Hurace.Timer;
using Hurace.Simulator;
using Hurace.Mvvm.ViewModels.Controls;

namespace Hurace.RaceControl.ViewModels.Simulator
{
    public class SimulatorViewModel : TabViewModel<MainViewModel>
    {
        [Dependency] public IClockService ClockService { get; set; }

        public CommandViewModel ShowSimulatorSettingCommandViewModel { get; set; }

        public SimulatorViewModel()
        {
            ShowSimulatorSettingCommandViewModel = new CommandViewModel(
                "Switch to simulator", "Switch to simulator",
                () => OpenSimulatorSetting());
        }

        public override Task OnInitAsync()
        {
            return Task.CompletedTask;
        }

        public override Task OnDestroyAsync()
        {
            return Task.CompletedTask;
        }

        private Task OpenSimulatorSetting()
        {
            var clock = new SimulatedRaceClock();
            ClockService.RaceClock = clock;

            var window = new SimulatorSettingsView();
            window.Show();

            window.Closed += (_, __) =>
            {
                clock.Stop();
                window.DataContext = null;
                ClockService.RaceClock = App.Container.Resolve<IRaceClock>();
            };

            return Task.CompletedTask;
        }
    }
}
