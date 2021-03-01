using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Core.Logic;
using Hurace.Core.Models;
using Hurace.RaceControl.Services;
using Unity;
using Models = Hurace.Core.Models;
using Hurace.Core.Extensions;
using Hurace.Mvvm;
using Hurace.Mvvm.ViewModels;
using Hurace.RaceControl.ViewModels.Race;
using Hurace.Core.Logic.Models;

namespace Hurace.RaceControl.ViewModels.DisplayControl.ControlPanel
{
    public class StatsViewModel : NotifyPropertyChanged, IComponentViewModel
    {
        private readonly IStatisticLogic statisticLogic;
        private readonly IClockService clockService;

        private readonly int runNumber;
        private Models.Race race => Parent.Races.Selected.Race;

        public ObservableCollection<StatsItemViewModel> Stats { get; set; }
            = new ObservableCollection<StatsItemViewModel>();

        public IRaceViewModel Parent { get; set; }

        public string RaceTitle { get; set; }

        private bool isLoading;
        public bool IsLoading
        {
            get => isLoading;
            set => Set(ref isLoading, value);
        }

        public StatsViewModel(IRaceViewModel parent, int run)
        {
            if (run < 1 || run > 2) throw new ArgumentNullException(nameof(run));

            statisticLogic = App.Container.Resolve<IStatisticLogic>();
            clockService = App.Container.Resolve<IClockService>();

            Parent = parent ?? throw new ArgumentNullException();
            runNumber = run;
        }

        public async Task OnInitAsync()
        {
            var runText = runNumber == 1 ? "First" : "Second";
            RaceTitle = race.RaceType + " " + race.Gender + " (" + race.Name + " (" + runText + " run))";
            clockService.OnRunFinish += _ => LoadStats();

            IsLoading = true;
            await LoadStats();
            IsLoading = false;
        }

        public Task OnDestroyAsync()
        {
            clockService.OnRunFinish -= _ => LoadStats();
            return Task.CompletedTask;
        }

        private Task LoadStats()
        {
            Models.Skier GetSkierById(int id)
            {
                return Parent.AllSkiers.FirstOrDefault(s => s.Id == id);
            }

            App.Current.Dispatcher.Invoke(async () =>
            {
                var statistics = await this.statisticLogic.GetRaceStatistics(race.Id, runNumber, race.SensorAmount);
                var stats = statistics.Select(s => new StatsItemViewModel(s, GetSkierById(s.SkierId))).ToList();
                Stats.SetItems(stats);
            });

            return Task.CompletedTask;
        }
    }
}
