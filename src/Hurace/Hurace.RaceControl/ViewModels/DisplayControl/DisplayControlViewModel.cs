using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using Hurace.Core.Extensions;
using Hurace.Mvvm.Enums;
using Hurace.Mvvm.ViewModels;
using Hurace.Mvvm.ViewModels.Controls;
using Hurace.RaceControl.ViewModels.Controls;
using Hurace.RaceControl.ViewModels.DisplayControl.ControlPanel;
using Hurace.RaceControl.ViewModels.Race;
using Hurace.RaceControl.Views.DisplayControl.ControlPanel;
using Enums = Hurace.Core.Enums;

namespace Hurace.RaceControl.ViewModels.DisplayControl
{
    public class DisplayControlViewModel : TabViewModel<IRaceViewModel>
    {
        public CommandViewModel ShowStatsRun1Command { get; }
        public CommandViewModel ShowStatsRun2Command { get; }

        private bool showLiveView;
        public bool ShowLiveView
        {
            get => showLiveView;
            set => Set(ref showLiveView, value);
        }

        public CommandViewModel ShowLiveViewCommand { get; }

        public DisplayControlViewModel()
        {
            ShowStatsRun1Command = new CommandViewModel(
                "Open run 1", "Show for run 1",
                () => OpenStats(runNumber: 1),
                withStyle: ButtonStyle.Flat);

            ShowStatsRun2Command = new CommandViewModel(
                "Open run 2", "Show for run 2",
                () => OpenStats(runNumber: 2),
                withStyle: ButtonStyle.Flat);

            ShowLiveViewCommand = new CommandViewModel(
                "Open live view", "open live view",
                () => OpenLiveView(),
                withStyle: ButtonStyle.Flat);
        }

        public override Task OnInitAsync()
        {
            var currentRace = Parent.Races.Selected;

            ShowStatsRun1Command.Content = currentRace.Race.HasSecondRun() ? "Open run 1" : "Open run";
            ShowStatsRun2Command.ShowCommand = () => currentRace.Race.HasSecondRun();
            ShowLiveView = currentRace.Race.RaceState == Enums.RaceState.Running;

            return Task.CompletedTask;
        }
        private async Task OpenLiveView()
        {
            await OpenNewWindow(new LiveView(), new LiveViewModel());
        }

        private async Task OpenStats(int runNumber)
        {
            await OpenNewWindow(new StatsView(), new StatsViewModel(Parent, runNumber));
        }

        private async Task OpenNewWindow(Control view, IComponentViewModel viewModel)
        {
            var monitorViewModel = new MonitorViewModel(view, viewModel, Parent);
            await viewModel.OnInitAsync();
            
            var window = new MonitorWindow(monitorViewModel);
            window.Closed += async (_, __) =>
            {
                await viewModel.OnDestroyAsync();
            };
            
            window.Show();
        }

        public override Task OnDestroyAsync()
        {
            return Task.CompletedTask;
        }
    }
}
