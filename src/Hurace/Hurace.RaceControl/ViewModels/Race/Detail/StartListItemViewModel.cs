using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Hurace.Core.Enums;
using Hurace.Core.Extensions;
using Hurace.Core.Models;
using Hurace.Mvvm;
using Models = Hurace.Core.Models;

namespace Hurace.RaceControl.ViewModels.Race.Detail
{
    public class StartListItemViewModel : NotifyPropertyChanged
    {
        public Models.Skier Skier { get; private set; }
        public Models.StartList StartList { get; private set; }
        public ObservableCollection<RaceDataItemViewModel> RaceData { get; private set; }
            = new ObservableCollection<RaceDataItemViewModel>();
        public DateTime? TotalTime => RaceData.LastOrDefault()?.TotalTime;

        private int? position;
        public int? Position
        {
            get => position;
            set => Set(ref position, value);
        }

        private DateTime? diffTime;
        public DateTime? DiffTime
        {
            get => diffTime;
            set => Set(ref diffTime, value);
        }

        public string FullName => Skier.FullName();
        public string CountryCode => Skier.CountryCode;
        public int StartNumber => StartList.StartNumber;
        public bool IsDisqualified => StartList.IsDisqualified;

        private StartListState startListState;
        public StartListState StartListState
        {
            get => startListState;
            set => Set(ref startListState, value);
        }

        public StartListItemViewModel(Models.Skier skier, Models.StartList startList)
        {
            Skier = skier;
            StartList = startList;
        }

        public void Update(Models.Skier skier, Models.StartList startList)
        {
            Skier = skier;
            Raise(nameof(FullName));
            Raise(nameof(CountryCode));

            StartList = startList;
            Raise(nameof(StartNumber));
            Raise(nameof(IsDisqualified));
        }

        public void SetRaceData(
            IEnumerable<RaceData> raceData, 
            int sensorAmount, 
            int runningStartListId)
        {
            DateTime? startTime = raceData.FirstOrDefault()?.TimeStamp ?? null;
            RaceData.SetItems(raceData, r => new RaceDataItemViewModel(r, startTime));
            StartListState = StartList.GetStartListState(raceData, sensorAmount, runningStartListId);
        }
    }
}
