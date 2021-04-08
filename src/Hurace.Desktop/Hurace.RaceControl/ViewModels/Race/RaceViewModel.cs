using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using AutoMapper;
using Hurace.Core.Enums;
using Hurace.Core.Extensions;
using Hurace.Core.Logic;
using Hurace.Core.Logic.Models;
using Hurace.Core.Models;
using Hurace.Mvvm.ViewModels;
using Hurace.RaceControl.Services;
using Hurace.RaceControl.ViewModels.Controls;
using Hurace.RaceControl.ViewModels.Race.Detail;
using Microsoft.AspNetCore.SignalR.Client;
using Unity;
using Enums = Hurace.Core.Enums;
using Models = Hurace.Core.Models;

namespace Hurace.RaceControl.ViewModels.Race
{
    public class RaceViewModel : TabViewModel<MainViewModel>, IRaceViewModel
    {
        private int raceId => this.Races.Selected?.Race.Id ?? -1;
        private Gender gender => this.Races.Selected.Race.Gender;
        private int sensorAmount => this.Races.Selected.Race.SensorAmount;

        [Dependency] public IMapper mapper { get; set; }
        [Dependency] public IRaceLogic RaceLogic { get; set; }
        [Dependency] public IStartListLogic StartListLogic { get; set; }
        [Dependency] public ISkierLogic SkierLogic { get; set; }
        [Dependency] public IRaceDataLogic RaceDataLogic { get; set; }
        [Dependency] public ILocationLogic LocationLogic { get; set; }
        [Dependency] public IClockService ClockService { get; set; }
        [Dependency] public HubConnection HubConnection { get; set; }

        public RaceListViewModel RaceListViewModel { get; set; }
        public RaceDetailViewModel RaceDetailViewModel { get; set; }

        public FilterViewModel<RaceListItemViewModel> Races { get; set; }

        private RaceEditViewModel editRace;
        public RaceEditViewModel Edit
        {
            get => this.editRace;
            set => this.Set(ref this.editRace, value);
        }

        public IEnumerable<Models.Skier> AllSkiers { get; set; }
        public StartListItemViewModel CurrentRun { get; set; }

        public ObservableCollection<StartListItemViewModel> StartList1 { get; set; }
            = new ObservableCollection<StartListItemViewModel>();
        public ObservableCollection<StartListItemViewModel> StartList2 { get; set; }
            = new ObservableCollection<StartListItemViewModel>();

        private StartListItemViewModel selectedStartListItem;
        public StartListItemViewModel SelectedStartListItem
        {
            get => this.selectedStartListItem;
            set => this.Set(ref this.selectedStartListItem, value);
        }

        public ObservableCollection<ComboBoxItemViewModel<int>> Locations { get; set; }
            = new ObservableCollection<ComboBoxItemViewModel<int>>();
        public ObservableCollection<ComboBoxItemViewModel<RaceType>> RaceTypes { get; set; }
            = new ObservableCollection<ComboBoxItemViewModel<RaceType>>();

        public RaceViewModel()
        {
            this.RaceListViewModel = new RaceListViewModel(this);
            this.RaceDetailViewModel = new RaceDetailViewModel(this);
            this.Races = new FilterViewModel<RaceListItemViewModel>(
                r => $"{r.RaceType} {r.Name}",
                this.RaceSelectionChanged);
        }

        public async Task RaceSelectionChanged(RaceListItemViewModel race)
        {
            this.RaceDetailViewModel.NavigationViewModel.CurrentItem.ViewModel.IsLoading = true;

            this.Edit = this.mapper.Map<RaceEditViewModel>(race.Race);
            this.Edit.LocationId = this.Locations.Where(l => l.Value == race.Race.LocationId).FirstOrDefault();
            this.Edit.RaceType = this.RaceTypes.Where(l => l.Value == race.Race.RaceType).FirstOrDefault();

            var skiers = this.AllSkiers.Where(s => s.Gender == this.gender).ToList();

            var startListViewModels1 = await this.GetStartListAsync(this.raceId, runNumber: 1);
            this.StartList1.SetItems(startListViewModels1);
            await this.SetRaceData(this.raceId, runNumber: 1, startList: this.StartList1);
            this.SetRacePositions(this.StartList1);

            this.StartList2.Clear();
            if (race.Race.HasSecondRun())
            {
                var startListViewModels2 = await this.GetStartListAsync(this.raceId, runNumber: 2);
                this.StartList2.SetItems(startListViewModels2);
                await this.SetRaceData(this.raceId, runNumber: 2, startList: this.StartList2);
                this.SetRacePositions(this.StartList2);
            }

            this.RaceDetailViewModel.NavigationViewModel.CurrentItem.ViewModel.IsLoading = false;

            this.RaceDetailViewModel.UpdateNavigationTabs();
        }

        private async Task SetRaceData(int raceId, int runNumber, IEnumerable<StartListItemViewModel> startList)
        {
            var raceData = await this.RaceDataLogic.GetByRaceIdAsync(raceId, runNumber);
            foreach (var item in startList)
            {
                var data = raceData.Where(r => r.StartListId == item.StartList.Id).ToList();
                item.SetRaceData(data, this.sensorAmount, this.ClockService?.CurrentRun?.StartList?.Id ?? -1);
            }
        }

        private void SetRacePositions(IEnumerable<StartListItemViewModel> startList)
        {
            var ordered = startList
                .Where(s => s.StartListState == StartListState.Done)
                .OrderBy(s => s.TotalTime)
                .ToList();

            var first = ordered.FirstOrDefault();

            ordered
                .ForEach((s, i) =>
                {
                    s.Position = i + 1;
                    s.DiffTime = new DateTime(((DateTime)s.TotalTime - (DateTime)first.TotalTime).Ticks);
                });
        }

        public override async Task OnInitAsync()
        {
            var races = await this.RaceLogic.GetAllAsync();
            var racesViewModels = races.Select(r => new RaceListItemViewModel(r)).ToList();
            this.Races.SetItems(racesViewModels);

            this.AllSkiers = await this.SkierLogic.GetAllAsync();

            var locations = await this.LocationLogic.GetAllAsync();
            this.Locations.SetItems(locations, l => new ComboBoxItemViewModel<int>(l.City, l.Id));

            this.RaceTypes.Clear();
            this.RaceTypes.Add(new ComboBoxItemViewModel<RaceType>("Downhill", RaceType.Downhill));
            this.RaceTypes.Add(new ComboBoxItemViewModel<RaceType>("Giant Slalom", RaceType.GiantSlalom));
            this.RaceTypes.Add(new ComboBoxItemViewModel<RaceType>("Slalom", RaceType.Slalom));
            this.RaceTypes.Add(new ComboBoxItemViewModel<RaceType>("SuperG", RaceType.SuperG));

            this.ClockService.OnRunUpdate += this.OnRaceDataUpdate;
            this.ClockService.OnRunFinish += this.OnRaceDataFinish;

            if (this.ClockService.RaceId == this.raceId)
            {
                var startList = Enumerable.Concat(this.StartList1, this.StartList2);
                this.ClockService.CurrentRun = startList.Where(s => s.StartList.Id == this.ClockService?.CurrentRun?.StartList.Id).FirstOrDefault();
                if (this.ClockService.CurrentRun != null)
                {
                    this.SelectedStartListItem = this.ClockService.CurrentRun;
                    var resumeData = this.ClockService.Resume();
                    this.ClockService.CurrentRun.SetRaceData(resumeData, this.sensorAmount, this.ClockService.CurrentRun.StartList.Id);
                }
            }
        }

        public override Task OnDestroyAsync()
        {
            this.ClockService.OnRunUpdate -= this.OnRaceDataUpdate;
            this.ClockService.OnRunFinish -= this.OnRaceDataFinish;

            return Task.CompletedTask;
        }

        private async Task SaveAsync()
        {
            Models.Race race = this.mapper.Map<Models.Race>(this.Edit);

            var result = await this.RaceLogic.SaveAsync(race);
            if (result.IsSuccess)
            {
                this.OnSuccessfulUpdate(race);
            }
        }

        public async Task SaveAsync(
            ObservableCollection<StartListItemViewModel> startListViewModel)
        {
            Models.Race race = this.mapper.Map<Models.Race>(this.Edit);
            var startList = startListViewModel.Select(s => s.StartList).ToList();

            var result = await this.RaceLogic.SaveAsync(race, 1, startList);
            if (result.IsSuccess)
            {
                this.OnSuccessfulUpdate(race);
            }
        }

        private void OnSuccessfulUpdate(Models.Race race)
        {
            this.Races.Selected.Update(race);

            if (this.Edit.Original.Id == 0)
            {
                this.Races.Add(this.Races.Selected);
            }

            this.Edit.Original = race;
            this.Edit.Raise(nameof(RaceEditViewModel.DisplayName));
            this.RaceDetailViewModel.UpdateNavigationTabs();
        }

        public async Task RemoveAsync()
        {
            bool successful = await this.RaceLogic.RemoveAsync(this.Edit.Original);
            if (successful)
            {
                this.Races.RemoveSelected();
                this.Edit = null;
            }
        }

        public Task NewAsync()
        {
            this.Races.Selected = new RaceListItemViewModel(new Models.Race());
            return Task.CompletedTask;
        }

        private async Task<IEnumerable<StartListItemViewModel>> GetStartListAsync(
            int raceId,
            int runNumber)
        {
            var startLists = await this.StartListLogic.GetByRaceIdAsync(raceId, runNumber);
            return startLists
                .OrderBy(s => s.RunNumber)
                .Select(startList => this.ToStartListItemViewModel(startList))
                .ToList();
        }

        public StartListItemViewModel ToStartListItemViewModel(StartList startList)
        {
            var skier = this.AllSkiers.Where(s => s.Id == startList.SkierId).First();
            return new StartListItemViewModel(skier, startList);
        }

        public async Task StartRaceAsync()
        {
            this.Edit.RaceState = RaceState.Running;
            await this.SaveAsync(this.StartList1);
        }

        public async Task StopRaceAsync()
        {
            this.Edit.RaceState = RaceState.Done;
            await this.SaveAsync();
        }

        public bool CanStopRaceAsync()
        {
            if (this.Edit == null)
            {
                return false;
            }

            return this.Edit.RaceState == RaceState.Running;
        }

        private void OnRaceDataUpdate(RaceData data)
        {
            this.Dispatcher.Invoke(async () =>
            {
                DateTime? startTime = this.ClockService.CurrentRun.RaceData.FirstOrDefault()?.TimeStamp;

                this.ClockService.CurrentRun.RaceData.Add(new RaceDataItemViewModel(data, startTime));
                this.ClockService.CurrentRun.Raise(nameof(StartListItemViewModel.TotalTime));

                await this.HubConnection.SendAsync(
                    "RunUpdate",
                    this.mapper.Map<LiveStatistic>(this.ClockService.CurrentRun));
            });
        }

        private Task OnRaceDataFinish(int startListId)
        {
            this.Dispatcher.Invoke(async () =>
            {
                var startList = Enumerable.Concat(this.StartList1, this.StartList2);
                var startListItem = startList.Where(s => s.StartList.Id == startListId).FirstOrDefault();
                if (startListItem != null)
                {
                    startListItem.StartListState = Enums.StartListState.Done;
                    startListItem.Raise(nameof(StartListItemViewModel.TotalTime));
                }

                this.SetRacePositions(this.StartList1);
                this.SetRacePositions(this.StartList2);

                CommandManager.InvalidateRequerySuggested();

                await this.HubConnection.SendAsync(
                    "RunStopped", 
                    "Finished",
                    this.mapper.Map<LiveStatistic>(this.ClockService.CurrentRun));
            });

            return Task.CompletedTask;
        }

        public async Task GrantRunAsync()
        {
            var startList = Enumerable.Concat(this.StartList1, this.StartList2);
            this.ClockService.CurrentRun = startList.Where(s => s.StartListState == Enums.StartListState.NotStarted).FirstOrDefault();
            this.ClockService.CurrentRun.StartListState = Enums.StartListState.Running;
            this.SelectedStartListItem = this.ClockService.CurrentRun;

            this.ClockService.StartListen(
                this.raceId,
                this.sensorAmount,
                this.ClockService.CurrentRun);

            if (this.HubConnection.State != HubConnectionState.Connected)
            {
                await this.HubConnection.StartAsync();
            }

            await this.HubConnection.SendAsync(
                "CurrentRunChange",
                this.mapper.Map<LiveStatistic>(this.ClockService.CurrentRun));
        }

        public async Task DisqualifyAsync()
        {
            if (this.selectedStartListItem.StartList.Id == this.ClockService.CurrentRun?.StartList.Id
             && this.ClockService.IsListening)
            {
                await this.HubConnection.SendAsync(
                    "RunStopped", 
                    "Disqualified", 
                    this.mapper.Map<LiveStatistic>(this.ClockService.CurrentRun));
                this.selectedStartListItem.RaceData.Clear();
                this.ClockService.StopListen();
            }

            var success = await this.StartListLogic.UpdateDisqualified(this.selectedStartListItem.StartList.Id, isDisqualified: true);
            if (success)
            {
                var startList = this.selectedStartListItem.StartList;
                startList.IsDisqualified = true;
                this.SelectedStartListItem.Update(this.selectedStartListItem.Skier, startList);
                this.SelectedStartListItem.StartListState = Enums.StartListState.Disqualified;
            }
        }
    }
}
