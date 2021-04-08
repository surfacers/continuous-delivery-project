using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Core.Extensions;
using Hurace.Core.Logic;
using Hurace.Mvvm;
using Hurace.Mvvm.ViewModels;
using Hurace.RaceControl.Services;
using Hurace.RaceControl.ViewModels.Race;
using Unity;
using Models = Hurace.Core.Models;

namespace Hurace.RaceControl.ViewModels.DisplayControl.ControlPanel
{
    public class StatsViewModel : NotifyPropertyChanged, IComponentViewModel
    {
        private readonly IStatisticLogic statisticLogic;
        private readonly IClockService clockService;

        private readonly int runNumber;
        private Models.Race race => this.Parent.Races.Selected.Race;

        public ObservableCollection<StatsItemViewModel> Stats { get; set; }
            = new ObservableCollection<StatsItemViewModel>();

        public IRaceViewModel Parent { get; set; }

        public string RaceTitle { get; set; }

        private bool isLoading;
        public bool IsLoading
        {
            get => this.isLoading;
            set => this.Set(ref this.isLoading, value);
        }

        public StatsViewModel(IRaceViewModel parent, int run)
        {
            if (run < 1 || run > 2)
            {
                throw new ArgumentNullException(nameof(run));
            }

            this.statisticLogic = App.Container.Resolve<IStatisticLogic>();
            this.clockService = App.Container.Resolve<IClockService>();

            this.Parent = parent ?? throw new ArgumentNullException();
            this.runNumber = run;
        }

        public async Task OnInitAsync()
        {
            var runText = this.runNumber == 1 ? "First" : "Second";
            this.RaceTitle = this.race.RaceType + " " + this.race.Gender + " (" + this.race.Name + " (" + runText + " run))";
            this.clockService.OnRunFinish += _ => this.LoadStats();

            this.IsLoading = true;
            await this.LoadStats();
            this.IsLoading = false;
        }

        public Task OnDestroyAsync()
        {
            this.clockService.OnRunFinish -= _ => this.LoadStats();
            return Task.CompletedTask;
        }

        private Task LoadStats()
        {
            Models.Skier GetSkierById(int id)
            {
                return this.Parent.AllSkiers.FirstOrDefault(s => s.Id == id);
            }

            App.Current.Dispatcher.Invoke(async () =>
            {
                var statistics = await this.statisticLogic.GetRaceStatistics(this.race.Id, this.runNumber, this.race.SensorAmount);
                var stats = statistics.Select(s => new StatsItemViewModel(s, GetSkierById(s.SkierId))).ToList();
                this.Stats.SetItems(stats);
            });

            return Task.CompletedTask;
        }
    }
}
