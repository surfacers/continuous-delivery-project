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
using Hurace.Core.Models;
using Hurace.Mvvm.ViewModels;
using Hurace.RaceControl.Services;
using Hurace.RaceControl.ViewModels.Controls;
using Hurace.RaceControl.ViewModels.Race.Detail;
using Unity;
using Models = Hurace.Core.Models;
using Enums = Hurace.Core.Enums;
using Microsoft.AspNetCore.SignalR.Client;
using Hurace.Core.Logic.Models;

namespace Hurace.RaceControl.ViewModels.Race
{
    public class RaceViewModel : TabViewModel<MainViewModel>, IRaceViewModel
    {
        private int raceId => Races.Selected?.Race.Id ?? -1;
        private Gender gender => Races.Selected.Race.Gender;
        private int sensorAmount => Races.Selected.Race.SensorAmount;

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
            get => editRace;
            set => Set(ref editRace, value);
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
            get => selectedStartListItem;
            set => Set(ref selectedStartListItem, value);
        }

        public ObservableCollection<ComboBoxItemViewModel<int>> Locations { get; set; }
            = new ObservableCollection<ComboBoxItemViewModel<int>>();
        public ObservableCollection<ComboBoxItemViewModel<RaceType>> RaceTypes { get; set; }
            = new ObservableCollection<ComboBoxItemViewModel<RaceType>>();

        public RaceViewModel()
        {
            RaceListViewModel = new RaceListViewModel(this);
            RaceDetailViewModel = new RaceDetailViewModel(this);
            Races = new FilterViewModel<RaceListItemViewModel>(
                r => $"{r.RaceType} {r.Name}",
                RaceSelectionChanged);
        }

        public async Task RaceSelectionChanged(RaceListItemViewModel race)
        {
            RaceDetailViewModel.NavigationViewModel.CurrentItem.ViewModel.IsLoading = true;

            Edit = mapper.Map<RaceEditViewModel>(race.Race);
            Edit.LocationId = Locations.Where(l => l.Value == race.Race.LocationId).FirstOrDefault();
            Edit.RaceType = RaceTypes.Where(l => l.Value == race.Race.RaceType).FirstOrDefault();

            var skiers = AllSkiers.Where(s => s.Gender == gender).ToList();

            var startListViewModels1 = await GetStartListAsync(raceId, runNumber: 1);
            StartList1.SetItems(startListViewModels1);
            await SetRaceData(raceId, runNumber: 1, startList: StartList1);
            SetRacePositions(StartList1);

            StartList2.Clear();
            if (race.Race.HasSecondRun())
            {
                var startListViewModels2 = await GetStartListAsync(raceId, runNumber: 2);
                StartList2.SetItems(startListViewModels2);
                await SetRaceData(raceId, runNumber: 2, startList: StartList2);
                SetRacePositions(StartList2);
            }

            RaceDetailViewModel.NavigationViewModel.CurrentItem.ViewModel.IsLoading = false;

            RaceDetailViewModel.UpdateNavigationTabs();
        }

        private async Task SetRaceData(int raceId, int runNumber, IEnumerable<StartListItemViewModel> startList)
        {
            var raceData = await RaceDataLogic.GetByRaceIdAsync(raceId, runNumber);
            foreach (var item in startList)
            {
                var data = raceData.Where(r => r.StartListId == item.StartList.Id).ToList();
                item.SetRaceData(data, sensorAmount, ClockService?.CurrentRun?.StartList?.Id ?? -1);
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
            Races.SetItems(racesViewModels);

            AllSkiers = await this.SkierLogic.GetAllAsync();

            var locations = await LocationLogic.GetAllAsync();
            Locations.SetItems(locations, l => new ComboBoxItemViewModel<int>(l.City, l.Id));

            RaceTypes.Clear();
            RaceTypes.Add(new ComboBoxItemViewModel<RaceType>("Downhill", RaceType.Downhill));
            RaceTypes.Add(new ComboBoxItemViewModel<RaceType>("Giant Slalom", RaceType.GiantSlalom));
            RaceTypes.Add(new ComboBoxItemViewModel<RaceType>("Slalom", RaceType.Slalom));
            RaceTypes.Add(new ComboBoxItemViewModel<RaceType>("SuperG", RaceType.SuperG));

            ClockService.OnRunUpdate += OnRaceDataUpdate;
            ClockService.OnRunFinish += OnRaceDataFinish;

            if (ClockService.RaceId == raceId)
            {
                var startList = Enumerable.Concat(StartList1, StartList2);
                ClockService.CurrentRun = startList.Where(s => s.StartList.Id == ClockService?.CurrentRun?.StartList.Id).FirstOrDefault();
                if (ClockService.CurrentRun != null)
                {
                    SelectedStartListItem = ClockService.CurrentRun;
                    var resumeData = ClockService.Resume();
                    ClockService.CurrentRun.SetRaceData(resumeData, sensorAmount, ClockService.CurrentRun.StartList.Id);
                }
            }
        }

        public override Task OnDestroyAsync()
        {
            ClockService.OnRunUpdate -= OnRaceDataUpdate;
            ClockService.OnRunFinish -= OnRaceDataFinish;

            return Task.CompletedTask;
        }

        private async Task SaveAsync()
        {
            Models.Race race = mapper.Map<Models.Race>(Edit);

            var result = await RaceLogic.SaveAsync(race);
            if (result.IsSuccess)
            {
                OnSuccessfulUpdate(race);
            }
        }

        public async Task SaveAsync(
            ObservableCollection<StartListItemViewModel> startListViewModel)
        {
            Models.Race race = mapper.Map<Models.Race>(Edit);
            var startList = startListViewModel.Select(s => s.StartList).ToList();

            var result = await RaceLogic.SaveAsync(race, 1, startList);
            if (result.IsSuccess)
            {
                OnSuccessfulUpdate(race);
            }
        }

        private void OnSuccessfulUpdate(Models.Race race)
        {
            Races.Selected.Update(race);

            if (Edit.Original.Id == 0)
            {
                Races.Add(Races.Selected);
            }

            Edit.Original = race;
            Edit.Raise(nameof(RaceEditViewModel.DisplayName));
            RaceDetailViewModel.UpdateNavigationTabs();
        }

        public async Task RemoveAsync()
        {
            bool successful = await RaceLogic.RemoveAsync(Edit.Original);
            if (successful)
            {
                Races.RemoveSelected();
                Edit = null;
            }
        }

        public Task NewAsync()
        {
            Races.Selected = new RaceListItemViewModel(new Models.Race());
            return Task.CompletedTask;
        }

        private async Task<IEnumerable<StartListItemViewModel>> GetStartListAsync(
            int raceId,
            int runNumber)
        {
            var startLists = await StartListLogic.GetByRaceIdAsync(raceId, runNumber);
            return startLists
                .OrderBy(s => s.RunNumber)
                .Select(startList => ToStartListItemViewModel(startList))
                .ToList();
        }

        public StartListItemViewModel ToStartListItemViewModel(StartList startList)
        {
            var skier = AllSkiers.Where(s => s.Id == startList.SkierId).First();
            return new StartListItemViewModel(skier, startList);
        }

        public async Task StartRaceAsync()
        {
            Edit.RaceState = RaceState.Running;
            await SaveAsync(StartList1);
        }

        public async Task StopRaceAsync()
        {
            Edit.RaceState = RaceState.Done;
            await SaveAsync();
        }

        public bool CanStopRaceAsync()
        {
            if (Edit == null) return false;

            return Edit.RaceState == RaceState.Running;
        }

        private void OnRaceDataUpdate(RaceData data)
        {
            Dispatcher.Invoke(async () =>
            {
                DateTime? startTime = ClockService.CurrentRun.RaceData.FirstOrDefault()?.TimeStamp;

                ClockService.CurrentRun.RaceData.Add(new RaceDataItemViewModel(data, startTime));
                ClockService.CurrentRun.Raise(nameof(StartListItemViewModel.TotalTime));

                await this.HubConnection.SendAsync("RunUpdate",
                    this.mapper.Map<LiveStatistic>(ClockService.CurrentRun));
            });
        }

        private Task OnRaceDataFinish(int startListId)
        {
            Dispatcher.Invoke(async () =>
            {
                var startList = Enumerable.Concat(StartList1, StartList2);
                var startListItem = startList.Where(s => s.StartList.Id == startListId).FirstOrDefault();
                if (startListItem != null)
                {
                    startListItem.StartListState = Enums.StartListState.Done;
                    startListItem.Raise(nameof(StartListItemViewModel.TotalTime));
                }

                SetRacePositions(StartList1);
                SetRacePositions(StartList2);

                CommandManager.InvalidateRequerySuggested();

                await this.HubConnection.SendAsync("RunStopped", "Finished",
                    this.mapper.Map<LiveStatistic>(ClockService.CurrentRun));
            });

            return Task.CompletedTask;
        }

        public async Task GrantRunAsync()
        {
            var startList = Enumerable.Concat(StartList1, StartList2);
            ClockService.CurrentRun = startList.Where(s => s.StartListState == Enums.StartListState.NotStarted).FirstOrDefault();
            ClockService.CurrentRun.StartListState = Enums.StartListState.Running;
            SelectedStartListItem = ClockService.CurrentRun;

            ClockService.StartListen(
                raceId,
                sensorAmount,
                ClockService.CurrentRun);

            if (this.HubConnection.State != HubConnectionState.Connected)
            {
                await this.HubConnection.StartAsync();
            }

            await this.HubConnection.SendAsync("CurrentRunChange",
                this.mapper.Map<LiveStatistic>(ClockService.CurrentRun));
        }

        public async Task DisqualifyAsync()
        {
            if (selectedStartListItem.StartList.Id == ClockService.CurrentRun?.StartList.Id
             && ClockService.IsListening)
            {
                await this.HubConnection.SendAsync("RunStopped", "Disqualified", 
                    this.mapper.Map<LiveStatistic>(ClockService.CurrentRun));
                selectedStartListItem.RaceData.Clear();
                ClockService.StopListen();
            }

            var success = await StartListLogic.UpdateDisqualified(selectedStartListItem.StartList.Id, isDisqualified: true);
            if (success)
            {
                var startList = selectedStartListItem.StartList;
                startList.IsDisqualified = true;
                SelectedStartListItem.Update(selectedStartListItem.Skier, startList);
                SelectedStartListItem.StartListState = Enums.StartListState.Disqualified;
            }
        }
    }
}
