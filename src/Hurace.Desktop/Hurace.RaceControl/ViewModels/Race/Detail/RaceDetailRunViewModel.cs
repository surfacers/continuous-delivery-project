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

        private int runNumber => (int)this.RouteData;
        private Models.Race race => this.Parent.Races.Selected.Race;

        private bool notStartedYet = false;
        public bool NotStartedYet 
        {
            get => this.notStartedYet;
            set => this.Set(ref this.notStartedYet, value);
        }

        public ObservableCollection<StartListItemViewModel> StartList
            => this.runNumber == 1 ? this.Parent.StartList1 : this.Parent.StartList2;

        public CommandViewModel GrantRunCommandViewModel { get; }
        public CommandViewModel DisqualifyCommandViewModel { get; }
        public CommandViewModel StartNextRunCommandViewModel { get; }

        public RaceDetailRunViewModel()
        {
            this.GrantRunCommandViewModel = new CommandViewModel(
                "Grant next run", 
                "Grant next run in start list",
                async () => await this.Parent.GrantRunAsync(),
                () => this.Parent.Races.Selected != null 
                   && this.StartList.Any(d => d.StartListState == Enums.StartListState.NotStarted)
                   && this.race.RaceState == Enums.RaceState.Running
                   && !this.ClockService.IsListening);

            this.DisqualifyCommandViewModel = new CommandViewModel(
                "Disqualify", 
                "Disqualify selected skier",
                async () => await this.Parent.DisqualifyAsync(),
                () => this.Parent.SelectedStartListItem != null && !this.Parent.SelectedStartListItem.IsDisqualified,
                withStyle: ButtonStyle.Flat);

            this.DisqualifyCommandViewModel.OnSuccess += () => this.NotificationService.ShowMessage("Disqualified successfully");
            this.DisqualifyCommandViewModel.OnFailure += (ex) => this.NotificationService.ShowMessage("Disqualified failed");

            this.StartNextRunCommandViewModel = new CommandViewModel(
                "Generate start list", 
                "Generate start list for second run",
                async () => await this.GenerateStartListForNextRunAsync(),
                () => !this.Parent.StartList1.Any(s => s.StartListState == Enums.StartListState.NotStarted));
        }

        public override Task OnInitAsync()
        {
            if (this.runNumber < 1 && this.runNumber > 2)
            {
                throw new ArgumentException($"RunNumber {this.runNumber} not supported!");
            }

            this.NotStartedYet = this.StartList.Count == 0;

            return Task.CompletedTask;
        }

        private async Task GenerateStartListForNextRunAsync()
        {
            var startList = await this.StartListLogic.GenerateStartListForRunAsync(this.race.Id, this.runNumber);
            this.NotStartedYet = startList.Count() == 0;

            var startListViewModels = startList.Select(s => this.Parent.ToStartListItemViewModel(s)).ToList();
            this.StartList.SetItems(startListViewModels);
            this.Parent.StartList2.SetItems(startListViewModels);
        }

        public override Task OnDestroyAsync()
        {
            return Task.CompletedTask;
        }
    }
}
