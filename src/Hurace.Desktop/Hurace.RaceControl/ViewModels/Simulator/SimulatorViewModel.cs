using System.Threading.Tasks;
using Hurace.Mvvm.ViewModels.Controls;
using Hurace.RaceControl.Services;
using Hurace.RaceControl.ViewModels.Controls;
using Hurace.RaceControl.Views.Simulator;
using Hurace.Simulator;
using Hurace.Timer;
using Unity;

namespace Hurace.RaceControl.ViewModels.Simulator
{
    public class SimulatorViewModel : TabViewModel<MainViewModel>
    {
        [Dependency] public IClockService ClockService { get; set; }

        public CommandViewModel ShowSimulatorSettingCommandViewModel { get; set; }

        public SimulatorViewModel()
        {
            this.ShowSimulatorSettingCommandViewModel = new CommandViewModel(
                "Switch to simulator", 
                "Switch to simulator",
                () => this.OpenSimulatorSetting());
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
            this.ClockService.RaceClock = clock;

            var window = new SimulatorSettingsView();
            window.Show();

            window.Closed += (_, __) =>
            {
                clock.Stop();
                window.DataContext = null;
                this.ClockService.RaceClock = App.Container.Resolve<IRaceClock>();
            };

            return Task.CompletedTask;
        }
    }
}
