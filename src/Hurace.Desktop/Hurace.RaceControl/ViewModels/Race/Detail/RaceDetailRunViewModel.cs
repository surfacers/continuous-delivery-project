using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Core.Extensions;
using Hurace.Core.Logic;
using Hurace.Mvvm.Enums;
using Hurace.Mvvm.ViewModels.Controls;
using Hurace.RaceControl.Services;
using Hurace.RaceControl.ViewModels.Controls;
using Unity;
using Enums = Hurace.Core.Enums;
using Models = Hurace.Core.Models;

namespace Hurace.RaceControl.ViewModels.Race.Detail
{
    public class RaceDetailRunViewModel : TabViewModel<IRaceViewModel>
    {
        [Dependency] public IStartListLogic StartListLogic { get; set; }
        [Dependency] public INotificationService NotificationService { get; set; }
        [Dependency] public IClockService ClockService { get; set; }

        private int runNumber => (int)RouteData;
        private Models.Race race => Parent.Races.Selected.Race;

        private bool notStartedYet = false;
        public bool NotStartedYet 
        {
            get => notStartedYet;
            set => Set(ref notStartedYet, value);
        }

        public ObservableCollection<StartListItemViewModel> StartList
            => runNumber == 1 ? Parent.StartList1 : Parent.StartList2;
        

        public CommandViewModel GrantRunCommandViewModel { get; }
        public CommandViewModel DisqualifyCommandViewModel { get; }
        public CommandViewModel StartNextRunCommandViewModel { get; }

        public RaceDetailRunViewModel()
        {
            GrantRunCommandViewModel = new CommandViewModel(
                "Grant next run", "Grant next run in start list",
                async () => await Parent.GrantRunAsync(),
                () => Parent.Races.Selected != null 
                   && StartList.Any(d => d.StartListState == Enums.StartListState.NotStarted)
                   && race.RaceState == Enums.RaceState.Running
                   && !ClockService.IsListening);
            
            DisqualifyCommandViewModel = new CommandViewModel(
                "Disqualify", "Disqualify selected skier",
                async () => await Parent.DisqualifyAsync(),
                () => Parent.SelectedStartListItem != null && !Parent.SelectedStartListItem.IsDisqualified,
                withStyle: ButtonStyle.Flat);

            DisqualifyCommandViewModel.OnSuccess += () => NotificationService.ShowMessage("Disqualified successfully");
            DisqualifyCommandViewModel.OnFailure += (ex) => NotificationService.ShowMessage("Disqualified failed");

            StartNextRunCommandViewModel = new CommandViewModel(
                "Generate start list", "Generate start list for second run",
                async () => await GenerateStartListForNextRunAsync(),
                () => !Parent.StartList1.Any(s => s.StartListState == Enums.StartListState.NotStarted));
        }

        public override Task OnInitAsync()
        {
            if (runNumber < 1 && runNumber > 2) throw new ArgumentException($"RunNumber {runNumber} not supported!");

            NotStartedYet = StartList.Count == 0;

            return Task.CompletedTask;
        }

        private async Task GenerateStartListForNextRunAsync()
        {
            var startList = await StartListLogic.GenerateStartListForRunAsync(race.Id, runNumber);
            NotStartedYet = startList.Count() == 0;

            var startListViewModels = startList.Select(s => Parent.ToStartListItemViewModel(s)).ToList();
            StartList.SetItems(startListViewModels);
            Parent.StartList2.SetItems(startListViewModels);
        }

        public override Task OnDestroyAsync()
        {
            return Task.CompletedTask;
        }
    }
}
